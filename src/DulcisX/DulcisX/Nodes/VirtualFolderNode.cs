using DulcisX.Core.Models.Enums;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    public class VirtualFolderNode : SolutionItemNode
    {
        public override NodeTypes NodeType => NodeTypes.VirtualFolder;

        public VirtualFolderNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {
        }
    }
}
