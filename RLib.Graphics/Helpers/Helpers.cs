// created: 2022/07/27 11:33
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

namespace RLib.Graphics.Helpers;

internal static class Helpers
{
    internal static bool NearlyEqual(this float f1, float f2, float epsilon = 0.01f)
        => Math.Abs(f1 - f2) < epsilon;
}