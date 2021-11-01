/********************************************************************
    created:	2018/6/12 14:05:27
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using RLib.Base;

namespace DataModel
{
    public static class EWeekEx
    {

        public static int Subtract(this EWeek w, EWeek other)
        {
            if (w >= other)
                return w.Pot().Value - other.Pot().Value;
            else
            {
                return w.Pot().Value + 7 - other.Pot().Value;
            }
        }

        public static DayOfWeek ToDayOfWeek(this EWeek w)
        {
            switch (w)
            {
                case EWeek.Sunday:
                    return DayOfWeek.Sunday;
                case EWeek.Monday:
                    return DayOfWeek.Monday;
                case EWeek.Tuesday:
                    return DayOfWeek.Tuesday;
                case EWeek.Wednesday:
                    return DayOfWeek.Wednesday;
                case EWeek.Thursday:
                    return DayOfWeek.Thursday;
                case EWeek.Friday:
                    return DayOfWeek.Friday;
                case EWeek.Saturday:
                    return DayOfWeek.Saturday;
            } 
            return DayOfWeek.Sunday;
        }

        public static EWeek ToEWeek(this DayOfWeek w)
        {
            switch (w)
            {
                case DayOfWeek.Sunday:
                    return EWeek.Sunday;
                case DayOfWeek.Monday:
                    return EWeek.Monday;
                case DayOfWeek.Tuesday:
                    return EWeek.Tuesday;
                case DayOfWeek.Wednesday:
                    return EWeek.Wednesday;
                case DayOfWeek.Thursday:
                    return EWeek.Thursday;
                case DayOfWeek.Friday:
                    return EWeek.Friday;
                case DayOfWeek.Saturday:
                    return EWeek.Saturday;
            } 
            return EWeek.Sunday;
        }

    }


    public partial class NumRange                                           // 数字区间
    {
        //public override string ToString()
        //{
        //    if (Op < ECompareOp.Between)
        //    {
        //        return EnumHelper.Tostring(Op) +" " + Left;
        //    }
        //    else
        //    {
        //        return Left + " " + EnumHelper.Tostring(Op) + " " + Right;
        //    }
        //}

        public string       ToStringPercent()
        {
            if (Op < ECompareOp.Between)
            {
                return EnumHelper.Tostring(Op) +" " + Left*100 +"%";
            }
            else
            {
                return Left*100 + "% " + EnumHelper.Tostring(Op) + " " + Right*100+"%";
            }
        }

#region ==



        //public override int GetHashCode()
        //{
        //    return color.GetHashCode() ^ (range?.GetHashCode()??0);
        //}


        public static bool      operator !=(NumRange lhs, NumRange rhs)
        {
            return !(lhs == rhs);
        }
        public static bool      operator ==(NumRange l, NumRange r)
        {
            if ((object)l == null != ((object)r == null))
                return false;

            if ((object) l != null)
            {
                if (l.Op == r.Op && RMath.Equal(l.Left, r.Left))
                {
                    if (l.HasRight == r.HasRight)
                    {
                        if (l.HasRight)
                            return RMath.Equal(l.Right, r.Right);
                        else
                            return true;
                    }
                }
                return false;

            }

            return true;
        }

#endregion




        public bool         Contains(double val)                            // 是否包含一个值
        {
            switch (Op)
            {
                    case ECompareOp.Equal:
                        return RMath.Equal(Left, Right);
                    break;
                    case ECompareOp.NotEqual:
                        return !RMath.Equal(Left, Right);
                    break;
                    case ECompareOp.Greater:
                        return val > Left;
                    break;
                    case ECompareOp.GreaterEqual:
                        return val >= Left;
                    break;
                    case ECompareOp.Smaller:
                        return val < Left;
                    break;
                    case ECompareOp.SmallerEqual:
                        return val <= Left;
                    break;
                    case ECompareOp.Between:
                        return val < Right&& val> Left;
                    break;
                    case ECompareOp.BetweenEqual:
                        return val <= Right&& val>= Left;
                    break;
                    default:
                    throw new Exception();
            }
            
            
        }


        public              NumRange(string str)
        {
            string[] strs = str.Split(' ');
            if (strs.Length == 2)
            {
                Op = EnumHelper.FromString<DataModel.ECompareOp>(strs[0]);
                Left = Convert.ToDouble(strs[1]);
            }
            else if (strs.Length == 3)
            {
                Left = Convert.ToDouble(strs[0]);
                Op = EnumHelper.FromString<DataModel.ECompareOp>(strs[1]);
                Right = Convert.ToDouble(strs[2]);
            }
        }
        public              NumRange(ECompareOp p, double l, double? r=null)
        {
            Op = p;
            Left = l;
            if (r != null&& 
                !double.IsNaN(r.Value)
                )
            {
                if(l > r.Value)
                    throw new ArgumentException("l > r");
                
                Right = r.Value;
            }
        }
    }

    public partial class VarDM
    {
        [JsonIgnore]
        public object Value
        {
            get
            {
                Type t = System.Type.GetType(this.Type);
                if (typeof(IStringSerializable).IsAssignableFrom(t))
                {// 目前主用于idataSeries的序列化
                    return t.Call("DeserializeString", Val);
                }
                else
                    return this.Val.ToJsonObj(System.Type.GetType(this.Type));
            }
            set
            {
                this.Type = value.GetType().AssemblyQualifiedName;

                if (value is IStringSerializable)
                {// 目前主用于idataSeries的序列化
                    this.Val = (value as IStringSerializable).SerializeString();
                }
                else

                    this.Val = value.ToJsonNoLoop();
            }
        }

        public VarDM(object o)
        {
            Value = o;
        }
    }



    public partial class TimeRange                                           // 数字区间
    {
        //public override string ToString()
        //{
        //    switch (type)
        //    {
        //        case ETimeRange.Normal:
        //            {
        //                if (op < ECompareOp.Between)
        //                {
        //                    return EnumHelper.Tostring(op) + " " + Left;
        //                }
        //                else
        //                {
        //                    return Left + " " + EnumHelper.Tostring(op) + " " + Right;
        //                }
        //            }
        //        case ETimeRange.Span:
        //            {
        //                return "Last: " + Span;
        //            }
        //        default:
        //            return ETimeRangeToStr(type);
        //    }
        //}

        public bool         Contains(DateTime val)                            // 是否包含一个值
        {
            if (!__ContainsCheckIntraDay(val)) return false;
            if (!__ContainsCheckWeek(val)) return false;
            DateTime? min = null;
            DateTime? max = null;
            DateTime now = DateTime.Now;
            switch (Type)
            {
                case ETimeRange.Normal:
                    min = Left;
                    max = Right;
                    return __ContainsCheckLeftRight(min, max, val, true);
                case ETimeRange.Span:
                    max = now;
                    min = now.Subtract(Span.Value);    //todo,未做测试，可能有问题;
                    return __ContainsCheckLeftRight(min, max, val, true);
                case ETimeRange.ThisYear:
                    min = now.Date.AddDays(1 - now.DayOfYear);
                    max = min.Value.AddYears(1);
                    return __ContainsCheckLeftRight(min, max, val, false);
                case ETimeRange.LastYear:
                    max = now.Date.AddDays(1 - now.DayOfYear);
                    min = max.Value.AddYears(-1);
                    return __ContainsCheckLeftRight(min, max, val, false);
                case ETimeRange.ThisMonth:
                    min = now.AddDays(1 - now.Day);
                    max = min.Value.AddMonths(1);
                    return __ContainsCheckLeftRight(min, max, val, false);
                case ETimeRange.LastMonth:
                    max = now.AddDays(1 - now.Day);
                    min = max.Value.AddMonths(-1);
                    return __ContainsCheckLeftRight(min, max, val, false);
                case ETimeRange.ThisWeek:
                    int thisweek = (int)now.DayOfWeek;
                    if (thisweek == 0)
                    {
                        max = now.Date.AddDays(1);
                        min = max.Value.AddDays(-7);
                    }
                    else
                    {
                        min = now.Date.AddDays(1 - thisweek);
                        max = min.Value.AddDays(7);
                    }
                    return __ContainsCheckLeftRight(min, max, val, false);
                case ETimeRange.LastWeek:
                    int lastweek = (int)now.DayOfWeek;
                    if (lastweek == 0)
                    {
                        max = now.Date.AddDays(-6);
                        min = max.Value.AddDays(-7);
                    }
                    else
                    {
                        min = now.Date.AddDays(-6 - lastweek);
                        max = min.Value.AddDays(7);
                    }
                    return __ContainsCheckLeftRight(min, max, val, false);
                case ETimeRange.Today:
                    min = now.Date;
                    max = min.Value.AddDays(1);
                    return __ContainsCheckLeftRight(min, max, val, false);
                case ETimeRange.Yestoday:
                    max = now.Date;
                    min = max.Value.AddDays(-1);
                    return __ContainsCheckLeftRight(min, max, val, false);
                default:
                    return true;
            }
        }

        public bool         Contains(TimeSpan val)                            // 是否包含一个值
        {
            throw new NotImplementedException();
            
        }

        public static ETimeRange StrToETimeRange(string str)
        {
            switch (str)
            {
                case "所有":
                    return ETimeRange.All;
                case "当天":
                    return ETimeRange.Today;
                case "昨天":
                    return ETimeRange.Yestoday;
                case "本周":
                    return ETimeRange.ThisWeek;
                case "上周":
                    return ETimeRange.LastWeek;
                case "当月":
                    return ETimeRange.ThisMonth;
                case "上月":
                    return ETimeRange.LastMonth;
                case "今年":
                    return ETimeRange.ThisYear;
                case "去年":
                    return ETimeRange.LastYear;
                default:
                    return ETimeRange.All;
            }
        }

        public static string ETimeRangeToStr(ETimeRange e)
        {
            switch (e)
            {
                case ETimeRange.Today:
                    return "当天";
                case ETimeRange.Yestoday:
                    return "昨天";
                case ETimeRange.ThisWeek:
                    return "本周";
                case ETimeRange.LastWeek:
                    return "上周";
                case ETimeRange.ThisMonth:
                    return "当月";
                case ETimeRange.LastMonth:
                    return "上月";
                case ETimeRange.ThisYear:
                    return "今年";
                case ETimeRange.LastYear:
                    return "去年";
                case ETimeRange.All:
                    return "所有";
                    default:
                        return "NA";
            }
        }

        private bool        __ContainsCheckLeftRight(DateTime? min, DateTime? max, DateTime val, bool canbeEqual)
        {
            if (min != null)
            {
                if (val < min) return false;
            }
            if (max != null)
            {
                if (canbeEqual)
                {
                    if (val > max) return false;
                }
                else
                {
                    if (val >= max) return false;
                }
            }
            return true;
        }

        private bool        __ContainsCheckIntraDay(DateTime val)
        {
            if (IntradayStart != null)
            {
                if (val.TimeOfDay < ((DateTime)IntradayStart).TimeOfDay)
                    return false;
            }
            if (IntradayEnd != null)
            {
                if (val.TimeOfDay > ((DateTime)IntradayEnd).TimeOfDay)
                    return false;
            }
            return true;
        }

        private bool        __ContainsCheckWeek(DateTime val)
        {
            if (EWeek != null)
            {
                DayOfWeek dayOfWeek = val.DayOfWeek;
                int week = (int)Math.Pow(2, (int)dayOfWeek);
                if (((int)EWeek & week) != 0)
                {
                    return true;
                }
                return false;
            }
            return true;

        }


        public DateTime?    IntradayStart
        {
            get
            {
                if (HasIntradayStartTick)
                    return new DateTime(IntradayStartTick);
                else
                    return null;
            }
            set
            {
                if (value != null)
                    IntradayStartTick = value.Value.Ticks;
            }
        }
        public DateTime?    IntradayEnd
        {
            get
            {
                if (HasIntradayEndTick)
                    return new DateTime(IntradayEndTick);
                else
                    return null;
            }
            set
            {
                if (value != null)
                    IntradayEndTick = value.Value.Ticks;
            }
        }

        public DateTime?    Left
        {
            get
            {
                if (HasLeftTick)
                    return new DateTime(LeftTick);
                else
                    return null;
            }
        }
        public DateTime?    Right
        {
            get
            {
                if (HasRightTick)
                    return new DateTime(RightTick);
                else
                    return null;
            }
        }
        public TimeSpan?    Span
        {
            get
            {
                if (Type== ETimeRange.Span && HasLeftTick)
                    return new TimeSpan(LeftTick);
                else
                    return null;
            }
        }
        public EWeek?       EWeek
        {
            get
            {
                if (HasWeekDay)
                    return WeekDay;
                else
                    return null;
            }
            set
            {
                if (value != null)
                    WeekDay = value.Value;
            }
        }


        public              TimeRange(ECompareOp p, DateTime l, DateTime? r)
        {
            Type = ETimeRange.Normal;
            Op = p;
            LeftTick = l.Ticks;
            if (r != null)
                RightTick = r.Value.Ticks;
        }

        public              TimeRange(TimeSpan l)
        {
            Type = ETimeRange.Span;
            LeftTick = l.Ticks;
        }
        public              TimeRange(ETimeRange t)
        {
            Type = t;
        }
    }



}
