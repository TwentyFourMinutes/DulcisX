using System;

namespace DulcisX.Core.Models.Enums
{
    [Flags]
    public enum NodeTypes
    {
        Document = 1,
        Folder = 2,
        Project = 4,
        MiscellaneousFilesProject = 8,
        SolutionItemsProject = 16,
        VirtualProject = 32,
        VirtualFolder = 64,
        Solution = 128,
        All = ~0
    }
}
