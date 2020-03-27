using DulcisX.Core.Enums;
using Microsoft.Internal.VisualStudio.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace DulcisX.Core.Components
{
    internal class SolutionConfigurationOptions : IVsPersistSolutionOpts
    {
        internal Dictionary<Guid, StartupOption> StartupProjects { get; }

        internal bool IsMultiStartup { get; private set; }

        internal SolutionConfigurationOptions()
        {
            StartupProjects = new Dictionary<Guid, StartupOption>();
        }

        public int SaveUserOptions(IVsSolutionPersistence pPersistence)
            => VSConstants.E_NOTIMPL;

        public int LoadUserOptions(IVsSolutionPersistence pPersistence, uint grfLoadOpts)
             => VSConstants.E_NOTIMPL;

        public int WriteUserOptions(IStream pOptionsStream, string pszKey)
             => VSConstants.E_NOTIMPL;

        public int ReadUserOptions(IStream pOptionsStream, string pszKey)
        {
            const string startupToken = "dwStartupOpt\0=";
            const string delimiterToken = ";";
            const string sohToken = "\u0001";

            var startupTokenBytes = Encoding.Unicode.GetBytes(startupToken);
            var delimiterTokenBytes = Encoding.Unicode.GetBytes(delimiterToken);
            var sohTokenBytes = Encoding.Unicode.GetBytes(sohToken);

            byte[] bytes = null;

            using (var stream = new DataStreamFromComStream(pOptionsStream))
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }

            var i = 0;
            var isMultiStartup = false;

            do
            {
                if (i % 2 == 0 && bytes[i] == 0 && bytes[i + 1] == 0)
                {
                    i += 2;
                    continue;
                }

                if (!isMultiStartup && Match(bytes, delimiterTokenBytes, i))
                {
                    if (Match(bytes, sohTokenBytes, i - 4))
                    {
                        IsMultiStartup = isMultiStartup = true;
                    }
                    else
                    {
                        IsMultiStartup = false;
                        return VSConstants.S_OK;
                    }
                }

                if (Match(bytes, startupTokenBytes, i))
                {

                    var guidBytes = new byte[38 * 2];
                    Array.Copy(bytes, i - guidBytes.Length - 2, guidBytes, 0, guidBytes.Length);
                    var guid = new Guid(Encoding.Unicode.GetString(guidBytes));

                    var options = BitConverter.ToInt32(bytes, i + startupTokenBytes.Length + 2);

                    if ((options & 1) != 0)
                    {
                        StartupProjects.Add(guid, StartupOption.Start);
                    }
                    else if ((options & 2) != 0)
                    {
                        StartupProjects.Add(guid, StartupOption.StartWithDebugging);
                    }
                }

                i++;
            }
            while (i < bytes.Length);

            return VSConstants.S_OK;
        }

        private bool Match(byte[] bytes, byte[] tokenBytes, int index)
        {
            var found = true;

            for (int j = 0; j < tokenBytes.Length; j++)
            {
                if (bytes[index + j] != tokenBytes[j])
                {
                    found = false;
                    break;
                }
            }

            return found;
        }
    }
}
