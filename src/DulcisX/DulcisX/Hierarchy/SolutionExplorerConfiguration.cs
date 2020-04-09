using DulcisX.Core.Extensions;
using DulcisX.Hierarchy.Events;
using Microsoft.VisualStudio.Shell.Interop;
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

            container.RegisterCOMInstance<SVsUIHierWinClipboardHelper, IVsUIHierWinClipboardHelper>(package);
        }
    }
}
