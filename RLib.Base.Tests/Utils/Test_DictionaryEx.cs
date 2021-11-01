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
using Force.DeepCloner;
using NUnit.Framework;

namespace RLib.Base.Tester.Utils
{

    public class Test_DictionaryEx
    {
        public static Dictionary<int, int> Dic = new Dictionary<int, int>() {{1, 1}, {2, 2}};

        [Test]
        public void Test_Dictionary()
        {
            Dictionary<int, int> dic = Dic.ShallowClone();

            try
            {
                dic.Add(1, 1);
            }
            catch (Exception e)
            {
            }

            Assert.AreEqual(dic.AddIfNotContain(1, 3).Count, 2);
            Assert.AreEqual(dic.AddIfNotContain(4, 5).Count, 2);
            


        }
    }
}
