///********************************************************************
//    created:	2018/3/7 20:23:58
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
//using ProtoBuf;

//namespace RLib.Base
//{
//    public struct Tick:ITick
//    {
//#region ITimeValue
//        public DateTime     Time
//        {
//            get { return _time; }
//            set
//            {
//                _time = value;
//            }
//        }

//        public IReadonlyTimeValue  Last { get { return _pLast; } set { _pLast = value; }}                                   // 如果有
//        IReadonlyTimeValue _pLast ;


//        public IExtensible  ToDm() // 序列化成protobufdm
//        {
//            return new TickDM()
//            {
//                time = _time.Ticks,
//                ask = _ask,
//                bid = _bid,
//                askVol = _askVolume,
//                bidVol = _bidVolume
//            };
//        }


//        public int CompareTo(object obj)                            // 比较时间，而不是值
//        {
//            return (int)(Time.Ticks - (obj as IReadonlyTimeValue).Time.Ticks);
//        }


//#endregion

//#region Overrides
        

//        public static Tick Parse(string source)
//        {
//            throw new NotImplementedException();

//            //IFormatProvider invariantEnglishUs = (IFormatProvider)TypeConverterHelper.InvariantEnglishUS;
//            //TokenizerHelper tokenizerHelper = new TokenizerHelper(source, invariantEnglishUs);
//            //string str = tokenizerHelper.NextTokenRequired();
//            //RSize size = !(str == "Empty") ? new RSize(Convert.ToDouble(str, invariantEnglishUs), Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs)) : RSize.Empty;
//            //tokenizerHelper.LastTokenRequired();
//            //return size;
//        }
//        public override string ToString()
//        {
//            return string.Format("{0} Bid:{1} Ask:{2} BidVolume:{3} AskVolume:{4}", Time, Bid, Ask, BidVolume, AskVolume);
//        }
//        public string       ToString(IFormatProvider provider)
//        {
//            return this.ConvertToString((string)null, provider);
//        }
//        //string IFormattable.ToString(string format, IFormatProvider provider)
//        //{
//        //    return this.ConvertToString(format, provider);
//        //}
//        internal string     ConvertToString(string format, IFormatProvider provider)
//        {
//            throw new NotImplementedException();

//            //if (this.IsEmpty)
//            //    return "Empty";
//            //char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
//            //return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[3] { (object)numericListSeparator, (object)this._width, (object)this._height });
//        }

//        public override bool Equals(object o)
//        {
//            if (o == null || !(o is Tick))
//                return false;
//            return Tick.Equals(this, (Tick)o);
//        }
//        public bool         Equals(Tick value)
//        {
//            return Tick.Equals(this, value);
//        }

//        public override int GetHashCode()
//        {
//            return this.Ask.GetHashCode() ^ this.Bid.GetHashCode();
//        }

//        public static bool  Equals(Tick l, Tick r)
//        {
//            if (
//                l.Ask.Equals(r.Ask) &&
//                l.Bid.Equals(r.Bid))
//                return true;
//            return false;
//        }
//        public static bool operator ==(Tick l, Tick r)
//        {
//            return Equals(l, r);
//        }
//        public static bool operator !=(Tick l, Tick r)
//        {
//            return !(l == r);
//        }

//#endregion

//        //public 
      
//        public float        Ask
//        {
//            get
//            {
//                return this._ask;
//            }
//            set
//            {

//                this._ask = value;
//            }
//        }
//        public float        Bid
//        {
//            get
//            {
//                return this._bid;
//            }
//            set
//            {
//                this._bid = value;
//            }
//        }

//        public float       Price { get { return Ask; } } 

//        public long        AskVolume
//        {
//            get
//            {
//                return this._askVolume;
//            }
//            set
//            {

//                this._askVolume = value;
//            }
//        }
//        public long        BidVolume
//        {
//            get
//            {
//                return this._bidVolume;
//            }
//            set
//            {
//                this._bidVolume = value;
//            }
//        }

//        public float        Spread { get { return Bid - Ask; } }

//#region C&D
//        public              Tick(DateTime time, float ask, float bid, long askv = 0, long bidv = 0)
//        {
//            _time = time;
//            _ask = ask;
//            _bid = bid;
//            _askVolume = askv;
//            _bidVolume = bidv;

//            _pLast = null;
//        }
//        public              Tick(TickDM dm)
//        {
//            _time = new DateTime(dm.time);
//            _ask = dm.ask;
//            _bid = dm.bid;
//            _askVolume = dm.askVol;
//            _bidVolume = dm.bidVol;

//            _pLast = null;
//        }
//#endregion

//#region Members
//        internal DateTime   _time;
//        internal float     _ask;
//        internal float     _bid;
//        internal long       _askVolume;
//        internal long       _bidVolume;
//#endregion
//    }

//}
