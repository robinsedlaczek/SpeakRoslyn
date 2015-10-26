using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WaveDev.SpeakRoslyn.DosAnalyzers
{
    /// <summary>
    /// This Code Analyzer checks if all methods have xml code documentation.
    /// It has been developed for the #SpeakRoslyn workshop to show the capabilities of the .NET Compiler Platform.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WaveDevSpeakRoslynDosAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SpeakRoslynAnalyzers";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Documentation";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(analysisContext => AnalyzeSyntax(analysisContext), ImmutableArray.Create(SyntaxKind.MethodDeclaration));
        }

        /// <summary>
        /// This method checks if all methods have xml code documentation. Therefore, it uses a <see cref="CSharpSyntaxWalker"/> that visits 
        /// <see cref="MethodDeclarationSyntax"/> nodes in order to perform the check method by method.
        /// </summary>
        /// <param name="analysisContext">The context given by the host environment. It contains e.g. the node where the user is currently on with the cursor.</param>
        private void AnalyzeSyntax(SyntaxNodeAnalysisContext analysisContext)
        {
            // First we create the syntax walker and let him visit the current syntax node.
            var syntaxWalker = new FindUndocumentedMethodsSyntaxWalker();
            syntaxWalker.VisitMethodDeclaration(analysisContext.Node as MethodDeclarationSyntax);

            // For each method without xml code documentation, we report a diagnostic to the environment.
            foreach (var methodDeclaration in syntaxWalker.MethodsWithoutDoc)
            {
                var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation());
                analysisContext.ReportDiagnostic(diagnostic);
            }
        }
    }
}
