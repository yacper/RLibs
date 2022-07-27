// created: 2022/07/26 14:55
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using Microsoft.Maui.Graphics;
using RLib.Graphics.Helpers;

namespace RLib.Graphics;

public class Shadow
{
    public SizeF Offset { get; set; }
    public float Blur   { get; set; }
    public Color Color  { get; set; }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashcode = Color.GetHashCode() ^ Offset.GetHashCode() ^ Blur.GetHashCode();
            return hashcode;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Shadow other)
            return Color.Equals(other.Color)
                && Offset.Equals(other.Offset)
                && Blur.NearlyEqual(other.Blur)
                ;

        return base.Equals(obj);
    }

  
}

public static class ShadowEx
{
    public static void Apply(this ICanvas canvas, Shadow shadow)
    {
        if(shadow != null)
            canvas.SetShadow(shadow.Offset, shadow.Blur, shadow.Color);
    }
}


