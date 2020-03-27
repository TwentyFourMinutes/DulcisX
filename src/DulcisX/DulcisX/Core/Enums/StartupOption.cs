namespace DulcisX.Core.Enums
{
    /// <summary>
    /// Represents the startup option for a <see cref="Nodes.ProjectNode"/>.
    /// </summary>
    public enum StartupOption
    {
        /// <summary>
        /// The <see cref="Nodes.ProjectNode"/> won't start at Application start.
        /// </summary>
        None,
        /// <summary>
        /// The <see cref="Nodes.ProjectNode"/> will start at Application start.
        /// </summary>
        Start,
        /// <summary>
        /// The <see cref="Nodes.ProjectNode"/> will start at Application start and attach the debugger to the new process.
        /// </summary>
        StartWithDebugging
    }
}
