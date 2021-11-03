/********************************************************************
    created:	2020-07-23 14:50:05
    author:		joshua
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using NUnit.Framework;

namespace RLib.Base.Tester.Utils
{
	public class TestEnumerableEx
	{
        public class AA
        {
			public int O { get; set; }

            public AA(int o)
            {
                O = o;
            }
        }

        public class BB:AA
        {
            public BB(int o)
			:base(o)
            {
            }
        }


		public static List<int> LLEmpty = new List<int> { };
		public static List<int> LLOrdered = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		public static List<double> LLOrderedDouble = new List<double> { 0.0d, 1.0d, 2.0d, 3.0d, 4.0d, 5.0d, 6.0d, 7.0d, 8.0d, 9.0d };
		public static List<int> LLDisOrdered = new List<int>
		{
				8, 3, 1, 4,9, 0, 5, 6, 2, 7
		};

		public static List<AA> LAEmpty = new List<AA> { };
		public static List<AA> LAOrdered = new List<AA> { new AA(0), new AA(1), new AA(2), new AA(3), new AA(4)};
		public static List<BB> LBOrdered = new List<BB> { new BB(0), new BB(1), new BB(2), new BB(3), new BB(4)};

        [Test]
		public void Test_Last()
		{
            try
            {
			    Assert.AreEqual(LLEmpty.Last(), 0);
            }
            catch (Exception e)
            {// no element
            }
			Assert.AreEqual(LLEmpty.LastOrDefault(0, 1), 1);

			Assert.AreEqual(LLOrdered.Last(), 9);
			Assert.AreEqual(LLOrdered.Last(1), 8);

            try
            {
			    Assert.AreEqual(LLOrdered.Last(11), 7);
            }
            catch (Exception e)
            {
            }
		}
        [Test]
		public void Test_Last_Obj()
		{
            try
            {
			    Assert.AreEqual(LAEmpty.Last(), 0);
            }
            catch (Exception e)
            {// no element
            }

            AA a = new AA(1);
			Assert.AreEqual(LAEmpty.LastOrDefault(0, a), a);

			Assert.AreEqual(LLOrdered.Last(), 9);
			Assert.AreEqual(LLOrdered.Last(1), 8);

            try
            {
			    Assert.AreEqual(LLOrdered.Last(11), 7);
            }
            catch (Exception e)
            {
            }
		}


		[Test]
		public void Test_MaxOrDefault()
		{
			Assert.AreEqual(LLEmpty.MaxOrDefault(), 0);
			Assert.AreEqual(LLEmpty.MaxOrDefault(1), 1);
			Assert.AreEqual(LLOrdered.MaxOrDefault(), 9);
			Assert.AreEqual(LLOrdered.MaxOrDefault(2), 9);
			Assert.AreEqual(LLOrderedDouble.MaxOrDefault(), 9.0d);
		}
		[Test]
		public void Test_MinOrDefault()
		{
			Assert.AreEqual(LLEmpty.MinOrDefault(), 0);
			Assert.AreEqual(LLEmpty.MinOrDefault(1), 1);
			Assert.AreEqual(LLOrdered.MinOrDefault(), 0);
			Assert.AreEqual(LLOrdered.MinOrDefault(2), 0);
			Assert.AreEqual(LLOrderedDouble.MinOrDefault(), 0.0d);
		}
	
	}

}
