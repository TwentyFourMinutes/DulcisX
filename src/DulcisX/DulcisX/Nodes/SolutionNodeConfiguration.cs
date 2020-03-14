using DulcisX.Core;
using SimpleInjector;

namespace DulcisX.Nodes
{
    internal class SolutionNodeConfiguration : IContainerConfiguration
    {
        public void ConfigureServices(PackageX package, Container container)
        {
            container.RegisterSingleton(() => Events.SolutionEvents.Create(package.Solution));
            container.RegisterSingleton(() => Events.SolutionBuildEvents.Create(package.Solution));
            container.RegisterSingleton(() => Events.OpenNodeEvents.Create(package.Solution));
            container.RegisterSingleton(() => Events.NodeSelectionEvents.Create(package.Solution));
        }
    }
}
