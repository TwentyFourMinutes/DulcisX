using System;

namespace DulcisX.Nodes.Events
{
    public interface ISolutionEvents : ISolutionLoadEvents
    {
        EventDistributor<Action<ProjectNode, bool>> OnProjectOpened { get; }

        EventDistributor<Action<ProjectNode, bool, CancelTraslaterToken>> OnQueryProjectClose { get; }

        EventDistributor<Action<ProjectNode, bool>> OnProjectClose { get; }

        EventDistributor<Action<ProjectNode, ProjectNode>> OnProjectLoaded { get; }

        EventDistributor<Action<ProjectNode, CancelTraslaterToken>> OnQueryProjectUnload { get; }

        EventDistributor<Action<ProjectNode, ProjectNode>> OnProjectUnload { get; }

        EventDistributor<Action<ProjectNode>> OnProjectRenamed { get; }

        event Action<ProjectNode> OnProjectAdd;

        event Action<ProjectNode> OnProjectRemove;

        event Action<bool> OnSolutionOpened;

        event Action<CancelTraslaterToken> OnQuerySolutionClose;

        event Action OnSolutionClose;

        event Action OnSolutionClosed;
    }
}
