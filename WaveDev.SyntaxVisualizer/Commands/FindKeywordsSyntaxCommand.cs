using Microsoft.CodeAnalysis;
using System;

namespace WaveDev.SyntaxVisualizer.Commands
{
    public class FindKeywordsSyntaxCommand : ISyntaxCommand
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
                return "Find Keywords";
            }
        }

        public void Execute()
        {


        }
    }
}
