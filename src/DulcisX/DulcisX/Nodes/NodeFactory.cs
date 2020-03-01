using DulcisX.Core.Models.Enums;
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
            var node = GetProjectItemNode(solution, null, hierarchy, itemId);

            if (node is null)
            {
                node = GetSolutionItemNode(solution, hierarchy, itemId);
            }

            return node;
        }

        internal static BaseNode GetSolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            var manager = solution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            var hierarchyItem = manager.GetHierarchyItem(hierarchy, itemId);

            if (ExtentedHierarchyUtilities.IsRealProject(hierarchy) ||
                HierarchyUtilities.IsFaultedProject(hierarchyItem.HierarchyIdentity) ||
                HierarchyUtilities.IsStubHierarchy(hierarchy))
            {
                return new ProjectNode(solution, hierarchy);
            }
            else if (HierarchyUtilities.IsVirtualProject(hierarchyItem.HierarchyIdentity))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.VirtualProject);
            }
            else if (ExtentedHierarchyUtilities.IsSolutionItemsProject(hierarchy))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.SolutionItemsProject);
            }
            else if (HierarchyUtilities.IsSolutionFolder(hierarchyItem.HierarchyIdentity))
            {
                return new VirtualFolderNode(solution, hierarchy, itemId);
            }
            else if (HierarchyUtilities.IsSolutionNode(hierarchyItem.HierarchyIdentity))
            {
                return solution;
            }

            return null;
        }

        internal static BaseNode GetProjectItemNode(SolutionNode solution, ProjectNode project, IVsHierarchy hierarchy, uint itemId)
        {
            var manager = solution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            var hierarchyItem = manager.GetHierarchyItem(hierarchy, itemId);

            if (HierarchyUtilities.IsPhysicalFolder(hierarchyItem.HierarchyIdentity))
            {
                return new FolderNode(solution, project, itemId);
            }
            else if (HierarchyUtilities.IsProject(hierarchyItem.HierarchyIdentity))
            {
                return project;
            }
            else if (HierarchyUtilities.IsPhysicalFile(hierarchyItem.HierarchyIdentity))
            {
                return new DocumentNode(solution, project, itemId);
            }
            else if (ExtentedHierarchyUtilities.IsRealProject(hierarchy) ||
                HierarchyUtilities.IsFaultedProject(hierarchyItem.HierarchyIdentity) ||
                HierarchyUtilities.IsStubHierarchy(hierarchy))
            {
                return new ProjectNode(solution, hierarchy);
            }
            else if (HierarchyUtilities.IsVirtualProject(hierarchyItem.HierarchyIdentity))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.VirtualProject);
            }
            else if (ExtentedHierarchyUtilities.IsMiscellaneousFilesProject(hierarchy))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.MiscellaneousFilesProject);
            }
            else if (ExtentedHierarchyUtilities.IsSolutionItemsProject(hierarchy))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.SolutionItemsProject);
            }
            else if (HierarchyUtilities.IsSolutionFolder(hierarchyItem.HierarchyIdentity))
            {
                return new VirtualFolderNode(solution, hierarchy, itemId);
            }

            return null;
        }
    }
}
