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
    public static System.Windows.Media.Color ToColor(this Color c) => System.Windows.Media.Color.FromArgb((byte)(c.Alpha*255), (byte)(c.Red*255), (byte)(c.Green*255), (byte)(c.Blue*255));

    public static Point ToPoint(this System.Windows.Point p) => new Point(p.X, p.Y);

    public static Point CenterPoint(this Rect r) { return new Point(r.Left + r.Width / 2, r.Top + r.Height / 2); }

    public static Size Ceiling(this Size value) { return new Size(Math.Ceiling(value.Width), Math.Ceiling(value.Height)); }
    public static Size Floor(this Size value) { return new Size(Math.Floor(value.Width), Math.Floor(value.Height)); }
  
}