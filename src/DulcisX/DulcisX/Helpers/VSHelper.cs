using DulcisX.Core.Enums.VisualStudio;

namespace DulcisX.Helpers
{
    public static class VsHelper
    {
        public static bool IsItemIdNil(uint itemId)
            => itemId == CommonNodeIds.Nil;

        public static bool IsTrue(int value)
            => value == 1;

        public static bool IsFalse(int value)
            => value == 0;
    }
}
