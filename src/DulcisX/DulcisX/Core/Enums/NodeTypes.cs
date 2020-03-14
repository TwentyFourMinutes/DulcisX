using System;

namespace DulcisX.Core.Enums
{
    [Flags]
    public enum NodeTypes
    {
        Unknown = 1 << 0,
        Document = 1 << 1,
        Folder = 1 << 2,
        Project = 1 << 3,
        MiscellaneousFilesProject = 1 << 4,
        SolutionItemsProject = 1 << 5,
        VirtualProject = 1 << 6,
        VirtualFolder = 1 << 7,
        Solution = 1 << 8,
        All = ~(-1 << 9)
    }
}
