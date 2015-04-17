using System.ComponentModel;
using System.Windows;
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

            var paragraph = new Paragraph();
            var document = new FlowDocument(paragraph);

            paragraph.Inlines.Add(new Run(MainViewModel.Instance.SourceCode));

            SourceCodeTextBox.Document = document;
            SourceCodeTextBox.IsInactiveSelectionHighlightEnabled = true;

            _commandWindow = new CommandWindow();
            _commandWindow.DataContext = MainViewModel.Instance;
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

                var start = SourceCodeTextBox.Document.ContentStart.GetPositionAtOffset(syntax.SpanStart + 2);
                var end = SourceCodeTextBox.Document.ContentStart.GetPositionAtOffset(syntax.SpanEnd + 2);

                SourceCodeTextBox.Selection.Select(start, end);
                SourceCodeTextBox.SelectionBrush = Brushes.OrangeRed;

                //var textRange = new TextRange(SourceCodeTextBox.Document.ContentStart, SourceCodeTextBox.Document.ContentEnd);
                //textRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);

                //textRange = new TextRange(start, end);
                //textRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            }
        }
        private void OnTreeViewSelectedItemChanged<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            var selectedSyntaxModel = (DataContext as MainViewModel).SelectedSourceSyntax = e.NewValue as ISyntaxViewModel;
        }

    }
}
