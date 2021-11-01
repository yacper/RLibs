/********************************************************************
    created:	2021/8/20 15:17:29
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
    public static class ExcelEx
    {
        public static List<T> FromExcel<T>(this string path, string sheet = null,  bool useHeader = true, int headerRow= 0, int from = 1, int? to= null)
        {
            ExcelSerializer se = new ExcelSerializer();
            return se.Deserialize<T>(path, sheet, useHeader, headerRow, from, to);
        }
        public static List<T> FromExcelAll<T>(this string path, bool useHeader = true, int headerRow= 0, int from = 1, int? to= null)
        {
            ExcelSerializer se = new ExcelSerializer();
            return se.DeserializeAll<T>(path, useHeader, headerRow, from, to);
        }

        public static List<T> FromExcel<T>(this Stream stream, string sheet = null,  bool useHeader = true, int headerRow= 0, int from = 1, int? to= null)
        {
            ExcelSerializer se = new ExcelSerializer();
            return se.Deserialize<T>(stream, sheet, useHeader, headerRow, from, to);
        }

        public static List<T> FromExcelAll<T>(this Stream stream, bool useHeader = true, int headerRow= 0, int from = 1, int? to= null)
        {
            ExcelSerializer se = new ExcelSerializer();
            return se.DeserializeAll<T>(stream, useHeader, headerRow, from, to);
        }

        public static bool  ToExcel<T>(this IEnumerable<T> data, string path, string sheet = null, bool writeHeader = true)
        {
            ExcelSerializer se = new ExcelSerializer();
            return se.Serialize<T>(data, path, sheet, writeHeader);
        }
    }
}
