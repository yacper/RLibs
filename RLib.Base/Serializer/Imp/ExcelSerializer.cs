/********************************************************************
    created:	2021/7/27 13:04:27
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace RLib.Base
{
    public class ExcelSerializer: IExcelSerializer
    {

        public bool         Serialize<T>(IEnumerable<T> data, string path, bool writeHeader = true)
        {
            return Serialize<T>(data, path, null, writeHeader);

        }

        public List<T> Deserialize<T>(string path, bool useHeader = true, int headerRow = 0, int from = 1, int? to = null)
        {
            return Deserialize<T>(path, null, useHeader, headerRow, from, to);

        }

        public List<T> Deserialize<T>(string path, int from = 0, int? to = null)
        {
            return Deserialize<T>(path, null, false, 0, from, to);
        }


        public bool         Serialize<T>(IEnumerable<T> data, string path, string sheet = null, bool writeHeader = true)
        {
            try
            {
                if (FileEX.IsInUsing(path))
                {
                    Logger.Error($"{path} 正被其他程序占用！");

                    return false;
                }

                IWorkbook wb = ReadWorkbook(path);
                if (wb == null)
                    wb = ExcelEx.NewWorkbook(path);

                if (wb == null)
                    return false;

                ISheet s = wb.GetSheet(sheet);
                if (s != null)
                    s.ClearRows();
                else
                    s = sheet != null ? wb.CreateSheet(sheet) : wb.CreateSheet();

                if (writeHeader)
                {
                    __WriteHeader(s, typeof(T));
                    __WriteBody(s, data, 1);
                }
                else
                    __WriteBody(s, data, 0);

                using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    s.Workbook.Write(fs);

                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Serialize {path}:{e}");

                return false;
            }
        }

        public List<T>      Deserialize<T>(string path, string sheet = null, bool useHeader = true, int headerRow = 0, int from = 1, int? to = null)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return Deserialize<T>(fileStream, sheet, useHeader, headerRow, from, to);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error Deserialize {path}:{e}");
            }

            return new List<T>();
        }

        public List<T> DeserializeAll<T>(string path, bool useHeader = true, int headerRow = 0, int from = 1, int? to = null)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return DeserializeAll<T>(fileStream, useHeader, headerRow, from, to);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error Deserialize {path}:{e}");
            }

            return new List<T>();
        }

        public List<T>      DeserializeAll<T>(Stream stream, bool useHeader = true, int headerRow = 0, int from = 1, int? to = null)
        {
            List<T> ret =new List<T>();
            try
            {
                foreach (ISheet s in __ReadAllSheets(stream))
                {
                    Dictionary<string, int> headers = null;
                    if (useHeader)
                        headers = __ReadHeaders(s);

                    ret.AddRange(__ReadBody<T>(s, headers, from, to));
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error Desialize:" + e);
            }

            return ret;
        }

        public List<T>      Deserialize<T>(Stream stream, string sheet = null, bool useHeader = true, int headerRow = 0, int from = 1, int? to = null)
        {
            if (stream == null)
            {
                Logger.Error($"stream 为Null");
                return new List<T>();
            }

            try
            {
                ISheet s = __ReadSheet(stream, sheet);
                if (s == null)
                    return new List<T>();

                Dictionary<string, int> headers = null;
                if (useHeader)
                    headers = __ReadHeaders(s);

                return __ReadBody<T>(s, headers, from, to);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Deserialize {stream}:{e}");
            }

            return new List<T>();
        }

#region Helpers

        public List<string> GetSheetNames(Stream stream) // 获取所有Sheet名
        {
            List<string> ret = new List<string>();
            XSSFWorkbook workbook = null;
            try
            {
                workbook = new XSSFWorkbook(stream); //xlsx数据读入workbook  
                for (int i = 0; i != workbook.NumberOfSheets; ++i)
                {
                    ret.Add(workbook.GetSheetName(i));

                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                // 不能dispose，会导致stream被释放
                //try
                //{
                //    if (workbook != null)
                //        workbook.Dispose();
                //}
                //catch (Exception e)
                //{
                //    Debug.WriteLine(e);
                //}

            }

            return ret;
        }


        ISheet              __ReadSheet(Stream stream, string sheetName = null) // 从一个已有的excel中读出一个sheet
        {
            IWorkbook workbook = null; //新建IWorkbook对象  
            ISheet sheet = null;

            try
            {
                try
                {
                    workbook = WorkbookFactory.Create(stream);
                }
                catch (Exception e)
                {
                    Logger.Error($"无法创建workbook:{e}");
                }

                if (workbook.NumberOfSheets == 0)
                {
                    Logger.Error("workbook 不包含Sheet!");
                    return null;
                }

                //
                if (!string.IsNullOrWhiteSpace(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Logger.Error("不包含Sheet:" + sheetName);
                        return null;
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0); //获取第一个工作表  
                }
            }
            catch (Exception e)
            {
                Logger.Error("读Excel失败:" +   " " +e);
            }

            return sheet;
        }


        IEnumerable<ISheet> __ReadAllSheets(Stream stream) // 从一个已有的excel中读出所有
        {
            IWorkbook workbook = null; //新建IWorkbook对象  
            List<ISheet> sheets = new List<ISheet>();

            try
            {
                try
                {
                    workbook = WorkbookFactory.Create(stream);
                }
                catch (Exception e)
                {
                }

                if (workbook.NumberOfSheets == 0)
                {
                    Logger.Error(" worksheet 不包含Sheet!");
                    return sheets;
                }

                for (int i = 0; i != workbook.NumberOfSheets; ++i)
                {
                    sheets.Add(workbook.GetSheetAt(i));

                }
            }
            catch (Exception e)
            {
                Logger.Error("读Excel失败:" +   " " +e);
            }

            return sheets;
        }

        IWorkbook ReadWorkbook(string path)
        {
            IWorkbook workbook = null; 
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (path.IndexOf(".xlsx") > 0) // 2007版本  
                    {
                        workbook = new XSSFWorkbook(fileStream); //xlsx数据读入workbook  
                    }
                    else if (path.IndexOf(".xls") > 0) // 2003版本  
                    {
                        workbook = new HSSFWorkbook(fileStream); //xls数据读入workbook  
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("读Excel失败:" + path + " " + e);
            }

            return workbook;
        }



        ISheet              __ReadSheet(string path, string sheetName = null) // 从一个已有的excel中读出一个sheet
        {
            ISheet sheet = null;

            try
            {
                IWorkbook workbook = null; //新建IWorkbook对象  
                FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                if (path.IndexOf(".xlsx") > 0) // 2007版本  
                {
                    workbook = new XSSFWorkbook(fileStream); //xlsx数据读入workbook  
                }
                else if (path.IndexOf(".xls") > 0) // 2003版本  
                {
                    workbook = new HSSFWorkbook(fileStream); //xls数据读入workbook  
                }

                if (workbook.NumberOfSheets == 0)
                {
                    Logger.Error(path + " 不包含Sheet!");
                    return null;
                }

                //
                if (!string.IsNullOrWhiteSpace(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Logger.Error(path + " 不包含Sheet:" + sheetName);
                        return null;
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0); //获取第一个工作表  
                }
            }
            catch (Exception e)
            {
                Logger.Error("读Excel失败:" +  path + " " +e);
            }
            finally
            {

                
            }

            return sheet;
        }

        ISheet              __CreateSheet(string path, string sheetName = null)
        {
            HSSFWorkbook  workbook2007 = new HSSFWorkbook ();  //新建xlsx工作簿  
            if(string.IsNullOrWhiteSpace(sheetName))
                return  workbook2007.CreateSheet();
            else
                return  workbook2007.CreateSheet(sheetName);
        }

        protected bool      __WriteHeader(ISheet sheet, Type t = null, int col = 0)
        {
            IRow nameRow = sheet.GetRow(0);
            if(nameRow == null)
                nameRow = sheet.CreateRow(0);          // 获取名字行
           

            int c = col;
            foreach (PropertyInfo info in t.GetProperties())
            {
                // 剔除Protobuf2 specified属性
                if (info.Name.EndsWith("Specified"))
                    continue;

                // 定义了jsonIgnore，忽略
                if(info.IsDefined(typeof(JsonIgnoreAttribute)))
                    continue;

                //// !必须同时有get和set，否则不序列化
                //if (info.GetGetMethod() == null || info.GetSetMethod() == null)
                //{
                //    // protobuf repeadted<>
                //    if (!info.PropertyType.IsGeneric(typeof(RepeatedField<>)))
                //        continue;
                //}

                // name cell
                ICell cell = (ICell)nameRow.CreateCell(c);
                cell.SetCellValue(info.Name);

                c++;
            }

            return true;
        }

        protected bool      __WriteBody<T>(ISheet sheet, IEnumerable<T> dms, int startRow = 1)
        {
            int i = startRow;
            foreach (T v in dms)
            {
                IRow r = sheet.CreateRow(i);   //创建10行  
                __WriteRow(r, v);

                i++;
            }

            return true;
        }

        protected bool      __WriteRow<T>(IRow r, T v) // 写行，可以嵌套写
        {
            Type t = v.GetType();
            int i = 0;
            foreach (PropertyInfo info in t.GetProperties())
            {
                // 剔除Protobuf2 specified属性
                if (info.Name.EndsWith("Specified"))
                    continue;

                // 定义了jsonIgnore，忽略
                if(info.IsDefined(typeof(JsonIgnoreAttribute)))
                    continue;

                //// !必须同时有get和set，否则不序列化
                //if (info.GetGetMethod() == null || info.GetSetMethod() == null)
                //{
                //    // protobuf repeadted<>
                //    if (!info.PropertyType.IsGeneric(typeof(RepeatedField<>)))
                //        continue;
                //}


                ICell cell = (ICell)r.CreateCell(i);
                __WriteCell(cell, v, info);
                i++;
            }

            return true;
        }

        protected void      __WriteCell(ICell cell, object o, PropertyInfo info)
        {
            object? v = info.GetValue(o);
            if (v == null)
                return;

            string tt = info.PropertyType.GetNotNullableType().FullName; // 正常使用这个类型，但如果有特殊指定

            if (info.PropertyType.GetNotNullableType().BaseType.FullName == "System.Enum") // enum 必须单独出来
            {
                cell.SetCellValue(EnumEx.ToString(info.PropertyType, v));   // 写enum的string表示
            }
            else
            {
                switch (tt)
                {
                    case "System.Boolean":
                        cell.SetCellValue((bool) v);
                        break;
                    case "System.Int32":
                        cell.SetCellValue((int) v);
                        break;
                    case "System.UInt32":
                        cell.SetCellValue((uint) v);
                        break;
                    case "System.Int64":
                        cell.SetCellValue((long) v);
                        break;
                    case "System.UInt64":
                        cell.SetCellValue((ulong) v);
                        break;
                    case "System.String":
                        cell.SetCellValue((string) v);
                        break;
                    case "System.Single":
                        cell.SetCellValue((float) v);
                        break;
                    case "System.Double":
                        if(!double.IsNaN((double)v))
                            cell.SetCellValue((double) v);
                        break;

                    case "System.DateTime":
                        cell.SetCellValue((DateTime)v);
                        break;
                    default:
                    {
                        try
                        {
                            string str = v.ToJson();
                            cell.SetCellValue(str);
                        }
                        catch (Exception e)
                        {
                            Logger.Error($"Write Cell Error: {info.Name}, {e.ToString()}");
                        }
                    }
                        break;
                }
            }

        }

        protected void      __ReadCell(ICell cell, object obj, PropertyInfo info)
        {
            if (cell.IsNullOrEmpty())
            {
                //if(!info.PropertyType.IsNullableType() && info.PropertyType != typeof(double))
                //    Logger.Error($"解析{info.Name}出错，type:{info.PropertyType} cell 为空");
                //else
                //    return; // null类型或者double，直接返回
                // todo: 空直接返回，要么默认值，要么null
                return;
            }

            string tt = info.PropertyType.GetNotNullableType().FullName;  // 正常使用这个类型，但如果有特殊指定

            try
            {
                //if (info.Name.Contains("_Dt")||
                //    info.Name.ToLower().Contains("time"))   // 包含time就作为datetime
                //    tt = "DateTime";  // 作为dt存储,用string表示，而不是原始的int64

                if (info.PropertyType.BaseType.FullName == "System.Enum") // enum 必须单独出来
                {
                    info.SetValue(obj, Enum.Parse(info.PropertyType, cell.StringCellValue));
                }
                else if (info.PropertyType.IsGeneric(typeof(RepeatedField<>)))
                {
                    // protobuf repeted

                    string filed = info.Name + "_";
                    filed = char.ToLower(filed[0]) + filed.Substring(1);
                    var ft = obj.GetType().GetField(filed, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (ft == null)
                    {
                        Logger.Error("can't find filed " + filed);
                        return;
                    }

                    ft.SetValue(obj, cell.StringCellValue.ToJsonObj(info.PropertyType));
                }
                else
                {
                    switch (tt)
                    {
                        case "System.Boolean":
                            info.SetValue(obj, cell.BooleanCellValue);
                            break;
                        case "System.Int32":
                            info.SetValue(obj, Convert.ToInt32(cell.NumericCellValue));
                            break;
                        case "System.UInt32":
                            info.SetValue(obj, Convert.ToUInt32(cell.NumericCellValue));
                            break;
                        case "System.Int64":
                            info.SetValue(obj, Convert.ToInt64(cell.NumericCellValue));
                            break;
                        case "System.UInt64":
                            info.SetValue(obj, Convert.ToUInt64(cell.NumericCellValue));
                            break;
                        case "System.String":
                            if (cell.CellType == CellType.String)
                                info.SetValue(obj, cell.StringCellValue.Trim());      // 对string前后trim一下，经常有写错的
                            else if (cell.CellType == CellType.Numeric)               //处理numeric Cell
                                info.SetValue(obj, cell.NumericCellValue.ToString()); // 对string前后trim一下，经常有写错的
                            break;
                        case "System.Single":
                            info.SetValue(obj, Convert.ToSingle(cell.NumericCellValue)); // 必须转（double无法隐式转single）
                            break;
                        case "System.Double":
                            info.SetValue(obj, Convert.ToDouble(cell.NumericCellValue)); // 必须转（double无法隐式转single）
                            break;

                        case "System.DateTime":
                            info.SetValue(obj, cell.DateCellValue); // parse成dateime，在转ticks
                            break;
                        default:
                        {
                            if (!cell.StringCellValue.IsNullOrWhiteSpace())
                                info.SetValue(obj, cell.StringCellValue.ToJsonObj(info.PropertyType));
                        }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error($"解析{info.Name}出错，type:{info.PropertyType} e:{e.ToString()}");
            }
        }


        protected Dictionary<string, int>  __ReadHeaders(ISheet sheet, int row = 0)
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();

            IRow nameRow = sheet.GetRow(row);
            foreach (ICell c in nameRow.Cells)
            {
                if(string.IsNullOrWhiteSpace(c.StringCellValue))
                    continue;

                ret[c.StringCellValue] = c.ColumnIndex;
            }

            return ret;
        }

        protected List<T>  __ReadBody<T>(ISheet sheet, Dictionary<string, int> headers = null, int fromRow = 1, int? toRow = null)
        {
            List<T> ret = new List<T>();

            int from = fromRow;
            int to = sheet.LastRowNum;
            if (toRow != null && toRow < to)
                to = toRow.Value;

            for (; from <= to; ++from)
            {
                IRow r = sheet.GetRow(from);

                T t = __ReadRow<T>(r, headers);
                if(t != null)
                    ret.Add(t);
            }

            return ret;
        }

        protected T         __ReadRow<T>(IRow r, Dictionary<string, int> headers = null) // 写行，可以嵌套写
        {
            Type t = typeof(T);
            T obj = Activator.CreateInstance<T>();
            obj.DoubleInit();
            int i = 0;
            foreach (PropertyInfo info in t.GetProperties())
            {
                // 剔除Protobuf2 specified属性
                if (info.Name.EndsWith("Specified"))
                    continue;

                // !必须同时有get和set，否则不序列化
                if (info.GetGetMethod() == null || info.GetSetMethod() == null)
                {
                    // protobuf repeadted<>
                    if (!info.PropertyType.IsGeneric(typeof(RepeatedField<>)))
                        continue;
                }


                int index = i;
                if (headers != null)
                {
                    if (!headers.TryGetValue(info.Name, out index))
                        continue;
                }

                ICell cell = r.GetCell(index);
                if (cell != null)
                    __ReadCell(cell, obj, info);

                i++;
            }

            return obj;
        }
#endregion

    }

}
