using DulcisX.Core.Models.Enums.VisualStudio;

namespace DulcisX.Helpers
{
    public static class VsHelper
    {
        public static bool IsItemIdNil(uint itemId)
            => itemId == CommonNodeIds.Nil;
    }
}
