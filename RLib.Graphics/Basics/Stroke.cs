// created: 2022/07/26 14:54
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using Microsoft.Maui.Graphics;

namespace RLib.Graphics;

public class Stroke
{
    public Color    StrokeColor       { get; set; } = Colors.Black;
    public float    StrokeSize        { get; set; } = 1;
    public LineJoin StrokeLineJoin    { get; set; } = LineJoin.Miter;
    public float    MiterLimit        { get; set; } = float.NaN;
    public LineCap  StrokeLineCap     { get; set; } = LineCap.Butt;
    public float[]  StrokeDashPattern { get; set; } = { };
    public float    StrokeDashOffset  { get; set; } = float.NaN;
}

public static class StrokeEx
{
    public static void Apply(this ICanvas canvas, Stroke stroke)
    {
        canvas.StrokeColor       = stroke.StrokeColor;
        canvas.StrokeSize        = stroke.StrokeSize;
        canvas.StrokeLineJoin    = stroke.StrokeLineJoin;
        canvas.MiterLimit        = stroke.MiterLimit;
        canvas.StrokeLineCap     = stroke.StrokeLineCap;
        canvas.StrokeDashPattern = stroke.StrokeDashPattern;
        canvas.StrokeDashOffset  = stroke.StrokeDashOffset;
    }
}

