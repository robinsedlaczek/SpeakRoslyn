using System.Windows.Media;

namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public interface ISyntaxViewModel
    {
        #region Public Members

        Brush Color
        {
            get;
        }

        string Kind
        {
            get;
        }

        string DisplayName
        {
            get;
        }

        ISyntaxViewModel SelectedSourceSyntax
        {
            get;
        }

        int SpanStart
        {
            get;
        }

        int SpanEnd
        {
            get;
        }

        #endregion

    }
}