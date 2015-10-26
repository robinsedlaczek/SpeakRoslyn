using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace WaveDev.SpeakRoslyn.DosAnalyzers
{
    /// <summary>
    /// This is the implementation of the Code Fix for missing xml code documentation on methods diagnostics.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(WaveDevSpeakRoslynDosAnalyzersCodeFixProvider)), Shared]
    public class WaveDevSpeakRoslynDosAnalyzersCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The title that is shown to the user in the Visual Studio UI.
        /// </summary>
        private const string title = "[DOS] Add xml code documentation";

        /// <summary>
        /// The IDs of the diagnostics that can be fixed by this Code Fixes provided by this class.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                return ImmutableArray.Create(WaveDevSpeakRoslynDosAnalyzersAnalyzer.DiagnosticId);
            }
        }

        /// <summary>
        /// This method returns the FixAllProvider that fixes all occurrences of a diagnostic with the Code Fix provided by this implementation.
        /// </summary>
        /// <returns>Returns the FixAllProvider (it is the standard BatchFixer in our case).</returns>
        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <summary>
        /// This method registers a <see cref="CodeAction"/> the performs th Code Fix for any found diagnostic.
        /// </summary>
        /// <param name="context">The context for the Code Fix. It contains the found dianogstics, the affected document, the cancellation token etc.</param>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            // First we ask the found diagnostic for the appropriate place in the code where the problem occurred. 
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Then we ask the current document for his root syntax node and try to find the affected MethodDeclarationSyntax within it.
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

            // With the found MethodDeclarationSyntax, we can now register a CodeAction that will fix the missing xml code documentation.
            // Visual Studio will execute this CodeAction to create the preview and if the user executes the CodeAction manually to
            // perform the fix on the current method.
            var action = CodeAction.Create(title, cancellationToken => CreateCodeDocumentation(context.Document, declaration, cancellationToken), title);
            context.RegisterCodeFix(action, diagnostic);
        }

        /// <summary>
        /// This method implements the concrete CodeFix for the missing xml code documentation issue.
        /// </summary>
        /// <param name="document">The current document that contains the method to fix.</param>
        /// <param name="methodDeclaration">The <see cref="MethodDeclarationSyntax"/> instance where the xml code documentation is missing.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that indicates when the user aborted the operation.</param>
        /// <returns>Returns the new document with the fixed method, that is, the method with the yxml code documentation.</returns>
        private async Task<Document> CreateCodeDocumentation(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            // First we setup a StringBuilder used to generate the code for the xml code documentation.
            var codeBuilder = new StringBuilder();
            var whitespaces = GenerateIndentionWhitespaces(methodDeclaration);

            // Then we generate the xml code documentation as strings and with that string, generate a new MethodDeclarationSyntax.
            GenerateSummaryXmlDocumentationElement(codeBuilder, whitespaces);
            GenerateParameterXmlDocumentationElements(methodDeclaration, codeBuilder, whitespaces);
            GenerateReturnTypeXmlDocumentationElement(methodDeclaration, codeBuilder, whitespaces);
            var newMethodDeclaration = await GenerateMethodDeclarationSyntax(methodDeclaration, cancellationToken, codeBuilder, whitespaces).ConfigureAwait(false);

            // At the end we have to respin the whole document with the new MethodDeclarationSyntax. That is because the object model
            // is immutable. We we have to reconstruct the whole syntax tree and the document containing it.
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var newRoot = root.ReplaceNode(methodDeclaration, newMethodDeclaration);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;
        }

        /// <summary>
        /// This method generates the new <see cref="MethodDeclarationSyntax"/> containing the xml code documentation.
        /// </summary>
        /// <param name="methodDeclaration">The old <see cref="MethodDeclarationSyntax"/> leaking the xml code documentation.</param>
        /// <param name="cancellationToken">The token that indicates that the user aborted the operation.</param>
        /// <param name="codeBuilder">The <see cref="StringBuilder"/> used to generate the string of the xml code documentation.</param>
        /// <param name="whitespaces">Whitespaces used for indention.</param>
        /// <returns>Returns the new <see cref="MethodDeclarationSyntax"/> containing the xml code documentation.</returns>
        private static async Task<MethodDeclarationSyntax> GenerateMethodDeclarationSyntax(MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken, StringBuilder codeBuilder, string whitespaces)
        {
            // First we parse the generated code for the xml code documentation to get the syntax model.
            var root = await CSharpSyntaxTree.ParseText(text: codeBuilder.ToString(), cancellationToken: cancellationToken)
                .GetRootAsync(cancellationToken).ConfigureAwait(false);

            // With the syntax model we generate the new leading trivia containg the syntax for the xml code documentation,
            // new lines before and after the new MethodDeclarationSyntax and the indention.

            var generatedTrivia = root
                .GetLeadingTrivia()
                .Insert(0, SyntaxFactory.EndOfLine("\r\n"))
                .Add(SyntaxFactory.Whitespace(whitespaces));

            var newMethodDeclaration = methodDeclaration
                .WithLeadingTrivia(generatedTrivia)
                .WithTrailingTrivia(SyntaxFactory.EndOfLine("\r\n"));

            return newMethodDeclaration;
        }

        /// <summary>
        /// This method generates the text for the xml code documentation of the return type/value.
        /// </summary>
        /// <param name="methodDeclaration">The old <see cref="MethodDeclarationSyntax"/> leaking the xml code documentation.</param>
        /// <param name="codeBuilder">The <see cref="StringBuilder"/> used to generate the string of the xml code documentation.</param>
        /// <param name="whitespaces">Whitespaces used for indention.</param>
        private static void GenerateReturnTypeXmlDocumentationElement(MethodDeclarationSyntax methodDeclaration, StringBuilder codeBuilder, string whitespaces)
        {
            // We only generate the xml code documentation for the return type, if the method has a return type.
            var returnTypeName = methodDeclaration.ReturnType.ToString();

            if (returnTypeName != "void")
                codeBuilder.Append(whitespaces).AppendLine(@"/// <returns></returns>");
        }

        /// <summary>
        /// This method generates the text for the xml code documentation of the parameter list.
        /// </summary>
        /// <param name="methodDeclaration">The old <see cref="MethodDeclarationSyntax"/> leaking the xml code documentation.</param>
        /// <param name="codeBuilder">The <see cref="StringBuilder"/> used to generate the string of the xml code documentation.</param>
        /// <param name="whitespaces">Whitespaces used for indention.</param>
        private static void GenerateParameterXmlDocumentationElements(MethodDeclarationSyntax methodDeclaration, StringBuilder codeBuilder, string whitespaces)
        {
            // We generate xml code documentation for each parameter that we find for the method.
            foreach (var parameter in methodDeclaration.ParameterList.Parameters)
            {
                codeBuilder
                    .Append(whitespaces)
                    .Append(@"/// <param name=""")
                    .Append(parameter.Identifier.ValueText)
                    .AppendLine(@"""></param>");
            }
        }

        /// <summary>
        /// This method generates the text for xml code documentation of the summary information about the method.
        /// </summary>
        /// <param name="codeBuilder">The <see cref="StringBuilder"/> used to generate the string of the xml code documentation.</param>
        /// <param name="whitespaces">Whitespaces used for indention.</param>
        private static void GenerateSummaryXmlDocumentationElement(StringBuilder codeBuilder, string whitespaces)
        {
            codeBuilder
                .Append(whitespaces).AppendLine(@"/// <summary>")
                .Append(whitespaces).AppendLine(@"/// ")
                .Append(whitespaces).AppendLine(@"/// </summary>");
        }

        /// <summary>
        /// This method generates the line indention for the new method.
        /// </summary>
        /// <param name="methodDeclaration">The old <see cref="MethodDeclarationSyntax"/> leaking the xml code documentation.</param>
        /// <returns></returns>
        private static string GenerateIndentionWhitespaces(MethodDeclarationSyntax methodDeclaration)
        {
            // First we look how many whitespaces the indention of the old method consists of.
            var whitespaces = string.Empty;
            var whitespaceTrivia = methodDeclaration.GetLeadingTrivia()
                .Where(trivia => trivia.Kind() == SyntaxKind.WhitespaceTrivia).FirstOrDefault();

            // Then we create a string that consists of so many whitespaces the idention of the old method consits of.
            whitespaceTrivia.Span.Length.Times(() => whitespaces += " ");

            return whitespaces;
        }
    }
}