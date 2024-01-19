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
        /// Randomly shuffles the input enumerable (can run in sub thread).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The input enumerable to shuffle.</param>
        /// <returns></returns>
        public static IEnumerable<T> ShuffleInSubThread<T>(this IEnumerable<T> enumerable)
        {
            List<T> list = new(enumerable);
            int n = list.Count;
            System.Random random = new System.Random();
            
            while (n > 1)
            {
                n--;
                int k = random.Next(0, n + 1);
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
        /// <summary>
        /// Converts an IEnumerator&lt;T&gt; to an IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerator.</typeparam>
        /// <param name="enumerator">The IEnumerator&lt;T&gt; to convert.</param>
        /// <returns>An IEnumerable&lt;T&gt; that contains the elements from the original IEnumerator&lt;T&gt;.</returns>
        public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
        /// <summary>
        /// Extension method for adding an item to a list if it is not already present.
        /// If the input list is null, a new list is created and the item is added.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The input list to which the item is attempted to be added.</param>
        /// <param name="item">The item to be added to the list.</param>
        /// <returns>The updated list with the item added if it was not already present.</returns>
        public static List<T> TryAdd<T>(this List<T> list, T item)
        {
            list ??= new List<T>();
            if (!list.Contains(item)) list.Add(item);
            return list;
        }

        /// <summary>
        /// Extension method for removing an item from a list if it is present.
        /// If the input list is null, a new list is created (if the item is present, it is removed).
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The input list from which the item is attempted to be removed.</param>
        /// <param name="item">The item to be removed from the list.</param>
        /// <returns>The updated list with the item removed if it was present.</returns>
        public static List<T> TryRemove<T>(this List<T> list, T item)
        {
            list ??= new List<T>();
            if (list.Contains(item)) list.Remove(item);
            return list;
        }
        
        /// <summary>
        /// Extension method for adding an item to a list if it is not already present.
        /// </summary>
        /// <param name="source">The source IEnumerable.</param>
        /// <param name="items">The input IEnumerable is attempted to be added.</param>
        /// <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> AddMultiple<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            return source.Concat(items);
        }

    } 
}
