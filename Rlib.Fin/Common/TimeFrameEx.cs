/********************************************************************
    created:	2021/11/2 20:25:07
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace RLib.Fin
{
    public static class TimeFrameEx
    {
        public static TimeSpan ToTimeSpan(this ETimeFrame tf)
        {
            switch (tf)
            {
                case ETimeFrame.T1:
                return new TimeSpan(0, 0, 0, 1);
                case ETimeFrame.M1:
                return new TimeSpan(0, 0, 1, 0);
                case ETimeFrame.M3:
                return new TimeSpan(0, 0, 3, 0);
                case ETimeFrame.M5:
                return new TimeSpan(0, 0, 5, 0);
                case ETimeFrame.M15:
                return new TimeSpan(0, 0, 15, 0);
                case ETimeFrame.M30:
                return new TimeSpan(0, 0, 30, 0);
                case ETimeFrame.H1:
                return new TimeSpan(0, 1, 0, 0);
                case ETimeFrame.H4:
                return new TimeSpan(0, 4, 0, 0);
                case ETimeFrame.Day:
                return new TimeSpan(1, 0, 0, 0);
                case ETimeFrame.Week:
                return new TimeSpan(7, 0, 0, 0);
            }

            throw new NotImplementedException();
        }

        public static DateTime AddTimeFrames(this DateTime dt, ETimeFrame tf, double n = 1)  // 添加时间框架
        {
            switch (tf)
            {
                //case ETimeFrame.T1:
                //return dt.AddSeconds(1 * n);
                //break;
                case ETimeFrame.M1:
                return dt.AddMinutes(1 * n);
                break;
                case ETimeFrame.M3:
                return dt.AddMinutes(3 * n);
                break;
                case ETimeFrame.M5:
                return dt.AddMinutes(5 * n);
                break;
                case ETimeFrame.M15:
                return dt.AddMinutes(15 * n);
                break;
                case ETimeFrame.M30:
                return dt.AddMinutes(30 * n);
                break;
                case ETimeFrame.H1:
                return dt.AddHours(1 * n);
                break;
                case ETimeFrame.H4:
                return dt.AddHours(4 * n);
                break;
                case ETimeFrame.Day:
                return dt.AddDays(1 * n);
                break;
                case ETimeFrame.Week:
                return dt.AddDays(7 * n);           // todo: 这里有问题
                break;
                case ETimeFrame.Month:
                return dt.AddMonths(1 * (int)n);
                break;
                case ETimeFrame.Year:
                return dt.AddYears(1 * (int)n);
                break;
            }
            throw new NotImplementedException();
        }

        //todo: 这块后面需要引入TradingSession等来完美实现
        // 根据tf取模，如果tf 为m5 9:06在leftAsOpen就是9:05
        // 如果tf为H1, 在leftAsOpen下为9:00
        // 重点：这里假设报价不到毫秒级，只到秒级（ts2是同一秒可以有多个报价变化，而中国股票市场是累积一秒内所有的成交作为1秒的报价信息）
        public static DateTime ModTimeFrame(this DateTime dt, ETimeFrame tf, bool leftAsOpen = true)
        {
            switch (tf)
            {
                case ETimeFrame.T1:
                return dt;
                break;
                case ETimeFrame.M1:
                if (dt.Second == 0)
                    return dt;

                if (leftAsOpen)
                    return dt.AddSeconds(-dt.Second);
                else
                    return dt.AddSeconds(-dt.Second).AddMinutes(1);
                break;
                case ETimeFrame.M3:
                if (dt.Second == 0)
                    return dt;

                if (leftAsOpen)
                    return dt.AddSeconds(-dt.Second);
                else
                    return dt.AddSeconds(-dt.Second).AddMinutes(3);
                break;
                case ETimeFrame.M5:
                {
                    if (dt.Second == 0 && dt.Minute % 5 == 0)
                        return dt;
                    DateTime ret = dt.ModTimeFrame(ETimeFrame.M1, leftAsOpen);  // 先取模到m1
                    int m = ret.Minute % 5;
                    if (leftAsOpen)
                        return ret.AddMinutes(-m);
                    else
                        return ret.AddMinutes(-m + 5);
                }
                break;
                case ETimeFrame.M15:
                {
                    if (dt.Second == 0 && dt.Minute % 15 == 0)
                        return dt;

                    DateTime ret = dt.ModTimeFrame(ETimeFrame.M1, leftAsOpen);  // 先取模到m1
                    int m = ret.Minute % 15;
                    if (leftAsOpen)
                        return ret.AddMinutes(-m);
                    else
                        return ret.AddMinutes(-m + 15);

                }
                case ETimeFrame.M30:
                {
                    if (dt.Second == 0 && dt.Minute % 30 == 0)
                        return dt;

                    DateTime ret = dt.ModTimeFrame(ETimeFrame.M1, leftAsOpen);  // 先取模到m1
                    int m = ret.Minute % 30;
                    if (leftAsOpen)
                        return ret.AddMinutes(-m);
                    else
                        return ret.AddMinutes(-m + 30);

                }
                case ETimeFrame.H1:
                {
                    if (dt.Second == 0 && dt.Minute == 0)
                        return dt;

                    DateTime ret = dt.ModTimeFrame(ETimeFrame.M1, leftAsOpen);  // 先取模到m1
                    int m = ret.Minute;
                    if (leftAsOpen)
                        return ret.AddMinutes(-m);
                    else
                        return ret.AddMinutes(-m + 60);

                }
                case ETimeFrame.H4:
                {
                    if (dt.Second == 0 && dt.Minute == 0 && dt.Hour % 4 == 0)
                        return dt;

                    DateTime ret = dt.ModTimeFrame(ETimeFrame.H1, leftAsOpen);  // 先取模到m1
                    int h = ret.Hour % 4;
                    if (leftAsOpen)
                        return ret.AddHours(-h);
                    else
                        return ret.AddHours(-h + 4);

                }
                case ETimeFrame.Day:
                {
                    // todo: 日线这个比较复杂，要根据不同市场的实际Session时间去换算
                    if (dt.Second == 0 && dt.Minute == 0 && dt.Hour == 0)
                        return dt;

                    return new DateTime(dt.Year, dt.Month, dt.Day);
                }
                case ETimeFrame.Week:
                {
                    // todo: 周线这个比较复杂，要根据不同市场的实际
                    // 周K的计算比较复杂leftAsOpen基本是以周六或者周一为一周的开始进行计算， rightAsOpen以周五进行计算
                    // 所以需要Feeder实际的以哪一天作为一星期的基准开始作为参照，如果周一作为基准，那么该州

                    return new DateTime(dt.Year, dt.Month, dt.Day);
                }
                case ETimeFrame.Month:
                {
                    // todo: 周线这个比较复杂，要根据不同市场的实际
                    // 周K的计算比较复杂leftAsOpen基本是以周六或者周一为一周的开始进行计算， rightAsOpen以周五进行计算
                    // 所以需要Feeder实际的以哪一天作为一星期的基准开始作为参照，如果周一作为基准，那么该州

                    return new DateTime(dt.Year, dt.Month, 1);
                }
                case ETimeFrame.Year:
                {
                    // todo: 

                    return new DateTime(dt.Year, 1, 1);
                }

            }

            throw new NotImplementedException();
        }

        public static bool Equal(this DateTime dt, DateTime other, ETimeFrame tf, bool leftAsOpen = true)
        {
            return dt.ModTimeFrame(tf, leftAsOpen) == other.ModTimeFrame(tf, leftAsOpen);
        }


        public static List<DateTime> Split(this DateTime dt, ETimeFrame tf, int n, bool leftAsOpen = true) // 讲一个timeframe时间段，分割成多少个时间点
        // 秒级
        {
            // 最少2个时间点
            Debug.Assert(n >= 2);
            Debug.Assert(tf <= ETimeFrame.Day);

            List<DateTime> ret = new List<DateTime>(n);
            n -= 1;

            TimeSpan ts = tf.ToTimeSpan();
            ts = new TimeSpan(0, 0, (int)(ts.TotalSeconds / n));

            if (leftAsOpen)
            {
                for (int i = 0; i != n; ++i)
                {
                    int span = (int)ts.TotalSeconds * i;

                    ret.Add(dt.AddSeconds(span));
                }

                // 最后一个点,准确
                ret.Add(dt.AddTimeFrames(tf, 1) - new TimeSpan(0, 0, 1));     // 最后一个点减1s
            }
            else
            {
                // 第一个点,准确
                ret.Add(dt.AddTimeFrames(tf, -1) + new TimeSpan(0, 0, 1));     // 第一个点+1s

                for (int i = n - 1; i != -1; --i)
                {
                    int span = (int)ts.TotalSeconds * -i;

                    ret.Add(dt.AddSeconds(span));
                }
            }

            return ret;
        }


        public static long FrameCount(this TimeSpan ts, ETimeFrame tf)
        {
            return (long)Math.Ceiling((double)(ts.Ticks / tf.ToTimeSpan().Ticks));
        }


        public static string ToString(this DateTime dt, ETimeFrame tf, bool shortFormat = true)
        {
            switch (tf)
            {
                case ETimeFrame.T1:
                case ETimeFrame.M1:

                case ETimeFrame.M5:

                case ETimeFrame.M15:

                case ETimeFrame.M30:

                case ETimeFrame.H1:

                case ETimeFrame.H4:
                if (shortFormat)
                    return dt.ToString("HH:mm");
                else
                    return dt.ToString("yyyy/MM/dd HH:mm");

                break;
                case ETimeFrame.Day:
                case ETimeFrame.Week:
                if (shortFormat)
                    return dt.ToString("MM/dd");
                else
                    return dt.ToString("yyyy/MM/dd");
                case ETimeFrame.Month:
                return dt.ToString("yyyy/MM");
                break;
                case ETimeFrame.Year:
                return dt.Year.ToString();
                break;
            }

            return "";

        }



    }
}
