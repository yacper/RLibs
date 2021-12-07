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
            return $"{Time} Price:{LastPrice,-10} Bid:{Bid,-10} Ask:{Ask,-10} Volume:{Volume,-10} SeqID:{SequenceID}";
        }

        public object       Clone()
        {
            return this.MemberwiseClone();
        }

        public Bar          ToBar(ETimeFrame tf, bool leftAsOpen = true)
        {
            return new Bar(Time.ModTimeFrame(tf, leftAsOpen), LastPrice, LastPrice, LastPrice, LastPrice, Volume);
        }

        public IMessage  ToDm() // 序列化成protobufdm
        {
            throw new NotImplementedException();
            //return new TickDM()
            //{
            //    //time = _time.Ticks,
            //    //ask = _ask,
            //    //bid = _bid,
            //    //askVol = _askVolume,
            //    //bidVol = _bidVolume
            //};
        }
       

        public string       SequenceID { get; set; }                             // 标识符


        public ETickerDirection   Dir { get; set; }                                    //TickerDirection, 买卖方向

        public double       LastPrice { get; set; }                              // 成交价
        public double       LastSize { get; set; }                               // 成交价

        // 注意：这两个量，在不同平台上意义有区别，有的是当前tick的交易量
        // 有的是当前分钟的，有的是当日的
        public double       Volume { get; set; }                                 // 交易量
        public double       TurnOver { get; set; }                               // 交易额

        public double       Ask { get; set; }
        public double       Bid { get; set; }
        public double       AskVol { get; set; }
        public double       BidVol { get; set; }
        public double       Spread => Ask - Bid;

        // 大部分平台，不提供多档数据
        public double[]     Bids { get; set; }
        public double[]     Asks { get; set; }
        public double[]     BidVols { get; set; }
        public double[]     AskVols { get; set; }
        public double[]     BidOrders { get; set; }
        public double[]     AskOrders { get; set; }

        // bid / ask 档位可以不对称
        public byte         BidDepth { get; set; }
        public byte         AskDepth { get; set; }

        // 如果有，指当天的
        public double       PreClose { get; set; }
        public double       Open { get; set; }
        public double       High { get; set; }
        public double       Low { get; set; }

        public Tick()
        {
            LastPrice = double.NaN;
            LastSize = double.NaN;

            Volume= double.NaN;
            TurnOver = double.NaN;

            Ask = double.NaN;
            Bid = double.NaN;
            AskVol = double.NaN;
            BidVol = double.NaN;

            Bids = null;
            Asks = null;
            BidVols = null;
            AskVols = null;
            BidOrders = null;
            AskOrders = null;

            BidDepth = 0;
            AskDepth = 0;

            PreClose = double.NaN;
            Open = double.NaN;
            High = double.NaN;
            Low = double.NaN;
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
