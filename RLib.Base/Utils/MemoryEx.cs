/********************************************************************
    created:	2018/12/25 14:33:17
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class MemoryEx
    {
        public static int   MemomrySize(this object o)                      // 基本度量占据的内存空间
        {
            return Marshal.ReadInt32(o.GetType().TypeHandle.Value, 4);
        }
    }
}
