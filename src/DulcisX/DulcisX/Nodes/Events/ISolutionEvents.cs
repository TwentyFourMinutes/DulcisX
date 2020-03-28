using System;

namespace DulcisX.Nodes.Events
{
    /// <summary>
    /// Provides generic events, which occur in a <see cref="SolutionNode"/>. Provided by the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsSolutionEvents"/> interface.
    /// </summary>
    public interface ISolutionEvents : ISolutionLoadEvents
    {
        /// <summary>
        /// Occurs when a <see cref="ProjectNode"/> gets opened in a <see cref="SolutionNode"/>. This either happens on Project reload or Project addition.
        /// </summary>
        EventDistributor<Action<ProjectNode, bool>> OnProjectOpened { get; }

        /// <summary>
        /// Occurs when someone asks to close a <see cref="ProjectNode"/>.
        /// </summary>
        EventDistributor<Action<ProjectNode, bool, CancelTraslaterToken>> OnQueryProjectClose { get; }

        /// <summary>
        /// Occurs when a <see cref="ProjectNode"/> gets closed in a <see cref="SolutionNode"/>. This either happens on Project unload or Project removal.
        /// </summary>
        EventDistributor<Action<ProjectNode, bool>> OnProjectClose { get; }

        /// <summary>
        /// Occurs when a <see cref="ProjectNode"/> got loaded.
        /// </summary>
        EventDistributor<Action<ProjectNode, ProjectNode>> OnProjectLoaded { get; }

        /// <summary>
        /// Occurs when someone asks to unload a <see cref="ProjectNode"/>.
        /// </summary>
        EventDistributor<Action<ProjectNode, CancelTraslaterToken>> OnQueryProjectUnload { get; }

        /// <summary>
        /// Occurs when a <see cref="ProjectNode"/> got unloaded.
        /// </summary>
        EventDistributor<Action<ProjectNode, ProjectNode>> OnProjectUnload { get; }

        /// <summary>
        /// Occurs when a <see cref="ProjectNode"/> got renamed.
        /// </summary>
        EventDistributor<Action<ProjectNode>> OnProjectRenamed { get; }

        /// <summary>
        /// Occurs when a <see cref="ProjectNode"/> got added to the <see cref="SolutionNode"/>.
        /// </summary>
        event Action<ProjectNode> OnProjectAdd;

        /// <summary>
        /// Occurs when a <see cref="ProjectNode"/> got removed to the <see cref="SolutionNode"/>.
        /// </summary>
        event Action<ProjectNode> OnProjectRemove;

        /// <summary>
        /// Occurs when a the Solution opened.
        /// </summary>
        event Action<bool> OnSolutionOpened;

        /// <summary>
        /// Occurs when someone asks to close the Solution.
        /// </summary>
        event Action<CancelTraslaterToken> OnQuerySolutionClose;

        /// <summary>
        /// Occurs when before the Solution closes.
        /// </summary>
        event Action OnSolutionClose;

        /// <summary>
        /// Occurs when the Solution closed.
        /// </summary>
        event Action OnSolutionClosed;

        /// <summary>
        /// Occurs when the Solution got renamed.
        /// </summary>
        event Action<string, string> OnSolutionRenamed;
    }
}
