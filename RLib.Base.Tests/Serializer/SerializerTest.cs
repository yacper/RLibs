///********************************************************************
//    created:	2020-07-23 14:50:05
//    author:		joshua
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Dynamic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using DataModel;
//using Google.Protobuf.WellKnownTypes;
//using Microsoft.VisualBasic;
//using NUnit.Framework;

//namespace RLib.Base.Tests
//{
//    public class Bar
//    {
//        public DateTime            Time { get; set; }
//        public double              Open { get; set; }
//        public double              High { get; set; }
//        public double              Low { get; set; }
//        public double              Close { get; set; }

//        public double              Volume { get; set; }
//    }

//    public class Symbol
//    {
//        public string Code { get; set; }
//        public Bar Bar { get; set; }
//    }

//    public class BarHeader
//    {
//        public DateTime            Time { get; set; }
//        public double              Open { get; set; }
//    }


//	public class SerializerTest
//	{
//		public static List<Bar> LBars = new List<Bar>
//        {
//            new Bar(){Time = DateTime.Parse("2021/06/01"), Open = 1800, High = 1815, Low = 1755, Close = 1810, Volume = 100},
//            new Bar(){Time = DateTime.Parse("2021/06/02"), Open = 1820, High = 1835, Low = 1775, Close = 1830, Volume = 200},
//            new Bar(){Time = DateTime.Parse("2021/06/03"), Open = 1830, High = 1855, Low = 1795, Close = 1840, Volume = 300},
//        };

//        public static List<dynamic> LDynamics = new List<dynamic>();        // 动态对象


//        public static List<object> LAnonymousObject = new List<object>      // 匿名对象
//    {
//            new { Time = DateTime.Parse("2021/06/01"), Open = 1800, High = 1815, Low = 1755, Close = 1810, Volume = 100},
//            new {Time = DateTime.Parse("2021/06/02"), Open = 1820, High = 1835, Low = 1775, Close = 1830, Volume = 200},
//            new {Time = DateTime.Parse("2021/06/03"), Open = 1830, High = 1855, Low = 1795, Close = 1840, Volume = 300},
//    };

//        public static List<object> LAnonymousObject2 = new List<object>      // 匿名对象
//    {
//            new { Time = DateTime.Parse("2021/06/04"), Open = 1800, High = 1815, Low = 1755, Close = 1810, Volume = 100},
//            new {Time = DateTime.Parse("2021/06/05"), Open = 1820, High = 1835, Low = 1775, Close = 1830, Volume = 200},
//    };



//        [SetUp]
//        public void Setup()
//        {
//            dynamic record = new ExpandoObject();
//            record.Time = DateTime.Parse("2021/06/01");
//            record.Open = 1800;
//            record.High = 1815;
//            record.Low = 1755;
//            record.Close = 1810;
//            record.Volume = 100;
//            LDynamics.Add(record);

//            record = new ExpandoObject();
//            record.Time = DateTime.Parse("2021/06/02");
//            record.Open = 1820;
//            record.High = 1835;
//            record.Low = 1775;
//            record.Close = 1830;
//            record.Volume = 200;
//            LDynamics.Add(record);

//            record = new ExpandoObject();
//            record.Time = DateTime.Parse("2021/06/03");
//            record.Open = 1830;
//            record.High = 1855;
//            record.Low = 1795;
//            record.Close = 1840;
//            record.Volume = 300;
//            LDynamics.Add(record);
//        }

//        [Test]
//		public void Test_Excel_write()
//        {

//            {
//                FutureContractDM dm = new FutureContractDM() { Code = "a" };
//                dm.SettlementMonths.AddRange(new[] { 1, 2, 3 });

//                dm.ToEnumerable().ToExcel("d:/1.xls");
//                var dms = "d:/1.xls".FromExcel<FutureContractDM>();
//            }


//            string n = typeof(NumRange).FullName;
//            n = typeof(string).FullName;
//            n = typeof(Range).FullName;
//            //n = typeof(Color).FullName;
//            n = typeof(FontDM).FullName;
//            n = typeof(TimeRange).FullName;

//            ExcelSerializer se = new ExcelSerializer();

//            se.Serialize(LBars, "bars.xls", true);
//            se.Serialize(LBars, "bars2.xls", false);
//            se.Serialize(LBars, "bars3.xls",  "bars");

//            (new List<Symbol>() {new Symbol() {Code = "aapl", Bar = LBars[0]}}).ToExcel("symbol.xls");
//        }
		
//        [Test]
//		public void Test_Excel_read()
//        {
//            ExcelSerializer se = new ExcelSerializer();
//            //List<Bar> lb = se.Deserialize<Bar>("bars.xls");
//            List<Bar> lb2 = se.Deserialize<Bar>("bars2.xls", null, false, 0, 0);
//            List<Bar> lb3 = se.Deserialize<Bar>("bars3.xls", "bars", true, 0, 1);

//            List<Symbol> ls = "symbol.xls".FromExcel<Symbol>();
//        }
	
//        [Test]
//		public void Test_Csv_write()
//        {
//            LBars.ToCsv("bars.csv");
//            LBars.ToCsv(new BarHeader(){Time = DateTime.Now, Open = 1}, "bars2.csv");
//            LAnonymousObject2.AppendCsv("bars2.csv");

//            LBars.ToCsv("bars4.csv", false);

//            LDynamics.ToCsv("bars3.csv");

//            LAnonymousObject.ToCsv(new BarHeader(){Time = DateTime.Now, Open = 1}, "bars6.csv");
//            LAnonymousObject2.AppendCsv("bars6.csv");
//        }
		
//        [Test]
//		public void Test_Csv_read()
//        {
//            var lb = "bars.csv".FromCsv<Bar>().ToList();
//            var lbb = "bars.csv".FromCsv().ToList();

//            var lb4 = "bars4.csv".FromCsv<Bar>(false).ToList();
//            var lbb4 = "bars4.csv".FromCsv(false).ToList();

//            BarHeader header = null;
//            IEnumerable<Bar> lb2 = "bars2.csv".FromCsv<BarHeader, Bar>(out header);

//        }
//	}

//}
