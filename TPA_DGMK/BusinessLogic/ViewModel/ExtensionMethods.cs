using System.Collections.Generic;
using System.Linq;

namespace ViewModel
{
    public static class ExtensionMethods
    {
        public static bool CheckIfItIsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
        public static IEnumerable<T> ReturnEmptyIfItIsNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }
    }
}
