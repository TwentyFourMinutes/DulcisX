using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    _dte2 = GetService<DTE, DTE2>();
                }
                return _dte2;
            }
        }

        public async ValueTask<DTE2> GetDTE2Async()
        {
            if (_dte2 is null)
            {
                _dte2 = await GetServiceAsync<DTE, DTE2>();
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

        #endregion
    }
}
