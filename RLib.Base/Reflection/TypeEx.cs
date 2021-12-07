/********************************************************************
    created:	2020/1/8 12:34:58
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NPOI.SS.Formula.Functions;

namespace RLib.Base
{
    public static class TypeEx
    {
        public static object CreateInstance(this Type t, params object[] args) // 主要目的是设置Para DefaultValue
        {
            object o = Activator.CreateInstance(t, args);
            //if (o != null)
            //{
            //    // 对nesting property赋初值
            //    foreach (IPara p in o.GetType().GetParas())
            //    {
            //        if (p.NestingParas != null)
            //        {
            //            object nest = null;
            //            if (p.ValueType.IsInterface)
            //                nest = App.Instance.Container.Resolve(p.ValueType);
            //            else
            //                nest = Activator.CreateInstance(p.ValueType);

            //            (p as Para).PI.SetValue(o, nest);
            //        }
            //    }

            //    // 设置para
            //    foreach (IPara p in o.GetParas())
            //    {
            //        //object def = RReflector.GetDefaultValue(p.ValueType);

            //        // 如果是默认值，设置def
            //        if (p.DefValue != null &&
            //           object.Equals(p.Value, p.ValueType.GetDefaultValue()))
            //        {
            //            p.Value = p.DefValue;
            //        }


            //        // 内部复合参数
            //        if (p.DefValue==null && p.NestingParas != null)
            //        {// 生成一个默认的对象，并对其赋嵌套para

            //            foreach (IPara n in p.NestingParas)
            //            {
            //                n.Value = n.DefValue;
            //            }
            //        }


            //    }
            //}

            return o;
        }

        public static T     CreateInstance<T>(params object[] args)
        {
            T ret = (T)CreateInstance(typeof(T), args);

            return ret;
        }


        public static object GetDefaultValue(this Type type)                /// 获取变量的默认值
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsBulitinType(this Type type)                        // 是否是系统类型， Enum也作为系统类型
        {
            return (type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object ||
                type.Name == "TimeSpan");  // 认为TimeSpan也是内置类型（虽然不在c#的内置类型里）
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

        public static Type GetNotNullableType(this Type type)               // 如果是nullable，获取其underlying
        {
            if (IsNullableType(type))
                //return System.Nullable.GetUnderlyingType(value);
                return type.GetGenericArguments().Single();
            else
                return type;
        }

        // todo: need test
        public static bool Is<T>(this Type t)
        {
            return typeof(T).IsAssignableFrom(t);
        }





        public static T     GetPropertyValue<T>(this Type t, string pro)    // 获取Type的property的值
        {

            //t.GetProperty()
            //PropertyInfo info = ProductType.GetProperties().Single(p =>
            //    p.Name == prop && p.PropertyType == ProductType);

            //if (info != null)
            //    return info.GetValue(m_pProtoObj);

            throw new NotImplementedException();
        }

        public static Type  GetTypeFromAllAssemblies(this string typeFullName)
        {
            List<Type> lt = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assemblies)
            {
                lt.AddRange(a.GetTypes());
            }

            return lt.FirstOrDefault(p => p.FullName == typeFullName);
        }


    }
}
