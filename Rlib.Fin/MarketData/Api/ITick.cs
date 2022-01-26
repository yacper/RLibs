/********************************************************************
    created:	2019/6/12 15:09:07
    author:		rush
    email:		
	
    purpose:	单笔成交数据
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using RLib.Base;

namespace RLib.Fin
{
    public enum ETickerDirection
    {
        Unknown = 0,
        Bid = 1,
        Ask = 2,
        Neutral = 3,
    }

    public struct Level         // 1档数据
    {
        public double Price = double.NaN;      // 价格
        public double Volume = double.NaN;     // 量
        public double Orders = double.NaN; // 订单数
    }

    public interface ITick:ITimeValue,ICloneable                    // 基本tick 大部分数据可能没有
    {
        double              LastPrice { get; }                              // 上一个成交价
        double              LastVolume { get; }                                // 上一个成交量
        double              LastTurnover { get; }                           // 上一个成交额

        double              TotalVolume { get; }                    // 当日成交量
        double              TotalTurnover { get; }                  // 当日成交额

        // 1档数据
        double              AskPrice { get; }
        double              AskVolume { get; }
        double              AskOrders { get; }
        double              BidPrice { get; }
        double              BidVolume { get; }
        double              BidOrders { get; }
        double              Spread { get; }


        // 大部分平台，不提供多档数据
        Level[]            BidLevels { get; }
        Level[]            AskLevels { get; }
        // bid / ask 档位可以不对称
        byte                BidDepth { get; }
        byte                AskDepth { get; }


        // 如果有，指当天的
        double              PreClose { get; }
        double              Open { get; }
        double              High { get; }
        double              Low { get; }

        double              UpLimitPrice { get; }            // 涨停价
        double              LowLimitPrice { get; }           // 跌停价

        // 期货
        double              OpenInterest { get; }       // 持仓
        double              PreOpenInterest { get; }        // 昨持仓

        EExchangeStatus    ExchangeStatus { get; }                         //交易所状态
    }

//    public interface ICnFutureTick : ITick              // 中国期货市场Tick
//    {
//
//        // 注意：这两个量，在不同平台上意义有区别，有的是当前tick的交易量
//        // 有的是当前分钟的，有的是当日的
//        double              Volume { get; }                                 // 交易量
//        double              TurnOver { get; }                               // 交易额
//
//    }



    //public interface IForexTick : ITick
    //{
    //    double Ask { get; }
    //    double Bid { get; }

    //    double Spread { get; }
    //}

    //public interface IStockTick:ITick
    //{
    //    double              Ask(int n);
    //    double              Bid(int n);

    //    double              AskVolume(int n);
    //    double              BidVolume(int n);

    //}
}
