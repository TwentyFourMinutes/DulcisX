using DulcisX.Core.Extensions;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SimpleInjector;

namespace DulcisX.Core
{
    internal sealed class PackageConfiguration : IContainerConfiguration
    {
        public void ConfigureServices(PackageX package, Container container)
        {
            container.RegisterSingleton(() => package.GetService<SComponentModel, IComponentModel>());

            container.RegisterSingleton(() =>
            {
                var componentModel = container.GetInstance<IComponentModel>();

                return componentModel.GetService<IVsHierarchyItemManager>();
            });

            container.RegisterSingleton(() =>
            {
                var componentModel = container.GetInstance<IComponentModel>();

                return componentModel.GetService<IVsHierarchyItemCollectionProvider>();
            });

            container.RegisterCOMInstance<SVsSolution, IVsSolution>(package);
            container.RegisterCOMInstance<SVsSolutionPersistence, IVsSolutionPersistence>(package);
            container.RegisterCOMInstance<SVsSolutionBuildManager, IVsSolutionBuildManager>(package);
            container.RegisterCOMInstance<SVsRunningDocumentTable, IVsRunningDocumentTable>(package);
            container.RegisterCOMInstance<SVsTrackProjectDocuments, IVsTrackProjectDocuments2>(package);
            container.RegisterCOMInstance<SVsShellMonitorSelection, IVsMonitorSelection>(package);
            container.RegisterCOMInstance<SVsStatusbar, IVsStatusbar>(package);
        }
    }
}
