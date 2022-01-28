/********************************************************************
    created:	2019/9/17 20:08:58
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoLib
{
    public class TimeSeries : List<DateTime>, ITimeSeries
    {
        public DateTime LastValue
        {
            get { return this.LastOrDefault(); }
        }

        public DateTime     Last(int index)
        {
            if (index <= Count - 1)
            {
                return this[Count - index];
            }
            else
                return default(DateTime);
        }
    }
}
