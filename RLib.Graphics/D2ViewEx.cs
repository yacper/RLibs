// created: 2022/09/30 16:01
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using Microsoft.Maui.Graphics;
using RLib.Graphics.Shape;

namespace RLib.Graphics;

public static class D2ViewEx
{
    public static void DrawArrow(this ID2View view, Rect rect, EDirection dir, Stroke stroke, Fill fill = null, WindingMode windingMode = WindingMode.NonZero, Rect? clip = null, int zindex = 0)
    {
        Point       center = rect.Center;
        PathF path = new PathF();
        switch (dir)
        {
            case EDirection.Up:
                {
                    path.MoveTo(new Point(center.X, rect.Top));
                    path.LineTo(new Point(rect.Right, rect.Top + rect.Height / 2));
                    path.LineTo(new Point(rect.Right - rect.Width / 4, rect.Top + rect.Height / 2));
                    path.LineTo(new Point(rect.Right - rect.Width / 4, rect.Bottom));
                    path.LineTo(new Point(rect.Left + rect.Width / 4, rect.Bottom));
                    path.LineTo(new Point(rect.Left + rect.Width / 4, rect.Top + rect.Height / 2));
                    path.LineTo(new Point(rect.Left, rect.Top + rect.Height / 2));
                    path.LineTo(new Point(rect.Center.X, rect.Top));
                }
                break;
            case EDirection.Right:
                {
                }
                break;
            case EDirection.Down:
                {
                    path.MoveTo(new Point(center.X, rect.Bottom));
                    path.LineTo(new Point(rect.Right, rect.Top + rect.Height / 2));
                    path.LineTo(new Point(rect.Right - rect.Width / 4, rect.Top + rect.Height / 2));
                    path.LineTo(new Point(rect.Right - rect.Width / 4, rect.Top));
                    path.LineTo(new Point(rect.Left + rect.Width / 4, rect.Top));
                    path.LineTo(new Point(rect.Left + rect.Width / 4, rect.Top + rect.Height / 2));
                    path.LineTo(new Point(rect.Left, rect.Top + rect.Height / 2));
                }
                break;
            case EDirection.Left:
                {
                }
                break;
        }

        view.DrawPath(path, stroke, fill, WindingMode.NonZero, clip, zindex);
    }


}