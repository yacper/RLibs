/********************************************************************
    created:	2018/4/2 13:50:15
    author:	rush
    email:		
	
    purpose:	excel 相关文件
                静态数据文件
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public delegate void    TableLoadedDelegate(IXTable val);




    public partial class XTable<TRow>
    {
        #region Static Apis
        public static string RootDir{ get { return "Tables/"; }}

        #endregion

    }

    public interface IXTable
    {
        string ResourceName { get; }
        bool IsReady { get; }                                        // 是否准备就绪

        void        Load(TableLoadedDelegate callback);
    }
}
