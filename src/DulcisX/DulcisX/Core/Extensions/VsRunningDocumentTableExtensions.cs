using DulcisX.Hierarchy;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core.Extensions
{
    /// <summary>
    /// <see cref="IVsRunningDocumentTable"/> and <see cref="IVsRunningDocTableEvents"/> specific Extensions.
    /// </summary>
    public static class VsRunningDocumentTableExtensions
    {
        /// <summary>
        /// Returns an <see cref="IPhysicalNode"/> for the given document cookie.
        /// </summary>
        /// <param name="rdt">The running document table of the current environment.</param>
        /// <param name="docCookie">The document cookie, which identfies the node within the <see cref="IVsRunningDocumentTable"/>.</param>
        /// <param name="solution">The solution of the current environment, usually <see cref="PackageX.Solution"/></param>
        /// <returns>A new instance of an <see cref="IPhysicalNode"/> for the given document cookie.</returns>
        public static IPhysicalNode GetNode(this IVsRunningDocumentTable rdt, uint docCookie, SolutionNode solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = rdt.GetDocumentInfo(docCookie, out _, out _, out _, out _, out var hierarchy, out uint itemId, out _);

            ErrorHandler.ThrowOnFailure(result);

            return (IPhysicalNode)NodeFactory.GetItemNode(solution, hierarchy, itemId);
        }
    }
}