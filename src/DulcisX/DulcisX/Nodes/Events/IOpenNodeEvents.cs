using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Nodes.Events
{
    public interface IOpenNodeEvents
    {
        EventDistributor<Action<BaseNode, _VSRDTFLAGS, uint, uint>> OnNodeLocked { get; }
        EventDistributor<Action<BaseNode, _VSRDTFLAGS, uint, uint>> OnNodeUnlocked { get; }
        EventDistributor<Action<BaseNode>> OnSaved { get; }
        EventDistributor<Action<BaseNode, bool, IVsWindowFrame>> OnNodeWindowShow { get; }
        EventDistributor<Action<BaseNode, IVsWindowFrame>> OnNodeWindowHidden { get; }
        EventDistributor<Action<BaseNode, VsRDTAttributeX>> OnAttributeChanged { get; }
        EventDistributor<Action<BaseNode, string, string>> OnRenamed { get; }
        EventDistributor<Action<BaseNode, string, string>> OnMoved { get; }
        EventDistributor<Action<BaseNode>> OnSave { get; }
    }
}
