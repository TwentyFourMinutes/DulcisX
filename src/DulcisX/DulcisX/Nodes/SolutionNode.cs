using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using SimpleInjector;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class SolutionNode : ItemNode, IPhysicalNode
    {
        public override NodeTypes NodeType => NodeTypes.Solution;

        public Container ServiceContainer { get; }

        public SolutionNode(IVsHierarchy hierarchy, Container container) : base(null, hierarchy, CommonNodeId.Solution)
        {
            ServiceContainer = container;
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }

        public override ItemNode GetParent()
            => null;

        public override ItemNode GetParent(NodeTypes nodeType)
            => null;

        public override IEnumerator<ItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
