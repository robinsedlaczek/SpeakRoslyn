using System.Windows;
using System.Windows.Controls;
using WaveDev.SyntaxVisualizer.Commands;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Views
{
    /// <summary>
    /// Interaction logic for CommandWindow.xaml
    /// </summary>
    public partial class CommandWindow : Window
    {
        public CommandWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel.Instance;
        }

        private void OnCommandsComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var command = (ISyntaxCommand)CommandsComboBox.SelectedItem;
            var result = command.Execute();

            var viewModel = (MainViewModel)DataContext;
            viewModel.SyntaxCommandResults = result;
        }

        private void OnCommandResultListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var syntaxViewModel = CommandResultListView.SelectedItem as ISyntaxViewModel;
            (DataContext as MainViewModel).SelectedSourceSyntax = syntaxViewModel;
        }
    }
}
