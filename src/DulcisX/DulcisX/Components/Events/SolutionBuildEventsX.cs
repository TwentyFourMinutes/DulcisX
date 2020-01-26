using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    public class SolutionBuildEventsX : IVsUpdateSolutionEvents
    {
        private uint _cookieUID;
        private readonly IVsSolutionBuildManager _buildManager;

        private SolutionBuildEventsX(IVsSolutionBuildManager buildManager)
            => _buildManager = buildManager;

        public delegate void BeforeSolutionBuild(ref bool shouldCancel);
        public delegate void BeforeProjectConfigurationBuild(ref bool shouldCancel);

        public event BeforeSolutionBuild OnBeforeSolutionBuild;

        public event Action<bool, bool, bool> OnAfterSolutionBuild;

        public event Action OnSolutionBuildCancel;

        public event BeforeProjectConfigurationBuild OnBeforeProjectConfigurationBuild;

        public event Action OnAfterProjectConfigurationChange;

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            bool tempBool = false;

            OnBeforeSolutionBuild?.Invoke(ref tempBool);

            pfCancelUpdate = tempBool ? 1 : 0;

            return VSConstants.S_OK;
        }

        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            OnAfterSolutionBuild?.Invoke(fSucceeded == 1, fModified == 1, fCancelCommand == 1);
            return VSConstants.S_OK;
        }

        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            bool tempBool = false;

            OnBeforeProjectConfigurationBuild?.Invoke(ref tempBool);

            pfCancelUpdate = tempBool ? 1 : 0;
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Cancel()
        {
            OnSolutionBuildCancel?.Invoke();
            return VSConstants.S_OK;
        }

        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            OnAfterProjectConfigurationChange?.Invoke();
            return VSConstants.S_OK;
        }

        internal void Destroy(SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = _buildManager.UnadviseUpdateSolutionEvents(_cookieUID);
            VsHelper.ValidateVSStatusCode(result);
        }

        internal static SolutionBuildEventsX Create(SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var manager = solution.ServiceProviders.GetService<SVsSolutionBuildManager, IVsSolutionBuildManager>();

            var solutionBuildEvents = new SolutionBuildEventsX(manager);

            var result = manager.AdviseUpdateSolutionEvents(solutionBuildEvents, out var cookieUID);

            VsHelper.ValidateVSStatusCode(result);

            solutionBuildEvents._cookieUID = cookieUID;

            return solutionBuildEvents;
        }
    }
}
