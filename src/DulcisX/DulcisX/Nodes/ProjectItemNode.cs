using DulcisX.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Nodes
{
    public abstract class ProjectItemNode : SolutionItemNode
    {
        public ProjectX ParentProject { get; }

        protected ProjectItemNode(SolutionNode solution, ProjectX project) : base(solution, project.UnderlyingHierarchy)
        {
            ParentProject = project;
        }
    }
}
