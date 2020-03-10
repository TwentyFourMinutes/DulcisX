using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Nodes.Events
{
    internal class SolutionBuildEvents : EventSink, ISolutionBuildEvents, IVsUpdateSolutionEvents
    {
        #region Events

        private EventDistributor<Action<ProjectNode>> _onAfterProjectConfigurationChange;
        public EventDistributor<Action<ProjectNode>> OnAfterProjectConfigurationChange
            => _onAfterProjectConfigurationChange ?? (_onAfterProjectConfigurationChange = new EventDistributor<Action<ProjectNode>>());

        public event Action<CancelTraslaterToken> OnBeforeSolutionBuild;
        public event Action<bool, bool, bool> OnAfterSolutionBuild;
        public event Action OnSolutionBuildCancel;
        public event Action<CancelTraslaterToken> OnBeforeProjectConfigurationBuild;

        #endregion

        private readonly IVsSolutionBuildManager _buildManager;

        private SolutionBuildEvents(SolutionNode solution, IVsSolutionBuildManager buildManager) : base(solution)
        {
            _buildManager = buildManager;
        }

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return CancelTranslaterFactory.Create(OnBeforeSolutionBuild, ref pfCancelUpdate,
                token => OnBeforeSolutionBuild.Invoke(token));
        }

        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            OnAfterSolutionBuild?.Invoke(VsConverter.Boolean(fSucceeded), VsConverter.Boolean(fModified), VsConverter.Boolean(fCancelCommand));

            return CommonStatusCodes.Success;
        }

        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return CancelTranslaterFactory.Create(OnBeforeProjectConfigurationBuild, ref pfCancelUpdate,
                token => OnBeforeProjectConfigurationBuild.Invoke(token));
        }

        public int UpdateSolution_Cancel()
        {
            OnSolutionBuildCancel?.Invoke();

            return CommonStatusCodes.Success;
        }

        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            var project = Solution.GetProject(pIVsHierarchy);

            _onAfterProjectConfigurationChange?.Invoke(project.NodeType, project);

            return CommonStatusCodes.Success;
        }

        internal static ISolutionBuildEvents Create(SolutionNode solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var buildManager = solution.ServiceContainer.GetCOMInstance<IVsSolutionBuildManager>();

            var solutionBuildEvents = new SolutionBuildEvents(solution, buildManager);

            var result = buildManager.AdviseUpdateSolutionEvents(solutionBuildEvents, out var cookieUID);

            ErrorHandler.ThrowOnFailure(result);

            solutionBuildEvents.SetCookie(cookieUID);

            return solutionBuildEvents;
        }

        public override void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = _buildManager.UnadviseUpdateSolutionEvents(Cookie);

            ErrorHandler.ThrowOnFailure(result);
        }
    }
}
