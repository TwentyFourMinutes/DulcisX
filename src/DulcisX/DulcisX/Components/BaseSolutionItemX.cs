
using DulcisX.Core.Models;

namespace DulcisX.Components
{
    public abstract class BaseSolutionItemX
    {
        public PropertiesX Properties { get; }

        protected BaseSolutionItemX(PropertiesX properties)
            => Properties = properties;
    }
}
