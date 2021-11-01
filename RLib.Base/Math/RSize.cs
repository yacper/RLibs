/********************************************************************
    created:	2018/1/6 14:37:31
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public struct RSize: IFormattable
    {
#region Statics
        public static RSize Empty { get { return RSize.s_empty; } }
        public static RSize Zero { get { return RSize.s_zero; } }
        public static RSize One { get { return RSize.s_one; } }
#endregion

#region Overrides
        public static RSize Parse(string source)
        {
            throw new NotImplementedException();

            //IFormatProvider invariantEnglishUs = (IFormatProvider)TypeConverterHelper.InvariantEnglishUS;
            //TokenizerHelper tokenizerHelper = new TokenizerHelper(source, invariantEnglishUs);
            //string str = tokenizerHelper.NextTokenRequired();
            //RSize size = !(str == "Empty") ? new RSize(Convert.ToDouble(str, invariantEnglishUs), Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs)) : RSize.Empty;
            //tokenizerHelper.LastTokenRequired();
            //return size;
        }
        public override string ToString()
        {
            return this.ConvertToString((string)null, (IFormatProvider)null);
        }
        public string       ToString(IFormatProvider provider)
        {
            return this.ConvertToString((string)null, provider);
        }
        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return this.ConvertToString(format, provider);
        }
        internal string     ConvertToString(string format, IFormatProvider provider)
        {
            throw new NotImplementedException();

            //if (this.IsEmpty)
            //    return "Empty";
            //char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            //return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[3] { (object)numericListSeparator, (object)this._width, (object)this._height });
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is RSize))
                return false;
            return RSize.Equals(this, (RSize)o);
        }
        public bool         Equals(RSize value)
        {
            return RSize.Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
                return 0;
            return this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }

        public static bool  Equals(RSize size1, RSize size2)
        {
            if (size1.IsEmpty)
                return size2.IsEmpty;
            if (size1.Width.Equals(size2.Width))
                return size1.Height.Equals(size2.Height);
            return false;
        }
        public static bool operator ==(RSize size1, RSize size2)
        {
            if (size1.Width == size2.Width)
                return size1.Height == size2.Height;
            return false;
        }
        public static bool operator !=(RSize size1, RSize size2)
        {
            return !(size1 == size2);
        }

        public static explicit operator RVector2(RSize size)
        {
            return new RVector2(size._width, size._height);
        }
        public static explicit operator RPoint(RSize size)
        {
            return new RPoint(size._width, size._height);
        }
#endregion


#region Properties
        public double        Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (this.IsEmpty)
                    throw new InvalidOperationException("Size_CannotModifyEmptySize");
                if (value < 0.0)
                    throw new ArgumentException("Size_WidthCannotBeNegative");
                this._width = value;
            }
        }
        public double        Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if (this.IsEmpty)
                    throw new InvalidOperationException("Size_CannotModifyEmptySize");
                if (value < 0.0)
                    throw new ArgumentException("Size_HeightCannotBeNegative");
                this._height = value;
            }
        }

        public bool         IsEmpty
        {
            get
            {
                return this._width < 0.0;
            }
        }
#endregion


#region C&D
        public              RSize(double width, double height)
        {
            if (width < 0.0 || height < 0.0)
                throw new ArgumentException("Size_WidthAndHeightCannotBeNegative");

            _width = width;
            _height = height;
        }
#endregion

#region Members
        private static readonly RSize s_empty = new RSize() { _width = double.NegativeInfinity, _height = double.NegativeInfinity };
        private static readonly RSize s_zero = new RSize() { _width = 0, _height = 0 };
        private static readonly RSize s_one = new RSize() { _width = 1, _height = 1 };
        internal double     _width;
        internal double     _height;
#endregion
    }

}
