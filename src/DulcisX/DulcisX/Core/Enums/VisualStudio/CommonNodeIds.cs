using Microsoft.VisualStudio;

namespace DulcisX.Core.Enums.VisualStudio
{
    public static class CommonNodeIds
    {
        public const uint Nil = VSConstants.VSITEMID_NIL;
        public const uint MutlipleSelectedNodes = VSConstants.VSITEMID_SELECTION;
        public const uint Root = VSConstants.VSITEMID_ROOT;
        public const uint Project = Root;
        public const uint VirtualFolder = Root;
        public const uint Solution = Root;
    }
}
