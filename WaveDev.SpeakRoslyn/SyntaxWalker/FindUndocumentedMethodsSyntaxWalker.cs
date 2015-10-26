﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace WaveDev.SpeakRoslyn.SyntaxWalker
{
    public class FindUndocumentedMethodsSyntaxWalker : CSharpSyntaxWalker
    {
        public IList<MethodDeclarationSyntax> MethodsWithoutDoc
        {
            get;
            private set;
        } = new List<MethodDeclarationSyntax>();

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
