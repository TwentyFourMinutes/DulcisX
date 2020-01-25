using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    public delegate void QueryProjectClose(IVsHierarchy hierarchy, bool isRemoving, ref bool shouldCancel);
    public delegate void QueryProjectUnload(IVsHierarchy hierarchy, ref bool shouldCancel);
    public delegate void QuerySolutionClose(ref bool shouldCancel);

    public class SolutionEventsX : IVsSolutionEvents
    {
        public event Action<IVsHierarchy, bool> OnAfterProjectOpen;

        public event QueryProjectClose OnQueryProjectClose;

        public event Action<IVsHierarchy, bool> OnBeforeProjectClose;

        public event Action<IVsHierarchy, IVsHierarchy> OnAfterProjectLoad;

        public event QueryProjectUnload OnQueryProjectUnload;

        public event Action<IVsHierarchy, IVsHierarchy> OnBeforeProjectUnload;

        public event Action<bool> OnAfterSolutionOpen;

        public event QuerySolutionClose OnQuerySolutionClose;

        public event Action OnBeforeSolutionClose;

        public event Action OnAfterSolutionClose;

        internal uint CookieUID { get; set; }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            OnAfterProjectOpen?.Invoke(pHierarchy, fAdded == 1);

            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            bool tempBool = false;

            OnQueryProjectClose?.Invoke(pHierarchy, fRemoving == 1, ref tempBool);

            pfCancel = tempBool ? 1 : 0;

            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            OnBeforeProjectClose?.Invoke(pHierarchy, fRemoved == 1);
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            OnAfterProjectLoad?.Invoke(pStubHierarchy, pRealHierarchy);
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            bool tempBool = false;

            OnQueryProjectUnload?.Invoke(pRealHierarchy, ref tempBool);

            pfCancel = tempBool ? 1 : 0;

            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            OnBeforeProjectUnload?.Invoke(pRealHierarchy, pStubHierarchy);
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
    }
}
