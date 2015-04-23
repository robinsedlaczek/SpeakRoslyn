using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    public interface ISyntaxCommand
    {
        void Init(SyntaxTree syntaxTree);

        string Name
        {
            get;
        }

        IEnumerable<ISyntaxViewModel> Execute();
    }
}
