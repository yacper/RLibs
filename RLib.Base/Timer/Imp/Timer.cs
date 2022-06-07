﻿//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using DataModel;


//namespace RLib.Base
//{


//    public partial class Timer
//    {

//        public static double NowTimePoint
//        {
//            get { return (DateTime.Now - DateTime.Now.Date).TotalSeconds; }
//        }

//        public static DayOfWeek EWeekToDayOfWeek(EWeek week)
//        {
//            DayOfWeek dow;
//            switch (week)
//            {
//                case EWeek.Sunday:
//                    dow = DayOfWeek.Sunday;
//                    break;
//                case EWeek.Monday:
//                    dow = DayOfWeek.Monday;
//                    break;
//                case EWeek.Tuesday:
//                    dow = DayOfWeek.Tuesday;
//                    break;
//                case EWeek.Wednesday:
//                    dow = DayOfWeek.Wednesday;
//                    break;
//                case EWeek.Thursday:
//                    dow = DayOfWeek.Thursday;
//                    break;
//                case EWeek.Friday:
//                    dow = DayOfWeek.Friday;
//                    break;
//                case EWeek.Saturday:
//                    dow = DayOfWeek.Saturday;
//                    break;
//                default:
//                    dow = DayOfWeek.Monday;
//                    break;
//            }
//            return dow;
//        }

//        public static EWeek DayOfWeekConvertToEWeek(DayOfWeek dayOfWeek)
//        {
//            EWeek week = EWeek.Monday;

//            switch (dayOfWeek)
//            {
//                case DayOfWeek.Friday:
//                    week = EWeek.Friday;
//                    break;
//                case DayOfWeek.Monday:
//                    week = EWeek.Monday;
//                    break;
//                case DayOfWeek.Saturday:
//                    week = EWeek.Saturday;
//                    break;
//                case DayOfWeek.Sunday:
//                    week = EWeek.Sunday;
//                    break;
//                case DayOfWeek.Thursday:
//                    week = EWeek.Thursday;
//                    break;
//                case DayOfWeek.Tuesday:
//                    week = EWeek.Tuesday;
//                    break;
//                case DayOfWeek.Wednesday:
//                    week = EWeek.Wednesday;
//                    break;
//            }

//            return week;
//        }

//        /// <summary>
//        /// 添加绝对时间Timer
//        /// </summary>
//        /// <param name="callback">回调</param>
//        /// <param name="paras">参数</param>
//        /// <param name="timePoints">时间点列表，时间点数值为某日初到指定时间点所间隔的秒数</param>
//        /// <param name="week">执行的周日</param>
//        /// <param name="times">执行的次数,小于等于0则执行无数次</param>
//        /// <returns></returns>
//        public static object Add(Action<object[]> callback, object[] paras, List<double> timePoints, EWeek week = EWeek.Everyday, int times = 1, uint intervalWeek = 0)
//        {
//            var timePointStructs = new List<TimePoint>();
//            for (int i = 0; i < timePoints.Count; i++)
//            {
//                timePointStructs.Add(new TimePoint(timePoints[i], null));
//            }

//            return Add(callback, paras, timePointStructs, week, times, intervalWeek);
//        }

//        /// <summary>
//        /// 添加绝对时间Timer
//        /// </summary>
//        /// <param name="callback">回调</param>
//        /// <param name="paras">参数</param>
//        /// <param name="timePoints">时间点列表，时间点数值为某日初到指定时间点所间隔的秒数</param>
//        /// <param name="week">执行的周日</param>
//        /// <param name="times">执行的次数,小于等于0则执行无数次</param>
//        /// <returns></returns>
//        public static object Add(Action<object[]> callback, object[] paras, List<TimePoint> timePoints, EWeek week = EWeek.Everyday, int times = 1, uint intervalWeek = 0)
//        {
//            if (callback == null)
//            {
//                //Logger.Main.LogError("添加无效Timer");
//                Debug.Assert(false, "添加无效Timer");
//                return null;
//            }

//            object id = new object();
//            var titem = new TimerItemTiming(id, callback, paras, timePoints, week, times, intervalWeek);
//            titem.Init();
//            Add(titem);

//            return titem.ID;
//        }
       
//        /// <summary>
//        /// 添加间隔执行Timer
//        /// </summary>
//        /// <param name="callback">回调</param>
//        /// <param name="paras">参数</param>
//        /// <param name="timeSpan">时间间隔</param>
//        /// <param name="times">执行的次数,小于等于0则执行无数次</param>
//        /// <returns></returns>
//        public static object Add(Action<object[]> callback, object[] paras, TimeSpan timeSpan, int times)
//        {
//            if (callback == null)
//            {
//                //Logger.Main.LogError("添加无效Timer");
//                Debug.Assert(false, "添加无效Timer");
//                return null;
//            }
            
//            object id = new object();
//            var titem = new TimerItemInterval(id, callback, paras, timeSpan, times);
//            titem.Init();
//            Add(titem);

//            return titem.ID;
//        }

//        public static object Add(Action<object[]> callback, object[] paras, double second, int times)
//        {
//            return Add(callback, paras, TimeSpan.FromSeconds(second), times);
//        }

//        public static object   Add(Action<object[]> callback, object[] paras, TimeSpan timeSpan)     //执行一次
//        {
//            return Add(callback, paras, timeSpan,1);
//        }

//        public static object Add(Action<object[]> callback, object[] paras, double second)
//        {
//            return Add(callback, paras, TimeSpan.FromSeconds(second));
//        }

//        public static bool     Remove(object id)
//        {
//            if (m_removeList.Find(item => item.ID == id) != null)
//                return false;

//            for (int i = 0; i < m_timerItemList.Count; i++)
//            {
//                if (m_timerItemList[i].ID == id)
//                {
//                    m_removeList.Add(m_timerItemList[i]);
//                    return true;
//                }
//            }
//            return false;
//        }

//        public static DateTime? GetTimerNextTime(object timerID)
//        {
//            for (int i = 0; i < m_timerItemList.Count; i++)
//            {
//                if (m_timerItemList[i].ID == timerID)
//                {
//                    return m_timerItemList[i].NextTime;
//                }
//            }
//            return null;
//        }


//        #region Lua帮助函数
//        public static object[] GenObjectArray(object obj)
//        {
//            return new object[] { obj };
//        }
//        public static object[] GenObjectArray(object obj1, object obj2)
//        {
//            return new object[] { obj1,obj2 };
//        }
//        public static object[] GenObjectArray(object obj1, object obj2, object obj3)
//        {
//            return new object[] { obj1, obj2, obj3 };
//        }
//        public static object[] GenObjectArray(object obj1, object obj2, object obj3, object obj4)
//        {
//            return new object[] { obj1, obj2, obj3, obj4 };
//        }
//        #endregion

//        public static void Update()        
//        {
//            if (m_removeList.Count > 0)
//            {
//                for (int i = 0; i < m_removeList.Count; i++)
//                {
//                    m_timerItemList.Remove(m_removeList[i]);
//                }
//                m_removeList.Clear();
//            }

//            List<ITimerItem> executes = new List<ITimerItem>();

//            DateTime now = DateTime.Now;
//            for (int i = 0; i < m_timerItemList.Count; i++)
//            {
//                var item = m_timerItemList[i];

//                if (item.NextTime <= now)
//                {
//                    executes.Add(item);
//                }
//                else
//                {
//                    break;
//                }
//            }
            
//            if (executes.Count > 0)
//            {
//                for (int i = 0; i < executes.Count; i++)
//                {
//                    var executeItem = executes[i];
//                    try
//                    {
//                        executeItem.Call();
//                    }
//                    catch (Exception ex)
//                    {
//                        RLibBase.Logger.Error(ex.ToString());
//                        //Debug.Assert(false, ex.ToString());
//                    }

//                    //Timer执行一次之后如果没有完结需在队列中重新排序
//                    m_timerItemList.Remove(executeItem);
//                    if (!executeItem.Complete)
//                    {
//                        Add(executeItem);
//                    }

//                }
//            }
//        }

//        protected static void Add(ITimerItem titem)
//        {
//            //按时间倒序
//            bool isInsert = false;
//            for (int i = 0; i < m_timerItemList.Count; i++)
//            {
//                if (titem.NextTime <= m_timerItemList[i].NextTime)
//                {
//                    m_timerItemList.Insert(i, titem);
//                    isInsert = true;
//                    break;
//                }
//            }
//            if (!isInsert)
//                m_timerItemList.Add(titem);
//        }

//        protected static List<ITimerItem> m_removeList = new List<ITimerItem>();
//        protected static List<ITimerItem> m_timerItemList = new List<ITimerItem>();
//    }
//}
