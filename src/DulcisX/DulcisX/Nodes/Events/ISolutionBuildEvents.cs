using System;

namespace DulcisX.Nodes.Events
{
    public interface ISolutionBuildEvents
    {
        event Action<CancelTraslaterToken> OnSolutionBuild;

        event Action<bool, bool, bool> OnSolutionBuilt;

        event Action OnSolutionBuildCancel;

        event Action<CancelTraslaterToken> OnProjectConfigurationBuild;

        EventDistributor<Action<ProjectNode>> OnProjectConfigurationChanged { get; }
    }
}
