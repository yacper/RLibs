/********************************************************************
    created:	2021/7/13 10:56:43
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public static class DecimalEx
    {
        public static double ToDouble(this decimal? val, double def = 0)
        {
            if (val == null)
                return def;

            return Convert.ToDouble(val);
        }
    }
}
