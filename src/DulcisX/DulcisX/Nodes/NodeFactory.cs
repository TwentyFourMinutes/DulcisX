using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    internal static class NodeFactory
    {
        internal static BaseNode GetItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var node = GetProjectItemNode(solution, null, hierarchy, itemId);

            if (node is UnknownNode)
            {
                node = GetSolutionItemNode(solution, hierarchy, itemId);
            }

            return node;
        }

        internal static BaseNode GetSolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var manager = solution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            var hierarchyItem = manager.GetHierarchyItem(hierarchy, itemId);

            if (ExtentedHierarchyUtilities.IsRealProject(hierarchy) ||
                HierarchyUtilities.IsFaultedProject(hierarchyItem.HierarchyIdentity) ||
                HierarchyUtilities.IsStubHierarchy(hierarchy))
            {
                return new ProjectNode(solution, hierarchy);
            }
            else if (ExtentedHierarchyUtilities.IsSolutionItemsProject(hierarchy))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.SolutionItemsProject);
            }
            else if (HierarchyUtilities.IsVirtualProject(hierarchyItem.HierarchyIdentity))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.VirtualProject);
            }
            else if (HierarchyUtilities.IsSolutionFolder(hierarchyItem.HierarchyIdentity))
            {
                return new VirtualFolderNode(solution, hierarchy);
            }
            else if (HierarchyUtilities.IsSolutionNode(hierarchyItem.HierarchyIdentity))
            {
                return solution;
            }

            return new UnknownNode(solution, hierarchy, itemId);
        }

        internal static BaseNode GetProjectItemNode(SolutionNode solution, ProjectNode project, IVsHierarchy hierarchy, uint itemId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var manager = solution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            var hierarchyItem = manager.GetHierarchyItem(hierarchy, itemId);

            if (itemId == CommonNodeIds.Root)
            {
                if (ExtentedHierarchyUtilities.IsRealProject(hierarchy) ||
                    HierarchyUtilities.IsFaultedProject(hierarchyItem.HierarchyIdentity) ||
                    HierarchyUtilities.IsStubHierarchy(hierarchy))
                {
                    return project ?? new ProjectNode(solution, hierarchy);
                }
                else if (HierarchyUtilities.IsVirtualProject(hierarchyItem.HierarchyIdentity))
                {
                    return project ?? new ProjectNode(solution, hierarchy, NodeTypes.VirtualProject);
                }
                else if (ExtentedHierarchyUtilities.IsMiscellaneousFilesProject(hierarchy))
                {
                    return project ?? new ProjectNode(solution, hierarchy, NodeTypes.MiscellaneousFilesProject);
                }
                else if (ExtentedHierarchyUtilities.IsSolutionItemsProject(hierarchy))
                {
                    return project ?? new ProjectNode(solution, hierarchy, NodeTypes.SolutionItemsProject);
                }
            }
            else
            {
                if (HierarchyUtilities.IsPhysicalFolder(hierarchyItem.HierarchyIdentity))
                {
                    return new FolderNode(solution, project, itemId);
                }
                else if (HierarchyUtilities.IsPhysicalFile(hierarchyItem.HierarchyIdentity))
                {
                    return new DocumentNode(solution, project, itemId);
                }
            }

            return new UnknownNode(solution, hierarchy, itemId);
        }
    }
}
