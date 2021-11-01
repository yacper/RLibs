/********************************************************************
    created:	2020/1/8 12:34:58
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
using NPOI.SS.Formula.Functions;

namespace RLib.Base
{
    public static class TypeEx
    {

        public static T     GetPropertyValue<T>(this Type t, string pro)    // 获取Type的property的值
        {

            //t.GetProperty()
            //PropertyInfo info = ProductType.GetProperties().Single(p =>
            //    p.Name == prop && p.PropertyType == ProductType);

            //if (info != null)
            //    return info.GetValue(m_pProtoObj);

            throw new NotImplementedException();
        }

        public static Type  GetTypeFromAllAssemblies(this string typeFullName)
        {
            List<Type> lt = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assemblies)
            {
                lt.AddRange(a.GetTypes());
            }

            return lt.FirstOrDefault(p => p.FullName == typeFullName);
        }


    }
}
