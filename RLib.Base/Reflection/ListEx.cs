/********************************************************************
    created:	2021/11/1 20:15:01
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class ListEx
    {
        public static Type ListType => typeof(System.Collections.IList);
        public static Type ArrayType => typeof(System.Array);

        public static bool AddIfNotHave<T>(this ICollection<T> l,  T o)
        {
            if (!l.Contains(o))
            {
                l.Add(o);
                return true;
            }

            return false;
        }

        public static bool  IsList(this Type t)
        {
            if (ListType.IsAssignableFrom(t) &&
                !ArrayType.IsAssignableFrom(t))
                return true;
            else
                return false;
        }

        public static bool  IsList(this object o)
        {
            return o.GetType().IsList();
        }

        public static IList MakeList(this Type t)                           // 对这个t做一个List<T>
        {
            var lt = typeof(List<>).MakeGenericType(t);
            var list = Activator.CreateInstance(lt);

            return list as IList;
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to which the items should be added.</param>
        /// <param name="collection">The collection whose elements should be added to the end of the list. The collection itself cannot be null, but it can contain elements that are null, if type <typeparamref name="T" /> is a reference type.</param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
        {
            if (list == null)
                throw new ArgumentNullException("list is null");
            if (collection == null)
                throw new ArgumentNullException("collection is null");
            if (list is List<T> objList) { objList.AddRange(collection); }
            else
            {
                foreach (T obj in collection)
                    list.Add(obj);
            }
        }

        /// <summary>
        /// Inserts the elements of a collection into the list at the specified index.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to which the items should be inserted.</param>
        /// <param name="index">The zero-based index at which the new elements should be inserted..</param>
        /// <param name="collection">The collection whose elements should be inserted into the list. The collection itself cannot be null, but it can contain elements that are null, if type <typeparamref name="T" />  is a reference type.</param>
        public static void InsertRange<T>(this IList<T> list, int index, IEnumerable<T> collection)
        {
            if (list == null)
                throw new ArgumentNullException("list is null");
            if (collection == null)
                throw new ArgumentNullException("collection is null");
            if (list is List<T> objList) { objList.InsertRange(index, collection); }
            else
            {
                int num = 0;
                foreach (T obj in collection)
                    list.Insert(index + num++, obj);
            }
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list from with the items should be removed.</param>
        /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed from the <see cref="T:System.Collections.Generic.IList`1" />.</returns>
        public static int RemoveAll<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
                throw new ArgumentNullException("list is null");
            if (match == null)
                throw new ArgumentNullException("math is null");
            if (list is List<T> objList)
                return objList.RemoveAll(match);
            int num = 0;
            for (int index = list.Count - 1; index >= 0; --index)
            {
                if (match(list[index]))
                {
                    list.RemoveAt(index);
                    ++num;
                }
            }

            return num;
        }


        /// <summary>
        /// Inserts a new value into a sorted collection.
        /// </summary>
        /// <typeparam name="T">The type of collection values, where the type implements IComparable of itself</typeparam>
        /// <param name="collection">The source collection</param>
        /// <param name="item">The item being inserted</param>
        public static void InsertSorted<T>(this IList<T> collection, T item)
            where T : IComparable<T>
        {
            InsertSorted(collection, item, Comparer<T>.Create((x, y) => x.CompareTo(y)));
        }

        /// <summary>
        /// Inserts a new value into a sorted collection.
        /// </summary>
        /// <typeparam name="T">The type of collection values</typeparam>
        /// <param name="collection">The source collection</param>
        /// <param name="item">The item being inserted</param>
        /// <param name="comparerFunction">An IComparer to comparer T values, e.g.
        /// Comparer&lt;T&gt;.Create((x, y) =&gt; (x.Property &lt; y.Property) ? -1 : (x.Property &gt; y.Property) ? 1 : 0)</param>
        public static void InsertSorted<T>(this IList<T> collection, T item, IComparer<T> comparerFunction)
        {
            if (collection.Count == 0)
            {
                // Simple add
                collection.Add(item);
            }
            else if (comparerFunction.Compare(item, collection[collection.Count - 1]) >= 0)
            {
                // Add to the end as the item being added is greater than the last item by comparison.
                collection.Add(item);
            }
            else if (comparerFunction.Compare(item, collection[0]) <= 0)
            {
                // Add to the front as the item being added is less than the first item by comparison.
                collection.Insert(0, item);
            }
            else
            {
                // Otherwise, search for the place to insert.
                int index = 0;
                if (collection is List<T> list) { index = list.BinarySearch(item, comparerFunction); }
                else if (collection is T[] arr) { index = Array.BinarySearch(arr, item, comparerFunction); }
                else
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (comparerFunction.Compare(collection[i], item) <= 0)
                        {
                            // If the item is the same or before, then the insertion point is here.
                            index = i;
                            break;
                        }

                        // Otherwise loop. We're already tested the last element for greater than count.
                    }
                }

                if (index < 0)
                {
                    // The zero-based index of item if item is found,
                    // otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, if there is no larger element, the bitwise complement of Count.
                    index = ~index;
                }

                collection.Insert(index, item);
            }
        }
    }
}
