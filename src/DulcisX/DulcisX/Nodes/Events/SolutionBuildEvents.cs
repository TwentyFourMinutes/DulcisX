using DulcisX.Core.Enums;
using DulcisX.Core.Extensions;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Nodes.Events
{
    internal class SolutionBuildEvents : NodeEventSink, ISolutionBuildEvents, IVsUpdateSolutionEvents
    {
        #region Events

        private EventDistributor<Action<ProjectNode>> _onProjectConfigurationChanged;
        public EventDistributor<Action<ProjectNode>> OnProjectConfigurationChanged
            => _onProjectConfigurationChanged ?? (_onProjectConfigurationChanged = new EventDistributor<Action<ProjectNode>>());

        public event Action<CancelTraslaterToken> OnSolutionBuild;
        public event Action<bool, bool, bool> OnSolutionBuilt;
        public event Action OnSolutionBuildCancel;
        public event Action<CancelTraslaterToken> OnProjectsConfigurationBuild;

        #endregion

        private readonly IVsSolutionBuildManager _buildManager;

        private SolutionBuildEvents(SolutionNode solution, IVsSolutionBuildManager buildManager) : base(solution)
        {
            _buildManager = buildManager;
        }

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return CancelTranslaterFactory.Create(OnSolutionBuild, ref pfCancelUpdate,
                token => OnSolutionBuild.Invoke(token));
        }

        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            OnSolutionBuilt?.Invoke(VsConverter.AsBoolean(fSucceeded), VsConverter.AsBoolean(fModified), VsConverter.AsBoolean(fCancelCommand));

            return CommonStatusCodes.Success;
        }

        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return CancelTranslaterFactory.Create(OnProjectsConfigurationBuild, ref pfCancelUpdate,
                token => OnProjectsConfigurationBuild.Invoke(token));
        }

        public int UpdateSolution_Cancel()
        {
            OnSolutionBuildCancel?.Invoke();

            return CommonStatusCodes.Success;
        }

        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            if (_onProjectConfigurationChanged is object)
            {
                var project = Solution.GetProject(pIVsHierarchy);

                _onProjectConfigurationChanged.Invoke(project.GetNodeType(), project);
            }

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
