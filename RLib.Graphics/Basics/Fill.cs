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
}


