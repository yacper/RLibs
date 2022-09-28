// created: 2022/07/26 14:54
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace RLib.Graphics;

public class Stroke
{
    public override string ToString() => $"{Color.ToArgbHex()} {Size} LineJoin:{LineJoin} MiterLimit:{MiterLimit} LineCap:{LineCap} DashPattern:{DashPattern} DashOffset:{DashOffset}";

    public Color    Color       { get; set; } = Colors.Black;
    public float    Size        { get; set; } = 1;
    public LineJoin LineJoin    { get; set; } = LineJoin.Miter;
    public float    MiterLimit  { get; set; } = float.NaN;
    public LineCap  LineCap     { get; set; } = LineCap.Butt;
    public float[]  DashPattern { get; set; } = { };
    public float    DashOffset  { get; set; } = float.NaN;

    public Stroke WithColor(Color c)
    {
        var s = MemberwiseClone() as Stroke;
        s.Color = c;
        return s;
    }

    public Stroke WithSize(float sz)
    {
        var s = MemberwiseClone() as Stroke;
        s.Size = sz;
        return s;
    }


    public override int GetHashCode()
    {
        unchecked
        {
            int hashcode = Color.GetHashCode();
            hashcode = hashcode ^ Size.GetHashCode();
            hashcode = hashcode ^ LineJoin.GetHashCode();
            hashcode = hashcode ^ MiterLimit.GetHashCode();
            hashcode = hashcode ^ LineCap.GetHashCode();
            hashcode = hashcode ^ DashPattern.GetHashCode();
            hashcode = hashcode ^ DashOffset.GetHashCode();
            return hashcode;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Stroke other)
            return Color.Equals(other.Color)
                && Helpers.Helpers.NearlyEqual(Size, other.Size)
                && LineJoin == other.LineJoin
                && Helpers.Helpers.NearlyEqual(MiterLimit, other.MiterLimit)
                && LineCap == other.LineCap
                && DashPattern == other.DashPattern
                && Helpers.Helpers.NearlyEqual(DashOffset, other.DashOffset)
                ;


        return base.Equals(obj);
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class StrokeAttribute : DefaultValueAttribute // 输出
{
    public Color    Color       { get; set; } = Colors.Black;
    public float    Size        { get; set; } = 1;
    public LineJoin LineJoin    { get; set; } = LineJoin.Miter;
    public float    MiterLimit  { get; set; } = float.NaN;
    public LineCap  LineCap     { get; set; } = LineCap.Butt;
    public float[]  DashPattern { get; set; } = { };
    public float    DashOffset  { get; set; } = float.NaN;

    public Stroke Stroke => new Stroke()
    {
        Color   = Color, Size          = Size, LineJoin          = LineJoin, MiterLimit = MiterLimit,
        LineCap = LineCap, DashPattern = DashPattern, DashOffset = DashOffset
    };

    public StrokeAttribute(string? color)
        :base(null)
    {
        if(color != null)
            Color=Color.Parse(color);

        SetValue(Stroke);
    }

    //public StrokeAttribute()
    //{

    //}

//    public StrokeAttribute(string color) { Color = Color.FromArgb(color); }
 //   public StrokeAttribute() {}
}
//public class StrokeConverter : TypeConverter
//{
//    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { return sourceType == typeof(string); }

//    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
//    {
//        var name = value as string;
//        if (name != null)
//        {
//            var v = NeoAppCommon.Instance.Container.Resolve<IDatas>() as Datas;
//            v.Id = name;

//            return v;
//        }
//        else
//            return base.ConvertFrom(context, culture, value);
//    }
//}


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

    public static void Apply(this Stroke stroke, StrokeAttribute attr)
    {
        if (stroke == null || attr == null)
            return;

        stroke.Color       = attr.Color;
        stroke.Size        = attr.Size;
        stroke.LineJoin    = attr.LineJoin;
        stroke.MiterLimit  = attr.MiterLimit;
        stroke.LineCap     = attr.LineCap;
        stroke.DashPattern = attr.DashPattern;
        stroke.DashOffset  = attr.DashOffset;
    }
}