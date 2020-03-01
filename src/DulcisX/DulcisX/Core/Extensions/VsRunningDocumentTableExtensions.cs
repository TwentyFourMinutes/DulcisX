using DulcisX.Components;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
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
            ErrorHandler.ThrowOnFailure(result);

            return hierarchy.ConstructHierarchyItem(itemId, solution);
        }
    }
}