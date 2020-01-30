using DulcisX.Core.Models;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    public interface IRunningDocTableEventsX
    {
       event Action<uint, _VSRDTFLAGS, uint, uint> OnDocumentLocked;
       event Action<uint, _VSRDTFLAGS, uint, uint> OnDocumentUnlocked;
       event Action<uint> OnSaved;
       event Action<uint, bool, IVsWindowFrame> OnDocumentWindowShow;
       event Action<uint, IVsWindowFrame> OnDocumentWindowHidden;
       event Action<uint, __VSRDTATTRIB, DocumentStateX, DocumentStateX> OnAttributeChanged;
       event Action<uint> OnSave; 
    }
}
