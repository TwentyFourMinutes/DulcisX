
using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    public abstract class ProjectItemNode : ItemNode
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

            return new ProjectNode(ParentSolution, UnderlyingHierarchy);
        }

        public override ItemNode GetParent()
        {
            var parentItemId = UnderlyingHierarchy.GetProperty(ItemId, (int)__VSHPROPID.VSHPROPID_Parent);

            if (parentItemId == CommonNodeId.Nil)
            {
                return null;
            }

            return NodeFactory.GetProjectItemNode(ParentSolution, GetParentProject(), UnderlyingHierarchy, parentItemId);
        }

        public override ItemNode GetParent(NodeTypes nodeType)
        {
            if (nodeType == NodeTypes.Project)
            {
                return GetParentProject();
            }

            return base.GetParent(nodeType);
        }
    }
}
