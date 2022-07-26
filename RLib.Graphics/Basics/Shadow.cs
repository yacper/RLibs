// created: 2022/07/26 14:55
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using Microsoft.Maui.Graphics;

namespace RLib.Graphics;

public class Shadow
{
    public SizeF Offset { get; set; }
    public float Blur   { get; set; }
    public Color Color  { get; set; }
}

public static class ShadowEx
{
    public static void Apply(this ICanvas canvas, Shadow shadow)
    {
        canvas.SetShadow(shadow.Offset, shadow.Blur, shadow.Color);
    }
}


