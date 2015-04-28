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
            var result = SyntaxTree.GetRoot().DescendantNodes();

            return WrapResult(result);
        }
    }
}
