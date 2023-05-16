/********************************************************************
    created:	2021/8/20 15:17:29
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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




        public static IWorkbook NewWorkbook(string filePattern)
        {
            if (filePattern.EndsWith(".xlsx", true, CultureInfo.CurrentCulture)) // 2007 版本
                return new XSSFWorkbook();
            else if (filePattern.EndsWith(".xls", true, CultureInfo.CurrentCulture))
                return new HSSFWorkbook(); // 2003 版本
            return null;
        }

        public static void ClearRows(this ISheet sheet)
        {
            int to = sheet.LastRowNum;
            for (int i = 0; i != to; ++i)
            {
                IRow r = sheet.GetRow(i);
                sheet.RemoveRow(r);
            }
        }

        public static bool IsNullOrEmpty(this ICell cell)
        {
            if (cell != null)
            {
                // Uncomment the following lines if you consider a cell 
                // with no value but filled with color to be non-empty.
                //if (cell.CellStyle != null && cell.CellStyle.FillBackgroundColorColor != null)
                //    return false;

                switch (cell.CellType)
                {
                    case CellType.String:
                        return string.IsNullOrWhiteSpace(cell.StringCellValue);
                    case CellType.Boolean:
                    case CellType.Numeric:
                    case CellType.Formula:
                    case CellType.Error:
                        return false;
                }
            }

            // null, blank or unknown
            return true;
        }


    }
}
