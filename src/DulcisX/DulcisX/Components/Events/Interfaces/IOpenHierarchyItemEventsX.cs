using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    public interface IOpenHierarchyItemEventsX
    {
       event Action<HierarchyItemX, _VSRDTFLAGS, uint, uint> OnDocumentLocked;
       event Action<HierarchyItemX, _VSRDTFLAGS, uint, uint> OnDocumentUnlocked;
       event Action<HierarchyItemX> OnSaved;
       event Action<HierarchyItemX, bool, IVsWindowFrame> OnDocumentWindowShow;
       event Action<HierarchyItemX, IVsWindowFrame> OnDocumentWindowHidden;
       event Action<HierarchyItemX, VsRDTAttributeX> OnAttributeChanged;
       event Action<HierarchyItemX, string, string> OnRenamed;
       event Action<HierarchyItemX, string, string> OnMoved;
       event Action<HierarchyItemX> OnSave;
    }
}
