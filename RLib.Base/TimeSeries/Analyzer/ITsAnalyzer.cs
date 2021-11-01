///********************************************************************
//    created:	2017/12/20 11:45:04
//    author:	rush
//    email:		
	
//    purpose:	时间序列分析器
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RLib.Base
//{

//    public interface ITsAnalyzer
//    {
//#region 时间数据
//        int                 TimeFrame { get;}                               // 对应的TimeFrame，用int通用一些

//        DateTime            From { get; }
//        DateTime            To { get; }

//        int                 Extent { get; }                                 // 整个ts中所有stream得extent

//        DateTime            Time(int index);
//        int                 Index(DateTime time);                           // 找不到返回-1
//#endregion



//        IReadonlyTimeSeries Base { get; }                            // 底层序列

//        bool                IsLive { get; }                                 // 是否是活的数据，一个instance有可能加载的中间一段的历史数据，从而为死的数据

//        bool                IsLoadingData { get; }                          // 是否正在加载数据


//        IReadonlyObservableDictionary<string, IStream> Streams { get; }



//#region Scripts
//        IReadonlyObservableDictionary<ulong, ITsScript> Scripts { get; }                   // 用户无法操作
//        void                _UpdateScripts(int n);                               // 更新Scripts
//        void                _RefreshScripts();                              // 重置script(在加载大量历史数据后)
//        #endregion

//#region 属性更新通知

//        void                RaisePropertyChanged(string propertyName);

//        #endregion
//    }

//}
