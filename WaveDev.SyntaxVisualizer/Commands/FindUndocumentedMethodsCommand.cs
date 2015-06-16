using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SyntaxVisualizer.ViewModels;
using WaveDev.SyntaxVisualizer.SyntaxWalker;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindUndocumentedMethodsCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Undocumented Methods";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var walker = new MethodsWithoutDocCollector();
            walker.Visit(SyntaxTree.GetRoot());

            return WrapResult(walker.MethodsWithoutDoc);
        }
    }
}
