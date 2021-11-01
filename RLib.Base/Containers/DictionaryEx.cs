/********************************************************************
    created:	2020/7/29 0:27:43
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RLib.Base
{
    public static class DictionaryEx
    {
        public static IDictionary<TKey, TValue> AddIfNotContain<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value) // 如果不包含则添加
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, value);

            return dic;
        }



    }
}
