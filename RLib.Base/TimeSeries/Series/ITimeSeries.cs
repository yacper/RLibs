/********************************************************************
    created:	2017/6/22 10:53:54
    author:		rush
    email:		
	
    purpose:	时间序列
                !默认前提是使用者自己维护数据是根据时间排序的

*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public interface IReadonlyTimeSeries : IReadonlyObservableCollection
    {
        int                 TimeFrame { get;}                               // 对应的TimeFrame，用int通用一些

        DateTime            From { get; }
        DateTime            To { get; }

        bool                AllowDisorder { get; }                          // 允许乱序
        bool                DuplicateReplace { get; }                       // 重复置换，还是添加


        DateTime            Time(int index);
        int                 Index(DateTime time);                           // 找不到返回-1

        IList<object>       GetRange2(DateTime from, DateTime to);
        IReadonlyTimeSeries GetRange(DateTime from, DateTime to);
        bool                ContainRange(DateTime from, DateTime to);       // 是否包含timerange

        //void                Update(ITimeValue v);                           // 更新ts
    }

    public interface ITimeSeries : IReadonlyTimeSeries
    {
        new object          this[int index] { get; set; }
    }


    public interface IReadonlyTimeSeries<T>: IReadonlyObservableCollection<T>
    {
        int                 TimeFrame { get;}                               // 对应的TimeFrame，用int通用一些

        DateTime            From { get; }
        DateTime            To { get; }

        bool                AllowDisorder { get; }                          // 允许乱序
        bool                DuplicateReplace { get; }                       // 重复置换，还是添加

        DateTime            Time(int index);
        int                 Index(DateTime time);                           // 找不到返回-1

        IList<T>            GetRange2(DateTime from, DateTime to);
        IReadonlyTimeSeries GetRange(DateTime from, DateTime to);
        bool                ContainRange(DateTime from, DateTime to);       // 是否包含timerange

//        void                Update(ITimeValue v);                           // 更新ts
    }


    public interface ITimeSeries<T> : IObservableCollection<T>
    {
        int                 TimeFrame { get;}                               // 对应的TimeFrame，用int通用一些

        DateTime            From { get; }
        DateTime            To { get; }

        bool                AllowDisorder { get; }                          // 允许乱序
        bool                DuplicateReplace { get; }                       // 重复置换，还是添加

        DateTime            Time(int index);
        int                 Index(DateTime time);                           // 找不到返回-1

        IList<T>            GetRange2(DateTime from, DateTime to);
        IReadonlyTimeSeries GetRange(DateTime from, DateTime to);
        bool                ContainRange(DateTime from, DateTime to);       // 是否包含timerange

 //       void                Update(ITimeValue v);                           // 更新ts
    }

}
