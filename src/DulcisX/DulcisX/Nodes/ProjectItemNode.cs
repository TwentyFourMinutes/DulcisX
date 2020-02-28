
namespace DulcisX.Nodes
{
    public abstract class ProjectItemNode : SolutionItemNode
    {
        protected readonly ProjectNode ParentProject;

        protected ProjectItemNode(SolutionNode solution, ProjectNode project) : base(solution, project.UnderlyingHierarchy)
        {
            ParentProject = project;
        }

        public abstract ProjectNode GetParentProject();
    }
}
