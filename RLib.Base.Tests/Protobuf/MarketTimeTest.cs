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
using DataModel;
using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;

namespace RLib.Base.Tests
{
    public class MarketTimeEx
    {
        public static List<double> dl = new List<double>() {5.1, 1.2, 3, 8, 9.5};

        [Test]
        public void Test_MinMax()
        {
            {
                MarketTimeDm DM = new MarketTimeDm()
                {
                    Id = "1",
                    Name = "中国股市",
                    TimeZone = 8,
                };
                DaySessionDm Monday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Monday,
                };
                Monday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "9:30",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "13:00",
                        EndTime = "15:00",
                    }
                });
                DM.DaySessions.Add(Monday);

                DaySessionDm Tuesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Tuesday,
                };
                Tuesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        EndDayOfWeek = EWeek.Tuesday,
                        StartTime = "9:30",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        EndDayOfWeek = EWeek.Tuesday,
                        StartTime = "13:00",
                        EndTime = "15:00",
                    }
                });
                DM.DaySessions.Add(Tuesday);

                DaySessionDm Wednesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Wednesday,
                };
                Wednesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "9:30",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "13:00",
                        EndTime = "15:00",
                    }
                });
                DM.DaySessions.Add(Wednesday);

                DaySessionDm Thursday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Thursday,
                };
                Thursday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "9:30",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "13:00",
                        EndTime = "15:00",
                    }
                });
                DM.DaySessions.Add(Thursday);

                DaySessionDm Friday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Friday,
                };
                Friday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "9:30",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "13:00",
                        EndTime = "15:00",
                    }
                });
                DM.DaySessions.Add(Friday);

                DM.DaySessions.AddRange(new []{Monday, Tuesday, Wednesday, Thursday, Friday});

                DM.ToEnumerable().ToExcel("d:/cnstock.xls");
                var dms = "d:/cnstock.xls".FromExcel<MarketTimeDm>();
            }

            {
                MarketTimeDm DM = new MarketTimeDm()
                {
                    Id = "1",
                    Name = "中国期市-日盘",
                    Desc = "上午9:00-11:30,下午13:30-15:00",
                    TimeZone = 8,
                };
                DaySessionDm Monday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Monday,
                };
                Monday.Sessions.AddRange(new []
                { 
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Monday);

                DaySessionDm Tuesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Tuesday,
                };
                Tuesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "9:00",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "13:30",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Tuesday);

                DaySessionDm Wednesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Wednesday,
                };
                Wednesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Wednesday);

                DaySessionDm Thursday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Thursday,
                };
                Thursday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Thursday);

                DaySessionDm Friday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Friday,
                };
                Friday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                   
                });
                DM.DaySessions.Add(Friday);

                DM.DaySessions.AddRange(new []{Monday, Tuesday, Wednesday, Thursday, Friday});

                DM.ToEnumerable().ToExcel("d:/中国期市-日盘.xls");
                var dms = "d:/中国期市-日盘.xls".FromExcel<MarketTimeDm>();
            }


            {
                MarketTimeDm DM = new MarketTimeDm()
                {
                    Id = "1",
                    Name = "中国期市-夜盘1",
                    Desc = "上午9:00-11:30,下午13:30-15:00,下午21:00-23:00(夜盘)",
                    TimeZone = 8,
                };
                DaySessionDm Monday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Monday,
                };
                Monday.Sessions.AddRange(new []
                { 
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Friday,
                        EndTime = "23:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                   
                });
                DM.DaySessions.Add(Monday);

                DaySessionDm Tuesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Tuesday,
                };
                Tuesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Monday,
                        EndTime = "23:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "9:00",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "13:30",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Tuesday);

                DaySessionDm Wednesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Wednesday,
                };
                Wednesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "23:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Wednesday);

                DaySessionDm Thursday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Thursday,
                };
                Thursday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Wednesday,
                        EndTime = "23:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Thursday);

                DaySessionDm Friday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Friday,
                };
                Friday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Thursday,
                        EndTime = "23:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                   
                });
                DM.DaySessions.Add(Friday);

                DM.DaySessions.AddRange(new []{Monday, Tuesday, Wednesday, Thursday, Friday});

                DM.ToEnumerable().ToExcel("d:/中国期市-夜盘1.xls");
                var dms = "d:/中国期市-夜盘1.xls".FromExcel<MarketTimeDm>();
            }
 
            {
                MarketTimeDm DM = new MarketTimeDm()
                {
                    Id = "1",
                    Name = "中国期市-夜盘2",
                    Desc = "上午9:00-11:30,下午13:30-15:00,下午21:00-次日1:00(夜盘)",
                    TimeZone = 8,
                };
                DaySessionDm Monday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Monday,
                };
                Monday.Sessions.AddRange(new []
                { 
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Saturday,
                        EndTime = "1:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                   
                });
                DM.DaySessions.Add(Monday);

                DaySessionDm Tuesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Tuesday,
                };
                Tuesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "1:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "9:00",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "13:30",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Tuesday);

                DaySessionDm Wednesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Wednesday,
                };
                Wednesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Wednesday,
                        EndTime = "1:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Wednesday);

                DaySessionDm Thursday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Thursday,
                };
                Thursday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Thursday,
                        EndTime = "1:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Thursday);

                DaySessionDm Friday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Friday,
                };
                Friday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        StartTime = "21:00",
                        EndDayOfWeek = EWeek.Friday,
                        EndTime = "1:00",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                   
                });
                DM.DaySessions.Add(Friday);

                DM.DaySessions.AddRange(new []{Monday, Tuesday, Wednesday, Thursday, Friday});

                DM.ToEnumerable().ToExcel("d:/中国期市-夜盘2.xls");
                var dms = "d:/中国期市-夜盘2.xls".FromExcel<MarketTimeDm>();
            }

            {
                MarketTimeDm DM = new MarketTimeDm()
                {
                    Id = "1",
                    Name = "中国期市-夜盘3",
                    Desc = "上午9:00-11:30,下午13:30-15:00,下午21:00-次日2:30(夜盘)",
                    TimeZone = 8,
                };
                DaySessionDm Monday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Monday,
                };
                Monday.Sessions.AddRange(new []
                { 
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        StartTime = "22:30",
                        EndDayOfWeek = EWeek.Saturday,
                        EndTime = "2:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        EndDayOfWeek = EWeek.Monday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                   
                });
                DM.DaySessions.Add(Monday);

                DaySessionDm Tuesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Tuesday,
                };
                Tuesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Monday,
                        StartTime = "22:30",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "2:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "9:00",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "13:30",
                        EndDayOfWeek = EWeek.Tuesday,
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Tuesday);

                DaySessionDm Wednesday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Wednesday,
                };
                Wednesday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Tuesday,
                        StartTime = "22:30",
                        EndDayOfWeek = EWeek.Wednesday,
                        EndTime = "2:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        EndDayOfWeek = EWeek.Wednesday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Wednesday);

                DaySessionDm Thursday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Thursday,
                };
                Thursday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Wednesday,
                        StartTime = "22:30",
                        EndDayOfWeek = EWeek.Thursday,
                        EndTime = "2:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        EndDayOfWeek = EWeek.Thursday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                });
                DM.DaySessions.Add(Thursday);

                DaySessionDm Friday = new DaySessionDm()
                {
                    MainDayOfWeek = EWeek.Friday,
                };
                Friday.Sessions.AddRange(new []
                {
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Thursday,
                        StartTime = "22:30",
                        EndDayOfWeek = EWeek.Friday,
                        EndTime = "2:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "9:00",
                        EndTime = "11:30",
                    },
                    new TradingSessionDm()
                    {
                        StartDayOfWeek = EWeek.Friday,
                        EndDayOfWeek = EWeek.Friday,
                        StartTime = "13:30",
                        EndTime = "15:00",
                    },
                   
                });
                DM.DaySessions.Add(Friday);

                DM.DaySessions.AddRange(new []{Monday, Tuesday, Wednesday, Thursday, Friday});

                DM.ToEnumerable().ToExcel("d:/中国期市-夜盘3.xls");
                var dms = "d:/中国期市-夜盘3.xls".FromExcel<MarketTimeDm>();
            }
        }
    }
}
