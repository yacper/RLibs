/********************************************************************
    created:	2020/2/15 17:39:36
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace NeoLib
{
    public class TickSeries: List<ITick>, ITickSeries
    {
        public ISymbol      Symbol { get; set; }

        public new ITick this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                if ((uint)index >= (uint)this.Count)
                    throw new ArgumentOutOfRangeException();

                base[index] = value;

                (Time as TimeSeries)[index] = value.Time;
                (Value as Datas)[index] = value.Value;
              
                (Volume as Datas)[index] = value.Volume;
            }
        }

        public new void Add(ITick item)                                     // 判断如果有sequence的情况下，同sequence替换
        {
            if (item.SequenceID != null)
            {
                // 添加的时候，判断序列号排序
                if (this.Count == 0)
                    _Add(item);
                else if (/*DuplicateReplace &&*/ item.SequenceID.Value == this[Count - 1].SequenceID )  // 如果时间相同，置换
                {
                    this[Count - 1] = item;
                }
                else if (item.SequenceID >= this[Count - 1].SequenceID)
                {
                    _Add(item);
                }
                else
                {
                    bool bInserted = false;
                    for (int i = Count - 1; i != -1; --i)
                    {
                        if (item.SequenceID >base[i].SequenceID)
                        {
                            _Insert(i + 1, item);
                            bInserted = true;
                            break;
                        }
                        else if (item.SequenceID == base[i].SequenceID)
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
            else
            {
                //todo: tickserise 需要加入id，不然无法判断是否完全一致
                // 添加的时候，判断时间是否排序的
                if (this.Count == 0)
                    _Add(item);
                //else if (/*DuplicateReplace &&*/ item.Time.Equal(this[Count - 1].Time, ETimeFrame.T1))  // 如果时间相同，置换
                //{
                //    this[Count - 1] = item;
                //}
                else if (item.Time >= this[Count - 1].Time)
                {
                    _Add(item);
                }
                else
                {
                    bool bInserted = false;
                    for (int i = Count - 1; i != -1; --i)
                    {
                        if (item.Time >= base[i].Time)
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

        }

        public void     NotifyTickEvent()
        {
            Tick?.Invoke(this, Last());
        }

        public new void AddRange(IEnumerable<ITick> collection)
        {
            foreach (ITick t in collection)
            {
                Add(t);
            }
        }

        protected void      _Add(ITick item)
        {
            base.Add(item);

            (Time as TimeSeries).Add(item.Time);
            (Value as Datas).Add(item.Value);

            (Volume as Datas).Add(item.Volume);
        }

        protected void      _Insert(int index, ITick item)
        {
            base.Insert(index, item);

            (Time as TimeSeries).Add(item.Time);
            (Value as Datas).Add(item.Value);

            (Volume as Datas).Add(item.Volume);
        }



        public ITick        LastValue
        {
            get { return this.LastOrDefault(); }
        }

        public ITick        Last(int index = 0)
        {
            if (index <= Count - 1)
            {
                return this[Count-1 - index];
            }
            else
                return null;
        }

        public ITimeSeries  Time { get; protected set; }                                   // 
        public IDataSeries  Value { get; protected set; }

        public IDataSeries  Volume { get; protected set; }



        public DateTime?    From
        {
            get
            {
                return Count!=0? new DateTime?(base[0].Time):null;
            }
        }

        public DateTime?    To { get{ return Count!=0? new DateTime?(base[Count-1].Time):null; } }

        public new ITickSeries GetRange(int startIndex, int lastIndex)
        {
            List<ITick> l = base.GetRange(startIndex, lastIndex - startIndex +1);

            TickSeries   ret = new TickSeries();
            ret.AddRange(l);

            return ret;
        }

        public ITickSeries GetRange(DateTime from, DateTime to)
        {
            TickSeries   ret = new TickSeries();

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


        public int          LoadMoreHistory()
        {
            int count =  Symbol.LoadMoreHistory(ETimeFrame.T1);

            HistoryLoaded?.Invoke(this, count);

            return count;
        }

        public void         LoadMoreHistoryAsync(EventHandler<int> callback = null)
        {
            Symbol.LoadMoreHistoryAsync(ETimeFrame.T1, (s, e) =>
            {
                callback?.Invoke(this, e);
                HistoryLoaded?.Invoke(this, e);
            });
        }

        public event EventHandler<int> HistoryLoaded;
        public event EventHandler  Reloaded;
        public event Action<ITickSeries, ITick> Tick;


        public TickSeries()
        {
            Time = new TimeSeries();

            Value = new Datas();
            Volume = new Datas();
        }
    }
}
