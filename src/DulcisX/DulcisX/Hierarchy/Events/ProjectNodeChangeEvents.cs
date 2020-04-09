using DulcisX.Core.Enums;
using DulcisX.Core.Extensions;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Hierarchy.Events
{
    internal class ProjectNodeChangeEvents : NodeEventSink, IProjectNodeChangeEvents, IVsTrackProjectDocumentsEvents2, IVsTrackProjectDocumentsEvents4
    {
        public event Action<IEnumerable<AddedPhysicalNode<DocumentNode, VSADDFILEFLAGS>>> OnDocumentsAdded;

        public event Action<IEnumerable<AddedPhysicalNode<FolderNode, VSADDDIRECTORYFLAGS>>> OnFoldersAdded;

        public event Action<IEnumerable<RemovedPhysicalNode<PhysicalNodeRemovedFlags>>> OnDocumentsRemoved;

        public event Action<IEnumerable<RemovedPhysicalNode<PhysicalNodeRemovedFlags>>> OnFoldersRemoved;

        public event Action<IEnumerable<RenamedPhysicalNode<DocumentNode, VSRENAMEFILEFLAGS>>> OnDocumentsRenamed;

        public event Action<IEnumerable<RenamedPhysicalNode<FolderNode, VSRENAMEDIRECTORYFLAGS>>> OnFoldersRenamed;

        public event Action<IEnumerable<ChangedPhysicalSccNode<IPhysicalProjectItemNode, __SccStatus>>> OnDocumentSccStatusChanged;

        private readonly IVsTrackProjectDocuments2 _trackProjectDocuments;

        private ProjectNodeChangeEvents(SolutionNode solution, IVsTrackProjectDocuments2 trackProjectDocuments) : base(solution)
        {
            _trackProjectDocuments = trackProjectDocuments;
        }

        public int OnAfterAddFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDFILEFLAGS[] rgFlags)
        {
            OnDocumentsAdded?.Invoke(NodesChanged(rgpProjects, rgFirstIndices, rgpszMkDocuments.Length, (projectNode, iterator) =>
            {
                if (!projectNode.TryGetPhysicalNode<DocumentNode>(rgpszMkDocuments[iterator], out var document))
                {
                    throw new InvalidOperationException();
                }

                return new AddedPhysicalNode<DocumentNode, VSADDFILEFLAGS>(document, rgFlags[iterator]);
            }).ToCachingEnumerable());

            return CommonStatusCodes.Success;
        }

        public int OnAfterAddDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDDIRECTORYFLAGS[] rgFlags)
        {
            OnFoldersAdded?.Invoke(NodesChanged(rgpProjects, rgFirstIndices, rgpszMkDocuments.Length, (projectNode, iterator) =>
            {
                if (!projectNode.TryGetPhysicalNode<FolderNode>(rgpszMkDocuments[iterator], out var folder))
                {
                    throw new InvalidOperationException();
                }

                return new AddedPhysicalNode<FolderNode, VSADDDIRECTORYFLAGS>(folder, rgFlags[iterator]);
            }).ToCachingEnumerable());

            return CommonStatusCodes.Success;
        }

        public int OnAfterRenameFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEFILEFLAGS[] rgFlags)
        {
            OnDocumentsRenamed?.Invoke(NodesChanged(rgpProjects, rgFirstIndices, rgszMkNewNames.Length, (projectNode, iterator) =>
            {
                if (!projectNode.TryGetPhysicalNode<DocumentNode>(rgszMkNewNames[iterator], out var document))
                {
                    throw new InvalidOperationException();
                }

                return new RenamedPhysicalNode<DocumentNode, VSRENAMEFILEFLAGS>(document, rgszMkOldNames[iterator], rgszMkNewNames[iterator], rgFlags[iterator]);
            }).ToCachingEnumerable());

            return CommonStatusCodes.Success;
        }

        public int OnAfterRenameDirectories(int cProjects, int cDirs, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEDIRECTORYFLAGS[] rgFlags)
        {
            OnFoldersRenamed?.Invoke(NodesChanged(rgpProjects, rgFirstIndices, rgszMkNewNames.Length, (projectNode, iterator) =>
            {
                if (!projectNode.TryGetPhysicalNode<FolderNode>(rgszMkNewNames[iterator], out var folder))
                {
                    throw new InvalidOperationException();
                }

                return new RenamedPhysicalNode<FolderNode, VSRENAMEDIRECTORYFLAGS>(folder, rgszMkOldNames[iterator], rgszMkNewNames[iterator], rgFlags[iterator]);
            }).ToCachingEnumerable());

            return CommonStatusCodes.Success;
        }

        public void OnAfterRemoveFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgFlags)
        {
            OnDocumentsRemoved?.Invoke(NodesChanged(rgpProjects, rgFirstIndices, rgpszMkDocuments.Length, (projectNode, iterator) =>
            {
                return new RemovedPhysicalNode<PhysicalNodeRemovedFlags>(projectNode, rgpszMkDocuments[iterator], ((rgFlags[iterator] & 16u) != 0u) ? PhysicalNodeRemovedFlags.Removed : PhysicalNodeRemovedFlags.Deleted | PhysicalNodeRemovedFlags.Removed);
            }).ToCachingEnumerable());
        }

        public void OnAfterRemoveDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgFlags)
        {
            OnFoldersRemoved?.Invoke(NodesChanged(rgpProjects, rgFirstIndices, rgpszMkDocuments.Length, (projectNode, iterator) =>
            {
                return new RemovedPhysicalNode<PhysicalNodeRemovedFlags>(projectNode, rgpszMkDocuments[iterator], ((rgFlags[iterator] & 16u) != 0u) ? PhysicalNodeRemovedFlags.Removed : PhysicalNodeRemovedFlags.Deleted | PhysicalNodeRemovedFlags.Removed);
            }).ToCachingEnumerable());
        }

        public int OnAfterSccStatusChanged(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgdwSccStatus)
        {
            OnDocumentSccStatusChanged?.Invoke(NodesChanged(rgpProjects, rgFirstIndices, rgpszMkDocuments.Length, (projectNode, iterator) =>
            {
                if (!projectNode.TryGetPhysicalNode<IPhysicalProjectItemNode>(rgpszMkDocuments[iterator], out var physicalNode))
                {
                    throw new InvalidOperationException();
                }

                return new ChangedPhysicalSccNode<IPhysicalProjectItemNode, __SccStatus>(physicalNode, (__SccStatus)rgdwSccStatus[iterator]);
            }).ToCachingEnumerable());

            return CommonStatusCodes.Success;
        }

        #region Replaced by ExMethods

        public int OnAfterRemoveFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEFILEFLAGS[] rgFlags)
        {
            return CommonStatusCodes.NotImplemented;
        }

        public int OnAfterRemoveDirectories(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEDIRECTORYFLAGS[] rgFlags)
        {
            return CommonStatusCodes.NotImplemented;
        }

        #endregion

        #region Queries

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

        public void OnQueryRemoveFilesEx(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, uint[] rgFlags, VSQUERYREMOVEFILERESULTS[] pSummaryResult, VSQUERYREMOVEFILERESULTS[] rgResults)
        { }

        public void OnQueryRemoveDirectoriesEx(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, uint[] rgFlags, VSQUERYREMOVEDIRECTORYRESULTS[] pSummaryResult, VSQUERYREMOVEDIRECTORYRESULTS[] rgResults)
        { }

        #endregion

        private IEnumerable<T> NodesChanged<T>(IVsProject[] projects, int[] firstIndices, int itemCount, Func<ProjectNode, int, T> converter)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            int projItemIndex = 0;

            for (int i = 0; i < projects.Length; i++)
            {
                var currentProjectNode = NodeFactory.GetSolutionItemNode(Solution, (IVsHierarchy)projects[i], CommonNodeIds.Project);

                if (!(currentProjectNode is ProjectNode currentProject))
                {
                    continue;
                }

                int endProjectIndex = ((i + 1) == projects.Length)
                                          ? itemCount
                                          : firstIndices[i + 1];

                for (; projItemIndex < endProjectIndex; projItemIndex++)
                {
                    yield return converter.Invoke(currentProject, projItemIndex);
                }
            }
        }

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
    }
}
