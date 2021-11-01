/********************************************************************
    created:	2018/7/10 17:35:36
    author:	rush
    email:		
	
    purpose:	对Linq的扩充
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class EnumerableEx
    {

        public static TSource Last<TSource>(this IEnumerable<TSource> source, int offset)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source is IList<TSource> sourceList)
            {
                int count = sourceList.Count;
                if (count > 0)
                    return sourceList[count - offset -1];
            }
            else
            {
                return source.ToList().Last(offset);

            }
            throw new Exception($"No Element:{nameof(source)}");
        }

        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, int offset, TSource def = default(TSource))
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source is IList<TSource> sourceList)
            {
                int count = sourceList.Count;
                if (count > 0)
                    return sourceList[count - offset - 1];
            }
            else
            {
                return source.ToList().LastOrDefault(offset, def);
            }
            return def;
        }




		public static T MaxOrDefault<T>(this IEnumerable<T> source, T def = default(T))
        {
            if (source == null)
				throw new ArgumentNullException(nameof(source));

            if (source.Any())
	            return source.Max();
            else
	            return def;
		}
		public static T MaxOrDefault<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector,  T def = default(T))
        {
            return source.Select<TSource, T>(selector).MaxOrDefault(def);
        }

		public static T MinOrDefault<T>(this IEnumerable<T> source, T def = default(T))
        {
            if (source == null)
				throw new ArgumentNullException(nameof(source));

            if (source.Any())
	            return source.Min();
            else
	            return def;
		}
		public static T MinOrDefault<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector,  T def = default(T))
        {
            return source.Select<TSource, T>(selector).MinOrDefault(def);
        }

        //public static double MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector,  double def = -1d)
        //{
        //    return source.Select<TSource, double>(selector).MaxOrDefault(def);
        //}



        public static string ToString<TSource>(this IEnumerable<TSource> source, string splitter = " ") // 包含所有
        {
            string ret = "";
            foreach (var p in source)
            {
                ret += p +splitter;
            }

            return ret.TrimEnds(splitter);
        }


	    public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)  // 包含所有
	    {
	        foreach (TSource o in other)
	        {
	            if (!source.Contains(o))
	                return false;
	        }

	        return true;
	    }

	    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
	    {
			if(source == null)
				return true;

	        return !source.Any();
	    }


	    public static IEnumerable<TSource> ToEnumerable<TSource>(this TSource source)
	    {
			if(source == null)
				return new TSource[]{};
			else
				return new TSource[]{source};
	    }

	    public static TSource	RandomOne<TSource>(this IEnumerable<TSource> source)  // 从列表中随机选择一个
	    {
	        if (source.Count() == 0)
	            return default(TSource);

	        int index = MathEx.Random(0, source.Count());

	        return source.ElementAt(index);
	    }


	    public static void	ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)  // 比较特殊，仅仅是执行一下
	    {
		    foreach (TSource s in source)
		    {
			    action(s);
		    }
	    }

        /// 在一个序列中，返回符合条件的元素的下一个元素
        public static TSource Next<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            return source.SkipWhile(p => !Equals(p, value)).Skip(1).First();
        }
        public static TSource Next<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            bool found = false;
            foreach (TSource s in source)
            {
                if (found)
                    return s;

                if (predicate(s))
                    found = true;
            }

            return default(TSource);
            //throw new Exception("序列不包含指定元素");

            //return Next(source, source.First(predicate));  速度较慢
        }
        public static TSource NextOrDefault<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            return source.SkipWhile(p => !Equals(p, value)).Skip(1).FirstOrDefault();
        }
        public static TSource NextOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            bool found = false;
            foreach (TSource s in source)
            {
                if (found)
                    return s;

                if (predicate(s))
                    found = true;
            }

            return default(TSource);
        }

        /// 在一个序列中，返回符合条件的元素的前一个元素
        public static TSource Previous<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            if (!source.Contains(value))
            {
                return default(TSource);
                //throw new Exception("序列不包含指定元素");
            }

            return source.TakeWhile(p => !Equals(p, value)).LastOrDefault();
        }
        public static TSource Previous<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            TSource last= default(TSource);
            bool lastExists = false;
            foreach (TSource s in source)
            {
                if (predicate(s))
                {
                    if(lastExists)
                        return last;
                    else
                        throw new Exception("序列不包含指定元素");
                }

                last = s;
                lastExists = true;
            }

            throw new Exception("序列不包含指定元素");
        }
        public static TSource PreviousOrDefault<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            if (!source.Contains(value))
                return default(TSource);

            return source.TakeWhile(p => !Equals(p, value)).LastOrDefault();
        }
        public static TSource PreviousOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            TSource last= default(TSource);
            foreach(TSource s in source)
            {
                if(predicate(s))
                    return last;

                last = s;
            }

            return default(TSource);
        }


        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> source, TSource value)
        {
	        return source.Concat(value.ToEnumerable());
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> source, TSource value)
        {
	        return source.Union(value.ToEnumerable());
        }


        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<TSource, int> hashFunc, Func<TSource, TSource, bool> equalFunc)   // distinct 的简洁写法, 提供一个HashFunc，一个EqualFunc
        {
            return source.Distinct(new CommonCompare<TSource>(hashFunc, equalFunc));
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> second, Func<TSource, int> hashFunc, Func<TSource, TSource, bool> equalFunc)   // distinct 的简洁写法, 提供一个HashFunc，一个EqualFunc
        {
            return source.Union(second, new CommonCompare<TSource>(hashFunc, equalFunc));
        }
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> source,IEnumerable<TSource> second, Func<TSource, int> hashFunc, Func<TSource, TSource, bool> equalFunc)   // distinct 的简洁写法, 提供一个HashFunc，一个EqualFunc
        {
            return source.Except(second, new CommonCompare<TSource>(hashFunc, equalFunc));
        }

        public class CommonCompare<T> : IEqualityComparer<T>
        {
            private Func<T, int> _HashFunc;
            private Func<T, T, bool> _EqualFunc;
            public CommonCompare(Func<T, int> hash, Func<T, T, bool> equal)
            {
                this._HashFunc = hash;
                _EqualFunc = equal;
            }

            public bool Equals(T x, T y)
            {
                if (_EqualFunc != null)
                {
                    return _EqualFunc(x, y);
                }
                else
                {
                    return false;
                }
            }

            public int      GetHashCode(T obj)                              // 当
            {
                if (_HashFunc != null)
                {
                    return _HashFunc(obj);
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
