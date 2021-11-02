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


    public interface ITick:IReadonlyTimeValue,ICloneable                               // 基本tick
    {
        string              SequenceID { get; }                             // 平台的tick标识符

        double              LastPrice { get; }                              // 成交价
        double              LastSize { get; }                               // 成交价

        // 注意：这两个量，在不同平台上意义有区别，有的是当前tick的交易量
        // 有的是当前分钟的，有的是当日的
        double              Volume { get; }                                 // 交易量
        double              TurnOver { get; }                               // 交易额

        double              Ask { get; }
        double              Bid { get; }
        double              AskVol { get; }
        double              BidVol { get;}
        double              Spread { get; }

        // 大部分平台，不提供多档数据
        double[]            Bids { get; }
        double[]            Asks { get; }
        double[]            BidVols { get; }
        double[]            AskVols { get; }
        double[]            BidOrders { get; }
        double[]            AskOrders { get; }

        // bid / ask 档位可以不对称
        byte                BidDepth { get; }
        byte                AskDepth { get; }


        // 如果有，指当天的
        double              PreClose { get; set; }
        double              Open { get; set; }
        double              High { get; set; }
        double              Low { get; set; }


        //TradingStatus TradingStatus { get; set; }
        //TradingPeriod TradingPeriod { get; set; }
        ETickerDirection    Dir { get; }                                    //TickerDirection, 买卖方向
    }

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
