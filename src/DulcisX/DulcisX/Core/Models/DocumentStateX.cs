using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core.Models
{
    public class DocumentStateX
    {
        public string Name { get; set; }

        public uint Identifier { get; set; }

        public IVsHierarchy Hierarchy { get; set; }
    }
}
