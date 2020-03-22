using DulcisX.Core.Enums;
using DulcisX.Core.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public abstract class ProjectItemNode : BaseNode
    {

        private readonly ProjectNode _parentProject;

        protected ProjectItemNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project.UnderlyingHierarchy, itemId)
        {
            _parentProject = project;
        }

        protected ProjectItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {

        }

        public ProjectNode GetParentProject()
        {
            if (_parentProject is object)
            {
                return _parentProject;
            }

            var parentProject = NodeFactory.GetSolutionItemNode(ParentSolution, UnderlyingHierarchy, CommonNodeIds.Project);

            if (!(parentProject is ProjectNode))
                return null;

            return (ProjectNode)parentProject;
        }

        public override BaseNode GetParent()
        {
            var parentItemId = GetParentNodeId();

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
                yield return NodeFactory.GetProjectItemNode(ParentSolution, _parentProject, UnderlyingHierarchy, node);

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
        }
    }
}
