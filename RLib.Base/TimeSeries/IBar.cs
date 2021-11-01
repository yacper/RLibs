///********************************************************************
//    created:	2018/3/13 15:17:57
//    author:	rush
//    email:		
	
//    purpose:	基本bar
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RLib.Base
//{
//    public enum EBarType
//    {
//        Rise        = 1,        // 上升
//        Decline     = 2,        // 下降
//        Even        = 3         // 持平
//    }

//    public interface IReadonlyBar:  IReadonlyTimeValue
//    {
//        float               Open { get; }
//        float               High { get; }
//        float               Low { get; }
//        float               Close { get; }

//        long                Volume { get; }

//        EBarType            Type { get; }

//        IBar                 UpdateByTick(float v);
//    }


//    public interface IBar: ITimeValue
//    {
//        float               Open { get; set; }
//        float               High { get; set; }
//        float               Low { get; set; }
//        float               Close { get; set; }

//        long                Volume { get; set; }

//        /// 衍生值
//        float               Amplitude  { get; }                             // 振幅 

//        float               Change { get; }                                 // 涨跌
//        float               ChangePercent { get; }                          // 涨跌百分比


//        EBarType            Type { get; }

//        IBar                 UpdateByTick(float v);
//    }
//}
