using DulcisX.Core.Models;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    public interface IRunningDocTableEventsX
    {
       event Action<DocumentX, _VSRDTFLAGS, uint, uint> OnDocumentLocked;
       event Action<DocumentX, _VSRDTFLAGS, uint, uint> OnDocumentUnlocked;
       event Action<DocumentX> OnSaved;
       event Action<DocumentX, bool, IVsWindowFrame> OnDocumentWindowShow;
       event Action<DocumentX, IVsWindowFrame> OnDocumentWindowHidden;
       event Action<DocumentX, __VSRDTATTRIB, DocumentStateX, DocumentStateX> OnAttributeChanged;
       event Action<DocumentX> OnSave; 
    }
}
