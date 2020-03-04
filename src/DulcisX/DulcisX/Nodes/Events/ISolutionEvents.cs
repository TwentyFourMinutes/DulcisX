using System;

namespace DulcisX.Nodes.Events
{
    public delegate void QueryProjectClose(ProjectNode project, bool isRemoving, ref bool shouldCancel);
    public delegate void QueryProjectUnload(ProjectNode project, ref bool shouldCancel);
    public delegate void QuerySolutionClose(ref bool shouldCancel);

    public interface ISolutionEvents
    {
        EventDistributor<Action<ProjectNode, bool>> OnAfterProjectOpen { get; }

        EventDistributor<QueryProjectClose> OnQueryProjectClose { get; }

        EventDistributor<Action<ProjectNode, bool>> OnBeforeProjectClose { get; }

        EventDistributor<Action<ProjectNode, ProjectNode>> OnAfterProjectLoad { get; }

        EventDistributor<QueryProjectUnload> OnQueryProjectUnload { get; }

        EventDistributor<Action<ProjectNode, ProjectNode>> OnBeforeProjectUnload { get; }

        EventDistributor<Action<bool>> OnAfterSolutionOpen { get; }

        EventDistributor<QuerySolutionClose> OnQuerySolutionClose { get; }

        EventDistributor<Action> OnBeforeSolutionClose { get; }

        EventDistributor<Action> OnAfterSolutionClose { get; }
    }
}
