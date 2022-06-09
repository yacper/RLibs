﻿/********************************************************************
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

namespace RLib.Base.Utils;

public static class MemoryStreamEx
{
    public static string ToStringUtf8(this MemoryStream stream)
    {
        return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
    }

    public static string ToStringAscii(this MemoryStream stream)
    {
        return Encoding.ASCII.GetString(stream.GetBuffer(), 0, (int)stream.Length);
    }


}