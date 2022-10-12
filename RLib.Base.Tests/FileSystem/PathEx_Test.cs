/********************************************************************
    created:	2021/9/7 14:48:42
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace RLib.Base.Tests
{
    public class PathEx_Test
    {
		[Test]
	    public void Test()
        {
            string[] paths    = { @"d:\archives", "2001", "media", "images" };
            string   fullPath = Path.Combine(paths);
            paths.CombinePath().Should().Be(@"d:\archives\2001\media\images");

            @"d:\archives".CombinePath("2001").Should().Be(@"d:\archives\2001");

        }

    }
}
