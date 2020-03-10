using System;

namespace DulcisX.Nodes.Events
{
    public delegate void BeforeSolutionBuild(ref bool shouldCancel);
    public delegate void BeforeProjectConfigurationBuild(ref bool shouldCancel);

    public interface ISolutionBuildEvents
    {
        event BeforeSolutionBuild OnBeforeSolutionBuild;

        event Action<bool, bool, bool> OnAfterSolutionBuild;

        event Action OnSolutionBuildCancel;

        event BeforeProjectConfigurationBuild OnBeforeProjectConfigurationBuild;

        EventDistributor<Action<ProjectNode>> OnAfterProjectConfigurationChange { get; }
    }
}
