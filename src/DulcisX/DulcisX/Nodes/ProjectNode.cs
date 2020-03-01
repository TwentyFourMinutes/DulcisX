using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class ProjectNode : SolutionItemNode, IPhysicalNode
    {
        public IVsProject UnderlyingProject => (IVsProject)UnderlyingHierarchy;

        public override NodeTypes NodeType => NodeTypes.Project;

        public ProjectNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy, CommonNodeId.Project)
        {
        }

        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            VsHelper.ValidateSuccessStatusCode(result);

            return fullName;
        }
    }
}
