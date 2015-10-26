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
using Microsoft.CodeAnalysis.Text;
using System;

namespace WaveDev.SpeakRoslyn.DosAnalyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(WaveDevSpeakRoslynDosAnalyzersCodeFixProvider)), Shared]
    public class WaveDevSpeakRoslynDosAnalyzersCodeFixProvider : CodeFixProvider
    {
        private const string title = "[DOS] Add xml code documentation";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                return ImmutableArray.Create(WaveDevSpeakRoslynDosAnalyzersAnalyzer.DiagnosticId);
            }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

            var action = CodeAction.Create(title, cancellationToken => CreateCodeDocumentation(context.Document, declaration, cancellationToken), title);
            context.RegisterCodeFix(action, diagnostic);
        }

        private async Task<Document> CreateCodeDocumentation(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            var codeBuilder = new StringBuilder();
            var whitespaces = GenerateIndentionWhitespaces(methodDeclaration);

            GenerateSummaryXmlDocumentationElement(codeBuilder, whitespaces);
            GenerateParameterXmlDocumentationElements(methodDeclaration, codeBuilder, whitespaces);
            GenerateReturnTypeXmlDocumentationElement(methodDeclaration, codeBuilder, whitespaces);

            var newMethodDeclaration = await GenerateMethodDeclarationSyntax(methodDeclaration, cancellationToken, codeBuilder, whitespaces);
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var newRoot = root.ReplaceNode(methodDeclaration, newMethodDeclaration);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;
        }

        private static async Task<MethodDeclarationSyntax> GenerateMethodDeclarationSyntax(MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken, StringBuilder codeBuilder, string whitespaces)
        {
            var root = await CSharpSyntaxTree.ParseText(text: codeBuilder.ToString(), cancellationToken: cancellationToken)
                .GetRootAsync(cancellationToken);

            var generatedTrivia = root
                .GetLeadingTrivia()
                .Insert(0, SyntaxFactory.EndOfLine("\r\n"))
                .Add(SyntaxFactory.Whitespace(whitespaces));

            var newMethodDeclaration = methodDeclaration
                .WithLeadingTrivia(generatedTrivia)
                .WithTrailingTrivia(SyntaxFactory.EndOfLine("\r\n"));

            return newMethodDeclaration;
        }

        private static void GenerateReturnTypeXmlDocumentationElement(MethodDeclarationSyntax methodDeclaration, StringBuilder codeBuilder, string whitespaces)
        {
            var returnTypeName = methodDeclaration.ReturnType.ToString();

            if (returnTypeName != "void")
                codeBuilder.Append(whitespaces).AppendLine(@"/// <returns></returns>");
        }

        private static void GenerateParameterXmlDocumentationElements(MethodDeclarationSyntax methodDeclaration, StringBuilder codeBuilder, string whitespaces)
        {
            foreach (var parameter in methodDeclaration.ParameterList.Parameters)
            {
                codeBuilder
                    .Append(whitespaces)
                    .Append(@"/// <param name=""")
                    .Append(parameter.Identifier.ValueText)
                    .AppendLine(@"""></param>");
            }
        }

        private static void GenerateSummaryXmlDocumentationElement(StringBuilder codeBuilder, string whitespaces)
        {
            codeBuilder
                .Append(whitespaces).AppendLine(@"/// <summary>")
                .Append(whitespaces).AppendLine(@"/// ")
                .Append(whitespaces).AppendLine(@"/// </summary>");
        }

        private static string GenerateIndentionWhitespaces(MethodDeclarationSyntax methodDeclaration)
        {
            var whitespaces = string.Empty;
            var whitespaceTrivia = methodDeclaration.GetLeadingTrivia()
                .Where(trivia => trivia.Kind() == SyntaxKind.WhitespaceTrivia).FirstOrDefault();

            whitespaceTrivia.Span.Length.Times(() => whitespaces += " ");
            return whitespaces;
        }
    }
}