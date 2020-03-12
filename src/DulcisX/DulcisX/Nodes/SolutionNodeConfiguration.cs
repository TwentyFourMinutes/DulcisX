using DulcisX.Core;
using SimpleInjector;

namespace DulcisX.Nodes
{
    internal class SolutionNodeConfiguration : IContainerConfiguration
    {
        public void ConfigureServices(PackageX package, Container container)
        {
            var solution = package.GetSolution();

            container.RegisterSingleton(() => Events.SolutionEvents.Create(solution));
            container.RegisterSingleton(() => Events.SolutionBuildEvents.Create(solution));
            container.RegisterSingleton(() => Events.OpenNodeEvents.Create(solution));
            container.RegisterSingleton(() => Events.NodeSelectionEvents.Create(solution));
        }
    }
}
