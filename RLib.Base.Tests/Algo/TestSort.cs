using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace RLib.Base.Tester
{
    public class TestSort
    {
        public static List<int> LLOrdered = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
        public static List<int> LLDisOrdered = new List<int>
        {
                8, 3, 1, 4,9, 0, 5, 6, 2, 7
        };

        [Test]
        public void Test_BubbleSort()
        {
            List<int> l = new List<int>(LLDisOrdered);

            l.BubbleSort();

            Assert.AreEqual(l.SequenceEqual(LLOrdered), true);
        }
        [Test]
        public void Test_QSort()
        {
            List<int> l = new List<int>(LLDisOrdered);

            l.QSort();

            Assert.AreEqual(l.SequenceEqual(LLOrdered), true);
        }
    }
}
