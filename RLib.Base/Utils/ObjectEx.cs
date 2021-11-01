/********************************************************************
    created:	2019/7/5 16:27:29
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

namespace RLib.Base
{
    public static class ObjectEx
    {
        public const string NullText = "null";

        public static string NullableToString(this object obj)
        {
            if (obj != null)
                return obj.ToString();

            return NullText;
        }

        public static object GetFieldValue(this object o, string fieldName)
        {
            if (o == null)
                return null;

            return o.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic).GetValue(o);
        }
        public static T     GetFieldValue<T>(this object o, string fieldName)
        {
            if (o == null)
                return default(T);

            return (T)o.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic).GetValue(o);
        }


        public static object GetPropertyValue(this object o, string propertyName)
        {
            if (o == null)
                return null;

            return o.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic ).GetValue(o);
        }
        public static T     GetPropertyValue<T>(this object o, string propertyName)
        {
            if (o == null)
                return default(T);

            return (T)o.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic).GetValue(o);
        }
    }
}
