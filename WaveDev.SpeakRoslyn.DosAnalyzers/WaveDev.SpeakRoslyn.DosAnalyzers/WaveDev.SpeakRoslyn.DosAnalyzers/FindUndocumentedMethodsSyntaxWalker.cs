using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace WaveDev.SpeakRoslyn.DosAnalyzers
{
    /// <summary>
    /// This class implements a SyntaxWalker to visit method declarations and checks, if these methods are documented.
    /// </summary>
    public class FindUndocumentedMethodsSyntaxWalker : CSharpSyntaxWalker
    {
        /// <summary>
        /// Property to store all the method declarations that have no code documentation.
        /// </summary>
        public IList<MethodDeclarationSyntax> MethodsWithoutDoc
        {
            get;
            private set;

        } = new List<MethodDeclarationSyntax>();

        /// <summary>
        /// Logic to find all method declarations that are not docuemented with code doc.
        /// </summary>
        /// <param name="node">The current node to analyse.</param>
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var docSyntaxes = node.GetLeadingTrivia()
                                  .Where(trivia => trivia.GetStructure() != null)
                                  .Select(trivia => trivia.GetStructure())
                                  .OfType<DocumentationCommentTriviaSyntax>();

            if (docSyntaxes.FirstOrDefault() == null)
                MethodsWithoutDoc.Add(node);
        }
    }
}
