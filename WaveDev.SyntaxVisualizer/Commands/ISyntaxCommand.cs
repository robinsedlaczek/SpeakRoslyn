using Microsoft.CodeAnalysis;

namespace WaveDev.SyntaxVisualizer.Commands
{
    public interface ISyntaxCommand
    {
        void Init(SyntaxTree syntaxTree);

        string Name
        {
            get;
        }

        void Execute();
    }
}
