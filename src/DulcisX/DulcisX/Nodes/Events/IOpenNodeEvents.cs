using DulcisX.Core.Enums;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Nodes.Events
{
    /// <summary>
    /// Provides events, which occur on changes to Nodes which are in the <see cref="IVsRunningDocumentTable"/>.
    /// </summary>
    public interface IOpenNodeEvents
    {
        /// <summary>
        /// Occurs when an <see cref="IPhysicalNode"/> gets locked.
        /// </summary>
        EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>> OnNodeLocked { get; }
        /// <summary>
        /// Occurs when an <see cref="IPhysicalNode"/> gets unlocked.
        /// </summary>
        EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>> OnNodeUnlocked { get; }
        /// <summary>
        /// Occurs when an <see cref="IPhysicalNode"/> gets saved.
        /// </summary>
        EventDistributor<Action<IPhysicalNode>> OnSaved { get; }
        /// <summary>
        /// Occurs when an <see cref="IPhysicalNode"/> gets opened in an <see cref="IVsWindowFrame"/>.
        /// </summary>
        EventDistributor<Action<IPhysicalNode, bool, IVsWindowFrame>> OnNodeWindowShow { get; }
        /// <summary>
        /// Occurs when an <see cref="IPhysicalNode"/> has a <see cref="IVsWindowFrame"/> which got hidden.
        /// </summary>
        EventDistributor<Action<IPhysicalNode, IVsWindowFrame>> OnNodeWindowHidden { get; }
        /// <summary>
        /// Occurs when any attributes on an <see cref="IPhysicalNode"/> get changed.
        /// </summary>
        EventDistributor<Action<IPhysicalNode, OpenNodeAttribute>> OnAttributeChanged { get; }
        /// <summary>
        /// Occurs when an <see cref="IPhysicalNode"/> gets renamed.
        /// </summary>
        EventDistributor<Action<IPhysicalNode, string, string>> OnRenamed { get; }
        /// <summary>
        /// Occurs when an <see cref="IPhysicalNode"/> gets moved.
        /// </summary>
        EventDistributor<Action<IPhysicalNode, string, string>> OnMoved { get; }
        /// <summary>
        /// Occurs when an <see cref="IPhysicalNode"/> gets saved.
        /// </summary>
        EventDistributor<Action<IPhysicalNode>> OnSave { get; }
    }
}
