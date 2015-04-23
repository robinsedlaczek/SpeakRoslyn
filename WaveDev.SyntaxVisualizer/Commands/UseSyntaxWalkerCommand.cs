using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(ISyntaxCommand))]
    public class UseSyntaxWalkerCommand : ISyntaxCommand
    {
        private SyntaxTree _syntaxTree;

        public void Init(SyntaxTree syntaxTree)
        {
            _syntaxTree = syntaxTree;
        }

        public string Name
        {
            get
            {
                return "Use Syntax Walker";
            }
        }

        public IEnumerable<ISyntaxViewModel> Execute()
        {


            

            return null;

        }
    }
}
