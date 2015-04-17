using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel;
using WaveDev.SyntaxVisualizer.Commands;
using System;

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

            InitSyntaxCommands();
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
