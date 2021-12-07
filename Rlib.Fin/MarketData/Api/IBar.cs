/********************************************************************
    created:	2019/6/12 15:43:49
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLib.Base;

namespace RLib.Fin
{
    public enum EBarType
    {
        Rise        = 1,        // 上升
        Decline     = 2,        // 下降
        Even        = 3         // 持平
    }

    public interface IBar:IReadonlyTimeValue
    {
        double              Open { get; }
        double              High { get; }
        double              Low { get; }
        double              Close { get; }
        double              PreClose { get; }                               // 可能没有,nan表示

        double              Volume { get; }
        double              Turnover { get; }

        EBarType            Type { get; }
        double              Range { get; }                                  // 总区间
        double              Solid { get; }                                  // 实体距离
        double              TopShadow { get; }                              // 上影线距离
        double              BottomShadow { get; }                           // 下影响距离
        double              Amplitude { get; }                              // bar震幅
    }

    //public interface IForexBar:IBar
    //{
    //    double              AskOpen { get; }
    //    double              AskHigh { get; }
    //    double              AskLow { get; }
    //    double              AskClose { get; }

    //    double              BidOpen { get; }
    //    double              BidHigh { get; }
    //    double              BidLow { get; }
    //    double              BidClose { get; }

    //}

 



}
