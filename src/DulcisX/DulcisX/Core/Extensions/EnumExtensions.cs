using DulcisX.Core.Models.Enums;

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
