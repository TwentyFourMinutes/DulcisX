using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Nodes.Events
{
    internal class SolutionEvents : EventSink, ISolutionEvents, IVsSolutionEvents
    {
        #region Events

        private EventDistributor<Action<ProjectNode, bool>> _onAfterProjectOpen;
        public EventDistributor<Action<ProjectNode, bool>> OnAfterProjectOpen
            => _onAfterProjectOpen ?? (_onAfterProjectOpen = new EventDistributor<Action<ProjectNode, bool>>());

        private EventDistributor<QueryProjectClose> _onQueryProjectClose;
        public EventDistributor<QueryProjectClose> OnQueryProjectClose
             => _onQueryProjectClose ?? (_onQueryProjectClose = new EventDistributor<QueryProjectClose>());

        private EventDistributor<Action<ProjectNode, bool>> _onBeforeProjectClose;
        public EventDistributor<Action<ProjectNode, bool>> OnBeforeProjectClose
             => _onBeforeProjectClose ?? (_onBeforeProjectClose = new EventDistributor<Action<ProjectNode, bool>>());

        private EventDistributor<Action<ProjectNode, ProjectNode>> _onAfterProjectLoad;
        public EventDistributor<Action<ProjectNode, ProjectNode>> OnAfterProjectLoad
             => _onAfterProjectLoad ?? (_onAfterProjectLoad = new EventDistributor<Action<ProjectNode, ProjectNode>>());

        private EventDistributor<QueryProjectUnload> _onQueryProjectUnload;
        public EventDistributor<QueryProjectUnload> OnQueryProjectUnload
             => _onQueryProjectUnload ?? (_onQueryProjectUnload = new EventDistributor<QueryProjectUnload>());

        private EventDistributor<Action<ProjectNode, ProjectNode>> _onBeforeProjectUnload;
        public EventDistributor<Action<ProjectNode, ProjectNode>> OnBeforeProjectUnload
             => _onBeforeProjectUnload ?? (_onBeforeProjectUnload = new EventDistributor<Action<ProjectNode, ProjectNode>>());

        private EventDistributor<Action<bool>> _onAfterSolutionOpen;
        public EventDistributor<Action<bool>> OnAfterSolutionOpen
             => _onAfterSolutionOpen ?? (_onAfterSolutionOpen = new EventDistributor<Action<bool>>());

        private EventDistributor<QuerySolutionClose> _onQuerySolutionClose;
        public EventDistributor<QuerySolutionClose> OnQuerySolutionClose
             => _onQuerySolutionClose ?? (_onQuerySolutionClose = new EventDistributor<QuerySolutionClose>());

        private EventDistributor<Action> _onBeforeSolutionClose;
        public EventDistributor<Action> OnBeforeSolutionClose
            => _onBeforeSolutionClose ?? (_onBeforeSolutionClose = new EventDistributor<Action>());

        private EventDistributor<Action> _onAfterSolutionClose;
        public EventDistributor<Action> OnAfterSolutionClose
             => _onAfterSolutionClose ?? (_onAfterSolutionClose = new EventDistributor<Action>());

        #endregion

        private SolutionEvents(SolutionNode solution) : base(solution)
        {

        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            var project = Solution.GetProject(pHierarchy);

            OnAfterProjectOpen.Invoke(project.NodeType, project, fAdded == 1);

            return CommonStatusCodes.Success;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            bool tempBool = false;

            var project = Solution.GetProject(pHierarchy);

            OnQueryProjectClose.Invoke(project.NodeType, fRemoving == 1, ref tempBool);

            pfCancel = tempBool ? 1 : 0;

            return CommonStatusCodes.Success;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            var project = Solution.GetProject(pHierarchy);

            OnBeforeProjectClose.Invoke(project.NodeType, project, fRemoved == 1);

            return CommonStatusCodes.Success;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            var oldProject = Solution.GetProject(pStubHierarchy);
            var newProject = Solution.GetProject(pRealHierarchy);

            OnAfterProjectLoad.Invoke(newProject.NodeType | oldProject.NodeType, oldProject, newProject);

            return CommonStatusCodes.Success;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            bool tempBool = false;

            var project = Solution.GetProject(pRealHierarchy);

            OnQueryProjectUnload.Invoke(project.NodeType, project, ref tempBool);

            pfCancel = tempBool ? 1 : 0;

            return CommonStatusCodes.Success;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            var oldProject = Solution.GetProject(pRealHierarchy);
            var newProject = Solution.GetProject(pStubHierarchy);

            OnBeforeProjectUnload.Invoke(newProject.NodeType | oldProject.NodeType, oldProject, newProject);

            return CommonStatusCodes.Success;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            OnAfterSolutionOpen.Invoke(NodeTypes.Solution, fNewSolution == 1);

            return CommonStatusCodes.Success;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            bool tempBool = false;

            OnQuerySolutionClose.Invoke(NodeTypes.Solution, ref tempBool);

            pfCancel = tempBool ? 1 : 0;

            return CommonStatusCodes.Success;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            OnBeforeSolutionClose.Invoke(NodeTypes.Solution);
            return CommonStatusCodes.Success;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            OnAfterSolutionClose.Invoke(NodeTypes.Solution);
            return CommonStatusCodes.Success;
        }

        public override void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = Solution.UnderlyingSolution.UnadviseSolutionEvents(Cookie);

            ErrorHandler.ThrowOnFailure(result);
        }

        internal static ISolutionEvents Create(SolutionNode solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionEvents = new SolutionEvents(solution);

            var result = solution.UnderlyingSolution.AdviseSolutionEvents(solutionEvents, out var cookieUID);

            ErrorHandler.ThrowOnFailure(result);

            solutionEvents.SetCookie(cookieUID);

            return solutionEvents;
        }
    }
}
