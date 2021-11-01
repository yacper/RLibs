/********************************************************************
    created:	2020/1/6 17:57:32
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guid = System.Guid;

namespace RLib.Base
{
    public static class GuidEx
    {
        public static bool IsGuid(this string str)
        {
            Guid o;
            return Guid.TryParse(str, out o);
        }

        public static Guid? ToGuid(this string str)
        {
            Guid o;
            if (Guid.TryParse(str, out o))
                return o;
            else
                return null;
        }
    }
}
