using DulcisX.Core;
using SimpleInjector;

namespace DulcisX.Nodes
{
    internal class SolutionNodeConfiguration : IContainerConfiguration
    {
        public void ConfigureServices(PackageX package, Container container)
        {
            container.RegisterSingleton(() => Events.SolutionEvents.Create(package.GetSolution()));
            container.RegisterSingleton(() => Events.SolutionBuildEvents.Create(package.GetSolution()));
            container.RegisterSingleton(() => Events.OpenNodeEvents.Create(package.GetSolution()));
            container.RegisterSingleton(() => Events.NodeSelectionEvents.Create(package.GetSolution()));
        }
    }
}
