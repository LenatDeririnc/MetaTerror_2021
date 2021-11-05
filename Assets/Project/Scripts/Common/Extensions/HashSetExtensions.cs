using System.Collections.Generic;

namespace Common.Extensions
{
    public static class HashSetExtensions
    {
        public static void RemoveIfContains<T>(this HashSet<T> hashSet, T obj)
        {
            if (hashSet.Contains(obj))
                hashSet.Remove(obj);
        }
    }
}