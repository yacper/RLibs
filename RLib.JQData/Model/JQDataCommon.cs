/********************************************************************
    created:	2021/8/21 17:52:27
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.JQData
{
    // finnhub resolution
    public enum ETimeFrame
    {
        m1,
        m5,
        m15,
        m30,
        m60,
        m120,
        D1,
        W1,
        M1
    }

    public static class TimeFrame
    {
        public static string ToResolutionString(this ETimeFrame tf)
        {
            switch (tf)
            {
                case ETimeFrame.m1:
                    return "1m";
                case ETimeFrame.m5:
                    return "5m";
                case ETimeFrame.m15:
                    return "15m";
                case ETimeFrame.m30:
                    return "30m";
                case ETimeFrame.m60:
                    return "60m";
                case ETimeFrame.m120:
                    return "120m";
                case ETimeFrame.D1:
                    return "1d";
                case ETimeFrame.W1:
                    return "1w";
                case ETimeFrame.M1:
                    return "1M";
            }

            return "1d";
        }
    }

    public enum ECodeType       // 证券类型
    {
        stock, 
        fund, 
        index, 
        futures, 
        etf, 
        lof, 
        fja, 
        fjb, 
        QDII_fund, 
        open_fund, 
        bond_fund, 
        stock_fund, 
        money_market_fund, 
        mixture_fund, 
        options
    }

    public class Security
    {
        public string code { get; set; }
        public string display_name { get; set; }
        public string name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public ECodeType type { get; set; }

        public override string ToString()
        {
            return $"{type,-18} {code,-10} {display_name,14}({name})   start:{start_date}   end:{end_date}";
        }
    }

    public class SecurityInfo:Security
    {
        //code,display_name,name,start_date,end_date,type,parent
        //502050.XSHG,上证50B,SZ50B,2015-04-27,2200-01-01,fjb,502048.XSHG
        public string parent { get; set; }

        public override string ToString()
        {
            return $"{type,-18} {code,-10} {display_name,14}({name}) parent:{parent,-10}   start:{start_date}   end:{end_date}";
        }
    }

    public class Bar
    {
        //date,open,close,high,low,volume,money
        //2018-12-04 10:00,11.00,11.03,11.07,10.97,4302800,47472956.00
        //2018-12-04 10:30,11.04,11.04,11.06,10.98,3047800,33599476.00

        public DateTime date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }

        public double volume { get; set; }
        public double money { get; set; }

        ///当unit为1d时，包含以下返回值:
        public bool         paused { get; set; }                            //: 是否停牌，0 正常；1 停牌
        public double       high_limit { get; set; }                        //: 涨停价
        public double       low_limit { get; set; }                         //: 跌停价 当code为期货和期权时，包含以下返回值:
        public double       open_interest { get; set; }                     // 持仓量
        public double       avg { get; set; }                               // 均价
        public double       pre_close { get; set; }                         // 

        public override string ToString()
        {
            return $" {date,-30} O:{open,-10} H:{high,-10} L:{low,-10} C:{close,-10} V:{volume,-10} T:{money,-10}";
        }

    }

    public class Tick 
    {
//	time	current	high	low	volume	money	position	a1_p	a1_v	b1_p	b1_v	spread	vol
//0	2021/10/8 08:59:00.500	22555	22555	22555	280	31577000	83618	22565	20	22555	43	10	
//1	2021/10/8 09:00:00.500	22555	22575	22555	380	42857625	83616	22560	18	22555	39	5	100
//2	2021/10/8 09:00:01.000	22585	22585	22555	604	68133775	83559	22585	20	22565	3	20	224

        public DateTime time { get; set; }
        public double current { get; set; }     // 当前价

        public double high { get; set; }
        public double low { get; set; }

        public double volume { get; set; }      // 累计成交量
        public double money { get; set; }       // 累计成交额
        public double position { get; set; }    // 持仓量

        public double a1_v { get; set; }        // 一档卖量
        public double a1_p { get; set; }        // 一档卖价

        public double b1_v { get; set; }        // 一档买量
        public double b1_p { get; set; }        // 一档买价

        public override string ToString()
        {
            return $" {time,-30} C:{current,-10} {b1_p}({b1_v}) -- {a1_p}({a1_v}) H:{high,-10} L:{low,-10} V:{volume,-10} T:{money,-10} P:{position}";
        }
    }

    public class CurrentPrice
    {
        //code,current
        //000001.XSHE,13.35
        //600600.XSHG,42.4

        public string code { get; set; }
        public double current { get; set; }
    }

    public static class JQDataCommon
    {
        public static string ToJqDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

    }


}
