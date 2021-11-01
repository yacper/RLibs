/********************************************************************
    created:	2018/4/2 13:52:05
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;

namespace RLib.Base
{
    public partial class XTable<TRow>:IXTable
    {
        public virtual string ResourceName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool        IsReady { get { return m_bReady; } }                                        // 是否准备就绪


        public void         Load(TableLoadedDelegate callback)
        {
            string path = RootDir + ResourceName;
            OnTableLoadedEvent = callback;
            byte[] data = File.ReadAllBytes(path);

            OnResourceLoaded(data);
        }


        // 应该是一个通用的读取流程, 以后有时间再写
        public virtual void OnResourceLoaded(byte[] bytes)
        {
            //IWorkbook workbook = null;  //新建IWorkbook对象  
            //ISheet sheet = null;
            //FileStream fileStream = null;


            //// 打开excel
            //try
            //{
            //    fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            //    if (file.IndexOf(".xlsx") > 0) // 2007版本  
            //    {
            //        workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
            //    }
            //    else if (file.IndexOf(".xls") > 0) // 2003版本  
            //    {
            //        workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
            //    }

            //    if (workbook.NumberOfSheets == 0)
            //    {
            //        throw new Exception(file + " 不包含Sheet!");
            //    }

            //    sheet = workbook.GetSheetAt(0);  //获取第一个工作表  


            //    // 解析第一行的说明
            //    IRow specsRow = sheet.GetRow(0);
            //    IInstrument ins = __ParseSpec(specsRow.GetCell(0).StringCellValue);
            //    if (ins == null)
            //        return false;

            //    // 解析剩余所有data
            //    for (int i = 1; i <= sheet.LastRowNum; i++)  //对工作表每一行  
            //    {
            //        IRow row = sheet.GetRow(i);   //row读入第i行数据  
            //        if (row != null &&
            //            row.Cells[0] != null &&
            //            row.Cells[0].CellType != CellType.Blank)
            //        {
            //            object obj = __ParseRow(row);
            //            if (obj == null)
            //                break;

            //            ins._AddTv(Header.TF, obj as ITimeValue);
            //        }
            //        else
            //            break;
            //    }


            //    // 插入数据
            //    //string index = FileFeeder.___MakeIndex(ins.Name, ins.BaseTimeFrame);
            //    //feeder.Datas[index] = ts;

            //    return true;
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine("解析Excel出错:" + exception);
            //    return false;
            //}
            //finally
            //{
            //    fileStream.Close();
            //    workbook.Close();
            //}
            
        }


        protected  TableLoadedDelegate OnTableLoadedEvent;


#region Members

        
        protected List<TRow> m_pTableArray;
        protected bool          m_bReady;
#endregion
    }
}
