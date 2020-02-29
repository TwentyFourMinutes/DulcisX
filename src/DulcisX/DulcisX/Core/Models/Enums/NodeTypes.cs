using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Core.Models.Enums
{
    [Flags]
    public enum NodeTypes
    {
        Document = 1,
        Folder = 2,
        Project = 4,
        VirtualFolder = 8,
        Solution = 16,
        All = Folder | Project | VirtualFolder | Solution,
        Virtual = VirtualFolder,
        Physical = Folder | Project | Solution
    }
}
