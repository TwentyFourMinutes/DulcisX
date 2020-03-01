﻿using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class ProjectNode : SolutionItemNode, IPhysicalNode
    {
        public override NodeTypes NodeType => NodeTypes.Project;

        public ProjectNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy, CommonNodeId.Project)
        {
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}