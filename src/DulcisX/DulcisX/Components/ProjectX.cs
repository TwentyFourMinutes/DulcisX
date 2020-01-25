using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Components
{
    public class ProjectX
    {
        public IVsHierarchy UnderlyingHierarchy { get; }

        public ProjectX(IVsHierarchy hierarchy)
            => UnderlyingHierarchy = hierarchy;

        public void Test()
        {
            
        }
    }
}
