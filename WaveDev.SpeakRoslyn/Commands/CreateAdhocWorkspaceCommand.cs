using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using WaveDev.SpeakRoslyn.ViewModels;

namespace WaveDev.SpeakRoslyn.Commands
{
    [Export(typeof(SyntaxCommand))]
    public class CreateAdhocWorkspaceCommand : SyntaxCommand
    {
        public override string Name
        {
            get
            {
                return "Create adhoc Workspace...";
            }
        }

        public override IEnumerable<ISyntaxViewModel> Execute()
        {
            var sourceText = SyntaxTree.GetTextAsync().Result;

            var projectFilePath = null as string;
            var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Default, "WaveDev.Project.A", "WaveDev.Project", LanguageNames.CSharp, projectFilePath);

            var documentInfo = DocumentInfo.Create(DocumentId.CreateNewId(projectInfo.Id), "CodeFile_A.cs");
            documentInfo.WithTextLoader(TextLoader.From(TextAndVersion.Create(sourceText, VersionStamp.Default)));

            projectInfo = projectInfo.WithDocuments(new List<DocumentInfo> { documentInfo });

            var solutionFilePath = null as string;
            var solutionInfo = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default, solutionFilePath, new List<ProjectInfo> { projectInfo });


            // [RS] Important to use the default assemblies (Microsoft.CodeAnalysis.Workspaces...), too. When creating the workspace, 
            //      it tries to get a IWorkspaceTaskSchedulerFactory service. There is an implementation in the default assemblies, so
            //      we do not need to implement one here.
            var assemblies = MefHostServices
                .DefaultAssemblies
                .Add(Assembly.GetExecutingAssembly());

            //var assemblies = new List<Assembly>() { Assembly.GetExecutingAssembly() };

            var host = MefHostServices.Create(assemblies);

            // Adhoc Workspace
            var adhocWorkspace = new AdhocWorkspace(host);
            adhocWorkspace.AddSolution(solutionInfo);
            adhocWorkspace.OpenDocument(documentInfo.Id);
            var syntaxTree = adhocWorkspace.CurrentSolution.Projects.First().Documents.First().GetSyntaxTreeAsync().Result;


            // MS Build Workspace
            var properties = new Dictionary<string, string>();
            var msBuildWorkspace = MSBuildWorkspace.Create(properties, host);
            var solution = msBuildWorkspace.OpenSolutionAsync(@"E:\GIT Repositories\SpeakRoslyn\WaveDev.SpeakRoslyn.sln").Result;



            return new List<SyntaxTokenViewModel>();
        }

    }
}
