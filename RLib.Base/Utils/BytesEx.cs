/********************************************************************
    created:	2019/8/19 20:40:21
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class BytesEx
    {
        public static bool Save(this byte[] data, string filePath)          // 将数据存至一个文件
        {
            if (data == null)
                return false;

            



            return true;
        }

        public static string ToStringUtf8(this byte[] data)                       // tostring
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}
