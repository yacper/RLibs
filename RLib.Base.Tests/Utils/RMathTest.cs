/********************************************************************
    created:	2020/7/29 0:29:17
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RLib.Base.Tester.Utils
{

    public class ProtobufTest
    {
        public static List<double> dl = new List<double>() {5.1, 1.2, 3, 8, 9.5};

        [Test]
        public void Test_MinMax()
        {
            Assert.AreEqual(dl.Max(2), 9.5);
            Assert.AreEqual(dl.Max(3, 4), 9.5);
            
            Assert.AreEqual(dl.Max(5), 9.5);
            Assert.AreEqual(dl.Max(0, 4), 9.5);

            Assert.AreEqual(dl.Min(2), 8);
            Assert.AreEqual(dl.Min(3, 4), 8);
            
            Assert.AreEqual(dl.Min(5), 1.2);
            Assert.AreEqual(dl.Min(0, 4), 1.2);

        }
    }
}
