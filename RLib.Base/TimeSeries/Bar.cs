///********************************************************************
//    created:	2018/1/26 16:33:18
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
//    public struct Bar:IBar, IReadonlyBar
//    {
//#region Statics
//        public static Bar Zero { get { return Bar.s_zero; } }
//        public static Bar One { get { return Bar.s_one; } }
//#endregion



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
//        IReadonlyTimeValue _pLast;

//        public IExtensible  ToDm() // 序列化成protobufdm
//        {
//            return new BarDM()
//            {
//                time = _time.Ticks,
//                open = _open,
//                high = _high,
//                low = _low,
//                close = _close,
//                vol =_volume
//            };
//        }


//        public int CompareTo(object obj)                            // 比较时间，而不是值
//        {
//            return (int)(Time.Ticks - (obj as IReadonlyTimeValue).Time.Ticks);
//        }


//#endregion


//#region Overrides
//        public static Bar Parse(string source)
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
//            return string.Format(" {0} O:{1} H:{2} L:{3} C:{4} ", Time, Open, High, Low, Close);

////            return this.ConvertToString((string)null, (IFormatProvider)null);
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
//            if (o == null || !(o is Bar))
//                return false;
//            return Bar.Equals(this, (Bar)o);
//        }
//        public bool         Equals(Bar value)
//        {
//            return Bar.Equals(this, value);
//        }

//        public override int GetHashCode()
//        {
//            return this.Open.GetHashCode() ^ this.High.GetHashCode()^ this.Low.GetHashCode()^ this.Close.GetHashCode();
//        }

//        public static bool  Equals(Bar l, Bar r)
//        {
//            if (l.High.Equals(r.High) &&
//                l.Open.Equals(r.Open) &&
//                l.Low.Equals(r.Low) &&
//                l.Close.Equals(r.Close))
//                return true;
//            return false;
//        }
//        public static bool operator ==(Bar l, Bar r)
//        {
//            return Equals(l, r);
//        }
//        public static bool operator !=(Bar l, Bar r)
//        {
//            return !(l == r);
//        }




//#endregion

//#region Properties
//        public float        Open
//        {
//            get
//            {
//                return this._open;
//            }
//            set
//            {

//                this._open = value;
//            }
//        }
//        public float       High 
//        {
//            get
//            {
//                return this._high;
//            }
//            set
//            {
//                this._high = value;
//            }
//        }
//        public float        Low
//        {
//            get
//            {
//                return this._low;
//            }
//            set
//            {
//                this._low = value;
//            }
//        }
//        public float        Close
//        {
//            get
//            {
//                return this._close;
//            }
//            set
//            {
//                this._close = value;
//            }
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

//        public long         Volume
//        {
//            get { return _volume; } 
//            set
//            {
//                this._volume = value;
//            }
//        }


//        /// 衍生值
//        public float        Amplitude  { get { return High - Low; } }                             // 振幅 

//        public float        Change
//        {
//            get
//            {
//                if (Last != null)
//                {
//                    return Close - (Last as IBar).Close;
//                }
//                return 0;
//            }
//        } // 涨跌

//        public float        ChangePercent
//        {
//            get
//            {
//                if (Last != null)
//                    return Change / (Last as IBar).Close;
//                else
//                    return 0f;
//            }
//        } // 涨跌百分比


//        #endregion

//        public IBar       UpdateByTick(float v)
//        {
//            Bar ret = this;

//            if (v > ret.High)
//                ret.High = v;
//            else if (v < ret.Low)
//                ret.Low = v;

//            ret.Close = v;

//            return ret;
//        }

//#region C&D
//        public              Bar(DateTime time, float open, float high, float low, float close, long vol = 0 )
//        {
//            if (high < low)
//                throw new ArgumentException("Bad Args");

//            _time = time;

//            _open = open;
//            _high = high;
//            _low = low;
//            _close = close;
//            _volume = vol;

//            _pLast = null;
//        }



//        public              Bar(BarDM dm)
//        {
//            _time = new DateTime(dm.time);
//            _open = dm.open;
//            _high = dm.high;
//            _low = dm.low;
//            _close = dm.close;
//            _volume = dm.vol;

//            _pLast = null;
//        }
//#endregion

//#region Members
//        private static readonly Bar s_zero = new Bar() { _open = 0, _high = 0, _low = 0, _close = 0};
//        private static readonly Bar s_one = new Bar() { _open = 1, _high = 1, _low = 1, _close = 1 };

//        internal DateTime   _time;
//        internal float      _open;
//        internal float      _high;
//        internal float      _low;
//        internal float      _close;
//        internal long       _volume;
//#endregion
//    }
//}
