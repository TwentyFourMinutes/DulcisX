using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    _dte2 = GetService(typeof(DTE)) as DTE2;
                }
                return _dte2;
            }
        }

        public async ValueTask<DTE2> GetDTE2Async()
        {
            if (_dte2 is null)
            {
                _dte2 = (await GetServiceAsync(typeof(DTE))) as DTE2;
            }

            return _dte2;
        }

        #endregion
    }
}
