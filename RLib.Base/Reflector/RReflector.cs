/********************************************************************
    created:	2018/6/13 20:22:16
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DataModel;


namespace RLib.Base
{
    public static class RReflector
    {
        public static object CreateInstance(this Type t, params object[] args) // 主要目的是设置Para DefaultValue
        {
            object o = Activator.CreateInstance(t, args);
            if (o != null)
            {
                // 对nesting property赋初值
                foreach (IPara p in o.GetType().GetParas())
                {
                    if (p.NestingParas != null)
                    {
                        object nest = null;
                        if (p.ValueType.IsInterface)
                            nest = App.Instance.Container.Resolve(p.ValueType);
                        else
                            nest = Activator.CreateInstance(p.ValueType);

                        (p as Para).PI.SetValue(o, nest);
                    }
                }

                // 设置para
                foreach (IPara p in o.GetParas())
                {
                    //object def = RReflector.GetDefaultValue(p.ValueType);

                    // 如果是默认值，设置def
                    if (p.DefValue != null &&
                       object.Equals(p.Value, RReflector.GetDefaultValue(p.ValueType)))
                    {
                        p.Value = p.DefValue;
                    }


                    // 内部复合参数
                    if (p.DefValue==null && p.NestingParas != null)
                    {// 生成一个默认的对象，并对其赋嵌套para

                        foreach (IPara n in p.NestingParas)
                        {
                            n.Value = n.DefValue;
                        }
                    }


                }
            }

            return o;
        }

        public static T     CreateInstance<T>(params object[] args)
        {
            T ret = (T)CreateInstance(typeof(T), args);

            return ret;
        }

        public static bool  EqualsObject<T>(this T origin, T target)
        {
            try
            {
            //    foreach (PropertyInfo item in properties)
            //    {
            //        if (item.GetValue(newT, null) != null)
            //        {
            //            object t = item.GetValue(thisT, null);
            //            object obj2 = item.GetValue(newT, null);
            //            bool az = t.Equals(obj2);
            //            if (!az)
            //                return true;
            //        }
            //    }
            }
            catch (Exception e)
            {
            }
            return false;
        }


        /// <summary>
        /// 获取变量的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsBulitinType(Type type)                        // 是否是系统类型， Enum也作为系统类型
        {
            return (type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object ||
                type.Name == "TimeSpan");  // 认为TimeSpan也是内置类型（虽然不在c#的内置类型里）
        }

        public static bool  IsNumber(this object obj)                       // 判断是否是数字
        {
            if (object.Equals(obj, null))
            {
                return false;
            }

            Type objType = obj.GetType();
            objType = Nullable.GetUnderlyingType(objType) ?? objType;

            if (objType.IsPrimitive)
            {
                return objType != typeof(bool) &&
                    objType != typeof(char) &&
                    objType != typeof(IntPtr) &&
                    objType != typeof(UIntPtr);
            }

            return objType == typeof(decimal);
        }

        public static bool IsDouble(this Type t)
        {
            Type rt = t.IsNullableType()
                ? Nullable.GetUnderlyingType(t)
                : t;

            return rt.FullName == "System.Double";
        }


        public static bool IsNullableType(this Type type)                        // 是否是系统类型， Enum也作为系统类型
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            else
                return false;
        }
   

        public static Type RealType(this Type type)                        // 是否是系统类型， Enum也作为系统类型
        {
            if (IsNullableType(type))
                return type.GetGenericArguments().Single();
            else
                return type;
        }

        public static bool Is<T>(this Type t)
        {
            return typeof(T).IsAssignableFrom(t);
        }

        public static bool IsGeneric(this Type t, Type other)
        {
            //var td = dl.GetType().GetGenericTypeDefinition();
            //var tl = typeof(List<>);
            if (!t.IsGenericType||
                !other.IsGenericType)
                return false;

            return t.GetGenericTypeDefinition() == other;
        }
        public static bool IsGeneric(this object o, Type other)
        {
            return o.GetType().IsGeneric(other);
        }

        public static bool  IsListProperty(Type t)
        {
            if (typeof (System.Collections.IList).IsAssignableFrom(t) &&
                !typeof (System.Array).IsAssignableFrom(t))
                return true;
            else
                return false;
        }

        public static bool  IsList(this Type t)
        {
            if (typeof (System.Collections.IList).IsAssignableFrom(t) &&
                !typeof (System.Array).IsAssignableFrom(t))
                return true;
            else
                return false;
        }

        public static IList MakeList(this Type t)                           // 对这个t做一个List<T>
        {
            var lt = typeof(List<>).MakeGenericType(t);
            var list = Activator.CreateInstance(lt);

            return list as IList;
        }

        public static Type GetListElementType(this Type t)                           // 对这个t做一个List<T>
        {
            if (!IsListProperty(t))
                return null;

            return t.GetGenericArguments().Single();
        }


        public static bool IsEnumerableType(Type type)                      // 判断是否是集合类型
        {
            return (type.GetInterface("IEnumerable") != null);
        }

		public static void ShallowCopyTo(this object origin, object target)  // 对target进行浅copy  注意：该函数存在问题，目前没有测试所有可能性
		{
			System.Reflection.PropertyInfo[] properties = (target.GetType()).GetProperties();
			System.Reflection.FieldInfo[] fields = (origin.GetType()).GetFields();
			for(int i = 0; i < fields.Length; i++)
			{
				for(int j = 0; j < properties.Length; j++)
				{
					if(fields[i].Name == properties[j].Name && properties[j].CanWrite)
					{
						properties[j].SetValue(target, fields[i].GetValue(origin), null);
					}
				}
			}
		}

		public static void  SetProperty(this object obj, string property, object value)  
		{
            PropertyInfo pi = obj.GetType().GetProperty(property);
            if (pi == null)
            {
                throw new ArgumentException();
            }

            pi.SetValue(obj, value);
        }

        public static object  GetProperty(this object obj, string property)  
		{
            PropertyInfo pi = obj.GetType().GetProperty(property);
            if (pi == null)
            {
                throw new ArgumentException();
            }

            return pi.GetValue(obj);
        }

		public static object  Call(this object obj, string func, params object[] parameters)  
		{
            MethodInfo mi = obj.GetType().GetMethod(func);
            if (mi == null)
                throw new ArgumentException();

            return mi.Invoke(obj, parameters);
        }

		public static object  Call(this Type t, string func, params object[] parameters) // 创建一个临时变量来call
		{
		    object o = Activator.CreateInstance(t);

		    return o.Call(func, parameters);

		}
    }
}
