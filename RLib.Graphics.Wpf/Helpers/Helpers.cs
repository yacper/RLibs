// created: 2022/07/27 11:33
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using Microsoft.Maui.Graphics;

namespace RLib.Graphics.Wpf;

public static class Helpers
{
    public static Color ToColor(this System.Windows.Media.Color c) => Color.FromRgba(c.R, c.G, c.B, c.A);
    public static System.Windows.Media.Color ToColor(this Color c) => System.Windows.Media.Color.FromScRgb(c.Alpha, c.Red, c.Green, c.Blue);
    public static Point ToPoint(this System.Windows.Point p) => new Point(p.X, p.Y);

    public static Point CenterPoint(this Rect r) { return new Point(r.Left + r.Width / 2, r.Top + r.Height / 2); }

}