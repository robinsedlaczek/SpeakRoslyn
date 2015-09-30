using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using WaveDev.SpeakRoslyn.ViewModels;

namespace WaveDev.SpeakRoslyn.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindXmlLiteralTokenSyntaxCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "XmlLiteralTokens";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var xmlLiteralTokens = from token in SyntaxTree.GetRoot().DescendantTokens(descendIntoTrivia: true)
                                   where token.Kind() == SyntaxKind.XmlTextLiteralToken
                                   select token;

            var viewModels = new List<SyntaxTokenViewModel>();

            foreach (var token in xmlLiteralTokens)
                viewModels.Add(new SyntaxTokenViewModel(token));

            return viewModels;
        }
    }
}
