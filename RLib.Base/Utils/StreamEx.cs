/********************************************************************
    created:	2021/8/23 20:27:15
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

namespace RLib.Base
{
    public static class StreamEx
    {
        public static string String(this Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            {
                string text = reader.ReadToEnd();
                return text;
            }
        }


    }
}
