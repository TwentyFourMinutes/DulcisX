using DulcisX.Core.Enums;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents the most basic <see cref="ProjectNode"/> children Node.
    /// </summary>
    public abstract class ProjectItemNode : BaseNode, IProjectItemNode
    {
        private readonly ProjectNode _parentProject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectItemNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the Node sits in.</param>
        /// <param name="project">The Project in which the Node sits in.</param>
        /// <param name="itemId">The Unique Identifier for the Node in the <paramref name="project"/>.</param>
        protected ProjectItemNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project.UnderlyingHierarchy, itemId)
        {
            _parentProject = project;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectItemNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the Node sits in.</param>
        /// <param name="hierarchy">The Hierarchy of the Project in which the <see cref="ProjectItemNode"/> sits in.</param>
        /// <param name="itemId">The Unique Identifier for the <see cref="ProjectItemNode"/> in the <paramref name="hierarchy"/>.</param>
        protected ProjectItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {

        }

        /// <summary>
        /// Returns the Project in which the Nodes sits in.
        /// </summary>
        /// <returns>The parent Project in which the Nodes sits in.</returns>
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

        /// <inheritdoc/>
        public override BaseNode GetParent()
        {
            var parentItemId = GetParentNodeId();

            if (parentItemId == CommonNodeIds.Nil)
            {
                return null;
            }

            return NodeFactory.GetProjectItemNode(ParentSolution, GetParentProject(), UnderlyingHierarchy, parentItemId);
        }

        /// <inheritdoc/>
        public override BaseNode GetParent(NodeTypes nodeType)
        {
            if (nodeType == NodeTypes.Project)
            {
                return GetParentProject();
            }

            return base.GetParent(nodeType);
        }

        /// <inheritdoc/>
        public override IEnumerable<BaseNode> GetChildren()
        {
            var node = HierarchyUtilities.GetFirstChild(UnderlyingHierarchy, ItemId, true);

            while (!VsHelper.IsItemIdNil(node))
            {
                yield return NodeFactory.GetProjectItemNode(ParentSolution, _parentProject, UnderlyingHierarchy, node);

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
        }

        /// <summary>
        /// Returns the default namespace for the current Node.
        /// </summary>
        /// <returns>A string which contains the default namespace for the current Node.</returns>
        public string GetDefaultNamespace()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, ItemId, (int)__VSHPROPID.VSHPROPID_DefaultNamespace);
        }
    }
}