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
    public Color Color { get; set; } = Colors.Black;
    public Paint Paint { get; set; } = null;
}

public static class FillEx
{
    public static void Apply(this ICanvas canvas, Fill fill, Rect? rect = null)
    {
        canvas.FillColor = fill.Color;
        if(fill.Paint != null && rect != null)
            canvas.SetFillPaint(fill.Paint, rect.Value);
    }
}


