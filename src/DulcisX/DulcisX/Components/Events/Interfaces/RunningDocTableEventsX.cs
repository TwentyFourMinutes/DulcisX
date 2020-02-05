using DulcisX.Core.Models;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    public interface IRunningDocTableEventsX
    {
       event Action<HierarchyItemX, _VSRDTFLAGS, uint, uint> OnDocumentLocked;
       event Action<HierarchyItemX, _VSRDTFLAGS, uint, uint> OnDocumentUnlocked;
       event Action<HierarchyItemX> OnSaved;
       event Action<HierarchyItemX, bool, IVsWindowFrame> OnDocumentWindowShow;
       event Action<HierarchyItemX, IVsWindowFrame> OnDocumentWindowHidden;
       event Action<HierarchyItemX, __VSRDTATTRIB, DocumentStateX, DocumentStateX> OnAttributeChanged;
       event Action<HierarchyItemX> OnSave; 
    }
}
