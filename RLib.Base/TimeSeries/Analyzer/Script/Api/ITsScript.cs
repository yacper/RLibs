///********************************************************************
//    created:	2016/11/5 19:20:12
//    author:		rush
//    email:		
	
//    purpose:	脚本
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using RLib.Base;

//namespace RLib.Base
//{
//    public enum ETsScriptType
//    {
//        Indicator   = 1,
//        Oscillator  = 2,
//        Strategy    = 3
//    }

//    public enum ESourceType         // source 类型
//    {
//        Tick,                       // tick stream
//        Bar                         // bar stream
//    }


//    public interface ITsScript:IDynamicProduct<string>  // 同一个名字只有一种
//    {
//#region 脚本固有属性，必填Properties
//        string              Name { get; }
//        string              Group { get;  }
//        ESourceType         RequiredSource { get; }                    // source 类型

//        void                OnUpdate(int period);
//#endregion


//        ETsScriptType       ScriptType { get;}
//        IStream             Source { get; }
//        string              GenID { get; }                                  // 这个GENID可以生成一个tsScript
//        ITsAnalyzer         Host { get; set; }

////        string              Label { get; }


//        IReadonlyObservableDictionary<string, IReadonlyStream> Streams { get; }
//        IStream             AddStream(string id, EStreamType type, EStreamShapeType shape, string label, RColor color, int firstPeriod, int extent);
//        IStream             AddInternalStream(string id, EStreamType type, int firstPeriod, int extent);  

//        void                _Update(int period);                            // 系統更新

//        string              ToString(int period);
//    }



//}
