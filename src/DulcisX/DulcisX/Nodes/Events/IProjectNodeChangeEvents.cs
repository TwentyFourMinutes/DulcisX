using System;
using System.Collections.Generic;

namespace DulcisX.Nodes.Events
{
    /// <summary>
    /// Currently not being used
    /// </summary>
    public interface IProjectNodeChangeEvents
    {
        event Action<DocumentNode> OnDocumentAdded;
        event Action<IEnumerable<DocumentNode>> OnBulkDocumentsAdded;

        event Action<FolderNode> OnFolderAdded;
        event Action<IEnumerable<FolderNode>> OnBulkFoldersAdded;

        event Action<DocumentNode> OnDocumentRemoved;
        event Action<IEnumerable<DocumentNode>> OnBulkDocumentsRemoved;

        event Action<FolderNode> OnFolderRemoved;
        event Action<IEnumerable<FolderNode>> OnBulkFoldersRemoved;

        event Action<DocumentNode> OnDocumentRenamed;
        event Action<IEnumerable<DocumentNode>> OnBulkDocumentsRenamed;

        event Action<FolderNode> OnFolderRenamed;
        event Action<IEnumerable<FolderNode>> OnBulkFoldersRenamed;

        event Action<DocumentNode> OnDocumentSccStatusChanged;
        event Action<IEnumerable<DocumentNode>> OnBulkDocumentSccStatusChanged;
    }
}
