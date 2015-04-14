using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Text;
using System.Threading;

namespace WaveDev.SyntaxVisualizer
{
    public class SyntaxAnalyzer
    {
        #region Public Methods

        public SyntaxTree Go(string sourceCode)
        {
            var parseOptions = CSharpParseOptions.Default;
            var path = string.Empty;
            var encoding = null as Encoding;
            var cancellationToken = default(CancellationToken);

            var tree = CSharpSyntaxTree.ParseText(sourceCode, parseOptions, path, encoding, cancellationToken);
            var root = tree.GetRoot();

            VisitNode(root);

            return tree;
        }

        #endregion

        #region Private Methods

        private void VisitNode(SyntaxNode node, int level = 0)
        {
            var tabs = string.Empty;

            for (var index = 0; index < level; index++)
                tabs += "\t";

            Console.WriteLine(tabs + "Node: " + node.Kind() + " - " + node.ToString().Substring(0, Math.Min(node.ToString().Length, 15)));

            level++;
            tabs += "\t";

            var childsNodes = node.ChildNodes();

            foreach (var child in childsNodes)
            {
                Console.WriteLine(tabs + "Node: " + child.Kind() + " - " + child.ToString().Substring(0, Math.Min(child.ToString().Length, 15)));

                VisitNode(child, level);
            }

            var childTokens = node.ChildTokens();

            foreach (var child in childTokens)
            {
                Console.WriteLine(tabs + "Token: " + child.Kind() + " - " + child.ToString().Substring(0, Math.Min(child.ToString().Length, 15)));
            }
        }

        #endregion
    }
}
