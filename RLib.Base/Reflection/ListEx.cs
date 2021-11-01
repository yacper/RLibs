/********************************************************************
    created:	2021/11/1 20:15:01
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
    public static class ListEx
    {
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


    }
}
