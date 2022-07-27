// created: 2022/07/26 14:54
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using Microsoft.Maui.Graphics;

using RLib.Graphics.Helpers;
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

    public override int GetHashCode()
    {
        unchecked
        {
            int hashcode = StrokeColor.GetHashCode();
            hashcode = hashcode  ^ StrokeSize.GetHashCode();
            hashcode = hashcode  ^ StrokeLineJoin.GetHashCode();
            hashcode = hashcode  ^ MiterLimit.GetHashCode();
            hashcode = hashcode  ^ StrokeLineCap.GetHashCode();
            hashcode = hashcode  ^ StrokeDashPattern.GetHashCode();
            hashcode = hashcode  ^ StrokeDashOffset.GetHashCode();
            return hashcode;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Stroke other)
            return StrokeColor.Equals(other.StrokeColor)
                && StrokeSize.NearlyEqual(other.StrokeSize) 
                && StrokeLineJoin == other.StrokeLineJoin
                && MiterLimit.NearlyEqual(other.MiterLimit)
                && StrokeLineCap == other.StrokeLineCap
                && StrokeDashPattern == other.StrokeDashPattern
                && StrokeDashOffset.NearlyEqual(other.StrokeDashOffset)
                ;


        return base.Equals(obj);
    }

}

public static class StrokeEx
{
    public static void Apply(this ICanvas canvas, Stroke stroke)
    {
        if (stroke == null)
            return;

        canvas.StrokeColor       = stroke.StrokeColor;
        canvas.StrokeSize        = stroke.StrokeSize;
        canvas.StrokeLineJoin    = stroke.StrokeLineJoin;
        canvas.MiterLimit        = stroke.MiterLimit;
        canvas.StrokeLineCap     = stroke.StrokeLineCap;
        canvas.StrokeDashPattern = stroke.StrokeDashPattern;
        canvas.StrokeDashOffset  = stroke.StrokeDashOffset;
    }
}

