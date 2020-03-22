using DulcisX.Core.Enums;
using DulcisX.Core.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    public class SolutionFolderNode : SolutionItemNode
    {
        public override NodeTypes NodeType => NodeTypes.SolutionFolder;

        public SolutionFolderNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy, CommonNodeIds.SolutionFolder)
        {
        }
    }
}
