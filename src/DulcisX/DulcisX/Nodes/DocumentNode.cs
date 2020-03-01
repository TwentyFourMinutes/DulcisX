using DulcisX.Core.Models.Enums;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class DocumentNode : ProjectItemNode, IPhysicalNode
    {
        public override NodeTypes NodeType => NodeTypes.Document;

        public DocumentNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project, itemId)
        {
        }

        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = ParentProject.UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            VsHelper.ValidateSuccessStatusCode(result);

            return fullName;
        }
    }
}
