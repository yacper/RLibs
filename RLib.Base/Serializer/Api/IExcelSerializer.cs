/********************************************************************
    created:	2021/7/27 12:48:23
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



    public interface IExcelSerializer : ISerializer
    {
        bool                Serialize<T>(IEnumerable<T> data, string path, string sheet = null, bool writeHeader = true);

        List<T>             Deserialize<T>(string path, string sheet = null, bool useHeader = true, int headerRow= 0, int from = 1, int? to= null);
        List<T>             Deserialize<T>(Stream stream, string sheet = null, bool useHeader = true, int headerRow = 0, int from = 1, int? to = null);

        List<T>             DeserializeAll<T>(Stream stream, bool useHeader = true, int headerRow = 0, int from = 1, int? to = null);
        List<T>             DeserializeAll<T>(string path, bool useHeader = true, int headerRow = 0, int from = 1, int? to = null);
    }

}
