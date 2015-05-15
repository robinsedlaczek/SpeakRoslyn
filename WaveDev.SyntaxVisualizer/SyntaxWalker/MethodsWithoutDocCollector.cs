using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace WaveDev.SyntaxVisualizer.SyntaxWalker
{
    internal class MethodsWithoutDocCollector : CSharpSyntaxWalker
    {
        public MethodsWithoutDocCollector()
        {
            MethodsWithoutDoc = new List<MethodDeclarationSyntax>();
        }

        public IList<MethodDeclarationSyntax> MethodsWithoutDoc
        {
            get;
            private set;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            //var docTriviaList = node.GetLeadingTrivia()
			//						  .Where(trivia => trivia.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia);

            var docSyntaxes = node.GetLeadingTrivia()
                                  .Where(trivia => trivia.GetStructure() != null)
                                  .Select(trivia => trivia.GetStructure())
                                  .OfType<DocumentationCommentTriviaSyntax>();

            if (docSyntaxes.FirstOrDefault() == null)
                MethodsWithoutDoc.Add(node);
        }

    }
}