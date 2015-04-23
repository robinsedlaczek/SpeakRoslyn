using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(ISyntaxCommand))]
    public class FindNodesSyntaxCommand : ISyntaxCommand
    {
        private SyntaxTree _syntaxTree;

        public void Init(SyntaxTree syntaxTree)
        {
            _syntaxTree = syntaxTree;
        }

        public string Name
        {
            get
            {
                return "Nodes";
            }
        }

        public IEnumerable<ISyntaxViewModel> Execute()
        {
            var rootNode = _syntaxTree.GetRoot();
            var nodes = rootNode.DescendantNodes();
            var result = new List<ISyntaxViewModel>();

            foreach (var node in nodes)
                result.Add(new SyntaxNodeViewModel(node));

            return result;
        }
    }
}
