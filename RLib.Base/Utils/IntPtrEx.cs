/********************************************************************
    created:	2019/8/21 16:25:23
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class IntPtrEx
    {
        public static string ToHex(this IntPtr p)                           // 转换成16进制
        {
            return string.Format("{0:x}", p.ToInt32());
        }

    }
}
