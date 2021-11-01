/********************************************************************
    created:	2020/3/14 18:55:33
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

namespace RLib.Base
{
    public static class RectEx
    {
        public static Point CenterPoint(this Rect r)
        {
            return new Point(r.Left + r.Width/2, r.Top + r.Height/2);
        }

    }
}
