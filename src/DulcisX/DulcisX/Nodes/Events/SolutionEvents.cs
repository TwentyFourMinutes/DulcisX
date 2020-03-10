using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Nodes.Events
{
    internal class SolutionEvents : EventSink, ISolutionEvents, IVsSolutionEvents, IVsSolutionEvents5
    {
        #region Events

        private EventDistributor<Action<ProjectNode, bool>> _onProjectOpened;
        public EventDistributor<Action<ProjectNode, bool>> OnProjectOpened
            => _onProjectOpened ?? (_onProjectOpened = new EventDistributor<Action<ProjectNode, bool>>());

        private EventDistributor<Action<ProjectNode, bool, CancelTraslaterToken>> _onQueryProjectClose;
        public EventDistributor<Action<ProjectNode, bool, CancelTraslaterToken>> OnQueryProjectClose
             => _onQueryProjectClose ?? (_onQueryProjectClose = new EventDistributor<Action<ProjectNode, bool, CancelTraslaterToken>>());

        private EventDistributor<Action<ProjectNode, bool>> _onProjectClose;
        public EventDistributor<Action<ProjectNode, bool>> OnProjectClose
             => _onProjectClose ?? (_onProjectClose = new EventDistributor<Action<ProjectNode, bool>>());

        private EventDistributor<Action<ProjectNode, ProjectNode>> _onProjectLoaded;
        public EventDistributor<Action<ProjectNode, ProjectNode>> OnProjectLoaded
             => _onProjectLoaded ?? (_onProjectLoaded = new EventDistributor<Action<ProjectNode, ProjectNode>>());

        private EventDistributor<Action<ProjectNode, CancelTraslaterToken>> _onQueryProjectUnload;
        public EventDistributor<Action<ProjectNode, CancelTraslaterToken>> OnQueryProjectUnload
             => _onQueryProjectUnload ?? (_onQueryProjectUnload = new EventDistributor<Action<ProjectNode, CancelTraslaterToken>>());

        private EventDistributor<Action<ProjectNode, ProjectNode>> _onProjectUnload;
        public EventDistributor<Action<ProjectNode, ProjectNode>> OnProjectUnload
             => _onProjectUnload ?? (_onProjectUnload = new EventDistributor<Action<ProjectNode, ProjectNode>>());


        public event Action<ProjectNode> OnProjectAdd;

        public event Action<ProjectNode> OnProjectRemove;

        public event Action<bool> OnSolutionOpened;

        public event Action<CancelTraslaterToken> OnQuerySolutionClose;

        public event Action OnSolutionClose;

        public event Action OnSolutionClosed;

        #endregion

        private Guid _lastProjectUnloaded = Guid.Empty;
        private string _lastProjectOpened = null;

        private SolutionEvents(SolutionNode solution) : base(solution)
        {

        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            var project = Solution.GetProject(pHierarchy);

            _onProjectOpened?.Invoke(project.NodeType, project, VsConverter.Boolean(fAdded));

            if (_lastProjectOpened is object &&
                _lastProjectOpened == project.GetFullName())
            {
                OnProjectAdd?.Invoke(project);
            }

            return CommonStatusCodes.Success;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return CancelTranslaterFactory.Create(_onQueryProjectClose, ref pfCancel, token =>
            {
                var project = Solution.GetProject(pHierarchy);

                _onQueryProjectClose.Invoke(project.NodeType, VsConverter.Boolean(fRemoving), token);
            });
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            var project = Solution.GetProject(pHierarchy);

            _onProjectClose?.Invoke(project.NodeType, project, VsConverter.Boolean(fRemoved));

            if (OnProjectRemove is object &&
                _lastProjectUnloaded == project.GetGuid())
            {
                OnProjectRemove.Invoke(project);
            }

            return CommonStatusCodes.Success;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            var oldProject = Solution.GetProject(pStubHierarchy);
            var newProject = Solution.GetProject(pRealHierarchy);

            _onProjectLoaded?.Invoke(newProject.NodeType, oldProject, newProject);

            return CommonStatusCodes.Success;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return CancelTranslaterFactory.Create(_onQueryProjectUnload, ref pfCancel, token =>
            {
                var project = Solution.GetProject(pRealHierarchy);

                _onQueryProjectUnload.Invoke(project.NodeType, project, token);
            });
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            var oldProject = Solution.GetProject(pRealHierarchy);
            var newProject = Solution.GetProject(pStubHierarchy);

            _onProjectUnload?.Invoke(newProject.NodeType, oldProject, newProject);

            if (OnProjectRemove is object)
            {
                _lastProjectUnloaded = newProject.GetGuid();
            }

            return CommonStatusCodes.Success;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            OnSolutionOpened?.Invoke(fNewSolution == 1);

            return CommonStatusCodes.Success;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return CancelTranslaterFactory.Create(OnQuerySolutionClose, ref pfCancel,
                token => OnQuerySolutionClose.Invoke(token));
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            OnSolutionClose?.Invoke();

            return CommonStatusCodes.Success;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            OnSolutionClosed?.Invoke();
            return CommonStatusCodes.Success;
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

        public override void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = Solution.UnderlyingSolution.UnadviseSolutionEvents(Cookie);

            ErrorHandler.ThrowOnFailure(result);
        }

        public void OnBeforeOpenProject(ref Guid guidProjectID, ref Guid guidProjectType, string pszFileName)
        {
            if (guidProjectType == VSConstants.CLSID.MiscellaneousFilesProject_guid ||
                guidProjectID != Guid.Empty)
            {
                return;
            }

            _lastProjectOpened = pszFileName;
        }
    }
}
