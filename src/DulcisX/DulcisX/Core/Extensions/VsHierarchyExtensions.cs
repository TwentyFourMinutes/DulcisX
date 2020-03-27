using DulcisX.Core.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;

namespace DulcisX.Core.Extensions
{
    /// <summary>
    /// <see cref="IVsHierarchy"/> specific Extensions.
    /// </summary>
    public static class VsHierarchyExtensions
    {
        /// <summary>
        /// Gets the parent <see cref="IVsHierarchy"/> of an <see cref="IVsHierarchy"/>. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the which the parent should be retrieved.</param>
        /// <param name="parentHierarchy">The parent <see cref="IVsHierarchy"/>.</param>
        /// <returns><see langword="true"/> if the operation returned a result; otherwise <see langword="false"/>.</returns>
        public static bool TryGetParentHierarchy(this IVsHierarchy hierarchy, out IVsHierarchy parentHierarchy)
            => hierarchy.TryGetProperty(CommonNodeIds.Root, (int)__VSHPROPID.VSHPROPID_ParentHierarchy, out parentHierarchy);

        /// <summary>
        /// Gets the nested <see cref="IVsHierarchy"/> of an <see cref="IVsHierarchy"/>. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the which the child should be retrieved.</param>
        /// <param name="itemId">The identifier for the node of which the child should be retrieved.</param>
        /// <param name="nestedHierarchy">The chil <see cref="IVsHierarchy"/>.</param>
        /// <returns><see langword="true"/> if the operation suceeded a result; otherwise <see langword="false"/>.</returns>
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

        /// <summary>
        /// Gets a value indicating whether a node is a container.
        /// </summary>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the node.</param>
        /// <param name="itemId">The identifier for the node.</param>
        /// <returns><see langword="true"/> if the node is a container; otherwise <see langword="false"/>.</returns>
        public static bool IsContainer(this IVsHierarchy hierarchy, uint itemId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var successExpandable = hierarchy.TryGetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Expandable, out uint expandableValue);

            var successContainer = hierarchy.TryGetProperty(itemId, (int)__VSHPROPID2.VSHPROPID_Container, out uint containerValue);

            return (successExpandable && expandableValue == 1u) || (successContainer && containerValue == 1u);
        }

        #region Get/Set Properties

        /// <summary>
        /// Gets a property of a node.
        /// </summary>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the node.</param>
        /// <param name="itemId">The identifier for the node.</param>
        /// <param name="propId">The property of which the value should be returned.</param>
        /// <returns>A value containing the property.</returns>
        public static uint GetProperty(this IVsHierarchy hierarchy, uint itemId, int propId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            ErrorHandler.ThrowOnFailure(result);

            return (uint)Convert.ToInt32(val);
        }

        /// <summary>
        /// Gets a property of a node.
        /// </summary>
        /// <typeparam name="TType">The type to which the value of the property should be casted to.</typeparam>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the node.</param>
        /// <param name="itemId">The identifier for the node.</param>
        /// <param name="propId">The property of which the value should be returned.</param>
        /// <returns>A value containing the property.</returns>
        public static TType GetProperty<TType>(this IVsHierarchy hierarchy, uint itemId, int propId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            ErrorHandler.ThrowOnFailure(result);

            return (TType)val;
        }

        /// <summary>
        /// Gets a property of a node.
        /// </summary>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the node.</param>
        /// <param name="itemId">The identifier for the node.</param>
        /// <param name="propId">The property of which the value should be returned.</param>
        /// <returns>A value boxed in an object containing the property.</returns>
        public static object GetPropertyObject(this IVsHierarchy hierarchy, uint itemId, int propId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            ErrorHandler.ThrowOnFailure(result);

            return val;
        }

        /// <summary>
        /// Gets a property of a node. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the node.</param>
        /// <param name="itemId">The identifier for the node.</param>
        /// <param name="propId">The property of which the value should be returned.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns><see langword="true"/> if the operation returned a result; otherwise <see langword="false"/>.</returns>
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

        /// <summary>
        /// Gets a property of a node. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TType">The type to which the value of the property should be casted to.</typeparam>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the node.</param>
        /// <param name="itemId">The identifier for the node.</param>
        /// <param name="propId">The property of which the value should be returned.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns><see langword="true"/> if the operation returned a result; otherwise <see langword="false"/>.</returns>
        public static bool TryGetProperty<TType>(this IVsHierarchy hierarchy, uint itemId, int propId, out TType value)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out var val);

            if (ErrorHandler.Failed(result))
            {
                value = default;
                return false;
            }

            value = (TType)val;

            return true;
        }

        /// <summary>
        /// Gets a property of a node. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the node.</param>
        /// <param name="itemId">The identifier for the node.</param>
        /// <param name="propId">The property of which the value should be returned.</param>
        /// <param name="value">The value of the property boxed in an object.</param>
        /// <returns><see langword="true"/> if the operation returned a result; otherwise <see langword="false"/>.</returns>
        public static bool TryGetPropertyObject(this IVsHierarchy hierarchy, uint itemId, int propId, out object value)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.GetProperty(itemId, propId, out value);

            return ErrorHandler.Succeeded(result);
        }

        /// <summary>
        /// Sets a property of a node. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="hierarchy">The <see cref="IVsHierarchy"/> of the node.</param>
        /// <param name="itemId">The identifier for the node.</param>
        /// <param name="propId">The property of which the value should be returned.</param>
        /// <param name="value">The value which should be set as the new value of the property.</param>
        /// <returns><see langword="true"/> if the operation returned a result; otherwise <see langword="false"/>.</returns>
        public static bool TrySetProperty(this IVsHierarchy hierarchy, uint itemId, int propId, object value)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.SetProperty(itemId, propId, value);

            return ErrorHandler.Succeeded(result);
        }

        #endregion
    }
}
