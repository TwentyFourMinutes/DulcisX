using DulcisX.Core;

namespace DulcisX.Hierarchy.Events
{
    /// <summary>
    /// Provides basic logic for the inheritance of native Visual Studio Node Events.
    /// </summary>
    public abstract class NodeEventSink : EventSink
    {
        /// <summary>
        /// Gets the Solution Node of the Solution Explorer.
        /// </summary>
        protected SolutionNode Solution { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FolderNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution Node of the Solution Explorer.</param>
        protected NodeEventSink(SolutionNode solution)
        {
            Solution = solution;
        }
    }
}
