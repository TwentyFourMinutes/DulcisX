using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Exceptions;

namespace DulcisX.Helpers
{
    public static class VsHelper
    {
        public static void ValidateSuccessStatusCode(int statusCode)
        {
            if (!HasSuccessCode(statusCode))
            {
                throw new InvalidVSStatusCodeExceptionX(statusCode);
            }
        }

        public static bool HasSuccessCode(int statusCode)
            => statusCode == CommonStatusCode.Success;

        public static bool HasFailed(int statusCode)
            => statusCode == CommonStatusCode.Failure;

        public static void ValidateHierarchyType(HierarchyItemTypeX actual, HierarchyItemTypeX expected)
        {
            if (actual != expected)
            {
                throw new InvalidHierarchyTypeExceptionX($"Expected {expected}, but is actually {actual}.");
            }
        }

        public static bool IsItemIdNil(uint itemId)
            => itemId == CommonNodeId.Nil;
    }
}
