using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        private string _sourceCode;
        private ISyntaxViewModel _sourceSyntax;

        #endregion

        #region Construction

        private MainViewModel()
        {
            ComposeApplicationParts();
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
            get
            {
                return _sourceCode;
            }

            set
            {
                _sourceCode = value;

                var analyzer = new SyntaxAnalyzer();
                _sourceSyntaxTree = analyzer.Go(_sourceCode);

                foreach (var command in _syntaxCommands)
                    command.Init(_sourceSyntaxTree);

                // [RS] Create syntax node view models recursively and then put the first syntax node view model into the root
                //      syntax node view model. This is because the view is bound to the SourceSyntax property and the tree items
                //      is are bound to the Children property of a syntax node view model. So we need to add the root element explicitely.
                var node = _sourceSyntaxTree.GetRoot();
                var syntaxNodeViewModel = new SyntaxNodeViewModel(node);
                var rootSyntaxNodeViewModel = new SyntaxNodeViewModel(syntaxNodeViewModel);

                SelectedSourceSyntax = null;
                SyntaxCommandResults = new List<ISyntaxViewModel>();
                SourceSyntax = rootSyntaxNodeViewModel;

                NotifyPropertyChanged();
            }
        }

        public ISyntaxViewModel SourceSyntax
        {
            get
            {
                return _sourceSyntax;
            }

            private set
            {
                _sourceSyntax = value;

                NotifyPropertyChanged();
            }
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

                NotifyPropertyChanged();
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

                NotifyPropertyChanged();
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

                NotifyPropertyChanged();
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

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
