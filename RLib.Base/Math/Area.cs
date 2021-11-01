/********************************************************************
    created:	2017/4/24 17:50:55
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public class RArea:IArea
    {
        public virtual bool Contains(RVector2 pt) { throw new NotImplementedException();}                           // 是否包含该点（忽略y坐标）

        public virtual RVector2  CenterPoint { get { return m_pCenter; } }                            //区域中心点
//        public virtual PointF  RandomPoint { get { throw new NotImplementedException();}}                            //区域内随机坐标
        public virtual EAreaType Type { get { return m_eType;} }

        public RColor        Color { get { return m_CColor; } set { m_CColor = value; } }                             // 表示颜色

        public bool         Show
        {
            get
            {
                return m_bShow;
            }
            set
            {
                m_bShow = value;
            }
        }

#region Members
        protected RColor     m_CColor;
        protected bool      m_bShow;
        protected RVector2    m_pCenter;
        protected EAreaType m_eType;
#endregion
    }

    public class RCircle : RArea, ICircle
    {
#region IArea
        public override bool Contains(RVector2 pt)
        {
            return Radius >= RVector2.Distance(CenterPoint, pt);
        }
#endregion

#region ICircle

        public float        Radius
        {
            get { return m_radius; }
        }
#endregion

#region C&D
        public              RCircle(RVector2 centerPoint, float radius)
        {
            m_radius = radius;
            m_pCenter = centerPoint;
        }
#endregion

#region Members
        float               m_radius;
#endregion
    }

    public partial class RTriangle : RArea, ITriangle
    {
#region IArea
        public override bool Contains(RVector2 pt)
        {
            return MathEx.PointInTriangle(X, Y, Z, pt);
        }

        public override RVector2 CenterPoint
        {
            get
            {
                //原理：一个顶点与对边中点组成的线段中点

                RVector2 yzCenterPos = new RVector2((Y.X + Z.X) / 2, (Y.Y + Z.Y) / 2);

                return new RVector2((X.X + yzCenterPos.X) / 2, (X.Y + yzCenterPos.Y) / 2);
            }
        }
#endregion




#region C&D
        public              RTriangle(RVector2 x, RVector2 y, RVector2 z)
        {
            m_x = x;
            m_y = y;
            m_z = z;
            m_eType = EAreaType.Triangle;
        }
#endregion

#region ITriangle
        public RVector2       X { get { return m_x; } }
        public RVector2       Y { get { return m_y; } }
        public RVector2       Z { get { return m_z; } }
#endregion

#region Members
        protected RVector2    m_x;
        protected RVector2    m_y;
        protected RVector2    m_z;
#endregion
    }

    public partial class RQuadrilateral : RArea, IQuadrilateral
    {
#region IArea
        public override bool Contains(RVector2 pt)
        {
            if (m_isInsideTriangle)
            {
                //Logger.Main.Log("$$___Quadrilateral.Contains 1",ELogOwner.WangLv);
                if (m_t1.Contains(pt) && !m_t2.Contains(pt))
                    return true;
            }
            else
            {
                //Logger.Main.Log("$$___Quadrilateral.Contains 2", ELogOwner.HuiQi);
                if (m_t1.Contains(pt) || m_t2.Contains(pt))
                    return true;
            }
         
            return false;
        }

        /// <summary>
        /// 目前实现的非精确中心点,取一个对角线的中点
        /// </summary>
        public override RVector2 CenterPoint
        {
            get 
            {
                return new RVector2((X.X + Z.X) / 2, (X.Y + Z.Y) / 2); 
            }
        }

        public override string ToString()
        {
            return "Quadrilateral (w = " + m_w + " x = " + m_x + " y = " + m_y + " z = " + m_z + ")";
        }
#endregion

#region IQuadrilateral
        public RVector2      this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return m_w;
                    case 1:
                        return m_x;
                    case 2:
                        return m_y;
                    case 3:
                        return m_z;
                }
                throw new ArgumentException("bad args");
            }
        }

        public RVector2      W
        {
            get { return m_w; }
        }
        public RVector2      X
        {
            get { return m_x; }
        }
        public RVector2      Y
        {
            get { return m_y; }
        }
        public RVector2      Z
        {
            get { return m_z; }
        }
#endregion

#region c&D
        public              RQuadrilateral(RVector2 w, RVector2 x, RVector2 y, RVector2 z)
        {
            m_w = w;
            m_x = x;
            m_y = y;
            m_z = z;

            //检查四边形是否构成了一个内部三角形
            char posName = __ExistInsideTriangle();
            if(posName != 'n')
            {
                switch (posName)
                {
                    case 'w':
                        m_t1 = new RTriangle(x, y, z);
                        m_t2 = new RTriangle(z, w, x);
                        break;
                    case 'x':
                        m_t1 = new RTriangle(y, z, w);
                        m_t2 = new RTriangle(w, x, y);
                        break;
                    case 'y':
                        m_t1 = new RTriangle(z, w, x);
                        m_t2 = new RTriangle(x, y, z);
                        break;
                    case 'z':
                        m_t1 = new RTriangle(w, x, y);
                        m_t2 = new RTriangle(y, z, w);
                        break;

                    default:
                        break;
                }
                m_isInsideTriangle = true;
                //Logger.Main.Log(">>>>>>>>have a insideRTriangle", ELogOwner.HuiQi);
            }
            else
            {
                m_t1 = new RTriangle(w, x, y);
                m_t2 = new RTriangle(y, z, w);
            }

        }
#endregion



        /// <summary>
        /// 检查是否有内部三角形
        /// </summary>
        /// <returns>返回点的名字</returns>
        char                __ExistInsideTriangle()
        {
            char posName = 'n'; //‘n’代表没有内三角
            if (MathEx.PointInTriangle(X, Y, Z, W))
            {
                posName = 'w';
            }
            else if (MathEx.PointInTriangle(Y, Z, W, X))
            {
                posName = 'x';
            }
            else if (MathEx.PointInTriangle( Z, W,X, Y))
            {
                posName = 'y';
            }
            else if (MathEx.PointInTriangle(X, Y, W, Z))
            {
                posName = 'z';
            }

            return posName;
        }

#region Members
        RTriangle            m_t1;
        RTriangle            m_t2;                                           //如果有内部三角则为此三角形
        bool                m_isInsideTriangle = false;

        RVector2             m_w;
        RVector2             m_x;
        RVector2             m_y;
        RVector2             m_z;
#endregion
    }


}
