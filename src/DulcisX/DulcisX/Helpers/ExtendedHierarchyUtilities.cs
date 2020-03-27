using DulcisX.Core.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Helpers
{
    /// <summary>
    /// Extends the <see cref="Microsoft.Internal.VisualStudio.PlatformUI.HierarchyUtilities"/> class.
    /// </summary>
    public static class ExtendedHierarchyUtilities
    {
        /// <summary>
        /// Gets a value indicating whether a <see cref="IVsHierarchy"/> is a Solution Items Project.
        /// </summary>
        /// <param name="hierarchy">The hierarchy which should be tested.</param>
        /// <returns><see langword="true"/> if the hierarchy is a Solution Items Project; otherwise <see langword="false"/>.</returns>
        public static bool IsSolutionItemsProject(IVsHierarchy hierarchy)
            => IsNodeType(hierarchy, VSConstants.CLSID.SolutionItemsProject_guid);

        /// <summary>
        /// Gets a value indicating whether a <see cref="IVsHierarchy"/> is a Miscellaneous Files Project.
        /// </summary>
        /// <param name="hierarchy">The hierarchy which should be tested.</param>
        /// <returns><see langword="true"/> if the hierarchy is a Miscellaneous Files Project; otherwise <see langword="false"/>.</returns>
        public static bool IsMiscellaneousFilesProject(IVsHierarchy hierarchy)
            => IsNodeType(hierarchy, VSConstants.CLSID.MiscellaneousFilesProject_guid);

        /// <summary>
        /// Gets a value indicating whether a <see cref="IVsHierarchy"/> is a real Project. The <see cref="Microsoft.Internal.VisualStudio.PlatformUI.HierarchyUtilities.IsProject(IVsHierarchyItemIdentity)"/> method returns <see langword="true"/> for Solution Folders, which this method doesn't.
        /// </summary>
        /// <param name="hierarchy">The hierarchy which should be tested.</param>
        /// <returns><see langword="true"/> if the hierarchy is a real Project; otherwise <see langword="false"/>.</returns>
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

        /// <summary>
        /// Gets a value indicating whether a <see cref="IVsHierarchy"/> matches a given <see cref="VSConstants.CLSID"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy which should be tested.</param>
        /// <param name="clsidGuid">The <see cref="VSConstants.CLSID"/> guid which should be tested for.</param>
        /// <returns><see langword="true"/> if the hierarchy matches the given <see cref="VSConstants.CLSID"/>; otherwise <see langword="false"/>.</returns>
        public static bool IsNodeType(IVsHierarchy hierarchy, Guid clsidGuid)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!(hierarchy is IPersist persist))
                return false;

            var result = persist.GetClassID(out Guid pClassID);

            if (ErrorHandler.Failed(result))
                return false;

            return pClassID == clsidGuid;
        }
    }
}
