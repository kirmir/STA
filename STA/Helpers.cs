using System.Collections.Generic;

namespace STA
{
    /// <summary>
    /// Contains different small methods for code simplifying.
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Swaps two objects inside lists.
        /// </summary>
        /// <typeparam name="T">The type of the objects.</typeparam>
        /// <param name="list1">First list of objects.</param>
        /// <param name="list2">Second list of objects.</param>
        /// <param name="i1">Index to swap inside first list.</param>
        /// <param name="i2">Index to swap inside second list.</param>
        public static void Swap<T>(IList<T> list1, IList<T> list2, int i1, int i2)
        {
            var t = list1[i1];
            list1[i1] = list2[i2];
            list2[i2] = t;
        }
    }
}
