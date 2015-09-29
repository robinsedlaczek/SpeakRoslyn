using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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
            
            // [Feedback] Why using a string as language parameter and not the LanguageNames enum?
            var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Default, "WaveDev.Project.A", "WaveDev.Project", LanguageNames.CSharp, projectFilePath);

            // [Feedback] DocumentId is created relative to ProjectId. 
            //            Why is the DocumentInfo not assigned to the ProjectInfo? 
            //            What are the differences between xId and xInfo types?
            //            What does the relation between DocumentId and ProjectId mean? => There is no such relation between ProjectId and SolutionId.
            var documentInfo = DocumentInfo.Create(DocumentId.CreateNewId(projectInfo.Id), "CodeFile_A.cs");
            documentInfo.WithTextLoader(TextLoader.From(TextAndVersion.Create(sourceText, VersionStamp.Default)));
            //            I have to add the documents to the project manually.
            projectInfo = projectInfo.WithDocuments(new List<DocumentInfo> { documentInfo });

            var solutionFilePath = null as string;
            var solutionInfo = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default, solutionFilePath, new List<ProjectInfo> { projectInfo });

            // var host = new MyHostServices()
            var workspace = new AdhocWorkspace();
            workspace.AddSolution(solutionInfo);


            workspace.OpenDocument(documentInfo.Id);
            var syntaxTree = workspace.CurrentSolution.Projects.First().Documents.First().GetSyntaxTreeAsync().Result;


            return new List<SyntaxTokenViewModel>();
        }
    }
}
