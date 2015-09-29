using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using WaveDev.SpeakRoslyn.ViewModels;

namespace WaveDev.SpeakRoslyn.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindKeywordsSyntaxCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Keywords";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var keywords = from token in SyntaxTree.GetRoot().DescendantTokens()
                           where token.Kind().ToString().Contains("Keyword")
                           select token;

            var viewModels = new List<SyntaxTokenViewModel>();

            foreach (var keywordToken in keywords)
                viewModels.Add(new SyntaxTokenViewModel(keywordToken));

            return viewModels;
        }
    }
}
