/********************************************************************
    created:	2017/4/26 11:35:05
    author:		rush
    email:		
	
    purpose:	数学直线 y=kx + b, 用斜截式表示，虽然不如ax + by +c = 0简单，但是比较直观
 *              垂直于X轴的 x=X;
                若知道两点坐标（x1,y1）（x2,y2）,则斜率k=（y2-y1）/（x2-x1）,直线方程y-y1=k(x-x1)或者y-y2=k(x-x2),化简之后是一样的.
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public class RLine
    {
        public float        K;
        public float        B;
        public float        KAngle { get { return (float)Math.Atan(K); } }  // 斜率角度

        ///  不存在斜率的情况
        public float        X = float.NaN;
        public bool         IsXVerticle { get { return !float.IsNaN(X); } }                            // 是否垂直于X


        public float        GetY(float x)
        {
            if (IsXVerticle)
                return float.NaN;

            return K*x + B;
        }
        public float        GetY(double x)
        {
            return GetY((float) x);
        }

        public bool         IsOnline(RVector2 p)                            // 是否是线上一点
        {
            return true;            // float 精度不够
            
            if (MathEx.Equal(GetY((float)p.X), p.Y))
                return true;
            else
                return false;
        }

        public RVector2     Intersect(RLine other)                          // 与另一根线相交, 获得交点
        {
            RVector2 ret = new RVector2();

            if (IsXVerticle)
            {
                ret.X = X;
                ret.Y = other.GetY(X);
                return ret;
            }

            if (other.IsXVerticle)
            {
                ret.X = other.X;
                ret.Y = GetY(ret.X);
                return ret;
            }


            if (MathEx.Equal(K, other.K))  // 斜率相同，无交点
                return null;

            ret.X = (other.B - B)/(K - other.K);
            ret.Y = GetY(ret.X);

            return ret;
        }

        public RLine        GetVerticleLine(RVector2 p)                     // 以p为交点作一条垂直线
        {
            System.Diagnostics.Debug.Assert(IsOnline(p));

            if(IsXVerticle)
                return new RLine(0, (float)p.Y);

            if (MathEx.Equal(K, 0f))
            {
                return new RLine((float)p.X);
            }

            float k = -1/K;
            float b = (float)(p.Y - k*p.X);

            return new RLine(k, b);
        }
        public RLine        GetParallelLine(float range)                    // 两平行线之间的距离, 可正可负
        {
            if(IsXVerticle)
                return new RLine(X+range);

            float k = K;
            float b = B + range;        // 暂时直接用b代替

            return new RLine(k, b);
        }

        public RQuadrilateral GetBoundingBox(RVector2 a, RVector2 b, float range)        // 以直线上的两个点作为终点，建立一个boundingBox
        {
            System.Diagnostics.Debug.Assert(IsOnline(a) && IsOnline(b));

            RLine paral1 = GetParallelLine(range);
            RLine paral2 = GetParallelLine(-range);

            RLine ver1 = GetVerticleLine(a);
            RLine ver2 = GetVerticleLine(b);

            RVector2 p1 = paral2.Intersect(ver1);
            RVector2 p2 = paral2.Intersect(ver2);
            RVector2 p3 = paral1.Intersect(ver2);
            RVector2 p4 = paral1.Intersect(ver1);

            return new RQuadrilateral(p1, p2, p3, p4);
        }

#region C&D
        public             RLine(float k, float b)
        {
            K = k;
            B = b;
        }
        public             RLine(RVector2 a, RVector2 b)                   // 2点确定一直线
        {
            if (MathEx.Equal(a.X, b.X))
                X = (float)a.X;
            else
            {
                K = (float)((b.Y - a.Y) / (b.X - a.X));
                B = (float)(a.Y - K * a.X);
            }
        }
        public              RLine(float x)                                  // 垂直x轴直线
        {
            X = x;
        }
#endregion


    }
}
