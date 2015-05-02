using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CommandWindow _commandWindow;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel.Instance;
            MainViewModel.Instance.PropertyChanged += OnMainViewModelPropertyChanged;

            _commandWindow = new CommandWindow();
            _commandWindow.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            MainViewModel.Instance.PropertyChanged -= OnMainViewModelPropertyChanged;

            if (_commandWindow != null)
            {
                _commandWindow.Close();
                _commandWindow = null;
            }
        }

        private void OnMainViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.SelectedSourceSyntax))
            {
                var syntax = (DataContext as MainViewModel).SelectedSourceSyntax;

                if (syntax == null)
                {
                    SourceCodeTextBox.Selection.Select(SourceCodeTextBox.Document.ContentStart.GetPositionAtOffset(0), SourceCodeTextBox.Document.ContentStart.GetPositionAtOffset(0));
                }
                else
                {
                    var start = SourceCodeTextBox.Document.ContentStart.GetPositionAtOffset(syntax.SpanStart + 2);
                    var end = SourceCodeTextBox.Document.ContentStart.GetPositionAtOffset(syntax.SpanEnd + 2);

                    SourceCodeTextBox.Selection.Select(start, end);
                    SourceCodeTextBox.SelectionBrush = Brushes.OrangeRed;
                }
            }
            else if (e.PropertyName == nameof(MainViewModel.SourceCode))
            {
                var paragraph = new Paragraph();
                var document = new FlowDocument(paragraph);

                paragraph.Inlines.Add(new Run(MainViewModel.Instance.SourceCode));

                SourceCodeTextBox.Document = document;
                SourceCodeTextBox.IsInactiveSelectionHighlightEnabled = true;

                SourceCodeTextBox.SelectAll();
            }
        }

        private void OnTreeViewSelectedItemChanged<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            // [RS] Use ListCollectionView for managing selected items etc. rather than doing it 
            //      in the code behind!

            (DataContext as MainViewModel).SelectedSourceSyntax = e.NewValue as ISyntaxViewModel;
        }

        private void OnOpenFileButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                DefaultExt = ".cs",
                Filter = "C# Code Files (.cs)|*.cs"
            };

            var result = dialog.ShowDialog();

            if (result == true)
            {
                var filename = dialog.FileName;
                var code = File.ReadAllText(filename);
                var viewModel = DataContext as MainViewModel;

                viewModel.SourceCode = code;
            }
        }
    }
}
