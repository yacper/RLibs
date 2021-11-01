/********************************************************************
    created:	2018/10/9 13:42:15
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public enum EQuater
    {
        Equater_None = 0,
        Spring      = 1,
        Summer      = 2,
        Autum       = 4,
        Winter      = 8
    }

	public static class DateTimeEx
	{
	    public static DateTime ToDateTime(this string dtStr)
	    {
	        return DateTime.Parse(dtStr);
	    }
	    public static TimeSpan ToTimeSpan(this string str)
	    {
	        return TimeSpan.Parse(str);
	    }

	    public static string ToFileLongFormat(this DateTime val)            // 提供一个用于存储的长时间格式 2008-9-4(12-19-14-333)
	    {
	        return $"{val.Year}-{val.Month}-{val.Day}({val.Hour}-{val.Minute}-{val.Second}-{val.Millisecond})";
	    }



	    public static EQuater Quater(this DateTime val)
	    {
	        int month = val.Month;

            if (month < 1 || month > 12)
            {
                string message = string.Format("Input parameter month ({0}) out of range[1 -- 12].", month);
                throw new ArgumentOutOfRangeException("month", message);
            }

            int q = ((month - 1) / 3 + 1);
	        switch (q)
	        {
                case 1:
                    return EQuater.Spring;
                case 2:
                    return EQuater.Summer;
                case 3:
                    return EQuater.Autum;
                case 4:
                    return EQuater.Winter;
	        }
            return EQuater.Equater_None;
        }

        public static int QuaterNum(this DateTime val)
	    {
	        int month = val.Month;

            if (month < 1 || month > 12)
            {
                string message = string.Format("Input parameter month ({0}) out of range[1 -- 12].", month);
                throw new ArgumentOutOfRangeException("month", message);
            }

            return ((month - 1) / 3 + 1);
        }

      


        public static DateTime FromUnixTimeStamp(this double ts, bool isMilliSecond = true)  // 从Unix时间戳 毫秒
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            if(isMilliSecond)
                return startTime.AddMilliseconds(ts);
            else
                return startTime.AddSeconds(ts);
        }

        public static double ToUnixTimeStamp(this DateTime dt, bool isMilliSecond = true)  // 从Unix时间戳 毫秒
        {
            long unixDate = (dt.ToUniversalTime().Ticks - 621355968000000000);
            if (isMilliSecond)
                return unixDate / 10000;
            else
                return unixDate / 10000000;
        }

    }
}
