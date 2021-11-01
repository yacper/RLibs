/********************************************************************
    created:	2018/1/6 15:46:43
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public struct RPoint : IFormattable
    {
#region Statics
        public static RPoint Zero { get { return s_zero; } }
        public static RPoint One { get { return s_one; } }
#endregion

#region Overrides
        /// <summary>从指定的 <see cref="T:System.String" /> 构造 <see cref="T:System.Windows.RPoint" />。</summary>
        /// <returns>等效的 <see cref="T:System.Windows.RPoint" /> 结构。</returns>
        /// <param name="source">点的字符串表示形式。</param>
        /// <exception cref="T:System.FormatException">
        /// <paramref name="source" /> 不是由两个逗号分隔或空格分隔的双精度值组成。</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source" /> 不包含两个数字。- 或 -<paramref name="source" /> 包含的分隔符过多。</exception>
        public static RPoint Parse(string source)
        {
            throw new NotImplementedException();

            //IFormatProvider invariantEnglishUs = (IFormatProvider)TypeConverterHelper.InvariantEnglishUS;
            //TokenizerHelper tokenizerHelper = new TokenizerHelper(source, invariantEnglishUs);
            //RPoint point = new RPoint(Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs), Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs));
            //tokenizerHelper.LastTokenRequired();
            //return point;
        }
        /// <summary>创建此 <see cref="T:System.Windows.RPoint" /> 的 <see cref="T:System.String" /> 表示形式。</summary>
        /// <returns>一个 <see cref="T:System.String" />，它包含此 <see cref="T:System.Windows.RPoint" /> 结构的 <see cref="P:System.Windows.RPoint.X" /> 和 <see cref="P:System.Windows.RPoint.Y" /> 值。</returns>
        public override string ToString()
        {
            return this.ConvertToString((string)null, (IFormatProvider)null);
        }
        /// <summary>创建此 <see cref="T:System.Windows.RPoint" /> 的 <see cref="T:System.String" /> 表示形式。</summary>
        /// <returns>一个 <see cref="T:System.String" />，它包含此 <see cref="T:System.Windows.RPoint" /> 结构的 <see cref="P:System.Windows.RPoint.X" /> 和 <see cref="P:System.Windows.RPoint.Y" /> 值。</returns>
        /// <param name="provider">区域性特定的格式设置信息。</param>
        public string ToString(IFormatProvider provider)
        {
            return this.ConvertToString((string)null, provider);
        }
        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return this.ConvertToString(format, provider);
        }
        internal string ConvertToString(string format, IFormatProvider provider)
        {
            throw new NotImplementedException();

            //char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            //return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[3] { (object)numericListSeparator, (object)this._x, (object)this._y });
        }

        public static bool Equals(RPoint point1, RPoint point2)
        {
            if (point1.X.Equals(point2.X))
                return point1.Y.Equals(point2.Y);
            return false;
        }
        public override bool Equals(object o)
        {
            if (o == null || !(o is RPoint))
                return false;
            return RPoint.Equals(this, (RPoint)o);
        }
        public bool Equals(RPoint value)
        {
            return RPoint.Equals(this, value);
        }
        public override int GetHashCode()
        {
            double num = this.X;
            int hashCode1 = num.GetHashCode();
            num = this.Y;
            int hashCode2 = num.GetHashCode();
            return hashCode1 ^ hashCode2;
        }


        public static bool operator ==(RPoint point1, RPoint point2)
        {
            if (point1.X == point2.X)
                return point1.Y == point2.Y;
            return false;
        }
        public static bool operator !=(RPoint point1, RPoint point2)
        {
            return !(point1 == point2);
        }

        public static RPoint operator +(RPoint point, RVector2 vector)
        {
            return new RPoint(point._x + vector._x, point._y + vector._y);
        }
        public static RPoint Add(RPoint point, RVector2 vector)
        {
            return new RPoint(point._x + vector._x, point._y + vector._y);
        }
        public static RPoint operator -(RPoint point, RVector2 vector)
        {
            return new RPoint(point._x - vector._x, point._y - vector._y);
        }
        public static RPoint Subtract(RPoint point, RVector2 vector)
        {
            return new RPoint(point._x - vector._x, point._y - vector._y);
        }
        public static RVector2 operator -(RPoint point1, RPoint point2)
        {
            return new RVector2(point1._x - point2._x, point1._y - point2._y);
        }
        public static RVector2 Subtract(RPoint point1, RPoint point2)
        {
            return new RVector2(point1._x - point2._x, point1._y - point2._y);
        }

        ///// <summary>将指定的 <see cref="T:System.Windows.RPoint" /> 转换为指定的 <see cref="T:System.Windows.Media.Matrix" />。</summary>
        ///// <returns>使用指定的矩阵转换指定点所得的结果。</returns>
        ///// <param name="point">要转换的点。</param>
        ///// <param name="matrix">变换矩阵。</param>
        //public static RPoint operator *(RPoint point, Matrix matrix)
        //{
        //    return matrix.Transform(point);
        //}

        ///// <summary>将指定的 <see cref="T:System.Windows.RPoint" /> 结构转换为指定的 <see cref="T:System.Windows.Media.Matrix" /> 结构。</summary>
        ///// <returns>已转换的点。</returns>
        ///// <param name="point">要转换的点。</param>
        ///// <param name="matrix">变换矩阵。</param>
        //public static RPoint Multiply(RPoint point, Matrix matrix)
        //{
        //    return matrix.Transform(point);
        //}


        public static explicit operator RSize(RPoint point)
        {
            return new RSize(Math.Abs(point._x), Math.Abs(point._y));
        }
        public static explicit operator RVector2(RPoint point)
        {
            return new RVector2(point._x, point._y);
        }
#endregion

#region Properties
        public double        X
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }
        public double        Y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }
#endregion


#region Funcs
        public void         Offset(double offsetX, double offsetY)
        {
            this._x = this._x + offsetX;
            this._y = this._y + offsetY;
        }

        public void         Offset(RPoint pt)
        {
            Offset(pt.X, pt.Y);
        }
#endregion

#region C&D
        public              RPoint(double x, double y)
        {
            this._x = x;
            this._y = y;
        }
#endregion

#region Members
        internal double      _x;
        internal double      _y;

        private static readonly RPoint s_zero = new RPoint() { _x = 0, _y = 0 };
        private static readonly RPoint s_one = new RPoint() { _x = 1, _y = 1 };
#endregion
    }

    public static class RPointEx
    {
        public static System.Drawing.Point ToPoint(this RPoint p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        public static RPoint ToRPoint(this Point p)
        {
            return new RPoint(p.X, p.Y);
        }
    }

}
