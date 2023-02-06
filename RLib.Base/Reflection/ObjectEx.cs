/********************************************************************
    created:	2019/7/5 16:27:29
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class ObjectEx
    {
        public static TObject WithProperty<TObject, TValue>(this TObject obj, Expression<Func<TObject, TValue>> memberLambda, TValue value)
        {
            var memberSelectorExpression = memberLambda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) { property.SetValue(obj, value, null); }
            }

            return obj;
        }


        public const string NullText = "null";

        public static string NullableToString(this object obj)
        {
            if (obj != null)
                return obj.ToString();

            return NullText;
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

        public static bool  EqualEx(this object a, object b)
        {
            if (object.Equals(a, b))
                return true;
            else
                return false;
        }

		public static bool  EqualEx(this string a, string b)
        {
            if (string.Compare(a, b) == 0)
                return true;
            else
                return false;
        }
       
        public static bool  EqualEx(this bool a, bool b)
        {
            if (a == b)
                return true;
            else
                return false;
        }
        public static bool  EqualEx(this long a, long b)
        {
            if (a == b)
                return true;
            else
                return false;
        }
        public static bool  EqualEx(this float a, float b)
        {
            if (Math.Abs(a - b) <= MathEx.Epsilon)
                return true;
            else
                return false;
        }
        public static bool  EqualEx(this double a, double b)
        {
            if (Math.Abs(a - b) <= MathEx.Epsilon)
                return true;
            else
                return false;
        } 
     




        public static object GetFieldValue(this object o, string fieldName)
        {
            if (o == null)
                return null;

            return o.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic)?.GetValue(o);
        }
        public static T     GetFieldValue<T>(this object o, string fieldName)
        {
            if (o == null)
                return default(T);

            return (T)o.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic)?.GetValue(o);
        }


        public static object GetPropertyValue(this object o, string propertyName)
        {
            if (o == null)
                return null;

            return o.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic )?.GetValue(o);
        }
        public static T     GetPropertyValue<T>(this object o, string propertyName)
        {
            if (o == null)
                return default(T);

            return (T)o.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public| BindingFlags.NonPublic)?.GetValue(o);
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
