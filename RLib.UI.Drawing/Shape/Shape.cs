/********************************************************************
    created:	2020/4/1 19:19:00
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NPOI.SS.Format;

namespace RLib.Base.Shape
{
    public class Shape:IShape
    {

        public virtual bool IsHitBoundry(Point x, double threshold = 3) // 边缘碰撞检测
        {
            throw new NotImplementedException();
        }

        public virtual Rect BoundingBox
        {
            get
            {
                throw new NotImplementedException();
            }
        } // 获取总的BoundingBox

    }


    public class SealingShape : Shape, ISealingShape                                 // 封闭式图形
    {
        public virtual bool Contains(Point pt){ throw new NotImplementedException();}                             // 是否在图形内部
    }


    public class Line : Shape, ILine
    {
        public static bool  GetKB(Point p1, Point p2, out double k, out double b) // 根据2点构成的直线，求k,b  [根据Line基本公式 y= kx + b]
        {
            // 如果是垂直的 
            if (RMath.Equal(p1.X, p2.X))
            {
                k = double.NaN;
                b = double.NaN;

                return false;
            }

            k = (p1.Y - p2.Y) / (p1.X - p2.X);
            b = p2.Y - k * p1.X;
            return true;
        }

        public static double?  GetY(Point p1, Point p2, double x)            // 2点构成的直线，给定一个x，求直线的y  [根据直线的2点式计算]
        {
            if (RMath.Equal(p1.X, p2.X))
            {
                if (RMath.Equal(p1.X, x))
                    return x;
                else
                    return null;
            }
            else if (RMath.Equal(p1.Y, p2.Y))
            {
                return p1.Y;
            }
            else
            {// 2点式 (y-y1)/(y2-y1)=(x-x1)/(x2-x1) (x1≠x2，y1≠y2)
                return (x - p1.X) / (p2.X - p1.X) * (p2.Y - p1.Y) + p1.Y;
            }
        }

        public override bool IsHitBoundry(Point x, double threshold = 3) // 边缘碰撞检测
        {
            return IsHitBoundry(x, threshold);

        }

        public bool IsHitBoundry(Point pt, double threshold = 3, bool extendLeft = false, bool extendRight = false) // 边缘碰撞检测
        {
            if (IsVerticle) // 垂直情况
            {
                return Math.Abs(pt.X - Start.X) <= threshold;
            }

            if (pt.X < Start.X && pt.X < End.X && !extendLeft)
                return false;

            if (pt.X > Start.X && pt.X > End.X && !extendRight)
                return false;
           

            double? y = GetY(Start, End, pt.X);
            bool ret = Math.Abs(y.Value - pt.Y) <= threshold;
            if (ret)
            {

            }

            return ret;
        }

        public override Rect BoundingBox
        {
            get { return  new Rect(Start, End); }
        } // 获取总的BoundingBox

        public bool         IsVerticle
        {
            get { return RMath.Equal(Start.X, End.X); }
        }                             // 是否是垂直线

        public bool         IsHorizontal
        {
            get
            {
                return RMath.Equal(Start.Y, End.Y); 
            }
        } // 是否是横线


        public Point               Start { get; set; }
        public Point               End { get; set; }
    }


    public class Ellipse:SealingShape, IEllipse                             //圆
    {
        public static KeyValuePair<double, double>? GetY(Point center, double radiusX, double radiusY, double x)            // 给定一个椭圆基本表示，和x，获取y(y有2个值) [根据椭圆标准方程 x^2/a^2 + y^2/b^2 = 1]
        {
            if (x > center.X - radiusX && x < center.X + radiusX)
            {
                double offsetX = x - center.X;

                double offsetY2 = (1 - (offsetX * offsetX) / (radiusX * radiusX)) * (radiusY * radiusY);
                double offsetY = Math.Sqrt(offsetY2);
                double y1 = offsetY + center.Y;
                double y2 = -offsetY + center.Y;

                return new KeyValuePair<double, double>(y1, y2);
            }
            else
                return null;
        }

        public override bool IsHitBoundry(Point pt, double threshold = 3) // 边缘碰撞检测
        {
            KeyValuePair<double, double>? kv = GetY(Center, RadiusX, RadiusY, pt.X);
            if (kv == null)
                return false;

            if (Math.Abs(kv.Value.Key - pt.Y) <= threshold)
                return true;

            return Math.Abs(kv.Value.Value - pt.Y) <= threshold;
        }

        public override Rect BoundingBox
        {
            get { return  new Rect(new Point(Center.X - RadiusX, Center.Y - RadiusY), new Size(RadiusX*2, RadiusY*2)); }
        } // 获取总的BoundingBox


        public bool                IsCircle
        {
            get { return RMath.Equal(RadiusX, RadiusY); }
        }


        public double              RadiusX { get; set; }                    // 半径
        public double              RadiusY { get; set; }                    // 半径
        public Point               Center { get; set; }                     // 半径
    }

    //public interface ITriangle : ISealingShape                                      //三角形
    //{
    //    Point               X { get; set; }
    //    Point               Y { get; set; }
    //    Point               Z { get; set; }
    //}

    public class Rectangle : SealingShape, IRectangle                             // 长方形
    {
        public override bool IsHitBoundry(Point pt, double threshold = 3) // 边缘碰撞检测
        {// 缩小后不包含，扩大后包含，认为其在边上
            Rect small = Rect;
            small.Inflate(-threshold, -threshold);
            if (small.Contains(pt))
                return false;
            Rect big = Rect;
            big.Inflate(threshold, threshold);
            return big.Contains(pt);
        }

        public override Rect BoundingBox
        {
            get { return  Rect; }
        } // 获取总的BoundingBox


        public override bool Contains(Point pt)
        {
            return Rect.Contains(pt);}                             // 是否在图形内部


        public Rect                Rect { get; set; }

    }




}
