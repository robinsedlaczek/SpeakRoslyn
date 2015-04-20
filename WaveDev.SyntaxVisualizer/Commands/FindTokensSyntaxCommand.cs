using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    public class FindTokensSyntaxCommand : ISyntaxCommand
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
                return "Tokens";
            }
        }

        public IEnumerable<ISyntaxViewModel> Execute()
        {
            var rootNode = _syntaxTree.GetRoot();
            var tokens = rootNode.DescendantTokens();
            var result = new List<ISyntaxViewModel>();

            foreach (var token in tokens)
                result.Add(new SyntaxTokenViewModel(token));

            return result;
        }
    }
}
