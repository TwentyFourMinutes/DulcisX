using DulcisX.Core.Models;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DulcisX.Core.Extensions
{
    public static class VsHierarchyExtensions
    {
        public static bool TryGetParentHierarchy(this IVsHierarchy hierarchy, out IVsHierarchy parentHierarchy)
            => hierarchy.TryGetProperty(CommonNodeIds.Root, (int)__VSHPROPID.VSHPROPID_ParentHierarchy, out parentHierarchy);

        public static bool TryGetNestedHierarchy(this IVsHierarchy hierarchy, uint itemId, out IVsHierarchy nestedHierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var guid = typeof(IVsHierarchy).GUID;

            var result = hierarchy.GetNestedHierarchy(itemId, ref guid, out var hierarchyPointer, out _);

            if (ErrorHandler.Failed(result) || hierarchyPointer == IntPtr.Zero)
            {
                nestedHierarchy = null;
                return false;
            }

            nestedHierarchy = (IVsHierarchy)Marshal.GetObjectForIUnknown(hierarchyPointer);
            Marshal.Release(hierarchyPointer);

            return true;
        }

        public static HierarchyItemTypeX GetHierarchyItemType(this IVsHierarchy hierarchy, uint itemId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (itemId == VSConstants.VSITEMID_ROOT)
            {
                if (hierarchy.TryGetPropertyObject(itemId, (int)__VSHPROPID5.VSHPROPID_OutputType, out _))
                {
                    return HierarchyItemTypeX.Project;
                }
                else if (hierarchy is IVsSolution)
                {
                    return HierarchyItemTypeX.Solution;
                }
                else
                {
                    return HierarchyItemTypeX.VirtualFolder;
                }
            }
            else
            {
                var project = (IVsProject)hierarchy;

                var result = project.GetMkDocument(itemId, out var path);

                ErrorHandler.ThrowOnFailure(result);

                if (File.Exists(path))
                {
                    return HierarchyItemTypeX.Document;
                }
                else
                {
                    return HierarchyItemTypeX.Folder;
                }
            }
        }

        public static HierarchyItemX ConstructHierarchyItem(this IVsHierarchy hierarchy, uint itemId, SolutionX solution, HierarchyItemTypeX? itemType = null, ProjectX parentProject = null, HierarchyItemX parentItem = null)
        {
            switch (itemType ?? hierarchy.GetHierarchyItemType(itemId))
            {
                case HierarchyItemTypeX.Solution:
                    return solution;
                case HierarchyItemTypeX.Project:
                    return new ProjectX(hierarchy, itemId, solution, parentItem);
                case HierarchyItemTypeX.Folder:
                    return new HierarchyItemX(hierarchy, itemId, HierarchyItemTypeX.Folder, ConstructorInstance.FromValue(solution), ConstructorInstance.FromValue(parentProject), parentItem);
                case HierarchyItemTypeX.VirtualFolder:
                    return new HierarchyItemX(hierarchy, itemId, HierarchyItemTypeX.VirtualFolder, ConstructorInstance.FromValue(solution), ConstructorInstance.FromValue(parentProject), parentItem);
                case HierarchyItemTypeX.Document:
                    return new DocumentX(hierarchy, itemId, solution, parentItem: parentItem);
                default:
                    return null;
            }
        }

        public static bool IsContainer(this IVsHierarchy hierarchy, uint itemId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var successExpandable = hierarchy.TryGetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Expandable, out uint expandableValue);

            var successContainer = hierarchy.TryGetProperty(itemId, (int)__VSHPROPID2.VSHPROPID_Container, out uint containerValue);

            return (successExpandable && expandableValue == 1u) || (successContainer && containerValue == 1u);
        }

        #region Get/Set Properties

        public static uint GetProperty(this IVsHierarchy hierarchy, uint itemId, int propId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            ErrorHandler.ThrowOnFailure(result);

            return (uint)Convert.ToInt32(val);
        }

        public static TType GetProperty<TType>(this IVsHierarchy hierarchy, uint itemId, int propId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            ErrorHandler.ThrowOnFailure(result);

            return (TType)val;
        }

        public static object GetPropertyObject(this IVsHierarchy hierarchy, uint itemId, int propId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            ErrorHandler.ThrowOnFailure(result);

            return val;
        }

        public static bool TryGetProperty(this IVsHierarchy hierarchy, uint itemId, int propId, out uint value)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            if (ErrorHandler.Failed(result))
            {
                value = default;
                return false;
            }

            value = (uint)Convert.ToInt32(val);

            return true;
        }

        public static bool TryGetProperty<TType>(this IVsHierarchy hierarchy, uint itemId, int propId, out TType type)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            if (ErrorHandler.Failed(result))
            {
                type = default;
                return false;
            }

            type = (TType)val;

            return true;
        }

        public static bool TryGetPropertyObject(this IVsHierarchy hierarchy, uint itemId, int propId, out object obj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out obj);

            return ErrorHandler.Succeeded(result);
        }

        public static bool TrySetProperty(this IVsHierarchy hierarchy, uint itemId, int propId, object val)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.SetProperty(itemId, propId, val);

            return ErrorHandler.Succeeded(result);
        }

        #endregion
    }
}
