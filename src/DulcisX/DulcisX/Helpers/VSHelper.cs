using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Exceptions;

namespace DulcisX.Helpers
{
    public static class VsHelper
    {
        public static void ValidateHierarchyType(HierarchyItemTypeX actual, HierarchyItemTypeX expected)
        {
            if (actual != expected)
            {
                throw new InvalidHierarchyTypeExceptionX($"Expected {expected}, but is actually {actual}.");
            }
        }

        public static bool IsItemIdNil(uint itemId)
            => itemId == CommonNodeIds.Nil;
    }
}
