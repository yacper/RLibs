// created: 2022/09/06 16:24
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace RLib.Base;

public static class PropertyEx
{

    public static List<PropertyInfo> GetPropertiesWithAttribute(this Type t, Type attributeType) // 系统的不会搜寻interface
    {
        List<PropertyInfo> ret = new List<PropertyInfo>();

        // 遍历t的继承链上的interface等
        foreach (Type it in t.GetInterfaces()) { ret.AddRange(it.GetPropertiesWithAttribute(attributeType)); }

        // 遍历t的所有property，找到带有attr的属性
        foreach (PropertyInfo pi in t.GetProperties())
        {
            var d = pi.GetCustomAttribute(attributeType);
            if (d != null)
                ret.Add(pi);
        }

        return ret;
    }

    public static List<PropertyInfo> GetPropertiesWithAttribute<T>(this Type t) where T : Attribute // 系统的不会搜寻interface
    {
        return t.GetPropertiesWithAttribute(typeof(T));
    }

    public static List<PropertyInfo> GetPropertiesWithAttribute(this Type t, IEnumerable<Type> attributeTypes) // 系统的不会搜寻interface
    {
        return attributeTypes.SelectMany(p => t.GetPropertiesWithAttribute(p)).ToList();
    }

    public static List<PropertyInfo> GetPropertiesWithAttribute<T>(this object o) where T : Attribute // 系统的不会搜寻interface
        => o.GetType().GetPropertiesWithAttribute<T>();

     public static List<PropertyInfo> GetPropertiesWithAttribute(this object o, IEnumerable<Type> attributeTypes) // 系统的不会搜寻interface
        => o.GetType().GetPropertiesWithAttribute(attributeTypes);
  

    public static string ToKvPropertiesJson(this object o, IEnumerable<string> properties, IEnumerable<JsonConverter>? exConverters=null )
    {
        Type                       t   = o.GetType();
        var                        pis = t.GetProperties();
        Dictionary<string, object> kvs = new Dictionary<string, object>();
        foreach (var s in properties)
        {
            var pi = pis.LastOrDefault(p => p.Name == s);
            if(pi != null)
                kvs[pi.Name]= pi.GetValue(o);
        }

        return kvs.ToJson(exConverters);
    }


    public static string ToKvPropertiesJson(this object o, IEnumerable<JsonConverter>? exConverters=null )
    {
        Type                       t   = o.GetType();
        var                        pis = t.GetProperties();
        Dictionary<string, object> kvs = new Dictionary<string, object>();
        foreach (PropertyInfo pi in pis)
        {
            kvs[pi.Name]= pi.GetValue(o);
        }

        return kvs.ToJson(exConverters);
    }
    public static string ToKvPropertiesWithAttributeJson<T>(this object o, IEnumerable<JsonConverter>? exConverters=null ) where T:Attribute
    {
        Type                       t   = o.GetType();
        var                        pis = t.GetPropertiesWithAttribute<T>();
        Dictionary<string, object> kvs = new Dictionary<string, object>();
        foreach (PropertyInfo pi in pis)
        {
            kvs[pi.Name]= pi.GetValue(o);
        }

        return kvs.ToJson(exConverters);
    }


    public static bool ApplyKvPropertiesJson(this object o, string json, IEnumerable<JsonConverter>? exConverters=null )
    {
        try
        {
            var obj = json.ToJsonObj(o.GetType(), exConverters);

            Dictionary<string, object> kvs = json.ToJsonObj<Dictionary<string, object>>();
            foreach (var kv in kvs)
            {
                o.SetProperty(kv.Key, obj.GetPropertyValue(kv.Key));
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);

            return false;
        }
    }

    public static bool ApplyPropertiesWithAttributeSequential<T>(this object o, params object[] parameterValues) where T: Attribute
    {
        try
        {
            var pis = o.GetPropertiesWithAttribute<T>();
            int count = Math.Min(pis.Count, parameterValues.Length);

            for (int i = 0; i != count; ++i)
            {
                var pi = pis[i];

                if (parameterValues[i] is IConvertible)
                    pi.SetValue(o, Convert.ChangeType(parameterValues[i], pi.PropertyType));
                else
                    pi.SetValue(o, parameterValues[i]);
            }

            return true;
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e);

            return false;
        }
    }


}