using DulcisX.Core.Components;
using DulcisX.Core.Extensions;
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
    /// <summary>
    /// Registers a new Package and provides all the Visual Studio Services to the environment.
    /// </summary>
    public abstract class PackageX : AsyncPackage, IServiceProviders
    {
        /// <summary>
        /// Occurs when the environment initializes the Package.
        /// </summary>
        public event Func<CancellationToken, IProgress<ServiceProgressData>, Task> OnInitializeAsync;

        /// <summary>
        /// Occurs when the environment disposes the Package.
        /// </summary>
        public event Action OnDisposing;

        private SolutionNode _solution;

        /// <summary>
        /// Gets the currently open Solution.
        /// </summary>
        public SolutionNode Solution
        {
            get
            {
                if (_solution is null)
                {
                    var solution = ServiceContainer.GetCOMInstance<IVsSolution>();

                    _solution = new SolutionNode(solution, this, ServiceContainer);
                }

                return _solution;
            }
        }

        private IVsStatusbar _statusBar;

        /// <summary>
        /// Gets the <see cref="IVsStatusbar"/> of the environment.
        /// </summary>
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

        private InfoBar _infoBar;

        /// <summary>
        /// Gets the InfoBar of the environment.
        /// </summary>
        public InfoBar InfoBar
        {
            get
            {
                if (_infoBar is null)
                {
                    _infoBar = new InfoBar(ServiceContainer.GetCOMInstance<IVsInfoBarUIFactory>(),
                                           ServiceContainer.GetCOMInstance<IVsInfoBarHost>(),
                                           WebBrowser);
                }

                return _infoBar;
            }
        }

        private WebBrowser _webBrowser;

        /// <summary>
        /// Gets the WebBrowser of the environment.
        /// </summary>
        public WebBrowser WebBrowser
        {
            get
            {
                if (_webBrowser is null)
                {
                    _webBrowser = new WebBrowser(ServiceContainer.GetCOMInstance<IVsWebBrowsingService>());
                }

                return _webBrowser;
            }
        }

        private VisualStudioInstance _vsInstance;

        /// <summary>
        /// Gets the <see cref="VisualStudioInstance"/> of the environment.
        /// </summary>
        public VisualStudioInstance VSInstance
        {
            get
            {
                if (_vsInstance is null)
                {
                    _vsInstance = new VisualStudioInstance(ServiceContainer.GetCOMInstance<IVsShell>());
                }

                return _vsInstance;
            }
        }

        /// <summary>
        /// Gets the <see cref="Container"/> which holds package and user specifc services.
        /// </summary>
        public Container ServiceContainer { get; }

        private readonly Assembly[] _containerConfigurationAssemblies;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageX"/> class.
        /// </summary>
        protected PackageX()
        {
            ServiceContainer = new Container();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageX"/> class with a list of assemblies which contain <see cref="IContainerConfiguration"/>s.
        /// </summary>
        /// <param name="containerConfigurationAssemblies">An Array which contains all assemblies witch contain <see cref="IContainerConfiguration"/>s.</param>
        protected PackageX(params Assembly[] containerConfigurationAssemblies) : base()
        {
            _containerConfigurationAssemblies = containerConfigurationAssemblies;
        }

        static PackageX()
        {
            EnumCore.Init(initializer => initializer.InitWith(Assembly.GetExecutingAssembly()), false);
        }

        #endregion

        protected sealed override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
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
                await base.InitializeAsync(cancellationToken, progress);
            }
        }

        #region Services

        /// <summary>
        /// Returns a service which is registered in the native Global Service Container.
        /// </summary>
        /// <typeparam name="TService">The type which should be retrieved.</typeparam>
        /// <returns>The service being requested if available, otherwise null.</returns>
        public TService GetGlobalService<TService>() where TService : class
            => GetGlobalService<TService, TService>();

        /// <summary>
        /// Returns a service which is registered in the native Global Service Container.
        /// </summary>
        /// <typeparam name="TBaseService">The service type of the implemention which should be retrieved.</typeparam>
        /// <typeparam name="TService">The implemention type of the service which should be retrieved.</typeparam>
        /// <returns>The service being requested if available, otherwise null.</returns>
        public TService GetGlobalService<TBaseService, TService>() where TBaseService : class
                                                                   where TService : class
            => GetGlobalService(typeof(TBaseService)) as TService;

        private IServiceProviders GetServiceProviders()
            => (IServiceProviders)this;

        #endregion

        protected sealed override void Dispose(bool disposing)
        {
            try
            {
                OnDisposing?.Invoke();
            }
            finally
            {
                try
                {
                    base.Dispose(disposing);
                }
                finally
                {
                    ServiceContainer.Dispose();
                }
            }
        }
    }
}
