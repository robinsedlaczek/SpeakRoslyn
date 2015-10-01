using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SpeakRoslyn.ViewModels;

namespace WaveDev.SpeakRoslyn.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindNodesSyntaxCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Nodes";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var rootNode = SyntaxTree.GetRoot();
            var nodes = rootNode.DescendantNodes();

            return WrapResult(nodes);
        }
    }
}
