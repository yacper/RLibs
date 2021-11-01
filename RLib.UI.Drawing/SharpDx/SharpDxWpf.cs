/********************************************************************
    created:	2020/2/19 13:20:26
    author:		rush
    email:		
	
    purpose:	SharpDX与Wpf直接的一些互转
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using RLib.Base;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using FontWeight = SharpDX.DirectWrite.FontWeight;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace RLib.UI.Drawing
{
    public static class SharpDxWpf
    {
        public static RawColor4?    ToRawColor4(this SolidColorBrush brush)
        {
            if (brush != null)
                return brush.Color.ToRawColor4();
            else
                return null;
        }

        public static RawColor4    ToRawColor4(this Color c)
        {
            return new RawColor4(c.R/(float)255, c.G/(float)255, c.B/(float)255, c.A/(float)255);
        }
       
        public static RawVector2    ToRawVector2(this Point c)
        {
            return new RawVector2((float)c.X, (float)c.Y);
        }

        public static RawRectangleF    ToRawRectangleF(this Rect c)
        {
            return new RawRectangleF((float)c.Left, (float)c.Top, (float)c.Right, (float)c.Bottom);
        }

        public static RawRectangleF?    ToRawRectangleF(this Rect? c)
        {
            if (c == null)
                return null;

            return new RawRectangleF((float)c.Value.Left, (float)c.Value.Top, (float)c.Value.Right, (float)c.Value.Bottom);
        }

        public static bool      Equal(this RawColor4 a, RawColor4 b)
        {
            if (MathEx.Equal(a.A, b.A) &&
                MathEx.Equal(a.R, b.R) &&
                MathEx.Equal(a.G, b.G) &&
                MathEx.Equal(a.B, b.B) 
            )
                return true;
            else
                return false;

        }

        public static Size  GetTextSize(string text,  string familyName,  float size, FontWeight fontWeight = FontWeight.Normal,  FontStyle fontStyle = FontStyle.Normal) // 获取Text绘制所需的Size
        {
            TextFormat tf = new TextFormat(FactoryDWrite, familyName, fontWeight, fontStyle, size);
            TextLayout tl = new TextLayout(FactoryDWrite, text, tf, 1000, 1000);
            TextMetrics tm = tl.Metrics;

            tl.Dispose();
            tf.Dispose();

            return new Size(tm.Width, tm.Height);
        }

        public static SharpDX.DirectWrite.Factory FactoryDWrite = new SharpDX.DirectWrite.Factory();

    }
}
