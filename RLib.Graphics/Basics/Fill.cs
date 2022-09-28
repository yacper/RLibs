// created: 2022/07/26 14:54
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

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

    public Color? Color { get; set; } = null;
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
            return Color.Equals(other.Color)
                && Paint.Equals(other.Paint)
                ;

        return base.Equals(obj);
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class FillAttribute : Attribute // 输出
{
    public Color    Color       { get; set; } = Colors.Black;
    public Paint Paint { get; set; } = null;
   
    public FillAttribute(string color)
    {
        Color = Color.FromArgb(color);
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


