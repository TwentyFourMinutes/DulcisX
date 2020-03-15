using DulcisX.Nodes;

namespace DulcisX.Core
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
