namespace DulcisX.Core.Enums.VisualStudio
{
    /// <summary>
    /// Used instead of <see cref="Microsoft.VisualStudio.Shell.Interop.__VSRDTATTRIB"/>, <see cref="Microsoft.VisualStudio.Shell.Interop.__VSRDTATTRIB2"/> and <see cref="Microsoft.VisualStudio.Shell.Interop.__VSRDTATTRIB3"/>. It unifies all of the listed enum members.
    /// </summary>
    public enum OpenNodeAttribute
    {
        #region __VSRDTATTRIB

        /// <summary>
        /// Hierarchical position of the document in the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable"/>.
        /// </summary>
        Hierarchy = 1,
        /// <summary>
        /// Item identifier of the document in the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable"/>.
        /// </summary>
        ItemID = 2,
        /// <summary>
        /// Full path to the document in the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable"/>.
        /// </summary>
        MkDocument = 4,
        /// <summary>
        /// Flag indicates that the data of the document in the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable"/> has changed.
        /// </summary>
        DocDataIsDirty = 8,
        /// <summary>
        /// Flag indicates that the data of the document in the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable"/> has not changed.
        /// </summary>
        DocDataIsNotDirty = 16,
        /// <summary>
        /// A mask for the flags passed to the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable.NotifyDocumentChanged(uint, uint)"/> method.
        /// </summary>
        NOTIFYDOCCHANGEDMASK = -65536,
        /// <summary>
        /// This attribute event is fired by calling the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable.NotifyDocumentChanged(uint, uint)"/> method.
        /// </summary>
        DocDataReloaded = 65536,
        /// <summary>
        /// This attribute event is fired by calling the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable.NotifyDocumentChanged(uint, uint)"/> method.
        /// </summary>
        AltHierarchyItemID = 131072,

        #endregion

        #region __VSRDTATTRIB2

        /// <summary>
        /// A mask for the flags passed to the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable3.NotifyDocumentChangedEx(uint, uint)"/> method.
        /// </summary>
        NotifyDocChangedXMask = -65512,
        /// <summary>
        /// The data of the document in the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable"/> is read-only.
        /// </summary>
        DocDataIsReadOnly = 262144,
        /// <summary>
        /// The data of the document in the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable"/> is readable and writable.
        /// </summary>
        DocDataIsNotReadOnly = 524288,

        #endregion

        #region __VSRDTATTRIB3

        /// <summary>
        /// A document was added to the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsRunningDocumentTable"/> in a fully-initialized state, or a document that had the property RDT_PendingInitialization has completed its initialization.
        /// </summary>
        DocumentInitialized = 1048576,
        /// <summary>
        /// A document that had the property RDT_PendingHierarchyInitialization has completed its hierarchy initialization.
        /// </summary>
        HierarchyInitialized = 2097152

        #endregion
    }
}
