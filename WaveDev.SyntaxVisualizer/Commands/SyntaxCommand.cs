using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    public abstract class SyntaxCommand
    {
        public void Init(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public string Name
        {
            get;
            protected set;
        }

        protected SyntaxTree SyntaxTree
        {
            get;
            set;
        }

        public abstract IEnumerable<ISyntaxViewModel> Execute();

        #region Protected Helper 

        protected IEnumerable<ISyntaxViewModel> WrapResult(IEnumerable<SyntaxNode> nodesToWrap)
        {
            var result = new List<ISyntaxViewModel>();

            foreach (var node in nodesToWrap)
                result.Add(new SyntaxNodeViewModel(node));

            return result;
        }

        protected IEnumerable<ISyntaxViewModel> WrapResult(IEnumerable<SyntaxToken> tokensToWrap)
        {
            var result = new List<ISyntaxViewModel>();

            foreach (var token in tokensToWrap)
                result.Add(new SyntaxTokenViewModel(token));

            return result;
        }

        protected IEnumerable<ISyntaxViewModel> WrapResult(IEnumerable<SyntaxTrivia> triviaToWrap)
        {
            var result = new List<ISyntaxViewModel>();

            foreach (var trivia in triviaToWrap)
                result.Add(new SyntaxTriviaViewModel(trivia));

            return result;
        }

        #endregion
    }
}
