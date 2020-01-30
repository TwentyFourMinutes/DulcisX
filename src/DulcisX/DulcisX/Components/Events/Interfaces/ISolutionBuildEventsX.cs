using System;

namespace DulcisX.Components.Events
{
    public delegate void BeforeSolutionBuild(ref bool shouldCancel);
    public delegate void BeforeProjectConfigurationBuild(ref bool shouldCancel);

    public interface ISolutionBuildEventsX
    {
        event BeforeSolutionBuild OnBeforeSolutionBuild;

        event Action<bool, bool, bool> OnAfterSolutionBuild;

        event Action OnSolutionBuildCancel;

        event BeforeProjectConfigurationBuild OnBeforeProjectConfigurationBuild;

        event Action OnAfterProjectConfigurationChange;
    }
}
