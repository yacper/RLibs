/********************************************************************
    created:	2021/7/27 12:00:58
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
    public interface ISerializer
    {
        bool                Serialize<T>(IEnumerable<T> data, string path, bool writeHeader = true);

        List<T>             Deserialize<T>(string path, bool useHeader = true, int headerRow= 0, int from = 1, int? to= null);
        List<T>             Deserialize<T>(string path, int from = 0, int? to= null);
    }
}
