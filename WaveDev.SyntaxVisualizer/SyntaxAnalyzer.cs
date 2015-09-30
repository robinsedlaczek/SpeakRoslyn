using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using System.Threading;

namespace WaveDev.SpeakRoslyn
{
    public class SyntaxAnalyzer
    {
        #region Public Methods

        public SyntaxTree Go(string sourceCode)
        {
            var parseOptions = CSharpParseOptions.Default.WithPreprocessorSymbols("DEBUG");
            var path = string.Empty;
            var encoding = null as Encoding;
            var cancellationToken = default(CancellationToken);

                var tree = CSharpSyntaxTree.ParseText(sourceCode, parseOptions, path, encoding, cancellationToken);

            return tree;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
