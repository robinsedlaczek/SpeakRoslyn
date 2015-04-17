using System.Windows;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Views
{
    /// <summary>
    /// Interaction logic for SyntaxTreeWindow.xaml
    /// </summary>
    public partial class SyntaxTreeWindow : Window
    {
        public SyntaxTreeWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel.Instance;
        }

        private void OnTreeViewSelectedItemChanged<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            var selectedSyntaxModel = (DataContext as MainViewModel).SelectedSourceSyntax = e.NewValue as ISyntaxViewModel;
        }
    }
}
