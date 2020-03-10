using System;

namespace DulcisX.Nodes.Events
{
    public interface ISolutionBuildEvents
    {
        event Action<CancelTraslaterToken> OnBeforeSolutionBuild;

        event Action<bool, bool, bool> OnAfterSolutionBuild;

        event Action OnSolutionBuildCancel;

        event Action<CancelTraslaterToken> OnBeforeProjectConfigurationBuild;

        EventDistributor<Action<ProjectNode>> OnAfterProjectConfigurationChange { get; }
    }
}
