using System;

namespace DulcisX.Core.Enums
{
    /// <summary>
    /// Used instead of <see cref="Microsoft.VisualStudio.Shell.Interop.__VSREMOVEFILEFLAGS2"/>.
    /// </summary>
    [Flags]
    public enum PhysicalNodeRemovedFlags
    {
        #region __VSREMOVEFILEFLAGS2

        /// <summary>
        /// If this flag is set, the file is removed from the project, but still exists on disk.
        /// </summary>
        Removed = 1,

        #endregion

        /// <summary>
        /// If this flag is set, the file is removed from disk.
        /// </summary>
        Deleted = 2
    }
}
