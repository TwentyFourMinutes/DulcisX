using DulcisX.Core.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes.Events
{
    public interface IProjectNodeChangeEvents
    {
        event Action<IEnumerable<AddedPhysicalNode<DocumentNode, VSADDFILEFLAGS>>> OnDocumentsAdded;

        event Action<IEnumerable<AddedPhysicalNode<FolderNode, VSADDDIRECTORYFLAGS>>> OnFoldersAdded;

        event Action<IEnumerable<RemovedPhysicalNode<PhysicalNodeRemovedFlags>>> OnDocumentsRemoved;

        event Action<IEnumerable<RemovedPhysicalNode<PhysicalNodeRemovedFlags>>> OnFoldersRemoved;

        event Action<IEnumerable<RenamedPhysicalNode<DocumentNode, VSRENAMEFILEFLAGS>>> OnDocumentsRenamed;

        event Action<IEnumerable<RenamedPhysicalNode<FolderNode, VSRENAMEDIRECTORYFLAGS>>> OnFoldersRenamed;

        event Action<IEnumerable<ChangedPhysicalSccNode<IPhysicalNode, __SccStatus>>> OnDocumentSccStatusChanged;
    }
}
