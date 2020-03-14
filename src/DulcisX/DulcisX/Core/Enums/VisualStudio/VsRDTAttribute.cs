namespace DulcisX.Core.Enums.VisualStudio
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.__vsrdtattrib?view=visualstudiosdk-2017
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.__vsrdtattrib2?view=visualstudiosdk-2017
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.__vsrdtattrib3?view=visualstudiosdk-2017
    /// </summary>
    public enum VsRDTAttribute
    {
        #region __VSRDTATTRIB

        Hierarchy = 1,

        ItemID = 2,

        MkDocument = 4,

        DocDataIsDirty = 8,

        DocDataIsNotDirty = 16,

        NOTIFYDOCCHANGEDMASK = -65536,

        DocDataReloaded = 65536,

        AltHierarchyItemID = 131072,

        #endregion

        #region __VSRDTATTRIB2

        DocDataIsNotReadOnly = 524288,

        DocDataIsReadOnly = 262144,

        NOTIFYDOCCHANGEDEXMASK = -65512,

        #endregion

        #region __VSRDTATTRIB3

        DocumentInitialized = 1048576,

        HierarchyInitialized = 2097152

        #endregion
    }
}
