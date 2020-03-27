using DulcisX.Core.Enums.VisualStudio;

namespace DulcisX.Helpers
{
    /// <summary>
    /// Provides useful helper methods across the Visual Studio SDK.
    /// </summary>
    public static class VsHelper
    {
        /// <summary>
        /// Gets a value indicating whether the identifier of the node is Nil.
        /// </summary>
        /// <param name="itemId">The identifier of the node which should be checked.</param>
        /// <returns><see langword="true"/> if the identifier is Nil; otherwise <see langword="false"/>.</returns>
        public static bool IsItemIdNil(uint itemId)
            => itemId == CommonNodeIds.Nil;
    }
}
