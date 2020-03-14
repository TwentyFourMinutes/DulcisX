using DulcisX.Core.Enums;
using DulcisX.Core.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    public class VirtualFolderNode : SolutionItemNode
    {
        public override NodeTypes NodeType => NodeTypes.VirtualFolder;

        public VirtualFolderNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy, CommonNodeIds.VirtualFolder)
        {
        }
    }
}
