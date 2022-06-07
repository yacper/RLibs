/********************************************************************
    created:	2017/6/21 15:57:30
    author:		rush
    email:		
	
    purpose:	由于Color实现在System.Drawing中，提供一个简单的实现

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public enum ERColor
    {
        Red,
        Blue, 
        Black
    }

    [Serializable]
    public struct RColor
    {

        public override string ToString()
        {
            return string.Format("#{0:x8}", m_nARGB);
        }


        public static readonly RColor Empty     = new RColor();                                //     表示值为 null 的颜色。

        public static readonly RColor Transparent = new RColor(00,255,255,255);                                //     表示值为 透明颜色。
        public static readonly RColor White     = new RColor(255, 255, 255, 255); //     获取 ARGB 值为 #FFFFFFFF 的系统定义的颜色。
        public static readonly RColor Black     = new RColor(255, 0, 0, 0);  //     获取 ARGB 值为 #FF000000 的系统定义的颜色。
        public static readonly RColor Red       = new RColor(255, 255, 0, 0);  //     获取 ARGB 值为 #FF000000 的系统定义的颜色。
        public static readonly RColor Green     = new RColor(255, 0, 255, 0);  //     获取 ARGB 值为 #FF000000 的系统定义的颜色。
        public static readonly RColor Blue      = new RColor(255, 0, 0, 255);  //     获取 ARGB 值为 #FF000000 的系统定义的颜色。
        public static readonly RColor Yellow    = new RColor(255, 255, 255, 0);  //     获取 ARGB 值为 #FF000000 的系统定义的颜色。


#region 重载赋值运算符
        public static implicit operator RColor(string hexString)
        {
            return RColor.FromString(hexString);
        }

        public static implicit operator RColor(ERColor c)
        {
            return RColor.FromERColor(c);
        }

      
#endregion


        public static bool operator !=(RColor left, RColor right)
        {
            return !(left == right);
        }
        public static bool operator ==(RColor left, RColor right)
        {
            if (left.ToArgb() == right.ToArgb())
                return true;
            else
                return false;
        }



        public byte A { get { return (byte)((m_nARGB & 0xFF000000) >> 24); } }
        public byte R { get { return (byte)((m_nARGB & 0x00FF0000) >> 16); } }
        public byte G { get { return (byte)((m_nARGB & 0x0000FF00) >> 8); } }
        public byte B { get { return (byte)(m_nARGB & 0x000000FF); } }

        public override bool Equals(object obj)
        {
			if(obj is RColor)
			{
				return Equals((RColor)obj);
			}
			
            return false;
        }
        public bool         Equals(RColor other)
        {
            return (MathEx.Equal(ToArgb(), other.ToArgb()));
        }

        public static RColor FromObject(object value) //     
        {
            if (value is RColor)
            {
                return (RColor)value;
            }
            else if (value is ERColor)
            {
                return  (ERColor)value;
            }
            else if (value is string)
            {
                return (string)value;
            }

            return RColor.Empty;
        }

        public static RColor FromArgb(int argb) //     从一个 32 位 ARGB 值创建 System.Drawing.RColor 结构。
        {
            return new RColor(argb);
        }

        public static RColor FromERColor(ERColor e) //     从一个 32 位 ARGB 值创建 System.Drawing.RColor 结构。
        {
            switch (e)
            {
                case ERColor.Blue:
                    return Blue;
                case ERColor.Red:
                    return Red;
                case ERColor.Black:
                    return Black;
                default:
                    return Empty;
            }
        }


        public static RColor From(object val)
        {
            if (val is string)
                return FromString(val as string);
            else if (val is int)
                return FromArgb((int) val);
            else if(val is ERColor)
                return FromERColor((ERColor)val);
            else
                return Empty;
        }
        //
        // 摘要:
        //     从指定的 System.Drawing.RColor 结构创建 System.Drawing.RColor 结构，但要使用新指定的 alpha 值。
        //     尽管此方法允许为 alpha 值传递 32 位值，但该值仅限于 8 位。
        //
        // 参数:
        //   alpha:
        //     新 System.Drawing.RColor 的 alpha 值。 有效值为从 0 到 255。
        //
        //   baseRColor:
        //     从中创建新 System.Drawing.RColor 的 System.Drawing.RColor。
        //
        // 返回结果:
        //     此方法创建的 System.Drawing.RColor。
        //
        // 异常:
        //   System.ArgumentException:
        //     alpha 小于 0 或大于 255。
        public static RColor FromArgb(int alpha, RColor baseColor)
        {
            return FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);
        }
        //
        // 摘要:
        //     从指定的 8 位颜色值（红色、绿色和蓝色）创建 System.Drawing.RColor 结构。 alpha 值默认为 255（完全不透明）。 尽管此方法允许为每个颜色分量传递
        //     32 位值，但每个分量的值仅限于 8 位。
        //
        // 参数:
        //   red:
        //     新 System.Drawing.RColor 的红色分量值。 有效值为从 0 到 255。
        //
        //   green:
        //     新 System.Drawing.RColor 的绿色分量值。 有效值为从 0 到 255。
        //
        //   blue:
        //     新 System.Drawing.RColor 的蓝色分量值。 有效值为从 0 到 255。
        //
        // 返回结果:
        //     此方法创建的 System.Drawing.RColor。
        //
        // 异常:
        //   System.ArgumentException:
        //     red、green 或 blue 小于 0 或大于 255。
        public static RColor FromArgb(int red, int green, int blue)
        {
            return FromArgb(255, red, green, blue);
        }
        //
        // 摘要:
        //     从四个 ARGB 分量（alpha、红色、绿色和蓝色）值创建 System.Drawing.RColor 结构。 尽管此方法允许为每个分量传递 32
        //     位值，但每个分量的值仅限于 8 位。
        //
        // 参数:
        //   alpha:
        //     alpha 分量。 有效值为从 0 到 255。
        //
        //   red:
        //     红色分量。 有效值为从 0 到 255。
        //
        //   green:
        //     绿色分量。 有效值为从 0 到 255。
        //
        //   blue:
        //     蓝色分量。 有效值为从 0 到 255。
        //
        // 返回结果:
        //     此方法创建的 System.Drawing.RColor。
        //
        // 异常:
        //   System.ArgumentException:
        //     alpha、red、green 或 blue 小于 0 或大于 255。
        public static RColor FromArgb(int alpha, int red, int green, int blue)
        {
            return new RColor(alpha, red, green, blue);
        }

        public static RColor FromArgb(byte alpha, byte red, byte green, byte blue)
        {
            return new RColor(alpha, red, green, blue);
        }

        public static RColor FromString(string hexString)
        {
            try
            {
                if (hexString.StartsWith("#"))
                    hexString = hexString.Substring(1, hexString.Length - 1);

                int a = Int32.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int r = Int32.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int g = Int32.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                int b = Int32.Parse(hexString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                return new RColor(a, r, g, b);
            }
            catch (Exception )
            {
                RLibBase.Logger.Error("无法解析RColor:" + hexString);
                return RColor.Empty;
            }
        }

        //
        // 摘要:
        //     获取此 System.Drawing.RColor 结构的“色调-饱和度-亮度”(HSB) 的亮度值。
        //
        // 返回结果:
        //     此 System.Drawing.RColor 的亮度。 亮度范围从 0.0 到 1.0，其中 0.0 表示黑色，1.0 表示白色。
        public float GetBrightness() { throw new NotImplementedException();}
        //
        // 摘要:
        //     返回此 System.Drawing.RColor 结构的哈希代码。
        //
        // 返回结果:
        //     一个整数值，用于指定此 System.Drawing.RColor 的哈希代码。
       // public override int GetHashCode();
        //
        // 摘要:
        //     获取此 System.Drawing.RColor 结构的“色调-饱和度-亮度”(HSB) 的色调值，以度为单位。
        //
        // 返回结果:
        //     此 System.Drawing.RColor 的色调，以度为单位。 在 HSB 颜色空间内，色调以度来测量，范围从 0.0 到 360.0。
        public float GetHue() { throw new NotImplementedException();}
        //
        // 摘要:
        //     获取此 System.Drawing.RColor 结构的“色调-饱和度-亮度”(HSB) 的饱和度值。
        //
        // 返回结果:
        //     此 System.Drawing.RColor 的饱和度。 饱和度的范围从 0.0 到 1.0，其中 0.0 为灰度，1.0 表示最饱和。
        public float GetSaturation() { throw new NotImplementedException();}
        //
        // 摘要:
        //     获取此 System.Drawing.RColor 结构的 32 位 ARGB 值。
        //
        // 返回结果:
        //     此 System.Drawing.RColor 的 32 位 ARGB 值。
        public int          ToArgb() { return m_nARGB; }

#region C&D
        public RColor(int a, int r, int g, int b)
        {
            System.Diagnostics.Debug.Assert(
                MathEx.IsBetween(a, 0, 255) &&
                MathEx.IsBetween(r, 0, 255) &&
                MathEx.IsBetween(g, 0, 255) &&
                MathEx.IsBetween(b, 0, 255) 
                );

            this = FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        }

        public RColor(byte a, byte r, byte g, byte b)
        {
            m_nARGB = a;
            m_nARGB = (m_nARGB << 8) | r;
            m_nARGB = (m_nARGB << 8) | g;
            m_nARGB = (m_nARGB << 8) | b;
        }

        public RColor(int argb)
        {
            m_nARGB = argb;
        }
        public RColor(uint argb)
        {
            m_nARGB = (int)argb;
        }

        // #FFFF0000
        public              RColor(string hexString)
        {
            this = FromString(hexString);
        }
      
#endregion

#region Members
        private int m_nARGB;
#endregion
    }
}
