using System.Windows;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnTreeViewSelectedItemChanged<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            var selectedSyntaxModel = (DataContext as MainViewModel).SelectedSourceSyntax = e.NewValue as ISyntaxViewModel;

            SourceCodeTextBox.SelectionStart = selectedSyntaxModel.SpanStart;
            SourceCodeTextBox.SelectionLength = selectedSyntaxModel.SpanEnd - selectedSyntaxModel.SpanStart;
        }

        private void OnSourceCodeTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            // [RS] When the TextBox loses focus the user can no longer see the selection.
            //      This is a hack to make the TextBox think it did not lose focus.
            e.Handled = true;
        }
    }
}
