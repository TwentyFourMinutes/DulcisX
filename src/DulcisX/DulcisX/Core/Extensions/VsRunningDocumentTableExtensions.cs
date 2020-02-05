using DulcisX.Components;
using DulcisX.Core.Models;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core.Extensions
{
    public static class VsRunningDocumentTableExtensions
    {
        public static HierarchyItemX GetHierarchyItem(this IVsRunningDocumentTable rdt, uint docCookie, SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = rdt.GetDocumentInfo(docCookie, out _, out _, out _, out _, out var hierarchy, out uint itemId, out _);
            VsHelper.ValidateSuccessStatusCode(result);

            return new HierarchyItemX(hierarchy, itemId, hierarchy.GetHierarchyItemType(itemId), ConstructorInstance.FromValue(solution), ConstructorInstance.Empty<ProjectX>());
        }
    }
}