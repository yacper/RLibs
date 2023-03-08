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
    public class MathEx_Test
    {
		[Test]
	    public void CartesianProduct_Test()
        {
            List<string> a = new() { "a", "b", "c" };
            List<string> op = new() { "+", "-", "*" };
            List<string> b = new() { "x", "y", "z" };

            //a.
            var combins = new[] { a, op, b }.CartesianProduct();

        }

		[Test]
	    public void CartesianProduct_Test2()
        {
            List<string> a = new() { "a", "b", "c" };
            List<string> op = new() { "+"};
            List<string> b = new() { "x", "y", "z" };

            //a.
            var combins = new[] { a, op, b }.CartesianProduct();

        }

		[Test]
	    public void CartesianProduct_Test3()
        {
            //a.
            var combins = new[] { new[]{"a", "b","c"}, new []{"x", "y", "z"} , new[]{"+"} }.CartesianProduct();

        }


    }
}
