using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SyntaxVisualizer.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindTriviasSyntaxCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Trivia";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var rootNode = SyntaxTree.GetRoot();
            var trivias = rootNode.DescendantTrivia();

            return WrapResult(trivias);
        }
    }
}
