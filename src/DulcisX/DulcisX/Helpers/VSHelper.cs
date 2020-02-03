using DulcisX.Core.Models.Enums;
using DulcisX.Exceptions;
using Microsoft.VisualStudio;

namespace DulcisX.Helpers
{
    public static class VsHelper
    {
        public static void ValidateSuccessStatusCode(int statusCode)
        {
            if (!HasSuccessCode(statusCode))
            {
                throw new InvalidVSStatusCodeException(statusCode);
            }
        }

        public static bool HasSuccessCode(int statusCode)
            => statusCode == VSConstants.S_OK;

        public static bool HasFailed(int statusCode)
            => statusCode == VSConstants.E_FAIL;

        public static void ValidateHierarchyType(HierarchyItemTypeX actual, HierarchyItemTypeX expected)
        {
            if (actual != expected)
            {
                throw new InvalidHierarchyTypeException($"Expected {expected}, but is actually {actual}.");
            }
        }

        public static bool IsItemIdNil(uint itemId)
            => itemId == VSConstants.VSITEMID_NIL;
    }
}
