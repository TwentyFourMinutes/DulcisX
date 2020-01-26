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
    public class PackageX : AsyncPackage, IServiceProviders
    {
        #region DTE

        private DTE2 _dte2;

        public DTE2 DTE2
        {
            get
            {
                if (_dte2 is null)
                {
                    _dte2 = this.GetService<SDTE, DTE2>();
                }
                return _dte2;
            }
        }

        public async ValueTask<DTE2> GetDTE2Async()
        {
            if (_dte2 is null)
            {
                _dte2 = await this.GetServiceAsync<SDTE, DTE2>();
            }

            return _dte2;
        }

        #endregion

        #region MethodEvents

        public event Func<CancellationToken, IProgress<ServiceProgressData>, Task> OnInitializeAsync;

        protected override Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            OnInitializeAsync?.Invoke(cancellationToken, progress);
            return base.InitializeAsync(cancellationToken, progress);
        }

        public event Func<bool> OnDisposing;

        protected override void Dispose(bool disposing)
        {
            OnDisposing?.Invoke();

            Solution.Dispose();

            base.Dispose(disposing);
        }

        #endregion

        #region Services

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
                    _solution = new SolutionX(this.GetService<SVsSolution, IVsSolution>(), GetServiceProviders());
                }

                return _solution;
            }
        }

        public async ValueTask<SolutionX> GetSolutionAsync()
        {
            if (_solution is null)
            {
                _solution = new SolutionX(await this.GetServiceAsync<SVsSolution, IVsSolution>(), GetServiceProviders());
            }

            return _solution;
        }

        #endregion
    }
}
