using Microsoft.CodeAnalysis;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public class MainViewModel
    {
        #region Private Fields

        private SyntaxTree _sourceSyntaxTree;

        #endregion

        #region Construction

        public MainViewModel()
        {
            SourceCode = 
@"namespace MyNamespace.SubNamespace
{
    public class Program
    {
        static void Main(string[] args)
        {
            var index = 1;
        }
    }
}";

            // TODO: [RS] Syntax analysis in ctor is not a good idea. Further, the analyze process should be async. 
            var analyzer = new SyntaxAnalyzer();
            _sourceSyntaxTree = analyzer.Go(SourceCode);

            var node = _sourceSyntaxTree.GetRoot();
            SourceSyntax = new SyntaxNodeViewModel(node);
        }

        #endregion

        #region Public Members

        public string SourceCode
        {
            get;
            set;
        }

        public ISyntaxViewModel SourceSyntax
        {
            get;
        }

        public ISyntaxViewModel SelectedSourceSyntax
        {
            get;
            set;
        }

        #endregion


    }
}
