using System.Collections.Generic;

namespace Funk
{
    public static class ExtensionMethods
    {
        public static void RemoveLast<T>(this List<T> list) => list.RemoveAt(list.Count - 1);
    }
}
