using Microsoft.CodeAnalysis;
using System.ComponentModel;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private SyntaxTree _sourceSyntaxTree;
        private ISyntaxViewModel _selectedSourceSyntax;
        private static MainViewModel s_instance;

        #endregion

        #region Construction

        private MainViewModel()
        {
//            SourceCode =
//@"namespace MyNamespace.SubNamespace
//{
//    public class Program
//    {
//        static void Main(string[] args)
//        {
//            var index = 1;

//#if DEBUG
//            index = 10;
//#endif
//        }
//    }
//}";

            SourceCode =
@"public void Do(string what)
{
    var so = true;

#if DEBUG
    so = false;
#endif

    if (so == what)
        DontDo();
}";

            // TODO: [RS] Syntax analysis in ctor is not a good idea. Further, the analyze process should be async. 
            var analyzer = new SyntaxAnalyzer();
            _sourceSyntaxTree = analyzer.Go(SourceCode);

            var node = _sourceSyntaxTree.GetRoot();
            SourceSyntax = new SyntaxNodeViewModel(node);
        }

        #endregion

        #region Public Members

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public static MainViewModel Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new MainViewModel();

                return s_instance;
            }
        }

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
            get
            {
                return _selectedSourceSyntax;
            }

            set
            {
                _selectedSourceSyntax = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedSourceSyntax)));
            }
        }

        #endregion


    }
}
