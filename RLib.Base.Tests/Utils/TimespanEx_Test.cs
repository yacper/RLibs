// created: 2023/05/04 17:58
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using FluentAssertions;
using NUnit.Framework;
using RLib.Base.Utils;

namespace RLib.Base.Tests.Utils;

public class TimespanEx_Test
{
    [Test]
    public void Test()
    {
        TimeSpan.Parse("00:00:00").IsTimeofday().Should().Be(true);
        TimeSpan.Parse("20:00:00").IsTimeofday().Should().Be(true);
        TimeSpan.Parse("24:00:00").IsTimeofday().Should().Be(false);

        TimeSpan.Parse("25:0:0").IsTimeofday().Should().Be(false);


        TimeSpan.Parse("23:0:0").IsWithin(TimeSpan.Parse("20:0:0"), TimeSpan.Parse("23:55:00")).Should().Be(true);
        TimeSpan.Parse("23:0:0").IsWithin(TimeSpan.Parse("20:0:0"), TimeSpan.Parse("0:00:00")).Should().Be(true);
        TimeSpan.Parse("23:0:0").IsWithin(TimeSpan.Parse("20:0:0"), TimeSpan.Parse("2:00:00")).Should().Be(true);
        TimeSpan.Parse("19:0:0").IsWithin(TimeSpan.Parse("20:0:0"), TimeSpan.Parse("2:00:00")).Should().Be(false);

    }

}