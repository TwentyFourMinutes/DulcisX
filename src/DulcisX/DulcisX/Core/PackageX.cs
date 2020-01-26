using DulcisX.Components;
using DulcisX.Core.Models.Interfaces;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace DulcisX.Core
{
    public class PackageX : AsyncPackage
    {
        #region DTE

        private DTE2 _dte2;

        public DTE2 DTE2
        {
            get
            {
                if (_dte2 is null)
                {
                    _dte2 = GetService<SDTE, DTE2>();
                }
                return _dte2;
            }
        }

        public async ValueTask<DTE2> GetDTE2Async()
        {
            if (_dte2 is null)
            {
                _dte2 = await GetServiceAsync<SDTE, DTE2>();
            }

            return _dte2;
        }

        #endregion

        #region Initialize

        public event Func<IProgress<ServiceProgressData>, CancellationToken, Task> OnInitializeAsync;

        protected override Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
            => OnInitializeAsync?.Invoke(progress, cancellationToken);

        #endregion

        #region Services

        public TService GetService<TService>() where TService : class
            => GetService<TService, TService>();

        public TService GetService<TBaseService, TService>() where TBaseService : class
                                                            where TService : class
            => GetService(typeof(TBaseService)) as TService;

        public Task<TService> GetServiceAsync<TService>() where TService : class
           => GetServiceAsync<TService, TService>();

        public async Task<TService> GetServiceAsync<TBaseService, TService>() where TBaseService : class
                                                                              where TService : class
            => (await GetServiceAsync(typeof(TBaseService))) as TService;

        public TService GetGlobalService<TService>() where TService : class
            => GetGlobalService<TService, TService>();

        public TService GetGlobalService<TBaseService, TService>() where TBaseService : class
                                                                   where TService : class
            => GetGlobalService(typeof(TBaseService)) as TService;

        private IServiceProviders GetServiceProviders()
            => (IServiceProviders)this;

        #endregion

        #region Solution

        private SolutionX _solution;

        public SolutionX Solution
        {
            get
            {
                if (_solution is null)
                {
                    _solution = new SolutionX(GetService<SVsSolution, IVsSolution>(), GetServiceProviders());
                }

                return _solution;
            }
        }

        public async ValueTask<SolutionX> GetSolutionAsync()
        {
            if (_solution is null)
            {
                _solution = new SolutionX(await GetServiceAsync<SVsSolution, IVsSolution>(), GetServiceProviders());
            }

            return _solution;
        }

        #endregion
    }
}
