// created: 2023/05/04 15:59
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using FluentAssertions;

namespace RLib.Base
{


public static class TimespanEx
{
    // 作为24小时制， [00:00 ~ 23:59:59)
    public static bool IsTimeofday(this TimeSpan ts)
    {
        var ticks = ts.Ticks;
        return ticks >= 0 && ticks <= TimeSpan.TicksPerDay;
    }


    // 作为24小时制， [00:00 ~ 23:59:59)
    // 可以跨一天 比如20:00 ~ 02:00 
    public static bool IsWithin(this TimeSpan ts, TimeSpan start, TimeSpan end)
    {
        ts.IsTimeofday().Should().Be(true);
        start.IsTimeofday().Should().Be(true);
        end.IsTimeofday().Should().Be(true);

        if (end >= start)
        {
            return ts >= start && ts <= end;
        }
        else
        {
            return ts >= start || ts <= end;
        }
    }

    
}
}