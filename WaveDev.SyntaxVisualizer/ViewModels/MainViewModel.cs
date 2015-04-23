using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #endregion

        #region Construction

        private MainViewModel()
        {
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

            // [RS] Create syntax node view models recursively and then put the first syntax node view model into the root
            //      syntax node view model. This is because the view is bound to the SourceSyntax property and the tree items
            //      is are bound to the Children property of a syntax node view model. So we need to add the root element explicitely.
            var node = _sourceSyntaxTree.GetRoot();
            var syntaxNodeViewModel = new SyntaxNodeViewModel(node);
            var rootSyntaxNodeViewModel = new SyntaxNodeViewModel(syntaxNodeViewModel);

            SourceSyntax = rootSyntaxNodeViewModel;

            InitSyntaxCommands();

            var values = Enum.GetValues(typeof(SyntaxKind));
            var foundValues = new List<object>();

            foreach (var value in values)
            {
                if (value.ToString().Contains("Bitwise") && value.ToString().Contains("Expression"))
                    foundValues.Add(value.ToString());
            }

            var a = 5;
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

        public IEnumerable<ISyntaxCommand> SyntaxCommands
        {
            get;
            private set;
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

        private void InitSyntaxCommands()
        {
            // TODO: [RS] Decouple with MEF!
            var commands = new List<ISyntaxCommand>();

            ISyntaxCommand command = new FindKeywordsSyntaxCommand();
            command.Init(_sourceSyntaxTree);
            commands.Add(command);

            command = new FindNodesSyntaxCommand();
            command.Init(_sourceSyntaxTree);
            commands.Add(command);

            command = new FindTokensSyntaxCommand();
            command.Init(_sourceSyntaxTree);
            commands.Add(command);

            command = new FindTriviasSyntaxCommand();
            command.Init(_sourceSyntaxTree);
            commands.Add(command);

            SyntaxCommands = commands;
        }

        #endregion

    }
}
