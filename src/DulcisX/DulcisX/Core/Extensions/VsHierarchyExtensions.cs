using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
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
