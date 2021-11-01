/********************************************************************
    created:	2015/11/27 14:45:06
    author:		donghuiqi
    email:		
	
    purpose:	TimerItem 定时执行
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel;

namespace RLib.Base
{

    public struct TimePoint
    {
        public TimePoint(double point, object data)
        {
            m_point = point;
            m_data = data;
        }

        public double Point { get { return m_point; } }
        public object Data { get { return m_data; } }

        double m_point;
        object m_data;
    }

    public class TimerItemTiming : TimerItemBasic
    {
        /// <summary>
        /// 定时执行TimerItem
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="callback">回调函数</param>
        /// <param name="paras">回调函数参数</param>
        /// <param name="timePoints">时间点列表，时间点数值为一日初到指定时间点所间隔的秒数</param>
        /// <param name="week">执行的周日</param>
        /// <param name="times">执行的次数</param>
        /// <param name="intervalWeek">间隔的周数</param>
        public TimerItemTiming(object id, Action<object[]> callback, object[] paras, List<TimePoint> timePoints, EWeek week = EWeek.Everyday, int times = 1, uint intervalWeek = 0)
            : base(id, callback, paras, times)
        {
            if (timePoints.Count == 0)
                throw new Exception("至少需要设置一个时间点");

            m_timePoints = timePoints;

            m_timePoints.Sort(delegate(TimePoint tp1, TimePoint tp2) {
                if (tp1.Point > tp2.Point) return 1;
                else if (tp1.Point == tp2.Point) return 0;
                else return -1;
            });

            m_week = week;
            m_intervalWeek = intervalWeek;
        }

        protected override DateTime GenNextTime()
        {
            if (m_nextTime == null)
            {
                DateTime weekDay;
                DateTime now = DateTime.Now;
                if ((Timer.DayOfWeekConvertToEWeek(now.DayOfWeek) & m_week) != 0)
                {
                    weekDay = now;
                }
                else
                {
                    weekDay = GetNextWeekDay(now);
                }

                DateChange(weekDay);
            }

            return TakeExecTime();
        }

        protected override object CallParasAddition()
        {
            return m_nextPoint.Data;
        }
        
        DateTime GetNextWeekDay(DateTime date)
        {
            //在获取周日的下一天时加上间隔周的总天数
            if (date.DayOfWeek == DayOfWeek.Sunday && m_intervalWeek > 0)
            {
                date.AddDays(m_intervalWeek * 7);
            }

            DateTime nextDay = date.AddDays(1);
            if ((Timer.DayOfWeekConvertToEWeek(nextDay.DayOfWeek) & m_week) != 0)
            {
                return nextDay;
            }
            else
            {
                return GetNextWeekDay(nextDay);
            }
        }


        #region 执行日期的时间点
        void DateChange(DateTime date)
        {
            if (m_date != date.Date)
            {
                m_date = date.Date;

                m_surplusPoints.Clear();

                if (DateEquips(date.Date,DateTime.Now.Date))
                {
                    //添加还没有过时的时间点
                    double nowTimePoint = (DateTime.Now - DateTime.Now.Date).TotalSeconds;
                    for (int i = 0; i < m_timePoints.Count; i++)
                    {
                        if (m_timePoints[i].Point >= nowTimePoint)
                        {
                            m_surplusPoints.Add(m_timePoints[i]);
                        }
                    }
                }
                else
                {
                    m_surplusPoints.AddRange(m_timePoints);
                }
            }
        }

        bool DateEquips(DateTime dt1, DateTime dt2)     //是否是同一天
        {
            if (dt1.Year == dt2.Year
                && dt1.Month == dt2.Month
                && dt1.Day == dt2.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        DateTime TakeExecTime()
        {
            if (m_surplusPoints.Count == 0)
            {
                DateChange(GetNextWeekDay(m_date));
            }

            m_nextPoint = m_surplusPoints[0];
            DateTime result = m_date.Date.AddSeconds(m_nextPoint.Point);
            m_surplusPoints.RemoveAt(0);
            return result;
        }


        TimePoint m_nextPoint;
        DateTime m_date = DateTime.MinValue;
        List<TimePoint> m_surplusPoints = new List<TimePoint>();   //剩余的时间点

        
        #endregion

        List<TimePoint> m_timePoints;
        EWeek m_week;
        uint m_intervalWeek;
    }
}
