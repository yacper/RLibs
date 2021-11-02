/********************************************************************
    created:	2021/8/25 18:14:17
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	符合sqlite的标准，sqlite无法存储nan，这里用double.minvalue代表nan
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Db.Ef
{
    public static class SqliteHelper
    {
        public static double ToDbDouble(this double val)
        {
            if (double.IsNaN(val))
                return double.MinValue;
            else
                return val;
        }

        public static double FromDbDouble(this double val)
        {
            if (val == double.MinValue)
                return double.NaN;
            else
                return val;
        }


    }
}
