/********************************************************************
    created:	2019/9/17 20:13:58
    author:		rush
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
    public class BarSeries:List<IBar>, IBarSeries
    {
#region inner func
        // ret - 是否baropened
        // 不通知, 在外部统一通知
        public bool     _UpdateByTick(ITick t, bool leftAsOpen = true)       
        {
            Bar last = Last() as Bar;

            if (last.Time.Equal(t.Time, TimeFrame, leftAsOpen))
            {
                if (t.Value > last.High)
                {
                    last.High = t.Value;
                    (High as Datas)[High.Count-1] = t.Value;
                }
                else if (t.Value < last.Low)
                {
                    last.Low = t.Value;

                    (Low as Datas)[Low.Count-1] = t.Value;
                }

                last.Close = t.Value;
                (Close as Datas)[Close.Count-1] = t.Value;

                last.Volume += t.Volume;
                (Volume as Datas)[Count -1] += t.Volume;

                return false;
            }
            else
            {
                Bar b = new Bar(t.Time.ModTimeFrame(TimeFrame, leftAsOpen), t.Value, t.Value, t.Value, t.Value, t.Volume);

                _Add(b);

                return true;
            }
        }
        

#endregion




        public ISymbol      Symbol { get; set; }

        public new IBar this[int index]
        {
            get
            {
                if (index >= Count)
                    return null;
                else
                    return base[index];
            }
            set
            {
                if ((uint)index >= (uint)this.Count)
                    throw new ArgumentOutOfRangeException();

                base[index] = value;

                (Time as TimeSeries)[index] = value.Time;

                (Open as Datas)[index] = value.Open;
                (High as Datas)[index] = value.High;
                (Low as Datas)[index] = value.Low;
                (Close as Datas)[index] = value.Close;

                (Volume as Datas)[index] = value.Volume;

            }
        }

        public IEnumerable<IDataSeries> SourceableDss
        {
            get
            {
                return new IDataSeries[]
                {
                    Open,
                    High,
                    Low,
                    Close,
                };
            }

        }



        public new void Add(IBar item)  // 按照时间序列进行排序插入
        {
            // 添加的时候，判断时间是否排序的
            if (this.Count == 0)
                _Add(item);
            else if (item.Time > this[Count - 1].Time)
            {
                _Add(item);
            }
            else if (item.Time < this[0].Time)
            {
                _Insert(0, item);
            }
            else if (/*DuplicateReplace &&*/ item.Time == this[Count - 1].Time  )  // 如果时间相同，置换
            {
                this[Count - 1] = item;
            }
            else if (/*DuplicateReplace &&*/ item.Time == this[0].Time)  // 如果时间相同，置换
            {
                this[0] = item;
            }
            else
            {
                // todo: 后期优化
                bool bInserted = false;
                for (int i = Count - 1; i != -1; --i)
                {
                    if (item.Time > base[i].Time)
                    {
                        _Insert(i + 1, item);
                        bInserted = true;
                        break;
                    }
                    else if (item.Time == base[i].Time)
                    {
                        this[i] = item;
                        bInserted = true;
                        break;
                    }
                }

                if (!bInserted)
                    _Insert(0, item);
            }
        }

        public new void AddRange(IEnumerable<IBar> collection)
        {
            foreach (IBar b in collection)
            {
                Add(b);
            }
        }

        public  void AddRange(IList<IBar> collection)
        {
            int n = collection.Count-1;
            for(; n != -1; --n )
            {
                Add(collection[n]);
            }
        }

        protected void _Add(IBar item)
        {
            base.Add(item);

            (Time as TimeSeries).Add(item.Time);

            (Open as Datas).Add(item.Open);
            (High as Datas).Add(item.High);
            (Low as Datas).Add(item.Low);
            (Close as Datas).Add(item.Close);

            (Volume as Datas).Add(item.Volume);
        }

        protected void _Insert(int index, IBar item)
        {
            base.Insert(index, item);

            (Time as TimeSeries).Insert(index, item.Time);

            (Open as Datas).Insert(index, item.Open);
            (High as Datas).Insert(index, item.High);
            (Low as Datas).Insert(index, item.Low);
            (Close as Datas).Insert(index, item.Close);

            (Volume as Datas).Insert(index, item.Volume);
        }


        public IBar         Last(int index = 0)
        {
            if (index <= Count - 1)
            {
                return this[Count -1 - index];
            }
            else
                return null;
        }



        public ITimeSeries         Time { get; protected set; }                                   // 

        public IDataSeries         Open { get; protected set; }
        public IDataSeries         High { get; protected set; }
        public IDataSeries         Low { get; protected set; }
        public IDataSeries         Close { get; protected set; }

        public IDataSeries         Volume { get; protected set; }

        public ETimeFrame          TimeFrame { get; protected set; }


        public DateTime?    From
        {
            get
            {
                return Count!=0? new DateTime?(base[0].Time):null;
            }
        }

        public DateTime?    To { get{ return Count!=0? new DateTime?(base[Count-1].Time):null; } }


        public new IBarSeries GetRange(int startIndex, int lastIndex)
        {
            List<IBar> l = base.GetRange(startIndex, lastIndex - startIndex +1);

            BarSeries   ret = new BarSeries();
            l.ForEach(p=>ret.Add(p));
            //ret.AddRange(l);

            return ret;

        }

        public IBarSeries   GetRange(DateTime from, DateTime to)
        {
            //todo: 简单要优化
            BarSeries   ret = new BarSeries();

            bool bin = false;
            foreach (var bd in this)
            {
                if(!bin)
                if (bd.Time >= from)
                    bin = true;

                if(bin)
                    ret.Add(bd);

                if(bd.Time >= to)
                    break;
            }

            return bin?ret:null;
        }

        public bool     ContainsRange(DateTime from, DateTime to) // 是否包含timerange
        {
            if (From <= from && To >= to)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Loads more historical bars. Method returns the number of loaded bars that were added to the beginning of the collection.
        /// </summary>
        /// <returns></returns>
        public int          LoadMoreHistory()
        {
            int count =  Symbol.LoadMoreHistory(TimeFrame);

            HistoryLoaded?.Invoke(this, count);

            return count;
        }

        /// <summary>Loads more historical bars asynchronously.</summary>
        /// <param name="callback"></param>
        public void         LoadMoreHistoryAsync(EventHandler<int> callback = null)
        {
            Symbol.LoadMoreHistoryAsync(TimeFrame, (s, e) =>
            {
                callback?.Invoke(this, e);
                HistoryLoaded?.Invoke(this, e);
            });
        }


        public void         NotifyTickEvent(bool isBarOpened)
        {
            Tick?.Invoke(this, isBarOpened);

            if(isBarOpened)
                BarOpened?.Invoke(this, this.Last());
        }

        /// <summary>
        /// Occurs when more history is loaded due to chart scroll on the left or due to API call.
        /// </summary>
        public event EventHandler<int> HistoryLoaded;

        /// <summary>Occurs when bars are refreshed due to reconnect.</summary>
        public event EventHandler  Reloaded;

        /// <summary>Occurs when a new Tick arrives.</summary>
        /// bool - isBarOpened
        public event EventHandler<bool> Tick;

        /// <summary>
        /// Occurs when the last bar is closed and a new bar is opened.
        /// </summary>
        public event EventHandler<IBar> BarOpened;



        public BarSeries(ETimeFrame tf = ETimeFrame.Day)
        {
            TimeFrame = tf;

            Time = new TimeSeries();

            Open = new Datas(){ Tag = "Open"};
            High = new Datas(){Tag = "High"};
            Low = new Datas(){Tag = "Low"};
            Close = new Datas(){ Tag = "Close"};

            Volume = new Datas(){ Tag= "Volume"};
        }


    }
}
