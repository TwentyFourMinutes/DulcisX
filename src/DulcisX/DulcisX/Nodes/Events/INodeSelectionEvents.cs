using System;
using System.Collections.Generic;

namespace DulcisX.Nodes.Events
{
    /// <summary>
    /// Provides events, which occur on Solution Explorer Node selection changes. Provided by the <see cref="Microsoft.VisualStudio.Shell.Interop.IVsSelectionEvents"/> interface.
    /// </summary>
    public interface INodeSelectionEvents
    {
        /// <summary>
        /// Occurs when user selected Nodes in the Solution Explorer change.
        /// </summary>
        event Action<IEnumerable<BaseNode>> OnSelected;
    }
}
