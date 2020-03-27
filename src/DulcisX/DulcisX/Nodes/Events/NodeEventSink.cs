using DulcisX.Core;

namespace DulcisX.Nodes.Events
{
    public abstract class NodeEventSink : EventSink
    {
        protected SolutionNode Solution { get; }

        protected NodeEventSink(SolutionNode solution)
        {
            Solution = solution;
        }
    }
}
