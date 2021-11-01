/********************************************************************
    created:	2017/4/25 11:28:08
    author:		rush
    email:		
	
    purpose:	2维向量, 二维 空间中的置换

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DataModel;

namespace RLib.Base
{
    public class RVector2DM
    {
        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString()
        {
            return "(" + X +"," + Y+")";
            //return this.ConvertToString((string)null, (IFormatProvider)null);
        }
    }


    public struct RVector2 : IFormattable
    {
#region Statics
        public static RVector2 zero { get { return s_zero; } }
        public static RVector2 one { get { return s_one; } }
        public static RVector2 right { get { return s_right; } }
        public static RVector2 up { get { return s_up; } }
#endregion

#region Overrides

        public object ToDm() // 序列化成protobufdm
        {
            return new RVector2DM(){ X = X, Y= Y};
        }

        /// <summary>将向量的字符串表示形式转换为等效的 <see cref="T:System.Windows.RVector2" /> 结构。</summary>
        /// <returns>等效的 <see cref="T:System.Windows.RVector2" /> 结构。</returns>
        /// <param name="source">向量的字符串表示形式。</param>
        public static RVector2 Parse(string source)
        {
            throw new NotImplementedException();
            //IFormatProvider invariantEnglishUs = (IFormatProvider)TypeConverterHelper.InvariantEnglishUS;
            //TokenizerHelper tokenizerHelper = new TokenizerHelper(source, invariantEnglishUs);
            //RVector2 vector = new RVector2(Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs), Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs));
            //tokenizerHelper.LastTokenRequired();
            //return vector;
        }
        /// <summary>返回此 <see cref="T:System.Windows.RVector2" /> 结构的字符串表示形式。</summary>
        /// <returns>一个字符串，表示此 <see cref="T:System.Windows.RVector2" /> 的 <see cref="P:System.Windows.RVector2.X" /> 和 <see cref="P:System.Windows.RVector2.Y" /> 值。</returns>
        public override string ToString()
        {
            return "(" + X +"," + Y+")";
            //return this.ConvertToString((string)null, (IFormatProvider)null);
        }
        /// <summary>使用指定的格式设置信息返回此 <see cref="T:System.Windows.RVector2" /> 结构的字符串表示形式。</summary>
        /// <returns>一个字符串，表示此 <see cref="T:System.Windows.RVector2" /> 的 <see cref="P:System.Windows.RVector2.X" /> 和 <see cref="P:System.Windows.RVector2.Y" /> 值。</returns>
        /// <param name="provider">特定于区域的格式设置信息。</param>
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

        public static bool operator ==(RVector2 vector1, RVector2 vector2)
        {
            if (RMath.Equal(vector1.X , vector2.X))
                return RMath.Equal(vector1.Y , vector2.Y);

            //if (vector1.X == vector2.X)
            //    return vector1.Y == vector2.Y;
            return false;
        }
        public static bool operator !=(RVector2 vector1, RVector2 vector2)
        {
            return !(vector1 == vector2);
        }
        public static bool Equals(RVector2 vector1, RVector2 vector2)
        {
            if (vector1.X.Equals(vector2.X))
                return vector1.Y.Equals(vector2.Y);
            return false;
        }
        public override bool Equals(object o)
        {
            if (o == null || !(o is RVector2))
                return false;
            return RVector2.Equals(this, (RVector2)o);
        }
        public bool Equals(RVector2 value)
        {
            return RVector2.Equals(this, value);
        }
        public override int GetHashCode()
        {
            double num = this.X;
            int hashCode1 = num.GetHashCode();
            num = this.Y;
            int hashCode2 = num.GetHashCode();
            return hashCode1 ^ hashCode2;
        }

        public static RVector2 operator -(RVector2 vector)
        {
            return new RVector2(-vector._x, -vector._y);
        }
        public static RPoint operator +(RVector2 vector, RPoint point)
        {
            return new RPoint(point._x + vector._x, point._y + vector._y);
        }

        public static RVector2 operator +(RVector2 vector1, RVector2 vector2)
        {
            return new RVector2(vector1._x + vector2._x, vector1._y + vector2._y);
        }
        public static RVector2 Add(RVector2 vector1, RVector2 vector2)
        {
            return new RVector2(vector1._x + vector2._x, vector1._y + vector2._y);
        }
        public static RVector2 operator -(RVector2 vector1, RVector2 vector2)
        {
            return new RVector2(vector1._x - vector2._x, vector1._y - vector2._y);
        }
        public static RVector2 Subtract(RVector2 vector1, RVector2 vector2)
        {
            return new RVector2(vector1._x - vector2._x, vector1._y - vector2._y);
        }
        public static RPoint Add(RVector2 vector, RPoint point)
        {
            return new RPoint(point._x + vector._x, point._y + vector._y);
        }

        /// <summary> 计算两个指定向量结构的点积并将结果以 <see cref="T:System.Double" /> 形式返回。</summary>
        /// <returns>返回一个 <see cref="T:System.Double" />，其中包含 <paramref name="vector1" /> 和 <paramref name="vector2" /> 的标量点积，标量点积可通过下面的公式计算得出：vector1.X * vector2.X + vector1.Y * vector2.Y</returns>
        /// <param name="vector1">要相乘的第一个向量。</param>
        /// <param name="vector2">要相乘的第二个向量。</param>
        public static double operator *(RVector2 vector1, RVector2 vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }
        /// <summary>计算两个指定向量的点积并将结果以 <see cref="T:System.Double" /> 形式返回。</summary>
        /// <returns>一个 <see cref="T:System.Double" />，其中包含 <paramref name="vector1" /> 和 <paramref name="vector2" /> 的标量点积，标量点积可通过下面的公式计算得出： (vector1.X * vector2.X) + (vector1.Y * vector2.Y) </returns>
        /// <param name="vector1">要相乘的第一个向量。</param>
        /// <param name="vector2">要相乘的第二个向量结构。</param>
        public static double Multiply(RVector2 vector1, RVector2 vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        public static RVector2 operator *(RVector2 vector, double scalar)
        {
            return new RVector2(vector._x * scalar, vector._y * scalar);
        }
        public static RVector2 Multiply(RVector2 vector, double scalar)
        {
            return new RVector2(vector._x * scalar, vector._y * scalar);
        }
        public static RVector2 operator *(double scalar, RVector2 vector)
        {
            return new RVector2(vector._x * scalar, vector._y * scalar);
        }
        public static RVector2 Multiply(double scalar, RVector2 vector)
        {
            return new RVector2(vector._x * scalar, vector._y * scalar);
        }
        public static RVector2 operator /(RVector2 vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }
        public static RVector2 Divide(RVector2 vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        ///// <summary> 使用指定的 <see cref="T:System.Windows.Media.Matrix" /> 变换指定向量的坐标空间。</summary>
        ///// <returns>按 <paramref name="matrix" /> 变换 <paramref name="vector" /> 的结果。</returns>
        ///// <param name="vector">要变换的向量。</param>
        ///// <param name="matrix">要应用于 <paramref name="vector" /> 的变换。</param>
        //public static RVector2 operator *(RVector2 vector, Matrix matrix)
        //{
        //    return matrix.Transform(vector);
        //}
        ///// <summary>使用指定的 <see cref="T:System.Windows.Media.Matrix" /> 变换指定向量的坐标空间。</summary>
        ///// <returns>按 <paramref name="matrix" /> 变换 <paramref name="vector" /> 的结果。</returns>
        ///// <param name="vector">要变换的向量结构。</param>
        ///// <param name="matrix">要应用于 <paramref name="vector" /> 的变换。</param>
        //public static RVector2 Multiply(RVector2 vector, Matrix matrix)
        //{
        //    return matrix.Transform(vector);
        //}

        /// <summary>计算两个向量的行列式。</summary>
        /// <returns>
        /// <paramref name="vector1" /> 和 <paramref name="vector2" /> 的行列式。</returns>
        /// <param name="vector1">要计算的第一个向量。</param>
        /// <param name="vector2">要计算的第二个向量。</param>
        public static double Determinant(RVector2 vector1, RVector2 vector2)
        {
            return vector1._x * vector2._y - vector1._y * vector2._x;
        }

        public static explicit operator RSize(RVector2 vector)
        {
            return new RSize(Math.Abs(vector._x), Math.Abs(vector._y));
        }
        public static explicit operator RPoint(RVector2 vector)
        {
            return new RPoint(vector._x, vector._y);
        }

        public static implicit operator RVector3(RVector2 v)
        {
            return new RVector3((float)v.X, 0, (float)v.Y);
        }
        public static implicit operator RVector2(RVector3 v)
        {
            return new RVector2(v.X, v.Z);
        }

#endregion

#region Properties
        public double       X
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
        public double       Y
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

        public double       this[int index]
        {
            get
            {
                System.Diagnostics.Debug.Assert(index >= 0 && index < 2);
                if (index == 0)
                    return X;
                else
                    return Y;
            }
            set
            {
                System.Diagnostics.Debug.Assert(index >= 0 && index < 2);
                if (index == 0)
                    X = value;
                else
                    Y = value;
            }
        }

        public double        Length
        {
            get
            {
                return (double)Math.Sqrt(this._x * this._x + this._y * this._y);
            }
        }
        /// <summary>获取此向量的长度的平方。</summary>
        /// <returns>此向量的 <see cref="P:System.Windows.RVector2.Length" /> 的平方。</returns>
        public double        LengthSquared
        {
            get
            {
                return this._x * this._x + this._y * this._y;
            }
        }

        public RVector2     Normalized                                      //     Returns this vector with a magnitude of 1 (Read Only).
        {
            get
            {
                double m = Length;
                return new RVector2(X/m, Y/m);
            }
        }
#endregion

#region Funcs
        /// <summary> 规范化此向量。</summary>
        public void         Normalize()
        {
            this = this / Math.Max(Math.Abs(this._x), Math.Abs(this._y));
            this = this / this.Length;
        }

        public static float Distance(RVector2 lhs, RVector2 rhs)            //     Returns the distance between a and b.
        {
	        double v1 = lhs.X - rhs.X, v2 = lhs.Y - rhs.Y;
			return (float)Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        public void         Negate()
        {
            this._x = -this._x;
            this._y = -this._y;
        }
        /// <summary>计算两个向量的叉积。</summary>
        /// <returns>
        /// <paramref name="vector1" /> 和 <paramref name="vector2" /> 的叉乘积。可使用下面的公式计算叉乘积：(Vector1.X * Vector2.Y) - (Vector1.Y * Vector2.X)</returns>
        /// <param name="vector1">要计算的第一个向量。</param>
        /// <param name="vector2">要计算的第二个向量。</param>
        public static double CrossProduct(RVector2 vector1, RVector2 vector2)
        {
            return vector1._x * vector2._y - vector1._y * vector2._x;
        }
        /// <summary>检索两个指定向量之间的角度（用度表示）。</summary>
        /// <returns>
        /// <paramref name="vector1" /> 和 <paramref name="vector2" /> 之间的角度（以度为单位）。</returns>
        /// <param name="vector1">要计算的第一个向量。</param>
        /// <param name="vector2">要计算的第二个向量。</param>
        public static double AngleBetween(RVector2 vector1, RVector2 vector2)
        {
            return (double)(Math.Atan2(vector1._x * vector2._y - vector2._x * vector1._y, vector1._x * vector2._x + vector1._y * vector2._y) * (180.0 / Math.PI));
        }

        public static RVector2 ClampLength(RVector2 vector, double maxLength) //     Returns a copy of vector with its magnitude clamped to maxLength.
        {
            return vector.Normalized * maxLength;
        }
        public static RVector2 Lerp(RVector2 from, RVector2 to, double t)    // //     Linearly interpolates between two vectors.
        {
            return new RVector2(
                RMath.Lerp(from.X, to.X, t),
                RMath.Lerp(from.Y, to.Y, t));
        }

#endregion

#region C&D
        public              RVector2(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public              RVector2(RVector2DM dm)
            :this(dm.X, dm.Y)
        {
        }
#endregion


#region Members
        private static readonly RVector2 s_zero = new RVector2(0f, 0f);
        private static readonly RVector2 s_one = new RVector2(1f, 1f);
        private static readonly RVector2 s_right = new RVector2(1f, 0f);
        private static readonly RVector2 s_up = new RVector2(0f, 1f);

        internal double     _x;
        internal double     _y;
#endregion
    }
}
