using System;
using System.Windows;
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

            SourceCodeTextEditor.Text = MainViewModel.Instance.SourceCode;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            MainViewModel.Instance.PropertyChanged += OnMainViewModelPropertyChanged;

            if (_syntaxTreeWindow == null)
            {
                _syntaxTreeWindow = new SyntaxTreeWindow();
                _syntaxTreeWindow.Show();
            }
        }

        private void OnMainViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.SelectedSourceSyntax))
            {
                var syntax = (DataContext as MainViewModel).SelectedSourceSyntax;

                SourceCodeTextBox.SelectionStart = syntax.SpanStart;
                SourceCodeTextBox.SelectionLength = syntax.SpanEnd - syntax.SpanStart;
            }
        }

        private void OnSourceCodeTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            // [RS] When the TextBox loses focus the user can no longer see the selection.
            //      This is a hack to make the TextBox think it did not lose focus.
            e.Handled = true;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
