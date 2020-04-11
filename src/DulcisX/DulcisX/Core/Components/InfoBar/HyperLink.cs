using System;

namespace DulcisX.Core
{
    internal class HyperLink
    {
        internal Uri Uri { get; }

        internal bool OpenInternally { get; }

        internal HyperLink(Uri uri, bool openInternally)
        {
            Uri = uri;
            OpenInternally = openInternally;
        }
    }
}
