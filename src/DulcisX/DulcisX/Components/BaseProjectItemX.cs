
using DulcisX.Core.Models;

namespace DulcisX.Components
{
    public abstract class BaseProjectItemX
    {
        public PropertiesX Properties { get; }

        protected BaseProjectItemX(PropertiesX properties)
            => Properties = properties;
    }
}
