using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public class SyntaxNodeViewModel : ISyntaxViewModel
    {
        #region Private Fields

        private SyntaxNode _wrappedSyntaxNode;

        #endregion

        #region Construction

        public SyntaxNodeViewModel(SyntaxNode node)
        {
            _wrappedSyntaxNode = node;

            DisplayName = "[Node] " + Kind;
            Children = new List<ISyntaxViewModel>();

            WrapLeadingSyntaxTrivias();
            WrapChildSyntaxNodes();
            WrapChildSyntaxTokens();
            WrapTrailingSyntaxTrivias();
        }

        #endregion

        #region Public Members

        public IEnumerable<ISyntaxViewModel> Children
        {
            get;
            private set;
        }

        public string Kind
        {
            get
            {
                return _wrappedSyntaxNode.Kind().ToString();
            }
        }

        public string DisplayName
        {
            get;
            private set;
        }

        #endregion

        #region Private Methods

        private void WrapChildSyntaxNodes()
        {
            var nodesToWrap = _wrappedSyntaxNode.ChildNodes();

            foreach (var nodeToWrap in nodesToWrap)
                (Children as List<ISyntaxViewModel>).Add(new SyntaxNodeViewModel(nodeToWrap));
        }

        private void WrapChildSyntaxTokens()
        {
            var tokensToWrap = _wrappedSyntaxNode.ChildTokens();

            foreach (var tokenToWrap in tokensToWrap)
                (Children as List<ISyntaxViewModel>).Add(new SyntaxTokenViewModel(tokenToWrap));
        }

        private void WrapLeadingSyntaxTrivias()
        {
            var triviasToWrap = _wrappedSyntaxNode.GetLeadingTrivia();

            foreach (var triviaToWrap in triviasToWrap)
                (Children as List<ISyntaxViewModel>).Add(new SyntaxTriviaViewModel(triviaToWrap));
        }

        private void WrapTrailingSyntaxTrivias()
        {
            var triviasToWrap = _wrappedSyntaxNode.GetTrailingTrivia();

            foreach (var triviaToWrap in triviasToWrap)
                (Children as List<ISyntaxViewModel>).Add(new SyntaxTriviaViewModel(triviaToWrap));
        }

        #endregion
    }
}