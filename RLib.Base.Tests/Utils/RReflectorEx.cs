///********************************************************************
//    created:	2020/7/29 0:29:17
//    author:	rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DataModel;
//using Google.Protobuf.Collections;
//using NUnit.Framework;

//namespace RLib.Base.Tests
//{

//    public class RReflectorEx
//    {
//        public static List<double> dl = new List<double>() {5.1, 1.2, 3, 8, 9.5};

//        [Test]
//        public void Test_MinMax()
//        {
//            {
//                var td = dl.GetType().GetGenericTypeDefinition();
//                var tl = typeof(List<>);

//                Assert.AreEqual(td, tl);
//            }



//            {
//                FutureContractDM dm = new FutureContractDM() { Code = "a" };
//                dm.SettlementMonths.AddRange(new[] { 1, 2, 3 });

//                var td = dm.SettlementMonths.GetType().GetGenericTypeDefinition();
//                var tl = typeof(RepeatedField<>);

//                Assert.AreEqual(td, tl);
//                var b = dm.SettlementMonths.IsGeneric(typeof(RepeatedField<>));

//            }



//        }
//    }
//}
