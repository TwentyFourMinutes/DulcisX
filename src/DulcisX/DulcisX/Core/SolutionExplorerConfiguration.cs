using DulcisX.Nodes.Events;
using SimpleInjector;

namespace DulcisX.Core
{
    internal class SolutionExplorerConfiguration : IContainerConfiguration
    {
        public void ConfigureServices(PackageX package, Container container)
        {
            container.RegisterSingleton(() => SolutionEvents.Create(package.SolutionExplorer.Solution));
            container.RegisterSingleton(() => SolutionBuildEvents.Create(package.SolutionExplorer.Solution));
            container.RegisterSingleton(() => OpenNodeEvents.Create(package.SolutionExplorer.Solution));
            container.RegisterSingleton(() => NodeSelectionEvents.Create(package.SolutionExplorer.Solution));
            container.RegisterSingleton(() => ProjectNodeChangeEvents.Create(package.SolutionExplorer.Solution));
        }
    }
}
