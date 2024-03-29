﻿/********************************************************************
    created:	2020/2/16 1:40:47
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class DoubleEx
    {
        public const string NullText = "null";
        public const string NanText = "nan";

        public static double Round(this double val, int scale = 2)
        {
            return Math.Round(val, scale);
        }


        public static bool NearlyEqual(this float f1, float f2, float epsilon = 0.00001f)
        => Math.Abs(f1 - f2) < epsilon;

        public static bool NearlyEqual(this double f1, double f2, double epsilon = 0.000001d)
        => Math.Abs(f1 - f2) < epsilon;

        public static int ToIntOrDefault(this double val, int? defaultValue = 0)
        {
            if (val.IsNullOrNan())
                return defaultValue.GetValueOrDefault();
            else
                return Convert.ToInt32(val);
        }

        public static string ToPercentString(this double val)
        {
            return (Math.Round((double) val * 100d, 2)).ToString() + "%";
        }

        public static object DoubleInit(this object o, double def = double.NaN)            // 对class 下的double property设置default
        {
            foreach (PropertyInfo info in o.GetType().GetProperties())
            {
                // !必须同时有get和set，否则不序列化
                if(info.GetGetMethod() == null || info.GetSetMethod() == null)
                    continue;

                if (info.PropertyType.IsDouble())
                {
                    info.SetValue(o, def);
                }
            }

            return o;
        }


        public static string ToString(this double val, int? precision = 2, int? cover = null, bool asRatio = false, bool symbol = false)
        {
            if (double.IsNaN(val))
                return NanText;

            string ret = null;
            double v = val;

            if (asRatio)
                val *= 100d;

            if (precision != null)
                v = Math.Round(val, precision.Value);

            if (cover != null && cover.Value > 0)
                ret = v.Cover(cover.Value);
            else
            {
                ret = v.ToString();
                if (v > (double)decimal.MinValue && v < (double)decimal.MaxValue)
                    ret = ((decimal)v).ToString();
            }

            if (asRatio)
                ret += "%";

            if (symbol)
            {
                if (ret.IndexOf("+") == -1 && ret.IndexOf("-") == -1)
                {
                    if (val >= 0)
                        ret = $"+{ret}";
                }
                else
                    ret = $" -{ret.Substring(1, ret.Length - 1)}";  
            }
                

            return ret;
        }

        public static bool SetIfChanged(this ref double l, double r)
        {
            if (MathEx.Equal(l, r))
                return false;
            l = r;
            return true;
        }
        public static bool SetIfChanged(this ref double? l, double r)
        {
            if (MathEx.Equal(l, r))
                return false;
            l = r;
            return true;
        }

        public static string Cover(this double val, int cover = 2)      // 补位， 16.2 -> 16.20
        {
            var mask = "{" + $"0:F{cover}" + "}";
            string ret = string.Format($"{mask}", val);

            int index = ret.IndexOf('.');
            if (index < 0)      // 没有小数点，直接补全返回
            {
                ret += ".";
                for (int i = 0; i < cover; i++)
                    ret += "0";
                return ret;
            }
            
            int digits = index > 0 ? ret.Length - 1 - index : ret.Length;
            if (digits == ret.Length)
            {
                ret    += ".";
                digits =  0;
            }
            else if (digits >= cover)
            {
                return ret;
            }

            digits = cover - digits;

            for (int i = 0; i != digits; ++i)
                ret += "0";

            return ret;
        }


        public static string ToSimple(this double v, int? precicion = null)
		{
		    bool negtive = false;
		    if (v < 0)
		    {
		        v = Math.Abs(v);
		        negtive = true;
		    }

		    string ret;

            var num = 1000;

		    if (v < num)
		        ret = v.ToString(precicion);
		    else if (v < Math.Pow(num, 2))  // 小于m的
		    {
		        double v2 = v / num;
		        if (MathEx.IsIntegear(v2))           // 到k
		            ret= v2 + "K";
                else if (precicion != null)
                {
                    ret = (v / num).ToString(precicion.Value) + "K";
                }
                else
                    ret = v.ToString(precicion);
		    }
		    else if (v < Math.Pow(num, 3))// 超过m的
		    {
		        double v2 = v / (num*num);
		        if (MathEx.IsIntegear(v2))
		            ret= v2 + "M";
                else if (precicion != null)
                {
                    ret = (v / (num*num)).ToString(precicion.Value) + "M";
                }
                else
                    ret = v.ToString(precicion);
		    }
            else// 超亿
            {
		        double v2 = v / (num*num * 100);
		        if (MathEx.IsIntegear(v2))
		            ret= v2 + "亿";
                else if (precicion != null)
                {
                    ret = (v / (num*num * 100)).ToString(precicion.Value) + "亿";
                }
                else
                    ret = v.ToString(precicion);
            }

		    if (negtive)
		        return "-" + ret;
		    else
		        return ret;
		}

        public static bool IsNullOrNan(this double? value)
        {
            return value == null ? true : double.IsNaN(value.Value);
        }
        public static bool IsNullOrNan(this double value)
        {
            return  double.IsNaN(value);
        }
        public static bool IsNullOrNanOrZero(this double? value)
        {
            if (value == null)
                return true;
            if (double.IsNaN(value.Value))
                return true;
            return value.Value.NearlyEqual(0);
        }
        public static bool IsNullOrNanOrZero(this double value)
        {
            if (double.IsNaN(value))
                return true;
            return value.NearlyEqual(0);
        }

        public static double GetValueOrNan(this double? value)
        {
            if (value != null)
                return value.Value;
            else
                return double.NaN;
        }

        public static double? ReplaceZeroToNull(this double? value)
        {
            double? nullable = value;
            double num = 0.0;
            if ((nullable.GetValueOrDefault() == num ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                return value;
            return new double?();
        }

        public static double? ToNullIfNaNOrInfinity(this double? value)
        {
            if (!value.HasValue || double.IsNaN(value.Value) || double.IsInfinity(value.Value))
                return new double?();
            return value;
        }

        public static void RaiseErrorIfNaNOrInfinity(this double targetPrice)
        {
            if (double.IsNaN(targetPrice) || double.IsInfinity(targetPrice))
                throw new ArgumentException(targetPrice.ToString() + " is not valid value for target price");
        }



#region Precesion & Scale
        public static int   Precision(this double value, bool withEndingZeros = true, bool withStartingZeros = false)
        {
            return GetLeftNumberOfDigits(value, withStartingZeros) + GetRightNumberOfDigits(value, withEndingZeros);
        }
        public static int   Scale(this double value, bool withEndingZeros = true)
        {
            return GetRightNumberOfDigits(value, withEndingZeros);
        }

        /// <summary>
        /// Number of digits to the right of the decimal point without ending zeros
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetRightNumberOfDigits(this double value, bool withEndingZeros = true)
        {
            var text = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (!withEndingZeros)
                text = text.TrimEnd('0');

            var decpoint = text.IndexOf(System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
            if (decpoint < 0)
                return 0;

            return text.Length - decpoint - 1;
        }
        /// <summary>
        /// Number of digits to the left of the decimal point without starting zeros
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetLeftNumberOfDigits(this double value, bool withStartingZeros = false)
        {
            var text = Math.Abs(value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (!withStartingZeros)
                text = text.TrimStart('0');

            var decpoint = text.IndexOf(System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
            if (decpoint == -1)
                return text.Length;

            return decpoint;
        }
#endregion


    }
}
