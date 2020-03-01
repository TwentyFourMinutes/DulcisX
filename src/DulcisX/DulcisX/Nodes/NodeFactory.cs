using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    internal static class NodeFactory
    {
        internal static ItemNode GetItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            var node = GetProjectItemNode(solution, null, hierarchy, itemId);

            if (node is null)
            {
                node = GetSolutionItemNode(solution, hierarchy, itemId);
            }

            return node;
        }

        internal static ItemNode GetSolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            var manager = solution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            var hierarchyItem = manager.GetHierarchyItem(hierarchy, itemId);

            if (HierarchyUtilities.IsSolutionFolder(hierarchyItem.HierarchyIdentity))
            {
                return new VirtualFolderNode(solution, hierarchy, itemId);
            }
            else if (HierarchyUtilities.IsProject(hierarchyItem.HierarchyIdentity))
            {
                return new VirtualFolderNode(solution, hierarchy, itemId);
            }

            return solution;
        }

        internal static ItemNode GetProjectItemNode(SolutionNode solution, ProjectNode project, IVsHierarchy hierarchy, uint itemId)
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

            return null;
        }
    }
}
