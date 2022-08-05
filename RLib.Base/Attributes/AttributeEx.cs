// created: 2022/07/27 18:10
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RLib.Base.Attributes;

public static class AttributeEx
{
    public static List<T> GetAllAttribute<T>(this Type t) where T : Attribute// 系统的不会搜寻interface
    { 
        List<T> ret = new List<T>();

        // 遍历t的继承链上的interface等
        foreach (Type it in t.GetInterfaces()) { ret.AddRange(it.GetAllAttribute<T>()); }

        // 遍历t的所有property，找到带有attr的属性
        foreach (PropertyInfo pi in t.GetProperties())
        { 
            var d = pi.GetCustomAttribute<T>();
            if (d != null)
                ret.Add(d);
        }

        return ret;
    }

    public static List<T> GetAllAttribute<T>(this object o) where T : Attribute // 系统的不会搜寻interface
        => o.GetType().GetAllAttribute<T>();


    public static List<PropertyInfo> GetAllPropertiesWithAttribute<T>(this Type t) where T : Attribute// 系统的不会搜寻interface
    { 
        List<PropertyInfo> ret = new List<PropertyInfo>();

        // 遍历t的继承链上的interface等
        foreach (Type it in t.GetInterfaces()) { ret.AddRange(it.GetAllPropertiesWithAttribute<T>()); }

        // 遍历t的所有property，找到带有attr的属性
        foreach (PropertyInfo pi in t.GetProperties())
        { 
            var d = pi.GetCustomAttribute<T>();
            if (d != null)
                ret.Add(pi);
        }

        return ret;
    }

    public static List<PropertyInfo> GetAllPropertiesWithAttribute<T>(this object o) where T : Attribute // 系统的不会搜寻interface
        => o.GetType().GetAllPropertiesWithAttribute<T>();

}