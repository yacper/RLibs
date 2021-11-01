/********************************************************************
    created:	2017/6/22 11:13:36
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using NPOI.DDF;

namespace RLib.Base
{
    public class TimeSeries<T>:RObservableCollection<T>,ITimeSeries, IReadonlyTimeSeries<T>, ITimeSeries<T>  where T:IReadonlyTimeValue
    {

#region Overrides
        public  new void        Add(T item)
        {
				//base.Add(item);
	   //     return;


			// 添加的时候，判断时间是否排序的
			if(this.Count == 0)
				base.Add(item);
            else if (DuplicateReplace && item.Time == this[Count - 1].Time )
			{
			   this[Count - 1] = item;
			}
			else if(item.Time >= this[Count - 1].Time)
			{
				base.Add(item);
			}
			else
			{
				bool bInserted = false;
				for(int i = Count - 1; i != -1; --i)
				{
					if(item.Time >= base[i].Time)
					{
						InsertItem(i+1, item);
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

				if(!bInserted)
					InsertItem(0, item);
			}
		}

#endregion


        public int          TimeFrame { get { return m_nTimeFrame; }}                               // 对应的TimeFrame，用int通用一些

        public DateTime     From { get { return base[0].Time; } }
        public DateTime     To { get{ return base[Count-1].Time; } }


        public bool         AllowDisorder { get { return m_bAllowDisorder; } }  // 允许乱序
        public bool         DuplicateReplace { get { return m_bDuplicateReplace; } } // 重复置换，还是添加

        public DateTime     Time(int index) {  return base[index].Time; }

        public int          Index(DateTime time) // 找不到返回-1
        {
            int ret = -1;
            // todo: 待优化
            foreach (T t in this)
            {
                if (t.Time == time)
                    break;
            }

            return ret;
        }



        public virtual void Update(ITimeValue v)                            // 更新ts
        {
            // 默认直接添加
//            Add(v);
        }


        public IReadonlyTimeSeries GetRange(DateTime from, DateTime to)
        {
            // todo: 简单实现, 后面要改
            TimeSeries<T>   ret = new TimeSeries<T>();

            bool bin = false;
            foreach (T bd in this)
            {
                if (bd.Time >= from)
                    bin = true;

                if(bin)
                    ret.Add(bd);

                if(bd.Time >= to)
                    break;
            }

            return ret;
            
        }

        public IList<T>     GetRange2(DateTime from, DateTime to)
        {
            List<T> ret = new List<T>();

            bool bin = false;
            foreach (T bd in this)
            {
                if (bd.Time == from)
                    bin = true;

                if(bin)
                    ret.Add(bd);

                if(bd.Time == to)
                    break;
            }

            return ret;
        }

        public bool         ContainRange(DateTime from, DateTime to) // 是否包含在timerange中
        {
            if (From <= from && To >= to)
                return true;
            else
                return false;
        }

        IList<object> IReadonlyTimeSeries.GetRange2(DateTime from, DateTime to)
        {
            return GetRange(from, to) as IList<object>;
        }


        object              IReadonlyObservableCollection.this[int index] { get { return this[index]; } }
        object              ITimeSeries.this[int index] { get { return this[index]; }set { this[index] = (T)value; } }


        new T               this[int index]
        {
            get
            {
                return base[index];
            }
            set { base[index] = (T) value; }
        }


#region c&D

        public              TimeSeries(int timeFrame = 0, bool duplicateReplace = true, bool alloDisorder = false)
        {
            m_nTimeFrame = timeFrame;

            m_bAllowDisorder = alloDisorder;
            m_bDuplicateReplace = duplicateReplace;
        }
#endregion

#region Members
        protected int       m_nTimeFrame;                                   // 对应的TimeFrame，用int通用一些

        protected bool m_bAllowDisorder;                          // 允许乱序
        protected bool m_bDuplicateReplace; // 重复置换，还是添加

        #endregion
    }


//    public class BarTS : TimeSeries<IBar>
//    {
//#region c&D
//        public              BarTS(int timeFrame = 0)
//            :base(timeFrame)
//        {
//        }
//#endregion
//    }
//    public class TickTS : TimeSeries<ITick>
//    {

//        //public override void Update(ITimeValue v) // 更新ts
//        //{
//        //    add
          
            
//        //}


//#region c&D
//        public              TickTS(int timeFrame = 0)
//            :base(timeFrame)
//        {
//        }
//#endregion
//    }
}
