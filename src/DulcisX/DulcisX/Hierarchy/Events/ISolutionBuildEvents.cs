using System;

namespace DulcisX.Hierarchy.Events
{
    /// <summary>
    /// Provides events, which occur on build changes in a <see cref="SolutionNode"/>. Provided by the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsUpdateSolutionEvents"/> interface.
    /// </summary>
    public interface ISolutionBuildEvents
    {
        /// <summary>
        /// Occurs before a <see cref="SolutionNode"/> gets build.
        /// </summary>
        event Action<CancelTraslaterToken> OnSolutionBuild;

        /// <summary>
        /// Occurs when a <see cref="SolutionNode"/> got built.
        /// </summary>
        event Action<bool, bool, bool> OnSolutionBuilt;

        /// <summary>
        /// Occurs when a <see cref="SolutionNode"/> build got cancled.
        /// </summary>
        event Action OnSolutionBuildCancel;

        /// <summary>
        /// Occurs before the first <see cref="ProjectNode"/> got build.
        /// </summary>
        event Action<CancelTraslaterToken> OnProjectsConfigurationBuild;

        /// <summary>
        /// Occurs when a <see cref="ProjectNode"/> build configuration changed.
        /// </summary>
        EventDistributor<Action<ProjectNode>> OnProjectConfigurationChanged { get; }
    }
}
