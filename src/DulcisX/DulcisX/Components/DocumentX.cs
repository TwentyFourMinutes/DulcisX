using DulcisX.Core.Models;
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

        internal DocumentX(IVsHierarchy hierarchy, uint itemId, SolutionX solution, ProjectX project = default, HierarchyItemX parentItem = default) : base(hierarchy, itemId, HierarchyItemTypeX.Document, ConstructorInstance.FromValue(solution), ConstructorInstance.FromValue(project), parentItem)
        {

        }
    }
}
