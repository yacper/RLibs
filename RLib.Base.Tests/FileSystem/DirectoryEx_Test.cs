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
    public class DirectoryEx_Test
    {
		[Test]
	    public void Test()
        {
            Directory.CreateDirectory("tt");
            File.WriteAllText("tt/1.txt", "hello");
            Directory.CreateDirectory("tt/tts");
            File.WriteAllText("tt/tts/2.txt", "world");

            DirectoryInfo tt = new DirectoryInfo("tt");
            tt.CopyTo("tt2/tt");

            Directory.Exists("tt2/tt").Should().Be(true);
            Directory.Exists("tt2/tt/tts").Should().Be(true);
            File.Exists("tt2/tt/1.txt").Should().Be(true);
            File.Exists("tt2/tt/tts/2.txt").Should().Be(true);

            Directory.Delete("tt", true);
            Directory.Delete("tt2", true);

        }

    }
}
