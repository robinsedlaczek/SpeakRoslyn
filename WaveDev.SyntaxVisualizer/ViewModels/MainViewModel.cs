using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using WaveDev.SyntaxVisualizer.Commands;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private SyntaxTree _sourceSyntaxTree;
        private ISyntaxViewModel _selectedSourceSyntax;
        private static MainViewModel s_instance;
        private IEnumerable<ISyntaxViewModel> _syntaxCommandResults;
        private IEnumerable<SyntaxCommand> _syntaxCommands;

        #endregion

        #region Construction

        private MainViewModel()
        {
            SourceCode =
@"/// <summary>
/// This method performs some magic.
/// </summary>
/// <param name=\""what\"">Something is needed to do magic things.</param>
public void Do(string what)
{
    var so = true;

#if DEBUG
    so = false;
#endif

    if (so == what)
        DontDo();
}

public int Foo()
{

}

public string Bar()
{

}
";

        // TODO: [RS] Syntax analysis in ctor is not a good idea. Further, the analyze process should be async. 
        var analyzer = new SyntaxAnalyzer();
            _sourceSyntaxTree = analyzer.Go(SourceCode);

            // [RS] Create syntax node view models recursively and then put the first syntax node view model into the root
            //      syntax node view model. This is because the view is bound to the SourceSyntax property and the tree items
            //      is are bound to the Children property of a syntax node view model. So we need to add the root element explicitely.
            var node = _sourceSyntaxTree.GetRoot();
            var syntaxNodeViewModel = new SyntaxNodeViewModel(node);
            var rootSyntaxNodeViewModel = new SyntaxNodeViewModel(syntaxNodeViewModel);

            SourceSyntax = rootSyntaxNodeViewModel;

            ComposeApplicationParts();


            // TODO: [RS] Remove...
            var values = new List<SyntaxKind>();
            foreach (var value in Enum.GetValues(typeof(SyntaxKind)))
            {
                if (value.ToString().Contains("Documentation"))
                    values.Add((SyntaxKind)value);
            }

            int a = 1;
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

        [ImportMany(typeof(SyntaxCommand))]
        public IEnumerable<SyntaxCommand> SyntaxCommands
        {
            get
            {
                return _syntaxCommands;
            }

            private set
            {
                _syntaxCommands = value;

                foreach (var command in _syntaxCommands)
                    command.Init(_sourceSyntaxTree);
            }
        }

        public IEnumerable<ISyntaxViewModel> SyntaxCommandResults
        {
            get
            {
                return _syntaxCommandResults;
            }

            set
            {
                _syntaxCommandResults = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SyntaxCommandResults)));
            }
        }

        #endregion

        #region Private Members

        private void ComposeApplicationParts()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        #endregion

    }
}
