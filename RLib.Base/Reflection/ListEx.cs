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

    }
}
