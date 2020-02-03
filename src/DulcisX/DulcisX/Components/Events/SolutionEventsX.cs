using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    internal class SolutionEventsX : EventCookieX, ISolutionEventsX, IVsSolutionEvents
    {
        public event Action<ProjectX, bool> OnAfterProjectOpen;

        public event QueryProjectClose OnQueryProjectClose;

        public event Action<ProjectX, bool> OnBeforeProjectClose;

        public event Action<IVsHierarchy, ProjectX> OnAfterProjectLoad;

        public event QueryProjectUnload OnQueryProjectUnload;

        public event Action<ProjectX, IVsHierarchy> OnBeforeProjectUnload;

        public event Action<bool> OnAfterSolutionOpen;

        public event QuerySolutionClose OnQuerySolutionClose;

        public event Action OnBeforeSolutionClose;

        public event Action OnAfterSolutionClose;

        private readonly SolutionX _solution;

        private SolutionEventsX(SolutionX solution)
            => _solution = solution;

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            OnAfterProjectOpen?.Invoke(_solution.GetProject(pHierarchy), fAdded == 1);
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            bool tempBool = false;

            OnQueryProjectClose?.Invoke(_solution.GetProject(pHierarchy), fRemoving == 1, ref tempBool);

            pfCancel = tempBool ? 1 : 0;

            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            OnBeforeProjectClose?.Invoke(_solution.GetProject(pHierarchy), fRemoved == 1);
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            OnAfterProjectLoad?.Invoke(pStubHierarchy, _solution.GetProject(pRealHierarchy));
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            bool tempBool = false;

            OnQueryProjectUnload?.Invoke(_solution.GetProject(pRealHierarchy), ref tempBool);

            pfCancel = tempBool ? 1 : 0;

            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            OnBeforeProjectUnload?.Invoke(_solution.GetProject(pRealHierarchy), pStubHierarchy);
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            OnAfterSolutionOpen?.Invoke(fNewSolution == 1);

            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            bool tempBool = false;

            OnQuerySolutionClose?.Invoke(ref tempBool);

            pfCancel = tempBool ? 1 : 0;

            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            OnBeforeSolutionClose?.Invoke();
            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            OnAfterSolutionClose?.Invoke();
            return VSConstants.S_OK;
        }

        internal void Destroy(SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = solution.UnderlyingSolution.UnadviseSolutionEvents(CookieUID);
            VsHelper.ValidateVSStatusCode(result);
        }

        internal static ISolutionEventsX Create(SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionEvents = new SolutionEventsX(solution);

            var result = solution.UnderlyingSolution.AdviseSolutionEvents(solutionEvents, out var cookieUID);

            VsHelper.ValidateVSStatusCode(result);

            solutionEvents.CookieUID = cookieUID;

            return solutionEvents;
        }
    }
}
