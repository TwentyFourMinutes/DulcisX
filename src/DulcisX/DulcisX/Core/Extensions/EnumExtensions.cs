using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Core.Extensions
{
    internal static class EnumExtensions
    {
        internal static bool ContainsMultipleFlags(this NodeTypes nodeType)
        {
            var nodeValue = (int)nodeType;

            return (nodeValue & (nodeValue - 1)) != 0;
        }
    }
}
