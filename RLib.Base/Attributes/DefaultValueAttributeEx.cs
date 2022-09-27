// created: 2022/07/27 17:59
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace RLib.Base.Attributes;

public static class DefaultValueAttributeEx
{
    public static void ApplyDefaultValues(this object o, Type t = null, IEnumerable<Type> excludes = null) // 如果有设置defaultvalue，apply
    {
        if(t == null)
            t = o.GetType();
        if (excludes == null)
            excludes = new List<Type>();

        // 遍历t的继承链上的interface等
        foreach (Type it in t.GetInterfaces()) { o.ApplyDefaultValues(it, excludes); }

        // 遍历t的所有property，找到带有attr的属性
        foreach (PropertyInfo pi in t.GetProperties().Where(p=>!excludes.Contains(p.PropertyType)))
        { 
            var d = pi.GetCustomAttribute<DefaultValueAttribute>();
            if (d != null)
            {
                if (d.Value is string && Type.GetTypeCode(pi.PropertyType) != TypeCode.String)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(pi.PropertyType);
                    if(converter != null)
                        pi.SetValue(o, converter.ConvertFromInvariantString(d.Value as string)); 
                    else
                        pi.SetValue(o, Convert.ChangeType(d.Value, pi.PropertyType)); 
                }
                else { pi.SetValue(o, Convert.ChangeType(d.Value, pi.PropertyType)); }
            }
        }
    }

    public static void ApplyDefaultValue(this object o, string propertyName, Type t = null) // 如果有设置defaultvalue，apply
    {
        if(t == null)
            t = o.GetType();
        // 遍历t的继承链上的interface等
        foreach (Type it in t.GetInterfaces()) { o.ApplyDefaultValue(propertyName, it); }

        // 遍历t的所有property，找到带有attr的属性
        PropertyInfo pi = t.GetProperty(propertyName);
        if(pi != null)
        { 
            var d = pi.GetCustomAttribute<DefaultValueAttribute>();
            if (d != null)
            {
                if (d.Value is string && Type.GetTypeCode(pi.PropertyType) != TypeCode.String)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(pi.PropertyType);
                    if(converter != null)
                        pi.SetValue(o, converter.ConvertFromInvariantString(d.Value as string)); 
                    else
                        pi.SetValue(o, Convert.ChangeType(d.Value, pi.PropertyType)); 
                }
                else { pi.SetValue(o, Convert.ChangeType(d.Value, pi.PropertyType)); }
            }
        }


    }
}