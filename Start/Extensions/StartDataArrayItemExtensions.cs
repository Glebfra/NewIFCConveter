using System.Collections.Generic;
using System.Linq;
using Start.API;

namespace Start.Extensions
{
    internal static class StartDataArrayItemExtensions
    {
        public static IEnumerable<StartDataArrayItem> GetElementsByType(this IEnumerable<StartDataArrayItem> arrayItems, 
            StartElementTypeEnum type)
        {
            return GetElementsByType(arrayItems, new[] { type });
        }
        
        public static IEnumerable<StartDataArrayItem> GetElementsByType(this IEnumerable<StartDataArrayItem> arrayItems, 
            IEnumerable<StartElementTypeEnum> types)
        {
            return arrayItems.Where(item => types.Contains(item.Type));
        }

        public static IEnumerable<StartDataArrayItem> GetConnElements(this StartDataArrayItem[] arrayItems, int ID)
        {
            StartDataArrayItem baseElement = arrayItems.Single(item => item.DataArrayIndex == ID);
            int[] baseElementNodeIds = baseElement.NodeIds;
            return (baseElementNodeIds.Length == 1
                    ? arrayItems.Where(item => item.NodeIds.Contains(baseElementNodeIds[0]))
                    : arrayItems.Where(item =>
                        item.NodeIds.Contains(baseElementNodeIds[0]) || item.NodeIds.Contains(baseElementNodeIds[1]))
                ).Where(item => item.DataArrayIndex != ID);
        }

        public static IEnumerable<StartDataArrayItem> GetConnElements(this StartDataArrayItem[] arrayItems,
            StartDataArrayItem baseElement)
        {
            return GetConnElements(arrayItems, baseElement.DataArrayIndex);
        }
    }
}