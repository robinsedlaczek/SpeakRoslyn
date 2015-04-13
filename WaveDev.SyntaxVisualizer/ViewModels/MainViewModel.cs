using Microsoft.CodeAnalysis;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public class MainViewModel
    {
        #region Private Fields

        private SyntaxTree _syntaxTree;

        #endregion

        #region Construction

        public MainViewModel()
        {
            // TODO: [RS] Syntax analysis in ctor is not a good idea. Further, the analyze process should be async. 
            var analyzer = new SyntaxAnalyzer();
            _syntaxTree = analyzer.Go();
        }

        #endregion

        #region Public Members

        public ISyntaxViewModel Syntax
        {
            get
            {
                var node = _syntaxTree.GetRoot();
                var model = new SyntaxNodeViewModel(node);

                return model;
            }
        }

        #endregion

    }
}
