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

	    [Test]
	    public void Drawdown_Test()
        {
            //a.
            var equities1 = new List<double>() { 1800, 1805, 1808};
            MathEx.MaxDrawdownAmount(equities1).DrawdownAmount.Should().Be(0);
            List<MathEx.DrawDown> dd1 = new();
            MathEx.GetDrawdowns(ref dd1, equities1);
            dd1.MaxDrawdownAmount().DrawdownAmount.Should().Be(0);
            dd1.MaxDrawdownRate().DrawdownAmount.Should().Be(0);

            var equities2 = new List<double>() { 1800, 1805, 1808, 1800};
            MathEx.MaxDrawdownAmount(equities2).DrawdownAmount.Should().Be(8);
            List<MathEx.DrawDown> dd2 = new();
            MathEx.GetDrawdowns(ref dd2, equities2);
            dd2.MaxDrawdownAmount().DrawdownAmount.Should().Be(8);
            dd2.MaxDrawdownRate().DrawdownAmount.Should().Be(8);

            var equities3 = new List<double>() { 1800, 1805, 1808, 1800, 1795, 1805, 1809, 1800, };
            MathEx.MaxDrawdownAmount(equities3).DrawdownAmount.Should().Be(13);
            List<MathEx.DrawDown> dd3 = new();
            MathEx.GetDrawdowns(ref dd3, equities3);
            dd3.MaxDrawdownAmount().DrawdownAmount.Should().Be(13);


            var equities4 = new List<double>() { 1800, 1805, 1808, 1800, 1795, 1805, 1809, 1800, 1810 };
            MathEx.MaxDrawdownAmount(equities4).DrawdownAmount.Should().Be(13);
            List<MathEx.DrawDown> dd4 = new();
            MathEx.GetDrawdowns(ref dd4, equities4);
            dd4.MaxDrawdownAmount().DrawdownAmount.Should().Be(13);

            var equities5 = new List<double>() { 1800, 1805, 1808, 1800, 1795, 1805, 1809, 1800, 1810, 1796.99 };
            MathEx.MaxDrawdownAmount(equities5).DrawdownAmount.Should().BeApproximately(13.01, 0.001);
            MathEx.MaxDrawdownRate(equities5).DrawdownAmount.Should().Be(13);
            List<MathEx.DrawDown> dd5 = new();
            MathEx.GetDrawdowns(ref dd5, equities5);
            dd5.MaxDrawdownAmount().DrawdownAmount.Should().BeApproximately(13.01, 0.001);
            dd5.MaxDrawdownRate().DrawdownAmount.Should().Be(13);
            equities5.AddRange(new List<double>(){1790, 1780});
            MathEx.GetDrawdowns(ref dd5, equities5, 10);
            dd5.MaxDrawdownAmount().DrawdownAmount.Should().Be(30);
            dd5.MaxDrawdownRate().DrawdownAmount.Should().Be(30);

            //var equities6 = new List<double>() { 1800, 1805, 1808, 1800, 1795, 1805, 1809, 1800, 1810, 1800, 1805,1795,1800, 1780 };
            //MathEx.MaxDrawdownAmount(equities4).DrawdownAmount.Should().Be(30);
            //List<MathEx.DrawDown> dd6 = new();
            //MathEx.GetDrawdowns(ref dd6, equities6);
            //dd4.MaxDrawdownAmount().DrawdownAmount.Should().Be(30);


            var equities6 = new List<double>() { 1800, 1806, 1808, 1800, 1796, 1806, 1809, 1800, 1810 };
            MathEx.MaxDrawdownAmount(equities6).DrawdownAmount.Should().Be(12);
            List<MathEx.DrawDown> dd6 = new();
            MathEx.GetDrawdowns(ref dd6, equities6);
            dd6.MaxDrawdownAmount().DrawdownAmount.Should().Be(12);
            equities6.Add(1800);
            MathEx.GetDrawdowns(ref dd6, equities6, equities6.Count-1);
            dd6.MaxDrawdownAmount().DrawdownAmount.Should().Be(12);
            equities6.Add(1805);
            MathEx.GetDrawdowns(ref dd6, equities6, equities6.Count-1);
            dd6.MaxDrawdownAmount().DrawdownAmount.Should().Be(12);
             equities6.Add(1795);
            MathEx.GetDrawdowns(ref dd6, equities6, equities6.Count-1);
            dd6.MaxDrawdownAmount().DrawdownAmount.Should().Be(15);
             equities6.Add(1800);
            MathEx.GetDrawdowns(ref dd6, equities6, equities6.Count-1);
            dd6.MaxDrawdownAmount().DrawdownAmount.Should().Be(15);
             equities6.Add(1780);
            MathEx.GetDrawdowns(ref dd6, equities6, equities6.Count-1);
            dd6.MaxDrawdownAmount().DrawdownAmount.Should().Be(30);
 
 
            //equities6.AddRange(new List<double>(){1800, 1805,1795,1800, 1780 });
            //MathEx.GetDrawdowns(ref dd6, equities6, dd6.LastOrDefault().TroughIndex+1);
            //dd6.MaxDrawdownAmount().DrawdownAmount.Should().Be(30);
            //dd6.MaxDrawdownRate().DrawdownAmount.Should().Be(30);



        }

    }
}
