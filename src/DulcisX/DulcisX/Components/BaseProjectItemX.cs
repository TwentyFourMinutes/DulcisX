
using DulcisX.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Components
{
    public abstract class BaseProjectItemX
    {
        public PropertiesX Properties { get; }

        protected BaseProjectItemX(PropertiesX properties)
            => Properties = properties;
    }
}
