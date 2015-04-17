using Infragistics.Documents;
using Infragistics.Documents.Parsing;
using System.ComponentModel;
using System.IO;
using System.Text;
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
        private SyntaxTreeWindow _syntaxTreeWindow;

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

            _syntaxTreeWindow = new SyntaxTreeWindow();
            _syntaxTreeWindow.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_syntaxTreeWindow != null)
            {
                _syntaxTreeWindow.Close();
                _syntaxTreeWindow = null;
            }

            MainViewModel.Instance.PropertyChanged -= OnMainViewModelPropertyChanged;
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

    }
}
