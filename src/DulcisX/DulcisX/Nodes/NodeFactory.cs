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

            var hierarchyIdentity = GetHierarchyIdentity(solution, hierarchy, itemId);

            var node = GetProjectItemNode(solution, null, hierarchy, itemId, hierarchyIdentity);

            if (node is UnknownNode)
            {
                node = GetSolutionItemNode(solution, hierarchy, itemId, hierarchyIdentity);
            }

            return node;
        }

        internal static BaseNode GetSolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId, IVsHierarchyItemIdentity hierarchyIdentity = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            hierarchyIdentity = hierarchyIdentity ?? GetHierarchyIdentity(solution, hierarchy, itemId);

            var project = GetProject(solution, null, hierarchy, hierarchyIdentity);

            if (project is null)
            {
                if (HierarchyUtilities.IsSolutionFolder(hierarchyIdentity))
                {
                    return new VirtualFolderNode(solution, hierarchy);
                }
                else if (HierarchyUtilities.IsSolutionNode(hierarchy, itemId))
                {
                    return solution;
                }
            }
            else
            {
                return project;
            }

            return new UnknownNode(solution, hierarchy, itemId);
        }

        internal static BaseNode GetProjectItemNode(SolutionNode solution, ProjectNode parentProject, IVsHierarchy hierarchy, uint itemId, IVsHierarchyItemIdentity hierarchyIdentity = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            hierarchyIdentity = hierarchyIdentity ?? GetHierarchyIdentity(solution, hierarchy, itemId);

            if (itemId == CommonNodeIds.Root)
            {
                var project = GetProject(solution, parentProject, hierarchy, hierarchyIdentity);

                if (project is object)
                {
                    return project;
                }
            }
            else
            {
                if (HierarchyUtilities.IsPhysicalFolder(hierarchyIdentity))
                {
                    if (parentProject is null)
                        return new FolderNode(solution, hierarchy, itemId);
                    else
                        return new FolderNode(solution, parentProject, itemId);
                }
                else if (HierarchyUtilities.IsPhysicalFile(hierarchyIdentity))
                {
                    if (parentProject is null)
                        return new DocumentNode(solution, hierarchy, itemId);
                    else
                        return new DocumentNode(solution, parentProject, itemId);
                }
            }

            return new UnknownNode(solution, hierarchy, itemId);
        }

        private static ProjectNode GetProject(SolutionNode solution, ProjectNode parentProject, IVsHierarchy hierarchy, IVsHierarchyItemIdentity hierarchyIdentity)
        {
            if (parentProject is object)
                return parentProject;

            if (ExtendedHierarchyUtilities.IsRealProject(hierarchy) ||
                    HierarchyUtilities.IsFaultedProject(hierarchyIdentity))
            {
                return new ProjectNode(solution, hierarchy);
            }
            else if (HierarchyUtilities.IsVirtualProject(hierarchyIdentity))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.VirtualProject);
            }
            else if (ExtendedHierarchyUtilities.IsMiscellaneousFilesProject(hierarchy))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.MiscellaneousFilesProject);
            }
            else if (ExtendedHierarchyUtilities.IsSolutionItemsProject(hierarchy))
            {
                return new ProjectNode(solution, hierarchy, NodeTypes.SolutionItemsProject);
            }

            return null;
        }

        private static IVsHierarchyItemIdentity GetHierarchyIdentity(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            var manager = solution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            return manager.GetHierarchyItem(hierarchy, itemId).HierarchyIdentity;
        }
    }
}
