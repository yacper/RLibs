/********************************************************************
    created:	2017/4/25 11:33:52
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public class RVector3
    {
#region Overrides
        public override bool Equals(object obj)
        {
			if(obj is RVector3)
			{
				return Equals((RVector3)this);
			}
			
            return false;
        }
        public bool         Equals(RVector3 other)
        {
            return (RMath.Equal(X, other.X) && 
                RMath.Equal(Y, other.Y) &&
                RMath.Equal(Z, other.Z)
                );
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }
        public override string ToString()
        {
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
        	return string.Format(currentCulture, "({0} {1} {2})", new object[] { 
				this.X.ToString(currentCulture), this.Y.ToString(currentCulture), this.Z.ToString(currentCulture) });
        }
#endregion



        public static RVector3 zero { get { return s_zero; } }
        public static RVector3 one { get { return s_one; } }
        public static RVector3 right { get { return s_right; } }
        public static RVector3 up { get { return s_up; } }

        public static RVector3 Back { get { return s_back; } }
        public static RVector3 Down { get { return s_down; } }
        public static RVector3 Forward { get { return s_forward; } }
        public static RVector3 Left { get { return s_left; } }


        public float        X;
        public float        Y;
        public float        Z;

        public float        this[int index]
        {
            get
            {
                System.Diagnostics.Debug.Assert(index >= 0 && index < 3);
                if (index == 0)
                    return X;
                else if (index == 1)
                    return Y;
                else
                    return Z;
            }
            set
            {
                System.Diagnostics.Debug.Assert(index >= 0 && index < 3);
                if (index == 0)
                    X = value;
                else if (index == 1)
                    Y = value;
                else
                    Z = value;
            }
        }

        public float        Magnitude { get { return (float)Math.Sqrt(X*X + Y*Y + Z*Z); } } //     Returns the length of this vector (Read Only).
        public float        SqrMagnitude { get { return (float)Math.Sqrt(Magnitude); } }
        public RVector3     Normalized                                      //     Returns this vector with a magnitude of 1 (Read Only).
        {
            get
            {
                float m = Magnitude;
                return new RVector3(X/m, Y/m, Z/m);
            }
        }


        public static RVector3 operator -(RVector3 a)
        {
            return new RVector3(-a.X, -a.Y, -a.Z);
        }
        public static RVector3 operator -(RVector3 a, RVector3 b)
        {
            return new RVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static bool operator !=(RVector3 lhs, RVector3 rhs)
        {
            return !(lhs == rhs);
        }
        public static RVector3 operator *(float d, RVector3 a)
        {
            return new RVector3(a.X * d, a.Y * d, a.Z*d);
        }
        public static RVector3 operator *(RVector3 a, float d)
        {
            return new RVector3(a.X * d, a.Y * d, a.Z *d);
        }
        public static RVector3 operator /(RVector3 a, float d)
        {
            return new RVector3(a.X / d, a.Y / d, a.Z /d);
        }
        public static RVector3 operator +(RVector3 a, RVector3 b)
        {
            return new RVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static bool operator ==(RVector3 lhs, RVector3 rhs)
        {
            if (RMath.Equal(lhs.X, rhs.X) &&
                RMath.Equal(lhs.Y, rhs.Y) &&
                RMath.Equal(lhs.Z, rhs.Z)
                )
                return true;
            else
                return false;
        }

        public void         Normalize()
        {
            float m = Magnitude;
            X = X/m;
            Y = Y/m;
            Z = Z/m;
        }
        public void         Scale(RVector3 scale)
        {
            X = X * scale.X;
            Y = Y * scale.Y;
            Z = Z * scale.Z;
        }


        public static float Dot(RVector3 lhs, RVector3 rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
        }
        public static RVector3 Cross(RVector3 lhs, RVector3 rhs)
        {
            return new RVector3(lhs.Y * rhs.Z - rhs.Y * lhs.Z,
                                 -(lhs.X * rhs.Z - rhs.X * lhs.Z),
                                 lhs.X * rhs.Y - rhs.X * lhs.Y);
        }

        public static float Distance(RVector3 a, RVector3 b)
        {
            float ret = (a.X - b.X) * (a.X - b.X) +
                     (a.Y - b.Y) * (a.Y - b.Y) +
                     (a.Z - b.Z) * (a.Z - b.Z);

            return (float)Math.Sqrt(ret);
        }

        public static float Angle(RVector3 from, RVector3 to)
        {
            return (float)Math.Acos(RVector3.Dot(from, to)/(from.Magnitude*to.Magnitude));
        }
        public static RVector3 Scale(RVector3 a, RVector3 b)
        {
            return new RVector3(a.X*b.X, a.Y*b.Y, a.Z*b.Z);
        }

        public static RVector3 Max(RVector3 lhs, RVector3 rhs)
        {
            return new RVector3(lhs.X > rhs.X ? lhs.X : rhs.X, 
			                   lhs.Y > rhs.Y ? lhs.Y : rhs.Y,
			                   lhs.Z > rhs.Z ? lhs.Z : rhs.Z
                               );
        }

        public static RVector3 Min(RVector3 lhs, RVector3 rhs)
        {
            return new RVector3(lhs.X < rhs.X ? lhs.X : rhs.X, 
			                   lhs.Y < rhs.Y ? lhs.Y : rhs.Y,
			                   lhs.Z < rhs.Z ? lhs.Z : rhs.Z
                               );
        }
        public static RVector3 ClampMagnitude(RVector3 vector, float maxLength)
        {
            return vector.Normalized * maxLength;
        }
        public static RVector3 Lerp(RVector3 from, RVector3 to, float t)
        {
            return new RVector3(
                RMath.Lerp(from.X, to.X, t),
                RMath.Lerp(from.Y, to.Y, t),
                RMath.Lerp(from.Z, to.Z, t)
                );
        }
     
   
        //public static void OrthoNormalize(ref RVector3 normal, ref RVector3 tangent);
        //public static void OrthoNormalize(ref RVector3 normal, ref RVector3 tangent, ref RVector3 binormal);
        ////
        //// 摘要:
        ////     Projects a vector onto another vector.
        //public static RVector3 Project(RVector3 vector, RVector3 onNormal);
        ////
        //// 摘要:
        ////     Projects a vector onto a plane defined by a normal orthogonal to the plane.
        //public static RVector3 ProjectOnPlane(RVector3 vector, RVector3 planeNormal);
        ////
        //// 摘要:
        ////     Reflects a vector off the plane defined by a normal.
        //public static RVector3 Reflect(RVector3 inDirection, RVector3 inNormal);
        ////
        //// 摘要:
        ////     Rotates a vector current towards target.
        //public static RVector3 RotateTowards(RVector3 current, RVector3 target, float maxRadiansDelta, float maxMagnitudeDelta);
  


#region C&D
        public RVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
#endregion

#region SMembers
        private static RVector3 s_zero = new RVector3(0f, 0f, 0f);
        private static RVector3 s_one = new RVector3(1f, 1f, 1f);
        private static RVector3 s_right = new RVector3(1f, 0f, 0f);
        private static RVector3 s_up = new RVector3(0f, 1f, 0f);
        private static RVector3 s_back  = new RVector3(0f, 0f, -1f);
        private static RVector3 s_down = new RVector3(0f, -1f, 0f);
        private static RVector3 s_forward = new RVector3(0f, 0f, 1f);
        private static RVector3 s_left = new RVector3(-1f, 0, 0);
#endregion
    }
}
