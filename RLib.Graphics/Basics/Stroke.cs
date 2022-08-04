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
    public Color    Color       { get; set; } = Colors.Black;
    public float    Size        { get; set; } = 1;
    public LineJoin LineJoin    { get; set; } = LineJoin.Miter;
    public float    MiterLimit        { get; set; } = float.NaN;
    public LineCap  LineCap     { get; set; } = LineCap.Butt;
    public float[]  DashPattern { get; set; } = { };
    public float    DashOffset  { get; set; } = float.NaN;

    public Stroke WithColor(Color c)
    {
        var s = MemberwiseClone() as Stroke;
        s.Color = c;
        return s;
    }



    public override int GetHashCode()
    {
        unchecked
        {
            int hashcode = Color.GetHashCode();
            hashcode = hashcode  ^ Size.GetHashCode();
            hashcode = hashcode  ^ LineJoin.GetHashCode();
            hashcode = hashcode  ^ MiterLimit.GetHashCode();
            hashcode = hashcode  ^ LineCap.GetHashCode();
            hashcode = hashcode  ^ DashPattern.GetHashCode();
            hashcode = hashcode  ^ DashOffset.GetHashCode();
            return hashcode;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Stroke other)
            return Color.Equals(other.Color)
                && Size.NearlyEqual(other.Size) 
                && LineJoin == other.LineJoin
                && MiterLimit.NearlyEqual(other.MiterLimit)
                && LineCap == other.LineCap
                && DashPattern == other.DashPattern
                && DashOffset.NearlyEqual(other.DashOffset)
                ;


        return base.Equals(obj);
    }

}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class StrokeAttribute : Attribute // 输出
{
    public Color    Color       { get; set; } = Colors.Black;
    public float    Size        { get; set; } = 1;
    public LineJoin LineJoin    { get; set; } = LineJoin.Miter;
    public float    MiterLimit        { get; set; } = float.NaN;
    public LineCap  LineCap     { get; set; } = LineCap.Butt;
    public float[]  DashPattern { get; set; } = { };
    public float    DashOffset  { get; set; } = float.NaN;

    public StrokeAttribute(string color)
    {
        Color = Color.FromArgb(color);
    }
}



public static class StrokeEx
{
    public static void Apply(this ICanvas canvas, Stroke stroke)
    {
        if (stroke == null)
            return;

        canvas.StrokeColor       = stroke.Color;
        canvas.StrokeSize        = stroke.Size;
        canvas.StrokeLineJoin    = stroke.LineJoin;
        canvas.MiterLimit        = stroke.MiterLimit;
        canvas.StrokeLineCap     = stroke.LineCap;
        canvas.StrokeDashPattern = stroke.DashPattern;
        canvas.StrokeDashOffset  = stroke.DashOffset;
    }
}

