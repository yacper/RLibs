/********************************************************************
    created:	2019/8/19 20:40:21
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class ColorEx 
    {
        public static Color ToColor(this string hexString)
        {
            try
            {
                if (hexString.StartsWith("#"))
                    hexString = hexString.Substring(1, hexString.Length - 1);

                int a = Int32.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int r = Int32.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int g = Int32.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                int b = Int32.Parse(hexString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
            catch (Exception)
            {
                Debug.WriteLine($"Can't resolve color from {hexString}");

                return Color.Empty;
            }
        }

        public static string ToHex(this Color c)                       // tostring
        {
            return $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
        }
    }
}
