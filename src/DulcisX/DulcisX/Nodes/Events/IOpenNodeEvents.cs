using DulcisX.Core.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Nodes.Events
{
    public interface IOpenNodeEvents
    {
        EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>> OnNodeLocked { get; }
        EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>> OnNodeUnlocked { get; }
        EventDistributor<Action<IPhysicalNode>> OnSaved { get; }
        EventDistributor<Action<IPhysicalNode, bool, IVsWindowFrame>> OnNodeWindowShow { get; }
        EventDistributor<Action<IPhysicalNode, IVsWindowFrame>> OnNodeWindowHidden { get; }
        EventDistributor<Action<IPhysicalNode, VsRDTAttribute>> OnAttributeChanged { get; }
        EventDistributor<Action<IPhysicalNode, string, string>> OnRenamed { get; }
        EventDistributor<Action<IPhysicalNode, string, string>> OnMoved { get; }
        EventDistributor<Action<IPhysicalNode>> OnSave { get; }
    }
}
