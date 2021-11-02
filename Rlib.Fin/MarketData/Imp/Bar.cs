/********************************************************************
    created:	2019/6/12 15:45:56
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using Google.Protobuf;
using RLib.Base;
using Rlib.Fin;

namespace RLib.Fin
{
    public class Bar:TimeValue, IBar
    {
#region Statics
        public static readonly Bar Zero = new Bar() { Open = 0, High = 0, Low = 0, Close = 0};
        public static readonly Bar One = new Bar() { Open = 1, High = 1, Low = 1, Close = 1 };

        public static List<Tick> EmulateTicks(IBar b, ETimeFrame tf, bool leftAsOpen = true, int scale = 2)                  // 根据bar模拟出一系列的ticks
        {// 使用福汇的模拟方式 https://fxcodebase.com/wiki/index.php/Price_Simulation
            // 每个bar会被模拟成连续的8个点
            List<Tick> ret = new List<Tick>(8);
            List<DateTime> times = b.Time.Split(tf, 8, leftAsOpen);
            double vol = b.Volume / 8;
            if (b.Type == EBarType.Rise ||
                b.Type == EBarType.Even)
            {
                ret.Add(new Tick() { Time = times[0], LastPrice = b.Open, Volume = vol });
                ret.Add(new Tick() { Time = times[1], LastPrice = Math.Round(b.Open - b.BottomShadow / 2, scale), Volume = vol });
                ret.Add(new Tick() { Time = times[2], LastPrice = b.Low, Volume = vol });
                ret.Add(new Tick() { Time = times[3], LastPrice = Math.Round(b.Low + b.Range / 3, scale), Volume = vol });
                ret.Add(new Tick() { Time = times[4], LastPrice = Math.Round(b.High -  (b.Range / 3), scale), Volume = vol });
                ret.Add(new Tick() { Time = times[5], LastPrice = b.High, Volume = vol });
                ret.Add(new Tick() { Time = times[6], LastPrice = Math.Round(b.High - b.TopShadow / 2, scale), Volume = vol });
                ret.Add(new Tick() { Time = times[7], LastPrice = b.Close, Volume = vol });            
            }
            else
            {
                ret.Add(new Tick() { Time = times[0], LastPrice = b.Open, Volume = vol });
                ret.Add(new Tick() { Time = times[1], LastPrice = Math.Round(b.Open + b.TopShadow / 2, scale), Volume = vol });
                ret.Add(new Tick() { Time = times[2], LastPrice = b.High, Volume = vol });
                ret.Add(new Tick() { Time = times[3], LastPrice = Math.Round(b.High - b.Range / 3, scale), Volume = vol });
                ret.Add(new Tick() { Time = times[4], LastPrice = Math.Round(b.High - 2 * (b.Range / 3), scale), Volume = vol });
                ret.Add(new Tick() { Time = times[5], LastPrice = b.Low, Volume = vol });
                ret.Add(new Tick() { Time = times[6], LastPrice = Math.Round(b.Low + b.BottomShadow / 2, scale), Volume = vol });
                ret.Add(new Tick() { Time = times[7], LastPrice = b.Close, Volume = vol });
            }

            return ret;
        }

        //public static IEnumerable<Bar> ToLeftOpen(this IEnumerable<Bar> bars)
        //{

        //}

#endregion


#region ITimeValue
        public IMessage  ToDm() // 序列化成protobufdm
        {
            return new BarDM()
            {
                //time = _time.Ticks,
                //open = _open,
                //high = _high,
                //low = _low,
                //close = _close,
                //vol =_volume
            };
        }
#endregion

        public Bar      ToLeftOpen(ETimeFrame tf)           // 
        {
            Debug.Assert(tf >= ETimeFrame.M1 && tf < ETimeFrame.Day);

            Time = Time.AddTimeFrames(tf, -1);

            return this;
        }
        public Bar      ToRightOpen(ETimeFrame tf)           // 
        {
            Debug.Assert(tf >= ETimeFrame.M1 && tf < ETimeFrame.Day);

            Time = Time.AddTimeFrames(tf, 1);

            return this;
        }


#region Overrides
        public static Bar Parse(string source)
        {
            throw new NotImplementedException();

            //IFormatProvider invariantEnglishUs = (IFormatProvider)TypeConverterHelper.InvariantEnglishUS;
            //TokenizerHelper tokenizerHelper = new TokenizerHelper(source, invariantEnglishUs);
            //string str = tokenizerHelper.NextTokenRequired();
            //RSize size = !(str == "Empty") ? new RSize(Convert.ToDouble(str, invariantEnglishUs), Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs)) : RSize.Empty;
            //tokenizerHelper.LastTokenRequired();
            //return size;
        }
        public override string ToString()
        {
            return string.Format($" {Time} O:{Open} H:{High} L:{Low} C:{Close} V:{Volume}");

//            return this.ConvertToString((string)null, (IFormatProvider)null);
        }
        public string       ToString(IFormatProvider provider)
        {
            return this.ConvertToString((string)null, provider);
        }
        //string IFormattable.ToString(string format, IFormatProvider provider)
        //{
        //    return this.ConvertToString(format, provider);
        //}
        internal string     ConvertToString(string format, IFormatProvider provider)
        {
            throw new NotImplementedException();

            //if (this.IsEmpty)
            //    return "Empty";
            //char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            //return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[3] { (object)numericListSeparator, (object)this._width, (object)this._height });
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is Bar))
                return false;
            return Bar.Equals(this, (Bar)o);
        }
        public bool         Equals(Bar value)
        {
            return Bar.Equals(this, value);
        }

        public override int GetHashCode()
        {
            return this.Open.GetHashCode() ^ this.High.GetHashCode()^ this.Low.GetHashCode()^ this.Close.GetHashCode();
        }

        public static bool  Equals(Bar l, Bar r)
        {
            if (l.High.Equals(r.High) &&
                l.Open.Equals(r.Open) &&
                l.Low.Equals(r.Low) &&
                l.Close.Equals(r.Close))
                return true;
            return false;
        }
        public static bool operator ==(Bar l, Bar r)
        {
            return Equals(l, r);
        }
        public static bool operator !=(Bar l, Bar r)
        {
            return !(l == r);
        }
#endregion

#region Properties
        public double       Open { get; set; }
        public double       High { get; set; }
        public double       Low { get; set; }
        public double       Close { get; set; }
        public double       PreClose { get; set; }                               // 可能没有,nan表示
      

        public EBarType     Type
        {
            get
            {
                if (Close > Open)
                    return EBarType.Rise;
                else if (Close < Open)
                    return EBarType.Decline;
                else
                    return EBarType.Even;
            }
        }

        public double       Range
        {
            get { return High - Low; }
        }

        public double       Solid => Math.Abs(Close - Open);
        public double       TopShadow => Math.Min(High - Open, High - Close);
        public double       BottomShadow => Math.Min(Close - Low, Open - Low);
        public double       Amplitude => (High - Low) / Low;

        public double       Volume { get; set; }
        public double       Turnover { get; set; }
       

        //public double        Change
        //{
        //    get
        //    {
        //        if (Last != null)
        //        {
        //            return Close - (Last as IBar).Close;
        //        }
        //        return 0;
        //    }
        //} // 涨跌

        //public double        ChangePercent
        //{
        //    get
        //    {
        //        if (Last != null)
        //            return Change / (Last as IBar).Close;
        //        else
        //            return 0f;
        //    }
        //} // 涨跌百分比


        #endregion

        public IBar       _UpdateByTick(double v)
        {
            Bar ret = this;

            if (v > ret.High)
                ret.High = v;
            else if (v < ret.Low)
                ret.Low = v;

            ret.Close = v;

            return ret;
        }

        #region C&D

        public Bar()
        {
            Time = DateTime.MinValue;
            Open = double.NaN;
            High = double.NaN;
            Low = double.NaN;
            Close = double.NaN;
            Volume = double.NaN;
            Turnover = double.NaN;

        }
        public Bar(DateTime time, double open, double high, double low, double close, double vol = double.NaN)
        {
            if (high < low)
                throw new ArgumentException("Bad Args");

            Time = time;

            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = vol;

            Turnover = double.NaN;
            PreClose = double.NaN;
        }

        //public              Bar(BarDM dm)
        //{
        //    //Time = new DateTime(dm.time);
        //    //_open = dm.open;
        //    //_high = dm.high;
        //    //_low = dm.low;
        //    //_close = dm.close;
        //    //_volume = dm.vol;
        //}
#endregion
    }

//    public struct ForexBar : IForexBar
//    {
//#region Statics
//        public static readonly Bar Zero = new Bar() { Open = 0, High = 0, Low = 0, Close = 0 };
//        private static readonly Bar One = new Bar() { Open = 1, High = 1, Low = 1, Close = 1 };
//#endregion

//#region ITimeValue
//        public DateTime     Time { get; set; }

//        public IExtensible  ToDm()                                          // 序列化成protobufdm
//        {
//            throw new NotImplementedException();
//            //return new FxcmBarDm()
//            //{
//            //    time = _time.Ticks,

//            //    BidOpen = _bidOpen,
//            //    BidHigh = _bidHigh,
//            //    BidLow = _bidLow,
//            //    BidClose = _bidClose,

//            //    BidVolume = _bidVolume,

//            //    AskOpen = _askOpen,
//            //    AskHigh = _askHigh,
//            //    AskLow = _askLow,
//            //    AskClose = _askClose,

//            //    AskVolume = _askVolume
//            //};
//        }

//        /// 衍生值
////        public float Amplitude { get { return High - Low; } }                             // 振幅 

//        //public float Change
//        //{
//        //    get
//        //    {
//        //        if (Last != null)
//        //        {
//        //            return Close - (Last as IBar).Close;
//        //        }
//        //        return 0;
//        //    }
//        //} // 涨跌

//        //public float ChangePercent
//        //{
//        //    get
//        //    {
//        //        if (Last != null)
//        //            return Change / (Last as IBar).Close;
//        //        else
//        //            return 0f;
//        //    }
//        //} // 涨跌百分比


//        public int          CompareTo(object obj)                            // 比较时间，而不是值
//        {
//            return (int)(Time.Ticks - (obj as IReadonlyTimeValue).Time.Ticks);
//        }
//        #endregion

//#region Overrides
//        public double       Open { get { return BidOpen; } }
//        public double       High { get { return BidHigh; } }
//        public double       Low { get { return BidLow; } }
//        public double       Close { get { return BidClose; } }

//        public IBar         UpdateByTick(double v)
//        {
//            ForexBar ret = this;

//            if (v > ret.High)
//                ret.BidHigh = v;
//            else if (v < ret.Low)
//                ret.BidLow = v;

//            ret.BidClose = v;

//            return ret;
//        }

//        #endregion

//        public override string ToString()
//        {
//            string ret = string.Format(" {0} BO:{1} BH:{2} BL:{3} BC:{4} AO:{5} AH:{6} AL:{7} AC:{8} Volume:{9})", Time, BidOpen, BidHigh,
//                BidLow, BidClose, AskOpen, AskHigh, AskLow, AskClose, Volume);

//            return ret;
//        }


//        public EBarType     Type
//        {
//            get
//            {
//                if (Open > Close)
//                    return EBarType.Rise;
//                else if (Open < Close)
//                    return EBarType.Decline;
//                else
//                    return EBarType.Even;
//            }
//        }


//        public double       BidOpen { get; set; }
//        public double       BidHigh { get; set; }
//        public double       BidLow { get; set; }
//        public double       BidClose { get; set; }

//        public double       AskOpen { get; set; }
//        public double       AskHigh { get; set; }
//        public double       AskLow { get; set; }
//        public double       AskClose { get; set; }

//        public double       Volume { get; set; }


//#region c&D
//        public              ForexBar(DateTime time,
//                                    double bo, double bh, double bl, double bc, 
//                                    double ao, double ah, double al, double ac, 
//                                    double volume = 0d
//                                    )
//        {
//            Time = time;

//            BidOpen = bo;
//            BidHigh = bh;
//            BidLow = bl;
//            BidClose = bc;

//            AskOpen = ao;
//            AskHigh = ah;
//            AskLow = al;
//            AskClose = ac;

//            Volume = volume;
//        }

//        public              ForexBar(ForexBarDM dm)
//        {
//            Time = new DateTime(dm.Time);

//            BidOpen = dm.BidOpen;
//            BidHigh = dm.BidHigh;
//            BidLow = dm.BidLow;
//            BidClose = dm.BidClose;

//            AskOpen = dm.AskOpen;
//            AskHigh = dm.AskHigh;
//            AskLow = dm.AskLow;
//            AskClose = dm.AskClose;

//            Volume = dm.Volume;
//        }
//        #endregion

//    }


}
