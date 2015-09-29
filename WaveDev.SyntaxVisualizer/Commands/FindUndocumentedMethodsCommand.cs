using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SpeakRoslyn.SyntaxWalker;
using WaveDev.SpeakRoslyn.ViewModels;

namespace WaveDev.SpeakRoslyn.Commands
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
