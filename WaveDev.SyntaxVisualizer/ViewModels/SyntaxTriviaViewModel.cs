using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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

            DisplayName = "[Trivia] " + Kind;
        }

        #endregion

        #region Public Members

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

        #endregion

        #region Private Methods


        #endregion
    }
}