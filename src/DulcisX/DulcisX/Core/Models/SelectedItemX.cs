﻿using Microsoft.VisualStudio.Shell.Interop;
using static Microsoft.VisualStudio.VSConstants;

namespace DulcisX.Core.Models
{
    public class SelectedItemX
    {
        public VSITEMID ItemId { get; set; }
        public IVsHierarchy Hierarchy { get; set; }

        internal SelectedItemX(IVsHierarchy hierarchy, uint itemId)
            => (Hierarchy, ItemId) = (hierarchy, (VSITEMID)itemId);

    }
}
