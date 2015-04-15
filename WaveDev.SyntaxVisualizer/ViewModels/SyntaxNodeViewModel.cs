﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslyn.Utilities;
using System.Collections.Generic;
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

        public SyntaxNodeViewModel(SyntaxNode node)
        {
            var compilationUnitSyntax = node as CompilationUnitSyntax;
            var nsDeclSyntax = node as NamespaceDeclarationSyntax;
            var clsDeclSyntax = node as ClassDeclarationSyntax;
            var methDeclSyntax = node as MethodDeclarationSyntax;


            _wrappedSyntaxNode = node;

            Color = Brushes.Blue;
            DisplayName = Kind + " [" + _wrappedSyntaxNode.Span.Start + ".." + _wrappedSyntaxNode.Span.End + "]";

            Children = new List<ISyntaxViewModel>();

            WrapChildSyntaxNodes();
            WrapLeadingSyntaxTrivias();
            WrapChildSyntaxTokens();
            WrapTrailingSyntaxTrivias();

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