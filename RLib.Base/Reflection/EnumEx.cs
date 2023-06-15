/********************************************************************
    created:	2017/12/7 16:02:11
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class EnumEx
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                      as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
        public static T GetValueFromDescription<T>(this string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                                                 typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }



        public static bool  IsEnum(this object obj) 
        {
            if (obj.GetType().BaseType.FullName == "System.Enum")
                return true;
            else
                return false;
        }
        public static bool  IsEnum(this Type t) 
        {
            if (t.BaseType != null &&
				t.BaseType.FullName == "System.Enum")
                return true;
            else
                return false;
        }
        public static bool  IsPOTEnum(this Type t)                                // 是否是0，2，4可以或的ENum
        {
            if (!IsEnum(t))
                return false;

            Array ar = Enum.GetValues(t);
            foreach (var v in ar)
            {
                if ((int)v != 0 && 
                    !MathEx.IsPowerOfTwo((int) v))
                    return false;

            }

            return true;
        }


        public static T Next<T>(this T e) where T : Enum
        {
            var ar = Enum.GetValues(e.GetType()).OfType<T>();
            return ar.Next(e);
        }
        public static T NextPot<T>(this T e) where T : Enum
        {
            var ar = Enum.GetValues(e.GetType()).OfType<T>();
            var ret = ar.Next(e);
            if (MathEx.IsPowerOfTwo(Convert.ToInt32(ret)))
                return ret;
            else
                return ret.NextPot();
        }
        public static T Previous<T>(this T e) where T : Enum
        {
            var ar = Enum.GetValues(e.GetType()).OfType<T>();
            return ar.Previous(e);
        }
        public static T PreviousPot<T>(this T e) where T : Enum
        {
            var ar = Enum.GetValues(e.GetType()).OfType<T>();
            var ret = ar.Previous(e);
            if (MathEx.IsPowerOfTwo(Convert.ToInt32(ret)) ||
                Convert.ToInt32(ret) == 0)
                return ret;
            else
                return ret.PreviousPot();
        }


        public static int? Pot(this Enum e)
        {
            if(MathEx.IsPowerOfTwo(Convert.ToInt32(e)))
                return (int)Math.Log(Convert.ToInt32(e), 2);
                //return (int)Math.Log2(Convert.ToInt32(e));

            return null;
        }

        public static T ToEnum<T>(this string e) where T : struct
        {
            return Enum.Parse<T>( e);
        }


        public static T ToEnumOrDefault<T>(this string e, T def = default(T)) where T : struct
        {
            T ret = def;
            Enum.TryParse<T>( e, true, out ret);

            return ret;
        }




        public static T FromString<T>(string val) where T : Enum// 打印Enum
        { 
            T ret ;
            try
            {
                if (IsPOTEnum(typeof(T)))
                {
                    ret = default(T);
                    string[] vals = val.Split('|');
                    foreach (string v in vals)
                    {
                        T o = (T)Enum.Parse(typeof(T), v);
                        if (ret == null)
                            ret = o;
                        else
                            Set(ref ret, o, true);
                    }
                }
                else
                    ret = (T)Enum.Parse(typeof(T), val);

                return ret;
            }
            catch (Exception e)
            {
                return default(T);
            }

            //return (T)FromString(typeof(T), val);
        }
        public static Enum FromString(Type t, string val)
        {
            Enum ret= null ;
            if (IsPOTEnum(t))
            {
                //ret = default(t);
                string[] vals = val.Split('|');
                foreach (string v in vals)
                {
                    Enum o = (Enum)Enum.Parse(t, v);
                    if (ret == null)
                        ret = o;
                    else
                        ret = ret.Set(o, true);
                }
            }
            else
                ret = (Enum)Enum.Parse(t, val);

            return ret;
        }

        //public static object FromInt(Type t, int val)
        //{
        //    object ret = Enum.Parse(t, val);

        //    return ret;
        //}

        public static string ToString<T>(this T val) where T :Enum                           // 打印Enum
        {
            return ToString(val.GetType(), val);
        }

        public static string ToString(this Type t, object val)                          // 打印Enum
        {
            StringBuilder sb = new StringBuilder();

            if (IsPOTEnum(t))
            {
                Array ar = Enum.GetValues(t);
                foreach (var v in ar)
                {
                    if (MathEx.IsPowerOfTwo((int) v) && // 只检查2的幂次项
                        ((int) v & Convert.ToInt32(val)) != 0)
                    {
                        if (sb.Length != 0)
                            sb.Append("|");

                        sb.Append(v.ToString());
                    }
                }
                if(sb.Length == 0)
                    sb.Append(Convert.ToString(val));

            }
            else
                sb.Append(Convert.ToString(val));

            return sb.ToString();
        }


        public static IEnumerable<T> Atomics<T>(T val) // 将一个enum 分解成 原则enum（如果它是Pot enum的话）
        {
            return Atomics(val.GetType(), val).OfType<T>();
        }

        public static IEnumerable<object> Atomics(Type t, object val)       // 将一个enum 分解成 原则enum（如果它是Pot enum的话）
        {
            List<object> ret = new List<object>();

            Array ar = Enum.GetValues(t);
            foreach (var v in ar)
            {
                if (MathEx.IsPowerOfTwo((int)v) && // 只检查2的幂次项
                    ((int)v & Convert.ToInt32(val)) != 0)
                {
                    ret.Add(v);
                }
            }

            return ret;
        }

        public static IEnumerable<T> Flags<T>(this T val) where T : Enum   // 将一个enum 分解成 原则enum（如果它是Pot enum的话）
        {
            List<T> ret = new List<T>();

            Array ar = Enum.GetValues(typeof(T));
            foreach (var v in ar)
            {
                if (MathEx.IsPowerOfTwo((int)v) && // 只检查2的幂次项
                    ((int)v & Convert.ToInt32(val)) != 0)
                {
                    ret.Add((T)v);
                }
            }

            return ret;
        }



        public static IEnumerable<object> Atomics(this Type t)
        {
            List<object> ret = new List<object>();

            Array ar = Enum.GetValues(t);
            foreach (var v in ar)
            {
                if (MathEx.IsPowerOfTwo((int)v))
                {
                    ret.Add(v);
                }
            }

            return ret;
        }

        public static string ToPotString<T>(T val)                          // 打印Enum
        {
            return ToPotString(val.GetType(), val);
        }
	    public static string ToPotString(Type t, object val)				// 打印Enum
	    {
			StringBuilder sb = new StringBuilder();

			Array ar = Enum.GetValues(t);
			foreach(var v in ar)
			{
				if(MathEx.IsPowerOfTwo((int)v) && // 只检查2的幂次项
					((int)v & Convert.ToInt32(val)) != 0)
				{
					if(sb.Length != 0)
						sb.Append("|");

					sb.Append(v.ToString());
				}
			}
			if(sb.Length == 0)
				sb.Append(Convert.ToString(val));

			return sb.ToString();
	    }

        public static bool  IsSet<T>(T obj, T e) 
        {
            if ( (obj.GetHashCode()  & e.GetHashCode() ) != 0)          // enum 的hashcode就是本身，所以这样避免了装箱拆箱，效率最高
                return true;
            else
                return false;
        }

        public static bool  IsSet(this Enum obj, Enum e)
        {
            if ( (obj.GetHashCode()  & e.GetHashCode() ) != 0)          // enum 的hashcode就是本身，所以这样避免了装箱拆箱，效率最高
                return true;
            else
                return false;
        }

        public static void  Set<T>(ref T obj, T e, bool val)
        {
            if (val)
                obj = (T)(((int)(obj as object) | (int)(e as object)) as object);
            else
            {
                if (IsSet<T>(obj, e))
                {
                    obj = (T) (((int) (obj as object) ^ (int) (e as object)) as object);
                }
            }
        }

        //public static void  Set<T>(ref T obj, T e, bool val) where T:Enum
        //{
        //    if (obj == null)
        //        return;

        //    if (val)
        //    {
        //        obj = (T)Enum.ToObject(typeof(T), obj.GetHashCode()  | e.GetHashCode());
        //    }
        //    else
        //    {
        //        if (IsSet<T>(obj, e))
        //        {
        //            obj = (T)Enum.ToObject(typeof(T), obj.GetHashCode()  ^ e.GetHashCode());
        //        }
        //    }
        //}

        public static Enum Set(this Enum obj, Enum e, bool val)
        {
            if (val)
            {
                if (obj == null)
                    return e;

                return (Enum)Enum.ToObject(e.GetType(), obj.GetHashCode() | e.GetHashCode());              // as object 装箱/拆箱速度极慢
            }
            else
            {
                if (obj == null)
                    return obj;

                if (obj.IsSet(e))
                {
                    return (Enum)Enum.ToObject(e.GetType(), obj.GetHashCode() ^ e.GetHashCode());         // as object 装箱/拆箱速度极慢
                }
            }

            return obj;
        }
    }
}
