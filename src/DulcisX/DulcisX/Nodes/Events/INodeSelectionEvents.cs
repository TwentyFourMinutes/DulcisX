using System;
using System.Collections;
using System.Collections.Generic;

namespace DulcisX.Nodes.Events
{
    public interface INodeSelectionEvents
    {
        event Action<IEnumerable<BaseNode>> OnSelected;
    }
}
