namespace DulcisX.Core.Enums
{
    /// <summary>
    /// Common stream keys in the .suo file. Passed to <see cref="Nodes.SolutionNode.LoadUserConfiguration(Microsoft.VisualStudio.Shell.Interop.IVsPersistSolutionOpts, string)"/>.
    /// </summary>
    public static class CommonStreamKeys
    {
        /// <summary>
        /// Specifies the solution configuration strean inside the .suo file.
        /// </summary>
        public const string SolutionConfiguration = "SolutionConfiguration";
    }
}
