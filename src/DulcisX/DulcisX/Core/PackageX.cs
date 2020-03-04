using DulcisX.Core.Extensions;
using DulcisX.Core.Models;
using DulcisX.Core.Models.Interfaces;
using DulcisX.Nodes;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SimpleInjector;
using StringyEnums;
using System;
using System.Reflection;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace DulcisX.Core
{
    public abstract class PackageX : AsyncPackage, IServiceProviders
    {
        public event Func<CancellationToken, Task> OnInitializeAsync;

        public Container ServiceContainer { get; }

        private readonly Action<Container> _consumerServices;

        #region Constructors

        protected PackageX()
        {
            ServiceContainer = new Container();
        }

        protected PackageX(Action<Container> configuration) : this()
        {
            _consumerServices = configuration;
        }

        static PackageX()
        {
            EnumCore.Init(initializer => initializer.InitWith(Assembly.GetExecutingAssembly()), false);
        }

        #endregion

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            AddDefaultServices(ServiceContainer);

            _consumerServices?.Invoke(ServiceContainer);

            await OnInitializeAsync?.Invoke(cancellationToken);

            await base.InitializeAsync(cancellationToken, progress);
        }

        private void AddDefaultServices(Container container)
        {
            container.RegisterSingleton(() =>
            {
                return this.GetService<SComponentModel, IComponentModel>();
            });

            container.RegisterSingleton(() =>
            {
                var componentModel = container.GetInstance<IComponentModel>();

                return componentModel.GetService<IVsHierarchyItemManager>();
            });

            container.RegisterSingleton(() =>
            {
                return COMContainer.Create(this.GetService<SVsSolution, IVsSolution>());
            });

            container.RegisterSingleton(() =>
            {
                return COMContainer.Create(this.GetService<SVsSolutionPersistence, IVsSolutionPersistence>());
            });

            container.RegisterSingleton(() =>
            {
                return COMContainer.Create(this.GetService<SVsSolutionBuildManager, IVsSolutionBuildManager>());
            });

            container.RegisterSingleton(() =>
            {
                return COMContainer.Create(this.GetService<SVsRunningDocumentTable, IVsRunningDocumentTable>());
            });
        }

        #region Services

        public TService GetGlobalService<TService>() where TService : class
            => GetGlobalService<TService, TService>();

        public TService GetGlobalService<TBaseService, TService>() where TBaseService : class
                                                                   where TService : class
            => GetGlobalService(typeof(TBaseService)) as TService;

        private IServiceProviders GetServiceProviders()
            => (IServiceProviders)this;

        #endregion

        private SolutionNode _solutionNode;

        public SolutionNode GetSolution()
        {
            if (_solutionNode is object)
            {
                return _solutionNode;
            }

            var solution = ServiceContainer.GetCOMInstance<IVsSolution>();

            _solutionNode = new SolutionNode(solution, ServiceContainer);

            return _solutionNode;
        }
    }
}
