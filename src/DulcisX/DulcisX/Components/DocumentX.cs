using DulcisX.Core.Models.Enums;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Components
{
    public class DocumentX : HierarchyItemX
    {
        public int BuildAction
        {
            get => GetProperty<int>((int)__VSHPROPID4.VSHPROPID_BuildAction);
            set => SetProperty((int)__VSHPROPID4.VSHPROPID_BuildAction, value);
        }

        internal DocumentX(IVsHierarchy hierarchy, uint itemId) : base(hierarchy, itemId, HierarchyItemTypeX.Document)
        {

        }

        internal DocumentX(IVsHierarchy hierarchy, HierarchyItemX parentItem, uint itemId) : base(hierarchy, parentItem, itemId, HierarchyItemTypeX.Document)
        {

        }
    }
}
