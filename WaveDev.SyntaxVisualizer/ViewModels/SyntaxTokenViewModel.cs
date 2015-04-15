using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Roslyn.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public class SyntaxTokenViewModel : ISyntaxViewModel
    {
        #region Private Fields

        private SyntaxToken _wrappedSyntaxToken;

        #endregion

        #region Construction

        public SyntaxTokenViewModel(SyntaxToken token)
        {
            _wrappedSyntaxToken = token;

            Color = Brushes.Green;
            DisplayName = Kind + " [" + _wrappedSyntaxToken.Span.Start + ".." + _wrappedSyntaxToken.Span.End + "]";

            Children = new List<ISyntaxViewModel>();

            WrapLeadingSyntaxTrivias();
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

        public Brush Color
        {
            get;
            private set;
        }

        public string Kind
        {
            get
            {
                return _wrappedSyntaxToken.Kind().ToString();
            }
        }

        public string DisplayName
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
                return _wrappedSyntaxToken.Span.Start;
            }
        }

        public int SpanEnd
        {
            get
            {
                return _wrappedSyntaxToken.Span.End;
            }
        }

        #endregion

        #region Private Methods

        private void WrapLeadingSyntaxTrivias()
        {
            var triviasToWrap = _wrappedSyntaxToken.LeadingTrivia;

            foreach (var triviaToWrap in triviasToWrap)
                (Children as List<ISyntaxViewModel>).Add(new SyntaxTriviaViewModel(triviaToWrap));
        }

        private void WrapTrailingSyntaxTrivias()
        {
            var triviasToWrap = _wrappedSyntaxToken.TrailingTrivia;

            foreach (var triviaToWrap in triviasToWrap)
                (Children as List<ISyntaxViewModel>).Add(new SyntaxTriviaViewModel(triviaToWrap));
        }

        #endregion
    }
}