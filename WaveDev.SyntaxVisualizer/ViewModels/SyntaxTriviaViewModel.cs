using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Windows.Media;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public class SyntaxTriviaViewModel : ISyntaxViewModel
    {
        #region Private Fields

        private SyntaxTrivia _wrappedSyntaxTrivia;

        #endregion

        #region Construction

        public SyntaxTriviaViewModel(SyntaxTrivia trivia)
        {
            _wrappedSyntaxTrivia = trivia;

            Color = Brushes.Red;
            DisplayName = Kind + " [" + _wrappedSyntaxTrivia.Span.Start + ".." + _wrappedSyntaxTrivia.Span.End + "]";
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
                return _wrappedSyntaxTrivia.Kind().ToString();
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
                return _wrappedSyntaxTrivia.Span.Start;
            }
        }

        public int SpanEnd
        {
            get
            {
                return _wrappedSyntaxTrivia.Span.End;
            }
        }

        #endregion

        #region Private Methods


        #endregion
    }
}