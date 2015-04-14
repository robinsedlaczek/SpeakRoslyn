using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
            DisplayName = Kind + " [" + _wrappedSyntaxToken.Span.Start + ".." + _wrappedSyntaxToken.Span.End + "]"
                               + " [" + _wrappedSyntaxToken.FullSpan.Start + ".." + _wrappedSyntaxToken.FullSpan.End + "]";
        }

        #endregion

        #region Public Members

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


        #endregion
    }
}