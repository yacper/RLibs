/********************************************************************
    created:	2022/6/9 10:37:19
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base.Utils
{


public static class MemoryStreamEx
{
    public static string ToStringUtf8(this MemoryStream stream)
    {
        return Encoding.UTF8.GetString(stream.ToArray());
    }
    public static string ToBase64String(this MemoryStream stream)
    {
        return Convert.ToBase64String(stream.ToArray());
    }

    public static string ToStringAscii(this MemoryStream stream)
    {
        return Encoding.ASCII.GetString(stream.ToArray());
    }
    //public static string ToString(this MemoryStream stream)
    //{
    //    return LocalEncoding.GetString(stream.GetBuffer(), 0, (int)stream.Length);
    //}



    public static MemoryStream ToMemoryStreamUtf8(this string s)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(s);
        return new MemoryStream(byteArray);
    }
    public static MemoryStream ToMemoryStreamAscii(this string s)
    {
        byte[] byteArray = Encoding.ASCII.GetBytes(s);
        return new MemoryStream(byteArray);
    }
    public static MemoryStream ToMemoryStreamBase64(this string s)
    {
        return new MemoryStream(Convert.FromBase64String(s));
    }


}
}
