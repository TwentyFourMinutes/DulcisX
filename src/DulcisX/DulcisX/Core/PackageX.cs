using DulcisX.Core.Extensions;
using DulcisX.Core.Models;
using DulcisX.Nodes;
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
        public event Func<CancellationToken, IProgress<ServiceProgressData>, Task> OnInitializeAsync;
        public event Action OnDisposing;

        private SolutionNode _solution;

        public SolutionNode Solution
        {
            get
            {
                if (_solution is null)
                {
                    var solution = ServiceContainer.GetCOMInstance<IVsSolution>();

                    _solution = new SolutionNode(solution, ServiceContainer);
                }

                return _solution;
            }
        }

        private IVsStatusbar _statusBar;

        public IVsStatusbar StatusBar
        {
            get
            {
                if (_statusBar is null)
                {
                    _statusBar = ServiceContainer.GetCOMInstance<IVsStatusbar>();
                }

                return _statusBar;
            }
        }

        public Container ServiceContainer { get; }

        private readonly Assembly[] _containerConfigurationAssemblies;

        #region Constructors

        protected PackageX()
        {
            ServiceContainer = new Container();
        }

        protected PackageX(params Assembly[] containerConfigurationAssemblies) : base()
        {
            _containerConfigurationAssemblies = containerConfigurationAssemblies;
        }

        static PackageX()
        {
            EnumCore.Init(initializer => initializer.InitWith(Assembly.GetExecutingAssembly()), false);
        }

        #endregion

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            try
            {
                ContainerConstructor.Construct(this)
                                    .With(Assembly.GetExecutingAssembly())
                                    .With(_containerConfigurationAssemblies);

                await OnInitializeAsync?.Invoke(cancellationToken, progress);
            }
            finally
            {
                await base.InitializeAsync(cancellationToken, progress).ConfigureAwait(false);
            }
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

        protected override void Dispose(bool disposing)
        {
            try
            {
                OnDisposing?.Invoke();
            }
            finally
            {
                ServiceContainer.Dispose();

                base.Dispose(disposing);
            }
        }
    }
}
