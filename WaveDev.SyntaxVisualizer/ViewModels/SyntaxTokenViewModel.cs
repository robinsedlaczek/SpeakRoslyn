using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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

            DisplayName = "[Token] " + Kind;
        }

        #endregion

        #region Public Members

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

        #endregion

        #region Private Methods


        #endregion
    }
}