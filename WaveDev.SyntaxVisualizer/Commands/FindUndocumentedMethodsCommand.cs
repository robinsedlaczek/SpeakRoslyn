using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveDev.SyntaxVisualizer.SyntaxWalker;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindUndocumentedMethodsCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Find undocumented methods";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var walker = new FindUndocumentedMethodsSyntaxWalker();
            walker.Visit(SyntaxTree.GetRoot());

            return WrapResult(walker.MethodsWithoutDoc);
        }
    }
}
