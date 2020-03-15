using DulcisX.Core.Extensions;
using DulcisX.Core.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using DulcisX.Core;

namespace DulcisX.Nodes.Events
{
    /// <summary>
    /// Currently not being used
    /// </summary>
    internal class ProjectNodeChangeEvents : NodeEventSink, IProjectNodeChangeEvents, IVsTrackProjectDocumentsEvents2, IVsTrackProjectDocumentsEvents4
    {
        public event Action<DocumentNode> OnDocumentAdded;
        public event Action<IEnumerable<DocumentNode>> OnBulkDocumentsAdded;

        public event Action<FolderNode> OnFolderAdded;
        public event Action<IEnumerable<FolderNode>> OnBulkFoldersAdded;

        public event Action<DocumentNode> OnDocumentRemoved;
        public event Action<IEnumerable<DocumentNode>> OnBulkDocumentsRemoved;

        public event Action<FolderNode> OnFolderRemoved;
        public event Action<IEnumerable<FolderNode>> OnBulkFoldersRemoved;

        public event Action<DocumentNode> OnDocumentRenamed;
        public event Action<IEnumerable<DocumentNode>> OnBulkDocumentsRenamed;

        public event Action<FolderNode> OnFolderRenamed;
        public event Action<IEnumerable<FolderNode>> OnBulkFoldersRenamed;

        public event Action<DocumentNode> OnDocumentSccStatusChanged;
        public event Action<IEnumerable<DocumentNode>> OnBulkDocumentSccStatusChanged;

        private readonly IVsTrackProjectDocuments2 _trackProjectDocuments;

        public ProjectNodeChangeEvents(SolutionNode solution, IVsTrackProjectDocuments2 trackProjectDocuments) : base(solution)
        {
            _trackProjectDocuments = trackProjectDocuments;
        }

        public int OnAfterAddFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDFILEFLAGS[] rgFlags)
        {
            return CommonStatusCodes.Success;
        }

        public int OnAfterAddDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDDIRECTORYFLAGS[] rgFlags)
        {
            return CommonStatusCodes.Success;
        }

        public int OnAfterRemoveFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEFILEFLAGS[] rgFlags)
        {
            return CommonStatusCodes.Success;
        }

        public int OnAfterRemoveDirectories(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEDIRECTORYFLAGS[] rgFlags)
        {
            return CommonStatusCodes.Success;
        }

        public int OnAfterRenameFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEFILEFLAGS[] rgFlags)
        {
            return CommonStatusCodes.Success;
        }

        public int OnAfterRenameDirectories(int cProjects, int cDirs, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEDIRECTORYFLAGS[] rgFlags)
        {
            return CommonStatusCodes.Success;
        }

        public int OnAfterSccStatusChanged(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgdwSccStatus)
        {
            return CommonStatusCodes.Success;
        }

        public int OnQueryAddFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, VSQUERYADDFILEFLAGS[] rgFlags, VSQUERYADDFILERESULTS[] pSummaryResult, VSQUERYADDFILERESULTS[] rgResults)
            => CommonStatusCodes.Success;

        public int OnQueryRenameFiles(IVsProject pProject, int cFiles, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEFILEFLAGS[] rgFlags, VSQUERYRENAMEFILERESULTS[] pSummaryResult, VSQUERYRENAMEFILERESULTS[] rgResults)
            => CommonStatusCodes.Success;

        public int OnQueryRenameDirectories(IVsProject pProject, int cDirs, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEDIRECTORYFLAGS[] rgFlags, VSQUERYRENAMEDIRECTORYRESULTS[] pSummaryResult, VSQUERYRENAMEDIRECTORYRESULTS[] rgResults)
            => CommonStatusCodes.Success;

        public int OnQueryAddDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, VSQUERYADDDIRECTORYFLAGS[] rgFlags, VSQUERYADDDIRECTORYRESULTS[] pSummaryResult, VSQUERYADDDIRECTORYRESULTS[] rgResults)
            => CommonStatusCodes.Success;

        public int OnQueryRemoveFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, VSQUERYREMOVEFILEFLAGS[] rgFlags, VSQUERYREMOVEFILERESULTS[] pSummaryResult, VSQUERYREMOVEFILERESULTS[] rgResults)
            => CommonStatusCodes.Success;

        public int OnQueryRemoveDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, VSQUERYREMOVEDIRECTORYFLAGS[] rgFlags, VSQUERYREMOVEDIRECTORYRESULTS[] pSummaryResult, VSQUERYREMOVEDIRECTORYRESULTS[] rgResults)
            => CommonStatusCodes.Success;

        internal static IProjectNodeChangeEvents Create(SolutionNode solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var projectDocuments = solution.ServiceContainer.GetCOMInstance<IVsTrackProjectDocuments2>();

            var nodeChangeEvents = new ProjectNodeChangeEvents(solution, projectDocuments);

            var result = projectDocuments.AdviseTrackProjectDocumentsEvents(nodeChangeEvents, out var cookieUID);

            ErrorHandler.ThrowOnFailure(result);

            nodeChangeEvents.SetCookie(cookieUID);

            return nodeChangeEvents;
        }

        public override void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _trackProjectDocuments.UnadviseTrackProjectDocumentsEvents(Cookie);

            ErrorHandler.ThrowOnFailure(result);
        }

        public void OnQueryRemoveFilesEx(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, uint[] rgFlags, VSQUERYREMOVEFILERESULTS[] pSummaryResult, VSQUERYREMOVEFILERESULTS[] rgResults)
        {

        }

        public void OnQueryRemoveDirectoriesEx(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, uint[] rgFlags, VSQUERYREMOVEDIRECTORYRESULTS[] pSummaryResult, VSQUERYREMOVEDIRECTORYRESULTS[] rgResults)
        {

        }

        public void OnAfterRemoveFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgFlags)
        {

        }

        public void OnAfterRemoveDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgFlags)
        {

        }
    }
}
