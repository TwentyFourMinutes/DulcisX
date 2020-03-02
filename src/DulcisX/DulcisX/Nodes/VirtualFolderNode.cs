using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
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
