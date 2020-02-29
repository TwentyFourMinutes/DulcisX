using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Core.Models.Enums.VisualStudio
{
    public static class CommonNodeId
    {
        public const uint Nil = VSConstants.VSITEMID_NIL;
        public const uint Root = VSConstants.VSITEMID_ROOT;
        public const uint Project = Root;
        public const uint VirtualFolder = Root;
        public const uint Solution = Root;
    }
}
