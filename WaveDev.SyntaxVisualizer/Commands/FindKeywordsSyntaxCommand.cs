using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using WaveDev.SyntaxVisualizer.ViewModels;

namespace WaveDev.SyntaxVisualizer.Commands
{
    [Export(typeof(ISyntaxCommand))]
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
                return "Keywords";
            }
        }

        public IEnumerable<ISyntaxViewModel> Execute()
        {
            var keywords = from token in _syntaxTree.GetRoot().DescendantTokens()
                           where token.Kind().ToString().Contains("Keyword")
                           select token;

            var viewModels = new List<SyntaxTokenViewModel>();

            foreach (var keywordToken in keywords)
                viewModels.Add(new SyntaxTokenViewModel(keywordToken));

            return viewModels;
        }
    }
}
