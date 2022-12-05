// created: 2022/07/26 14:54
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace RLib.Graphics;

public class Fill
{
    public override string ToString()
    {
        var ret = "";
        if(Color!= null)
            ret = $"Color:{Color?.ToArgbHex()} ";
        if(Paint!=null)
            ret += $"Paint:Foreground.{Paint?.ForegroundColor} Background.{Paint?.BackgroundColor} IsTransparent.{Paint?.IsTransparent} ";
        return ret;
    }

    [TypeConverter(typeof(MediaColorToGraphicsColorTypeConverter))]
    public Color Color { get; set; } = null;
    public Paint Paint { get; set; } = null;

    public override int GetHashCode()
    {
        unchecked
        {
            int ret = 0;
            if (Color != null)
                ret ^= Color.GetHashCode();
            if(Paint != null)
                ret ^= Paint.GetHashCode();

            return ret;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Fill other)
            return  GetHashCode() == obj.GetHashCode();

        return base.Equals(obj);
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class FillAttribute : DefaultValueAttribute // 输出
{
    public Color    Color       { get; set; } = null;

#region Paint
    public Color PaintBackgroundColor { get; set; } = null;
    public Color PaintForegroundColor { get; set; } = null;

    public Color SolidPaintColor { get; set; } = null;
    
    //public Point LinearGradientPaintStartPoint { get; set; }
    //public Point LinearGradientPaintEndPoint { get; set; }

#endregion
    public Paint Paint
    {
        get
        {
            if (SolidPaintColor != null)
                return new SolidPaint(SolidPaintColor){ForegroundColor = PaintForegroundColor, BackgroundColor = PaintBackgroundColor};

            return null;
        }
    }

    public Fill Fill => new Fill() { Color = Color, Paint = Paint };
   

    public override object? Value => Fill;

    public FillAttribute(string color)
        :base(null)
    {
        if (color != null)
            Color = Color.Parse(color);

    }
}


public static class FillEx
{
    public static void Apply(this ICanvas canvas, Fill fill, Rect? rect = null)
    { 
        if (fill == null)
            return;

        canvas.FillColor = fill.Color;
        if(fill.Paint != null && rect != null)
            canvas.SetFillPaint(fill.Paint, rect.Value);
    }

    public static void Apply(this Fill fill, FillAttribute attr)
    {
        if (fill == null || attr == null)
            return;

        fill.Color = attr.Color;
        fill.Paint = attr.Paint;
    }


}


