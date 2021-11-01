/********************************************************************
    created:	2021/11/1 20:10:51
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class GenericEx
    {
        public static Type GetGenericArgument(this Type t, int index=0)     // 获取泛型第几位参数
        {
            if (t.IsGenericType)
                return t.GetGenericArguments()[index];
            else
                return null;
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

       

    }
}
