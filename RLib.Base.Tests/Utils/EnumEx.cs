/********************************************************************
    created:	2021/9/7 14:48:42
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RLib.Base.Tests
{
    public enum EE
    {
        a = 1,
        b = 2,
        c = 4,
        d = 8,
        e = 16,
        f = 64,
        ab = 3,
        cd = 12,
        all = 31
    }

    public class EnumExTest
    {
		[Test]
	    public void Test()
        {
            Assert.AreEqual(EE.b, EE.a.Next());
            Assert.AreEqual(EE.b, EE.a.NextPot());

            Assert.AreEqual((EE)0, EE.a.Previous());
            Assert.AreEqual((EE)0, EE.a.PreviousPot());

            Assert.AreEqual(EE.ab, EE.b.Next());
            Assert.AreEqual(EE.c, EE.b.NextPot());

            Assert.AreEqual(EE.a, EE.b.Previous());
            Assert.AreEqual(EE.a, EE.b.PreviousPot());

            Assert.AreEqual(EE.f, EE.e.NextPot());

            Assert.AreEqual(EE.all, EE.e.Next());
            try
            {
                var v = EE.e.NextPot();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

    }
}
