/********************************************************************
    created:	2019/9/13 18:49:47
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using RLib.Base;

namespace NeoLib
{
    public interface IBarSeries:IReadOnlyList<IBar> 
    {
        ISymbol             Symbol { get; }

        ITimeSeries         Time { get; }                                   // 

        IDataSeries         Open { get; }
        IDataSeries         High { get; }
        IDataSeries         Low { get; }
        IDataSeries         Close { get; }

        IDataSeries         Volume { get; }

        ETimeFrame          TimeFrame { get; }

        IBar                Last(int index = 0);
        int                 IndexOf(IBar item);

        DateTime?           From { get; }
        DateTime?           To { get; }

        IBarSeries          GetRange(int startIndex, int lastIndex);
        IBarSeries          GetRange(DateTime from, DateTime to);
        bool                ContainsRange(DateTime from, DateTime to);       // 是否包含timerange

        /// <summary>
        /// Loads more historical bars. Method returns the number of loaded bars that were added to the beginning of the collection.
        /// </summary>
        /// <returns></returns>
        int                 LoadMoreHistory();

        /// <summary>Loads more historical bars asynchronously.</summary>
        /// <param name="callback"></param>
        void                LoadMoreHistoryAsync(EventHandler<int> callback = null);

        /// <summary>
        /// Occurs when more history is loaded due to chart scroll on the left or due to API call.
        /// </summary>
        event EventHandler<int> HistoryLoaded;

        /// <summary>Occurs when bars are refreshed due to reconnect.</summary>
        event EventHandler  Reloaded;

        /// <summary>Occurs when a new Tick arrives.</summary>
        /// bool - isBarOpened
        event EventHandler<bool> Tick;

        /// <summary>
        /// Occurs when the last bar is closed and a new bar is opened.
        /// </summary>
        event EventHandler<IBar> BarOpened;
    }


    public interface IForexBarSeries:IBarSeries
    {

        IDataSeries         AskOpen { get; }
        IDataSeries         AskHigh { get; }
        IDataSeries         AskLow { get; }
        IDataSeries         AskClose { get; }

        IDataSeries         BidOpen { get; }
        IDataSeries         BidHigh { get; }
        IDataSeries         BidLow { get; }
        IDataSeries         BidClose { get; }
    }

}
