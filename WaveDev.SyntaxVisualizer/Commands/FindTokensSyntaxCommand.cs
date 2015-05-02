using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindTokensSyntaxCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Token";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var rootNode = SyntaxTree.GetRoot();
            var tokens = rootNode.DescendantTokens();

            return WrapResult(tokens);
        }
    }
}
