using System.Collections.Generic;
using System.Linq;

namespace TW.Utility.Extension
{
    public static class AEnumerableExtension
    {
        /// <summary>
        /// Randomly shuffles the input enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The input enumerable to shuffle.</param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            List<T> list = new(enumerable);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
            return list;
        }
        /// <summary>
        /// Returns a new enumerable of count random elements from the input enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The input enumerable to select random elements from.</param>
        /// <param name="count">The number of random elements to select from the input enumerable.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> enumerable, int count)
        {
            return enumerable.Shuffle().ToList().GetRange(0, count);
        }
        /// <summary>
        /// Returns a new random elements from the input enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The input enumerable to select random elements from.</param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            IEnumerable<T> enumerable1 = enumerable as T[] ?? enumerable.ToArray();
            return enumerable1.ElementAt(UnityEngine.Random.Range(0, enumerable1.Count()));
        }
    } 
}
