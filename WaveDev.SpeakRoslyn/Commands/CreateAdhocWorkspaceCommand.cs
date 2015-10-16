using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

            DocumentInfo documentInfo;
            SolutionInfo solutionInfo;
            projectInfo = MyMethod(sourceText, projectInfo, out documentInfo, out solutionInfo);


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

        private static ProjectInfo MyMethod(Microsoft.CodeAnalysis.Text.SourceText sourceText, ProjectInfo projectInfo, out DocumentInfo documentInfo, out SolutionInfo solutionInfo)
        {
            documentInfo = DocumentInfo.Create(DocumentId.CreateNewId(projectInfo.Id), "CodeFile_A.cs");
            documentInfo.WithTextLoader(TextLoader.From(TextAndVersion.Create(sourceText, VersionStamp.Default)));

            projectInfo = projectInfo.WithDocuments(new List<DocumentInfo> { documentInfo });

            var solutionFilePath = null as string;
            solutionInfo = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default, solutionFilePath, new List<ProjectInfo> { projectInfo });
            return projectInfo;
        }
    }
}
