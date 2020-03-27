using System;

namespace DulcisX.Core.Enums
{
    /// <summary>
    /// Passed to <see cref="Nodes.Events.EventDistributor{TDelegate}.Hook(NodeTypes, TDelegate)"/> in order to restrict the Nodes which the actions get raised for.
    /// </summary>
    [Flags]
    public enum NodeTypes
    {
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.UnknownNode"/>s.
        /// </summary>
        Unknown = 1 << 0,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.DocumentNode"/>s.
        /// </summary>
        Document = 1 << 1,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.FolderNode"/>s.
        /// </summary>
        Folder = 1 << 2,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.ProjectNode"/>s.
        /// </summary>
        Project = 1 << 3,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.ProjectNode"/>s which are a MiscellaneousFilesProject.
        /// </summary>
        MiscellaneousFilesProject = 1 << 4,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.ProjectNode"/>s which are a SolutionItemsProject.
        /// </summary>
        SolutionItemsProject = 1 << 5,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.ProjectNode"/>s which are a VirtualProject.
        /// </summary>
        VirtualProject = 1 << 6,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.SolutionFolderNode"/>s.
        /// </summary>
        SolutionFolder = 1 << 7,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for <see cref="Nodes.SolutionNode"/>s.
        /// </summary>
        Solution = 1 << 8,
        /// <summary>
        /// Restricts the <see cref="Nodes.Events.EventDistributor{TDelegate}"/> to raise events for all Nodes.
        /// </summary>
        All = ~(-1 << 9)
    }
}
