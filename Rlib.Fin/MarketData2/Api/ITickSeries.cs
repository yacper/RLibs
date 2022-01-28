/********************************************************************
    created:	2020/2/15 15:34:15
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using RLib.Base;

namespace NeoLib
{
    public interface ITickSeries:IReadOnlyList<ITick>
    {
        ITimeSeries         Time { get; }                                   // 
        IDataSeries         Value { get; }

        IDataSeries         Volume { get; }

        ITick               Last(int index = 0);
        int                 IndexOf(ITick item);

        DateTime?           From { get; }
        DateTime?           To { get; }

        ITickSeries         GetRange(int startIndex, int lastIndex);
        ITickSeries         GetRange(DateTime from, DateTime to);
        bool                ContainsRange(DateTime from, DateTime to);       // 是否包含timerange

        int                 LoadMoreHistory();
        void                LoadMoreHistoryAsync(EventHandler<int> callback = null);

        event EventHandler<int> HistoryLoaded;
        event EventHandler  Reloaded;
        event Action<ITickSeries, ITick> Tick;
    }

    public static class TimeValueSeriesEx
    {
        public static DateTime From(this IReadOnlyList<IReadonlyTimeValue> source) 
        {
            Debug.Assert(source != null && source.Any());
            return source[0].Time;
        }
        public static DateTime To(this IReadOnlyList<IReadonlyTimeValue> source) 
        {
            Debug.Assert(source != null && source.Any());
            return source.Last().Time;
        }
        public static IReadOnlyList<IReadonlyTimeValue> GetRange(this IReadOnlyList<IReadonlyTimeValue> source, DateTime t1, DateTime t2) 
        {
            

            return new List<IReadonlyTimeValue>();

        }

    }

}
