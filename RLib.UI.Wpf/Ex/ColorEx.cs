/********************************************************************
    created:	2020/3/4 16:55:09
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using RLib.Base;

namespace RLib.Base
{
    public static class ColorEx
    {

	    public static Color Parse(object c)
	    {
		    if (c is Color) return (Color)c ;
            else if (c is string) return FromString(c as string);
            else if (c is int) return FromInt32((int)c );

		    throw new NotImplementedException();
	    }
        public static Color FromString(string str)                          // #AARRGGBB/#RRGGBB/Blue
        {
            try
            {
                if (!str.StartsWith("#")) // 根据string name解析
                {
                    Color color = (Color)ColorConverter.ConvertFromString(str);

                    return color;
                }

                /// 否则认为是hexString
                string hexString = str.Substring(1, str.Length - 1);

                int index = 0;
                byte a = 255;
                if (hexString.Length == 8)
                {
                    a = byte.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    index = 2;
                }

                byte r = byte.Parse(hexString.Substring(0 + index, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hexString.Substring(2 +index, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hexString.Substring(4 +index , 2), System.Globalization.NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
            catch (Exception e)
            {
                RLibBase.Logger.Error("无法解析Color:" + str);
                return Colors.Black;
            }

        }

        public static Color FromInt32(int c) // #AARRGGBB/#RRGGBB
        {
            byte A = (byte)((c & 0xFF000000) >> 24);

            byte R = (byte)((c & 0x00FF0000) >> 16);
            byte G = (byte)((c & 0x0000FF00) >> 8);
            byte B = (byte)(c & 0x000000FF);

            return Color.FromArgb(A, R, G, B);
        }

        public static int   ToARGB(this Color c) // #AARRGGBB/#RRGGBB
        {
            int argb = c.A;
            argb = (argb << 8) | c.R;
            argb = (argb << 8) | c.G;
            argb = (argb << 8) | c.B;

            return argb;
        }

        public static Color ToColor(this string hexString) // #AARRGGBB/#RRGGBB
        {
            return FromString(hexString);
        }

    }
}
