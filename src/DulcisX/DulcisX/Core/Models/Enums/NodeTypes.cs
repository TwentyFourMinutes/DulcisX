using System;

namespace DulcisX.Core.Models.Enums
{
    [Flags]
    public enum NodeTypes
    {
        Unknown = 1,
        Document = 2,
        Folder = 4,
        Project = 8,
        MiscellaneousFilesProject = 16,
        SolutionItemsProject = 32,
        VirtualProject = 64,
        VirtualFolder = 128,
        Solution = 256,
        All = ~0
    }
}
