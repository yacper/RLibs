// created: 2022/07/27 17:59
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using System.ComponentModel;
using System.Reflection;

namespace RLib.Base.Attributes;

public static class DefaultValueAttributeEx
{
    public static void ApplyDefaultValues(this object o, Type t = null)      // 如果有设置defaultvalue，apply
    {
        if(t == null)
            t = o.GetType();
        // 遍历t的继承链上的interface等
        foreach (Type it in t.GetInterfaces()) { o.ApplyDefaultValues(it); }

        // 遍历t的所有property，找到带有attr的属性
        foreach (PropertyInfo pi in t.GetProperties())
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