///********************************************************************
//    created:	2018/1/9 14:44:34
//    author:	rush
//    email:		
	
//    purpose:	数据序列，可以没有date, 配合Analyzer使用
//                流，内容可以是float或者BAR
// *              Stream无法单独存在，其必然是对应某个Instance的，否则没有意义
// *              
// *              
// *              
//*********************************************************************/
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DataModel;

//namespace RLib.Base
//{
//    public enum EStreamType // 流类型
//    {
//        FloatStream = 0,
//        DoubleStream,
//        TickStream,
//        BarStream
        
//    }


//    public enum EStreamShapeType
//    {
//        Dot     = 0,
//        Line    = 1,
//        Bar     = 2,
//    }

//    public struct SLevelInfo   // Level 效果
//    {
//        public float        Level;
//        public ELineStyle   Style;
//        public int          Width;
//        public RColor       Color;

//        public  SLevelInfo(float level, ELineStyle style, int width, RColor color)
//        {
//            Level = level;
//            Style = style;
//            Width = width;
//            Color = color;
//        }
//    }
    
//    public interface IReadonlyStream : IHaveIDAndObservable<string>, IReadonlyTimeSeries
//    {
//        ITsAnalyzer         Analyzer { get; }
//        EStreamType         Type { get; }

//        bool                IsSuppurtVolume { get; }                        // 是否支持Volume 数据

//        string              ToString();

//        int                 First { get; }                                  // 第一个有意义的下标
//        int                 Extent { get; }                                 // 相比于内置stream，outpustream可以有延展

//        EStreamShapeType    ShapeType { get; }                              // 表现形式

//        bool                IsVisible { get; }                      // 是否可见
//        string              Label { get; }                             // 自己特别设置的Label

//#region Colors
//        RColor              Color { get; }                                  // 生成默认Color
//        RColor              GetColor(int index);    
//        void                SetColor(int index, RColor color);
//#endregion

//        ELineStyle          LineStyle { get; }
//        int                 LineWidth { get; }

//        IObservableCollection<SLevelInfo> Levels { get; }

//        object              Min(int from, int to);                          // 获取最大
//        object              Max(int from, int to);                          // 获取最小
//        object              Avg(int from, int to);                          // 平均值
//        object              Sum(int from, int to);                          // 总共

//        void                _Invalidate();                                  // 
//    }

//    public interface IStream : IReadonlyStream
//    {
//        new object          this[int index] { get; set; }

//        new EStreamShapeType ShapeType { get; set; }                              // 表现形式

//        new bool            IsVisible { get; set; }                         // 是否可见
//        new string          Label { get; set; }                             // 


//#region Colors
//        void                SetColor(int index, RColor color);
//#endregion

//        void                AddLevel(float level, RColor color, ELineStyle style = ELineStyle.LINE_SOLID, int width = 1);

//        new RColor          Color { get; set; }                             // Stream 颜色
//        new ELineStyle      LineStyle { get; set; }
//        new int             LineWidth { get; set; }
//    }


//    public interface IReadonlyStream<T> : IHaveIDAndObservable<string>, IReadonlyTimeSeries<T> 
//    {
//        ITsAnalyzer         Analyzer { get; }
//        EStreamType         Type { get; }

//        bool                IsSuppurtVolume { get; }                        // 是否支持Volume 数据

//        string              ToString();

//        int                 First { get; }                                  // 第一个有意义的下标
//        int                 Extent { get; }                                 // 相比于内置stream，outpustream可以有延展

//        EStreamShapeType    ShapeType { get; }                              // 表现形式

//        bool                IsVisible { get; }                         // 是否可见
//        string              Label { get; }                             // 

//#region Colors
//        RColor              Color { get; }                                  // Stream 颜色
//        RColor              GetColor(int index);    
//#endregion

//        ELineStyle          LineStyle { get; }
//        int                 LineWidth { get; }

//        IObservableCollection<SLevelInfo> Levels { get; }

//        T                   Min(int from, int to);                          // 获取最大
//        T                   Max(int from, int to);                          // 获取最小
//        T                   Avg(int from, int to);                          // 平均值
//        T                   Sum(int from, int to);                          // 总共

//        void                _Invalidate();                                  // 
//    }

//    public interface IStream<T> : IHaveIDAndObservable<string>, ITimeSeries<T> 
//    {
//        ITsAnalyzer         Analyzer { get; }
//        EStreamType         Type { get; }

//        bool                IsSuppurtVolume { get; }                        // 是否支持Volume 数据

//        string              ToString();

//        int                 First { get; }                                  // 第一个有意义的下标
//        int                 Extent { get; }                                 // 相比于内置stream，outpustream可以有延展


//        new EStreamShapeType ShapeType { get; set; }                              // 表现形式

//        new bool            IsVisible { get; set; }                         // 是否可见
//        new string          Label { get; set; }                             // 

//#region Colors
//        RColor              Color { get; }                                  // Stream 颜色
//        RColor              GetColor(int index);    
//        void                SetColor(int index, RColor color);
//#endregion

//        new ELineStyle      LineStyle { get; set; }
//        new int             LineWidth { get; set; }

//        IObservableCollection<SLevelInfo> Levels { get; }
//        void                AddLevel(float level, RColor color, ELineStyle style = ELineStyle.LINE_SOLID, int width = 1);

//        T                   Min(int from, int to);                          // 获取最大
//        T                   Max(int from, int to);                          // 获取最小
//        T                   Avg(int from, int to);                          // 平均值
//        T                   Sum(int from, int to);                          // 总共

//        void                _Invalidate();                                  // 
//    }
//}
