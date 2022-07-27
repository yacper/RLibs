/********************************************************************
    created:	2020/4/1 19:02:13
    author:		rush
    email:		
	
    purpose:	2d形状 主要用于碰撞测试
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Maui.Graphics;

namespace RLib.Graphics.Shape
{
    public enum EDirection
    {
        Left,
        Up,
        Right,
        Down,
    }


    public interface IShape
    {
        bool                IsHitBoundry(Point x, double threshold = 3);    // 边缘碰撞检测

        Rect                BoundingBox { get; }                            // 获取总的BoundingBox
    }

    public interface ISealingShape : IShape                                 // 封闭式图形
    {
        bool                Contains(Point pt);                             // 是否在图形内部
    }


    public interface ILine : IShape
    {
        Point               Start { get; set; }
        Point               End { get; set; }

        bool                IsHitBoundry(Point pt, double threshold = 3, bool extendLeft = false, bool extendRight = false);

        bool                IsVerticle { get; }                             // 是否是垂直线
        bool                IsHorizontal { get; }                           // 是否是横线
    }



    public interface IEllipse:ISealingShape                                 // 正椭圆， 不表示旋转
    {
        double              RadiusX { get; set; }                                 // 半径
        double              RadiusY { get; set; }                                 // 半径
        Point               Center { get; set; }                                 // 中心点

        bool                IsCircle { get; }
    }

    //public interface ITriangle : ISealingShape                                      //三角形
    //{
    //    Point               X { get; set; }
    //    Point               Y { get; set; }
    //    Point               Z { get; set; }
    //}

    public interface IRectangle : ISealingShape                             // 长方形
    {
        Rect                Rect { get; set; }
    }

    //public interface IPath : IShape
    //{
    //    List<Point>         Points { get; set; }
    //}

}
