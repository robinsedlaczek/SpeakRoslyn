using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class SyntaxAnalysisCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Simple Syntax Analysis";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var result = new List<ISyntaxViewModel>();

            var root = SyntaxTree.GetRoot();
            result.Add(WrapResult(root));

            var compilationUnitSyntax = (CompilationUnitSyntax)root;
            result.Add(WrapResult(compilationUnitSyntax));

            var namespaceSyntax = (NamespaceDeclarationSyntax)compilationUnitSyntax.Members[0];
            result.Add(WrapResult(namespaceSyntax));

            var classSyntax = (ClassDeclarationSyntax)namespaceSyntax.Members[0];
            result.Add(WrapResult(classSyntax));

            var methodSyntax = (MethodDeclarationSyntax)classSyntax.Members[0];
            result.Add(WrapResult(methodSyntax));

            return result;
        }
    }
}
