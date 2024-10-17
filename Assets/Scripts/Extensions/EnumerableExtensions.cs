using System.Collections.Generic;
using System.Linq;

internal static class EnumerableExtensions
{
    public static bool ContainsAll<T>(this IEnumerable<T> containingList, IEnumerable<T> lookupList)
    {
        return !lookupList.Except(containingList).Any();
    }
}
