using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WaveDev.SpeakRoslyn.ViewModels;

namespace WaveDev.SpeakRoslyn.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class FindTriviasSyntaxCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Trivia";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var rootNode = SyntaxTree.GetRoot();
            var trivias = rootNode.DescendantTrivia();

            return WrapResult(trivias);
        }
    }
}
