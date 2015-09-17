using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveDev.SyntaxVisualizer.SyntaxWalker
{
    public class FindUndocumentedMethodsSyntaxWalker : CSharpSyntaxWalker
    {
        public FindUndocumentedMethodsSyntaxWalker()
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
            var docSyntaxes = node.GetLeadingTrivia()
                                  .Where(trivia => trivia.GetStructure() != null)
                                  .Select(trivia => trivia.GetStructure())
                                  .OfType<DocumentationCommentTriviaSyntax>();

            if (docSyntaxes.FirstOrDefault() == null)
                MethodsWithoutDoc.Add(node);
        }
    }
}
