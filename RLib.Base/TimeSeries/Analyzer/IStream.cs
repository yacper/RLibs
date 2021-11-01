/********************************************************************
    created:	2018/1/9 14:44:34
    author:	rush
    email:		
	
    purpose:	数据序列，可以没有date, 配合Analyzer使用
                流，内容可以是float或者BAR
 *              Stream无法单独存在，其必然是对应某个Instance的，否则没有意义
 *              
 *              
 *              
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace RLib.Base
{
    public enum EStreamShapeType
    {
        Dot     = 0,
        Line    = 1,
        Bar     = 2,
    }

    public interface IReadonlyStream : IGroupItem<ulong>, IEnumerable
    {
#region ILIst, IReadOnlyCollection 为了兼容IList<object>, 并且能通过List<object>实现
        object              this[int index] { get; }
        int                 Count { get; }
#endregion

        string              ToString();

        int                 First { get; }                                  // 第一个有意义的下标
        int                 Extent { get; }                                 // 相比于内置stream，outpustream可以有延展

        EStreamShapeType    ShapeTypeProperty { get; }                              // 表现形式

        bool                IsVisibleProperty { get; }                      // 是否可见
        string              LabelProperty { get; }                             // 自己特别设置的Label

        RColor              ColorProperty { get; }                             // Stream 颜色
        ELineStyle          LineStyleProperty { get; }
        int                 LineWidthProperty { get; }

        object              Min(int from, int to);                          // 获取最大
        object              Max(int from, int to);                          // 获取最小
        object              Avg(int from, int to);                          // 平均值
        object              Sum(int from, int to);                          // 总共

        void                _Invalidate();                                  // 
    }

    public interface IStream : IReadonlyStream
    {
        new object              this[int index] { get; set; }

        new EStreamShapeType    ShapeTypeProperty { get; set; }                              // 表现形式

        new bool                IsVisibleProperty { get; set; }                         // 是否可见
        new string              LabelProperty { get; set; }                             // 

        new RColor              ColorProperty { get; set; }                             // Stream 颜色
        new ELineStyle          LineStyleProperty { get; set; }
        new int                 LineWidthProperty { get; set; }
    }


    public interface IReadonlyStream<T> : IGroupItem<ulong>, IReadOnlyList<T> where T:IComparable
    {
        string              ToString();

        int                 First { get; }                                  // 第一个有意义的下标
        int                 Extent { get; }                                 // 相比于内置stream，outpustream可以有延展

        EStreamShapeType    ShapeTypeProperty { get; }                              // 表现形式

        bool                IsVisibleProperty { get; }                         // 是否可见
        string              LabelProperty { get; }                             // 

        RColor              ColorProperty { get; }                             // Stream 颜色
        ELineStyle          LineStyleProperty { get; }
        int                 LineWidthProperty { get; }

        T                   Min(int from, int to);                          // 获取最大
        T                   Max(int from, int to);                          // 获取最小
        T                   Avg(int from, int to);                          // 平均值
        T                   Sum(int from, int to);                          // 总共

        void                _Invalidate();                                  // 
    }

    public interface IStream<T> : IGroupItem<ulong>, IList<T> where T:IComparable
    {
        string              ToString();

        int                 First { get; }                                  // 第一个有意义的下标
        int                 Extent { get; }                                 // 相比于内置stream，outpustream可以有延展


        new EStreamShapeType ShapeTypeProperty { get; set; }                              // 表现形式

        new bool            IsVisibleProperty { get; set; }                         // 是否可见
        new string          LabelProperty { get; set; }                             // 

        new RColor          ColorProperty { get; set; }                             // Stream 颜色
        new ELineStyle      LineStyleProperty { get; set; }
        new int             LineWidthProperty { get; set; }

        T                   Min(int from, int to);                          // 获取最大
        T                   Max(int from, int to);                          // 获取最小
        T                   Avg(int from, int to);                          // 平均值
        T                   Sum(int from, int to);                          // 总共

        void                _Invalidate();                                  // 
    }
}
