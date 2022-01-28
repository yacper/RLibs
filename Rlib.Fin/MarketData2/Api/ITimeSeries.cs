/********************************************************************
    created:	2019/9/17 20:03:39
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
    public interface ITimeSeries:IReadOnlyList<DateTime>
    {
        DateTime            LastValue { get; }
        DateTime            Last(int index);

        int                 IndexOf(DateTime item);
    }
}
