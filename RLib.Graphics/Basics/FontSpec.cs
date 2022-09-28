﻿// created: 2022/07/27 11:17
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using Microsoft.Maui.Graphics;
using RLib.Graphics.Helpers;

namespace RLib.Graphics;

public class FontSpec
{
    public override string ToString()
    {
        var ret = $"Size:{Size} ";
        if(Color!= null)
            ret += $"Color:{Color?.ToArgbHex()} ";
        if(Font!=null)
            ret += $"Font:{Font.ToString()} ";
        return ret;
    }


    public Color Color { get; set; } = Colors.Black;
    public float Size { get; set; } = 12;
    public IFont Font { get; set; } = Microsoft.Maui.Graphics.Font.Default;

    public FontSpec WithColor(Color c)
    {
        return new FontSpec()
        {
            Color = c,
            Size  = Size,
            Font  = Font
        };
    }
    public FontSpec WithSize(float size)
    {
        return new FontSpec()
        {
            Color = Color,
            Size  = size,
            Font  = Font
        };
    }
    public FontSpec WithFont(IFont font)
    {
        return new FontSpec()
        {
            Color = Color,
            Size  = Size,
            Font  =font 
        };
    }


    public override int GetHashCode()
    {
        unchecked
        {
            int ret = 0;
                ret ^= Color.GetHashCode();
                ret ^= Size.GetHashCode();
                ret ^= Font.GetHashCode();

            return ret;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is FontSpec other)
            return Color.Equals(other.Color)
                && Font.Equals(other.Font)
                && Size.NearlyEqual(other.Size)
                ;

        return base.Equals(obj);
    }
}

public static class FontSpecEx
{
    public static void Apply(this ICanvas canvas, FontSpec font)
    { 
        if (font == null)
            return;

        canvas.Font      = font.Font;
        canvas.FontColor = font.Color;
        canvas.FontSize  = font.Size;

        canvas.Antialias = true;        // default open
    }
}
