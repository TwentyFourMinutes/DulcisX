using System;

namespace DulcisX.Core.Enums.VisualStudio
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.__vsremovefileflags2?view=visualstudiosdk-2017
    /// </summary>
    [Flags]
    public enum PhysicalNodeRemovedFlags
    {
        #region __VSREMOVEFILEFLAGS2

        Removed = 1,

        #endregion

        Deleted = 2 | Removed
    }
}
