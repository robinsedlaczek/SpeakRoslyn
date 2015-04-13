namespace WaveDev.SyntaxVisualizer.ViewModels
{
    public interface ISyntaxViewModel
    {
        #region Public Members

        string Kind
        {
            get;
        }

        string DisplayName
        {
            get;
        }

        #endregion

    }
}