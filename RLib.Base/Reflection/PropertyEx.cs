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

namespace RLib.Base
{


public static class PropertyEx
{

    public static PropertyInfo GetPropertyWithName(this Type t, string pName)
    {
        var pi = t.GetProperty(pName);
        if(pi != null)
            return pi;

        foreach (Type it in t.GetInterfaces()) 
        {
           pi = it.GetPropertyWithName(pName);
           if (pi == null) 
                continue; 

           break;
        }
        
        return pi;
    }

    public static List<PropertyInfo> GetPropertiesWithAttribute(this Type t, Type attributeType, IEnumerable<string> excepts = null) // 系统的不会搜寻interface
    {
        List<PropertyInfo> ret = new List<PropertyInfo>();

        // 遍历t的继承链上的interface等
        foreach (Type it in t.GetInterfaces()) { ret.AddRange(it.GetPropertiesWithAttribute(attributeType, excepts)); }

        // 遍历t的所有property，找到带有attr的属性
        foreach (PropertyInfo pi in t.GetProperties())
        {
            var d = pi.GetCustomAttribute(attributeType);
            if (d != null && !ret.Contains(pi))
            {
                if (excepts != null)
                {
                    if (!excepts.Contains(pi.Name))
                        ret.Add(pi);
                }
                else
                    ret.Add(pi);
            }
        }

        return ret;
    }

    public static List<PropertyInfo> GetPropertiesWithAttribute<T>(this Type t, IEnumerable<string> excepts = null) where T : Attribute // 系统的不会搜寻interface
    {
        return t.GetPropertiesWithAttribute(typeof(T), excepts);
    }

    public static List<PropertyInfo> GetPropertiesWithAttribute(this Type t, IEnumerable<Type> attributeTypes, IEnumerable<string> excepts = null) // 系统的不会搜寻interface
    {
        List<PropertyInfo> pInfos = new List<PropertyInfo>();
        foreach (Type attr in attributeTypes) 
        {
            foreach(var pinfo in t.GetPropertiesWithAttribute(attr, excepts))
            {
                if(pInfos.Contains(pinfo))
                    continue;
                pInfos.Add(pinfo);
            }
        }
        return pInfos;
    }

    public static List<PropertyInfo> GetPropertiesWithAttribute<T>(this object o, IEnumerable<string> excepts = null) where T : Attribute // 系统的不会搜寻interface
        => o.GetType().GetPropertiesWithAttribute<T>(excepts);

     public static List<PropertyInfo> GetPropertiesWithAttribute(this object o, IEnumerable<Type> attributeTypes, IEnumerable<string> excepts = null) // 系统的不会搜寻interface
        => o.GetType().GetPropertiesWithAttribute(attributeTypes, excepts);
  
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

    public static void ApplyKvProperties(this object o, IEnumerable<KeyValuePair<string, object>> kvs)
    {
        if (kvs == null)
            return;
        try
        {
            foreach (var kv in kvs)
            {
                try
                {//单个内部出错正常

                        var pi = o.GetType().GetProperty(kv.Key);
                        if(pi.PropertyType.BaseType == typeof(Enum))
                            pi.SetValue(o, Enum.Parse(pi.PropertyType, (string)kv.Value));
                        else if (pi.GetValue(o) is IConvertible)
                            pi.SetValue(o, Convert.ChangeType(kv.Value, pi.PropertyType));
                        else
                            pi.SetValue(o, kv.Value);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static bool ApplyKvPropertiesJson(this object o, string json, IEnumerable<JsonConverter>? exConverters=null )
    {
        if (json.IsNullOrWhiteSpace())
            return false;
        try
        {
            Dictionary<string, object> kvs = json.ToJsonObj<Dictionary<string, object>>(exConverters);

            object obj = null;
            try
            {
                //if (exConverters != null)
                    obj = json.ToJsonObj(o.GetType(), exConverters);
            }
            catch (Exception e)
            {
            }
            foreach (var kv in kvs)
            {
                try
                {//单个内部出错正常

                    //if (exConverters == null)// 常规的方式
                    if (obj == null)// 常规的方式
                    {
                        var pi = o.GetType().GetProperty(kv.Key);
                        if(pi.PropertyType.BaseType == typeof(Enum))
                            pi.SetValue(o, Enum.Parse(pi.PropertyType, (string)kv.Value));
                        else if (pi.GetValue(o) is IConvertible)
                            pi.SetValue(o, Convert.ChangeType(kv.Value, pi.PropertyType));
                        else
                            pi.SetValue(o, kv.Value);
                    }
                    else// 需要额外转换的情况
                        o.SetProperty(kv.Key, obj.GetPropertyValue(kv.Key));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
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
}