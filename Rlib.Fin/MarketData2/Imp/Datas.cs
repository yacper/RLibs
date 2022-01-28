/********************************************************************
    created:	2019/9/13 14:56:14
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using RLib.Base;

namespace NeoLib
{
    public class Datas : List<double>, IDatas
    {
        public string       Tag { get; set; }                                // 用于标识

        public override string ToString()
        {
            if (Tag == null)
                return "";

            if (Tag.Contains('&')) // Indicator的输出
            {
                string[] ss = Tag.Split('&');
                string relyIndicatorID = ss[0];
                string relyIndicatorName = ss[1];
                string dsName = ss[2];

                return relyIndicatorName + " " + dsName;
            }
            else
                return Tag;     // MarketSeries ds
        }

        public string       SerializeString()
        {
            return Tag.NullableToString();
        }

        public object         DeserializeString(string str)
        {
            return new Datas() {Tag = str};
        }


        public new double this[int index]
        {
            get
            {
                if (index >= this.Count||
                    index < 0)
                    return double.NaN;
                else
                    return base[index];
            }
            set
            {
                if ( index ==  this.Count)
                        base.Add(value);
                else if ( index >  this.Count)
                {// 中间的用double.NaN填充
                    for (int i = Count; i < index; ++i)
                        base.Add(double.NaN);

                    base.Add(value);
                }
                else
                    base[index] = value;

            }
        }


        public int          FirstNotNanIndex
        {
            get
            {
                for (int i = 0; i != this.Count; ++i)
                {
                    if (!double.IsNaN(this[i]))
                        return i;
                }

                return -1;
            }
        } // 第一有有意义（非NAN）的Index


        public double       LastValue
        {
            get
            {
                if (this.Count != 0)
                    return this[Count - 1];
                else
                    return double.NaN;
            }
        }

        public double       Last(int index = 0)
        {
            if (index <= Count - 1)
            {
                return this[Count -1 - index];
            }
            else
                return double.NaN;
        }


    }


}
