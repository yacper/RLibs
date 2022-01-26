/********************************************************************
    created:	2019/6/12 15:15:39
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using Google.Protobuf;
using RLib.Base;

namespace RLib.Fin
{
    public class Tick:TimeValue, ITick
    {
        public override string ToString()
        {
            return $"{Time} Price:{LastPrice,-10} Bid:{BidPrice,-10} Ask:{AskPrice,-10} Volume:{LastVolume,-10} SeqID:{SequenceID}";
        }

        public object       Clone()
        {
            return this.MemberwiseClone();
        }

        //public Bar          ToBar(ETimeFrame tf, bool leftAsOpen = true)
        //{
        //    return new Bar(Time.ModTimeFrame(tf, leftAsOpen), LastPrice, LastPrice, LastPrice, LastPrice, Volume);
        //}

        //public IMessage  ToDm() // 序列化成protobufdm
        //{
        //    throw new NotImplementedException();
        //    //return new TickDM()
        //    //{
        //    //    //time = _time.Ticks,
        //    //    //ask = _ask,
        //    //    //bid = _bid,
        //    //    //askVol = _askVolume,
        //    //    //bidVol = _bidVolume
        //    //};
        //}
       

        public string       SequenceID { get; set; }                             // 标识符


        public ETickerDirection   Dir { get; set; }                                    //TickerDirection, 买卖方向


        public double              LastPrice { get; set; }                              // 上一个成交价
        public double              LastVolume { get; set; }                                // 上一个成交量
        public double              LastTurnover { get; set; }                           // 上一个成交额

        public double              TotalVolume { get; set; }                    // 当日成交量
        public double              TotalTurnover { get; set; }                  // 当日成交额

        // 1档数据
        public double AskPrice => BidDepth != 0 ? AskLevels[0].Price : double.NaN;
        public double AskVolume => AskLevels[0].Volume;
        public double AskOrders => AskLevels[0].Orders;
        public double BidPrice => BidLevels[0].Price;
        public double BidVolume => BidLevels[0].Volume;
        public double BidOrders => BidLevels[0].Orders;
        public double Spread => AskPrice - BidPrice;


        // 大部分平台，不提供多档数据
        public Level[]            BidLevels { get; set; }
        public Level[]            AskLevels { get; set; }
        // bid / ask 档位可以不对称
        public byte                BidDepth { get; set; }
        public byte                AskDepth { get; set; }

        // 如果有，指当天的
        public double       PreClose { get; set; }
        public double       Open { get; set; }
        public double       High { get; set; }
        public double       Low { get; set; }

        public double              UpLimitPrice { get; set; }            // 涨停价
        public double              LowLimitPrice { get; set; }           // 跌停价

        // 期货
        public double              OpenInterest { get; set; }       // 持仓
        public double              PreOpenInterest { get; set; }        // 昨持仓

        public EExchangeStatus    ExchangeStatus { get; set; }                         //交易所状态


        public Tick()
        {
            LastPrice = double.NaN;
            LastVolume = double.NaN;
            LastTurnover = double.NaN;

            TotalVolume= double.NaN;
            TotalTurnover = double.NaN;

            BidDepth = 1;
            AskDepth = 1;

            PreClose = double.NaN;
            Open = double.NaN;
            High = double.NaN;
            Low = double.NaN;

            UpLimitPrice = double.NaN;
            LowLimitPrice = double.NaN;

            OpenInterest = double.NaN;
            PreOpenInterest = double.NaN;

            ExchangeStatus = EExchangeStatus.Unknown;
        }

    }

    //public struct ForexTick : IForexTick
    //{
    //    public override string ToString()
    //    {
    //        return $"{Time} Bid:{Bid, -10} Ask:{Ask, -10} Volume:{Volume, -10} SeqID:{SequenceID}";
    //    }

    //    #region TimeValue
    //    public DateTime Time { get; set; }

    //    public IExtensible ToDm() // 序列化成protobufdm
    //    {
    //        return new TickDM()
    //        {
    //            //time = _time.Ticks,
    //            //ask = _ask,
    //            //bid = _bid,
    //            //askVol = _askVolume,
    //            //bidVol = _bidVolume
    //        };
    //    }
    //    public int CompareTo(object obj)                            // 比较时间，而不是值
    //    {
    //        return (int)(Time.Ticks - (obj as IReadonlyTimeValue).Time.Ticks);
    //    }
    //    #endregion

    //    public double Value
    //    {
    //        get { return Bid; }
    //    }                                  // 成交价
    //    public double Volume { get; set; }                                 // 交易量

    //    public long?        SequenceID { get; set; }                             // 标识符
    //    public double       TurnOver { get; set; }                               // 交易额

    //    public ETickerDirection?   Dir { get; set; }                                    //TickerDirection, 买卖方向


   

    //    public ForexTick(DateTime dt, double ask, double bid, double volume = 0d)
    //    {
    //        Time = dt;
    //        Ask = ask;
    //        Bid = bid;

    //        Volume = volume;

    //        SequenceID = null;
    //        TurnOver = double.NaN;
    //        Dir = null;
    //    }
    //}
}
