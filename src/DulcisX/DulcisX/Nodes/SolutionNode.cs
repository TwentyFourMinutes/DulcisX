using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace DulcisX.Nodes
{
    public class SolutionNode : ItemNode, IPhysicalNode
    {
        public IVsSolution UnderlyingSolution => (IVsSolution)UnderlyingHierarchy;

        public override NodeTypes NodeType => NodeTypes.Solution;

        public override SolutionNode ParentSolution => this;

        public Container ServiceContainer { get; }

        public SolutionNode(IVsHierarchy hierarchy, Container container) : base(null, hierarchy, CommonNodeId.Solution)
        {
            ServiceContainer = container;
        }

        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionFileName, out var fullName);

            VsHelper.ValidateSuccessStatusCode(result);

            return (string)fullName;
        }

        public override ItemNode GetParent()
            => null;

        public override ItemNode GetParent(NodeTypes nodeType)
            => null;
    }
}
