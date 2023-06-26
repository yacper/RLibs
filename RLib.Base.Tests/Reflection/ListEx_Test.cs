// created: 2023/06/26 15:51
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using NUnit.Framework;

namespace RLib.Base.Tests.Reflection;

public class ListEx_Test
{
    [Test]
    public void InsertSorted_Test()
    {
        List<int> l = new();
        l.InsertSorted(3);
        l.InsertSorted(2);
        l.InsertSorted(4);
        l.InsertSorted(1);

        l.Should().BeInAscendingOrder();

    }

    [Test]
    public void InsertSorted_Test_Descending()
    {
        List<int> l        = new();
        var       comparer = Comparer<int>.Create((x, y) =>y.CompareTo(x));
        l.InsertSorted(3, comparer);
        l.InsertSorted(2, comparer);
        l.InsertSorted(4, comparer);
        l.InsertSorted(1, comparer);

        l.Should().BeInDescendingOrder();
    }

    public class CompareC
    {
        public double Price { get; set; }
    }

    [Test]
    public void InsertSorted_Test_Class()
    {
        List<CompareC> l        = new();
        var       comparer = Comparer<CompareC>.Create((x, y) =>x.Price.CompareTo(y.Price));
        l.InsertSorted(new CompareC(){Price = 3}, comparer);
        l.InsertSorted(new CompareC(){Price = 2}, comparer);
        l.InsertSorted(new CompareC(){Price = 1}, comparer);
        l.InsertSorted(new CompareC(){Price = 4}, comparer);

        l[0].Price.Should().Be(1);
        l[1].Price.Should().Be(2);
        l[2].Price.Should().Be(3);
        l[3].Price.Should().Be(4);
    }

   

   
}