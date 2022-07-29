// created: 2022/07/27 11:33
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using Microsoft.Maui.Graphics;

namespace RLib.Graphics.Helpers;

public static class Helpers
{
    internal static bool NearlyEqual(this float f1, float f2, float epsilon = 0.01f)
        => Math.Abs(f1 - f2) < epsilon;

    internal static bool NearlyEqual(this double f1, double f2, float epsilon = 0.00001f)
        => Math.Abs(f1 - f2) < epsilon;



    public static Point TopLeft(this Rect r) { return r.Location; }
    public static Point TopRight(this Rect r) { return new Point(r.Right, r.Top); }
    public static Point TopCenter(this Rect r) { return new Point(r.Left+ r.Width/2, r.Top); }
    public static Point BottomLeft(this Rect r) { return new Point(r.Left, r.Bottom); }
    public static Point BottomRight(this Rect r) { return new Point(r.Right, r.Bottom); }
    public static Point BottomCenter(this Rect r) { return new Point(r.Left+r.Width/2, r.Bottom); }

}