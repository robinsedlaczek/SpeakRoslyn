using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    public class FindTriviasSyntaxCommand : ISyntaxCommand
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
                return "Trivias";
            }
        }

        public IEnumerable<ISyntaxViewModel> Execute()
        {
            var rootNode = _syntaxTree.GetRoot();
            var trivias = rootNode.DescendantTrivia();
            var result = new List<ISyntaxViewModel>();

            foreach (var trivia in trivias)
                result.Add(new SyntaxTriviaViewModel(trivia));

            return result;
        }
    }
}
