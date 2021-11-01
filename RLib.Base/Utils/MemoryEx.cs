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
    public class MemoryEx
    {
        public static int   ObjectSize(object o)                            // 基本度量object size
        {
            return Marshal.ReadInt32(o.GetType().TypeHandle.Value, 4);
        }
    }
}
