using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Helpers
{
    public static class ExtendedHierarchyUtilities
    {
        public static bool IsSolutionItemsProject(IVsHierarchy hierarchy)
            => IsNodeType(hierarchy, VSConstants.CLSID.SolutionItemsProject_guid);

        public static bool IsMiscellaneousFilesProject(IVsHierarchy hierarchy)
            => IsNodeType(hierarchy, VSConstants.CLSID.MiscellaneousFilesProject_guid);

        public static bool IsRealProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!(hierarchy is IVsProject project))
            {
                return false;
            }

            var result = project.GetMkDocument(CommonNodeIds.Project, out _);

            return ErrorHandler.Succeeded(result);
        }

        public static bool IsNodeType(IVsHierarchy hierarchy, Guid clisdGuid)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var persist = hierarchy as IPersist;
            var result = persist.GetClassID(out Guid pClassID);

            if (ErrorHandler.Failed(result))
                return false;

            return pClassID == clisdGuid;
        }
    }
}
