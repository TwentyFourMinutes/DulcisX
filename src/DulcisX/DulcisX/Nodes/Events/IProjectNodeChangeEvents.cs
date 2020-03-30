using DulcisX.Core.Enums;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes.Events
{
    /// <summary>
    /// Provides events, which occur on changes to <see cref="ProjectItemNode"/>s within all <see cref="ProjectNode"/>s. Provided by the <see cref="IVsTrackProjectDocumentsEvents2"/> interface.
    /// </summary>
    public interface IProjectNodeChangeEvents
    {
        /// <summary>
        /// Occurs when <see cref="DocumentNode"/>(s) get added to a <see cref="ProjectNode"/>.
        /// </summary>
        event Action<IEnumerable<AddedPhysicalNode<DocumentNode, VSADDFILEFLAGS>>> OnDocumentsAdded;

        /// <summary>
        /// Occurs when <see cref="FolderNode"/>(s) get added to a <see cref="ProjectNode"/>.
        /// </summary>
        event Action<IEnumerable<AddedPhysicalNode<FolderNode, VSADDDIRECTORYFLAGS>>> OnFoldersAdded;

        /// <summary>
        /// Occurs when <see cref="DocumentNode"/>(s) get removed from a <see cref="ProjectNode"/>.
        /// </summary>
        event Action<IEnumerable<RemovedPhysicalNode<PhysicalNodeRemovedFlags>>> OnDocumentsRemoved;

        /// <summary>
        /// Occurs when <see cref="FolderNode"/>(s) get added from a <see cref="ProjectNode"/>.
        /// </summary>
        event Action<IEnumerable<RemovedPhysicalNode<PhysicalNodeRemovedFlags>>> OnFoldersRemoved;

        /// <summary>
        /// Occurs when <see cref="DocumentNode"/>(s) get renamed.
        /// </summary>
        event Action<IEnumerable<RenamedPhysicalNode<DocumentNode, VSRENAMEFILEFLAGS>>> OnDocumentsRenamed;

        /// <summary>
        /// Occurs when <see cref="FolderNode"/>(s) get renamed.
        /// </summary>
        event Action<IEnumerable<RenamedPhysicalNode<FolderNode, VSRENAMEDIRECTORYFLAGS>>> OnFoldersRenamed;

        /// <summary>
        /// Occurs when the source control status of <see cref="IPhysicalNode"/>(s) change.
        /// </summary>
        event Action<IEnumerable<ChangedPhysicalSccNode<IPhysicalNode, __SccStatus>>> OnDocumentSccStatusChanged;
    }
}
