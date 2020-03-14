
using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public abstract class ProjectItemNode : BaseNode
    {

        protected readonly ProjectNode ParentProject;

        protected ProjectItemNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project.UnderlyingHierarchy, itemId)
        {
            ParentProject = project;
        }

        protected ProjectItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {

        }

        public ProjectNode GetParentProject()
        {
            if (ParentProject is object)
            {
                return ParentProject;
            }

            var parentProject = NodeFactory.GetSolutionItemNode(ParentSolution, UnderlyingHierarchy, CommonNodeIds.Project);

            if (!(parentProject is ProjectNode))
                return null;

            return (ProjectNode)parentProject;
        }

        public override BaseNode GetParent()
        {
            var parentItemId = UnderlyingHierarchy.GetProperty(ItemId, (int)__VSHPROPID.VSHPROPID_Parent);

            if (parentItemId == CommonNodeIds.Nil)
            {
                return null;
            }

            return NodeFactory.GetProjectItemNode(ParentSolution, GetParentProject(), UnderlyingHierarchy, parentItemId);
        }

        public override BaseNode GetParent(NodeTypes nodeType)
        {
            if (nodeType == NodeTypes.Project)
            {
                return GetParentProject();
            }

            return base.GetParent(nodeType);
        }

        public override IEnumerable<BaseNode> GetChildren()
        {
            var node = HierarchyUtilities.GetFirstChild(UnderlyingHierarchy, ItemId, true);

            while (!VsHelper.IsItemIdNil(node))
            {
                yield return NodeFactory.GetProjectItemNode(ParentSolution, ParentProject, UnderlyingHierarchy, node);

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }

        }
    }
}
