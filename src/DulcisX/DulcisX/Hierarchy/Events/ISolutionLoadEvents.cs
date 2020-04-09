using System;

namespace DulcisX.Hierarchy.Events
{
    /// <summary>
    /// Provides events, which occur on <see cref="SolutionNode"/> load. Provided by the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsSolutionLoadEvents"/> interface.
    /// </summary>
    public interface ISolutionLoadEvents
    {
        /// <summary>
        /// Occurs when the solution starts loading in the background.
        /// </summary>
        event Action<string> OnBackgroundSolutionLoad;

        /// <summary>
        /// Occurs when the solution completed loading in the background.
        /// </summary>
        event Action OnBackgroundSolutionLoaded;
    }
}
