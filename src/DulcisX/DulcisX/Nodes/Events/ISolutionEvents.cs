using System;

namespace DulcisX.Nodes.Events
{
    public interface ISolutionEvents
    {
        EventDistributor<Action<ProjectNode, bool>> OnAfterProjectOpen { get; }

        EventDistributor<Action<ProjectNode, bool, CancelTraslaterToken>> OnQueryProjectClose { get; }

        EventDistributor<Action<ProjectNode, bool>> OnBeforeProjectClose { get; }

        EventDistributor<Action<ProjectNode, ProjectNode>> OnAfterProjectLoad { get; }

        EventDistributor<Action<ProjectNode, CancelTraslaterToken>> OnQueryProjectUnload { get; }

        EventDistributor<Action<ProjectNode, ProjectNode>> OnBeforeProjectUnload { get; }

        event Action<ProjectNode> OnProjectAdd;

        event Action<ProjectNode> OnProjectRemove;

        event Action<bool> OnAfterSolutionOpen;

        event Action<CancelTraslaterToken> OnQuerySolutionClose;

        event Action OnBeforeSolutionClose;

        event Action OnAfterSolutionClose;
    }
}
