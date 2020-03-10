using System;

namespace DulcisX.Nodes.Events
{
    public interface ISolutionLoadEvents
    {
        event Action<string> OnBackgroundSolutionLoad;
        event Action OnBackgroundSolutionLoaded;
    }
}
