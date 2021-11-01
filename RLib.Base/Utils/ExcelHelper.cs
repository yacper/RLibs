/********************************************************************
    created:	2019/7/3 16:34:15
    author:		rush
    email:		
	
    purpose:	方便Npoi Excel的各类操作
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace RLib.Base
{
    public static class ExcelHelper
    {

        public static ISheet CreateSheet(string path, string sheetName = "") // 创建一个xls并且返回一个Sheet
        {
            XSSFWorkbook workbook2007 = new XSSFWorkbook();  //新建xlsx工作簿  
            if(string.IsNullOrWhiteSpace(sheetName))
                return  workbook2007.CreateSheet();
            else
                return  workbook2007.CreateSheet(sheetName);
        }

        public static bool  WriteRow(ISheet sheet, int index, IEnumerable<object> objs) // 对sheet的写入一行
        {
            IRow r = sheet.GetRow(index);
            if(r == null)
                r = sheet.CreateRow(index);          // 获取名字行

            int i = 0;
            foreach (object o in objs)
            {
                XSSFCell cell = (XSSFCell)r.CreateCell(i);
                WriteCell(cell, o);

                i++;
            }

            return true;
        }

        public static bool WriteCell(XSSFCell cell, object o)
        {
            if (o != null)
            {
                if(o is string)
                    cell.SetCellValue((string)o);
                else if(o is bool)
                    cell.SetCellValue((bool)o);
                else if(RReflector.IsNumber(o))
                    cell.SetCellValue((double)o);
                else if (o is DateTime)
                    cell.SetCellValue((DateTime) o);
                else
                    return false;
            }

            return true;
        }

    }
}
