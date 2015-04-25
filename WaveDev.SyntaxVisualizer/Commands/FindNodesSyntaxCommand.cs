using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindNodesSyntaxCommand : SyntaxCommand
    {
        public FindNodesSyntaxCommand()
        {
            Name = "Nodes";
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var rootNode = SyntaxTree.GetRoot();
            var nodes = rootNode.DescendantNodes();

            return WrapResult(nodes);
        }
    }
}
