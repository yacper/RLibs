using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RLib.Base
{
public partial class Timer
{
    public static double NowTimePoint { get { return (DateTime.Now - DateTime.Now.Date).TotalSeconds; } }

    public static DayOfWeek EWeekToDayOfWeek(EWeek week)
    {
        DayOfWeek dow;
        switch (week)
        {
            case EWeek.Sunday:
                dow = DayOfWeek.Sunday;
                break;
            case EWeek.Monday:
                dow = DayOfWeek.Monday;
                break;
            case EWeek.Tuesday:
                dow = DayOfWeek.Tuesday;
                break;
            case EWeek.Wednesday:
                dow = DayOfWeek.Wednesday;
                break;
            case EWeek.Thursday:
                dow = DayOfWeek.Thursday;
                break;
            case EWeek.Friday:
                dow = DayOfWeek.Friday;
                break;
            case EWeek.Saturday:
                dow = DayOfWeek.Saturday;
                break;
            default:
                dow = DayOfWeek.Monday;
                break;
        }

        return dow;
    }

    public static EWeek DayOfWeekConvertToEWeek(DayOfWeek dayOfWeek)
    {
        EWeek week = EWeek.Monday;

        switch (dayOfWeek)
        {
            case DayOfWeek.Friday:
                week = EWeek.Friday;
                break;
            case DayOfWeek.Monday:
                week = EWeek.Monday;
                break;
            case DayOfWeek.Saturday:
                week = EWeek.Saturday;
                break;
            case DayOfWeek.Sunday:
                week = EWeek.Sunday;
                break;
            case DayOfWeek.Thursday:
                week = EWeek.Thursday;
                break;
            case DayOfWeek.Tuesday:
                week = EWeek.Tuesday;
                break;
            case DayOfWeek.Wednesday:
                week = EWeek.Wednesday;
                break;
        }

        return week;
    }


    protected static void Add(ITimerItem titem)
    {
        //按时间倒序
        bool isInsert = false;
        for (int i = 0; i < m_timerItemList.Count; i++)
        {
            if (titem.NextTime <= m_timerItemList[i].NextTime)
            {
                m_timerItemList.Insert(i, titem);
                isInsert = true;
                break;
            }
        }

        if (!isInsert)
            m_timerItemList.Add(titem);
    }

    protected static List<ITimerItem> m_removeList    = new List<ITimerItem>();
    protected static List<ITimerItem> m_timerItemList = new List<ITimerItem>();
}
}