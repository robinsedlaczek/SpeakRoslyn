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
        /// Logic to find all method declarations that are not docuemented with xml code documentation.
        /// </summary>
        /// <param name="node">The current node to analyse.</param>
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            // Find leading trivia that has structure (xml structure) and that is of type DocumentationCommentTriviaSyntax.
            var docSyntaxes = node.GetLeadingTrivia()
                                  .Where(trivia => trivia.GetStructure() != null)
                                  .Select(trivia => trivia.GetStructure())
                                  .OfType<DocumentationCommentTriviaSyntax>();

            // If we cannot find such trivia we know that there is no xml code documentation.
            if (docSyntaxes.FirstOrDefault() == null)
                MethodsWithoutDoc.Add(node);
        }
    }
}
