using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace WaveDev.SyntaxVisualizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            MatchSyntaxKindToType();
        }

        private void MatchSyntaxKindToType()
        {
            var enumValues = Enum.GetValues(typeof(SyntaxKind)).Cast<SyntaxKind>();

            var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(assembly => new { assembly.FullName, assembly.Name });
            var assemblyName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(assembly => assembly.Name == "Microsoft.CodeAnalysis.CSharp").FirstOrDefault();
            var types = Assembly.Load(assemblyName.FullName).GetTypes().Where(type => type.Namespace == "Microsoft.CodeAnalysis.CSharp.Syntax").Select(type => type.Name).ToList();

            var matches = new List<Tuple<string, string>>();

            foreach (var enumValue in enumValues)
            {
                var foundTypes = types.Where(type => type.Contains(enumValue.ToString() + "Syntax"));
                var typeDisplayString = string.Empty;

                foreach (var foundType in foundTypes)
                    typeDisplayString += foundType + " ";

                matches.Add(Tuple.Create<string, string>(enumValue.ToString(), typeDisplayString));

                //types.Remove(foundTypes);
            }

            foreach (var type in types)
                matches.Add(Tuple.Create<string, string>(string.Empty, type));

            var matrix = string.Empty;
            foreach (var match in matches)
                matrix += match.ToString() + Environment.NewLine;
        }
    }
}
