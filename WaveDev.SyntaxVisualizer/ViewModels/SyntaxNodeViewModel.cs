using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslyn.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public class SyntaxNodeViewModel : ISyntaxViewModel
    {
        #region Private Fields

        private SyntaxNode _wrappedSyntaxNode;

        #endregion

        #region Construction

        /// <summary>
        /// This constructor is used to create a syntax node view model that represents the root node of the syntax tree.
        /// </summary>
        /// <param name="node"></param>
        public SyntaxNodeViewModel(SyntaxNodeViewModel node)
        {
            var children = new List<ISyntaxViewModel>();
            children.Add(node);

            Children = children;
        }

        /// <summary>
        /// This constructor is used to create syntax node view models for nodes, that are not the root node of a syntax tree.
        /// </summary>
        /// <param name="node"></param>
        public SyntaxNodeViewModel(SyntaxNode node)
        {
            _wrappedSyntaxNode = node;

            Color = Brushes.Blue;
            DisplayName = Kind + " [" + _wrappedSyntaxNode.Span.Start + ".." + _wrappedSyntaxNode.Span.End + "]";

            Children = new List<ISyntaxViewModel>();

            WrapChildSyntaxNodes();
            WrapChildSyntaxTokens();

            Children = Children.OrderBy(syntax => syntax.SpanStart);
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

        public Brush Color
        {
            get;
            private set;
        }

        public ISyntaxViewModel SelectedSourceSyntax
        {
            get
            {
                return this;
            }
        }

        public int SpanStart
        {
            get
            {
                return _wrappedSyntaxNode.Span.Start;
            }
        }

        public int SpanEnd
        {
            get
            {
                return _wrappedSyntaxNode.Span.End;
            }
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

            var node = _wrappedSyntaxNode;

            foreach (var tokenToWrap in tokensToWrap)
                (Children as List<ISyntaxViewModel>).Add(new SyntaxTokenViewModel(tokenToWrap));
        }

        #endregion
    }
}