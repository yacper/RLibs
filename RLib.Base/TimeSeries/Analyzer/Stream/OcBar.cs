/********************************************************************
    created:	2018/1/13 19:53:11
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public enum EBarType
    {
        Rise        = 1,        // 上升
        Decline     = 2,        // 下降
        Even        = 3         // 持平
    }


    public struct OcBar:IComparable
    {
        public int CompareTo(object obj)
        {
            return Close.CompareTo(((OcBar)obj).Close);         // 默认用close比
        }


        public EBarType     Type
        {
            get
            {
                if (Open > Close)
                    return EBarType.Rise;
                else if (Open < Close)
                    return EBarType.Decline;
                else
                    return EBarType.Even;
            }
        }



        public float              Open { get; set; }
        public float              Close { get; set; }
    }
}
