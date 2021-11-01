/********************************************************************
    created:	2017/6/21 14:34:36
    author:		rush
    email:		
	
    purpose:	时间序列的对象

*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLib.Base;

namespace RLib.Base
{
    public interface IReadonlyTimeValue :  IComparable
    {
        DateTime            Time { get; }
    }

    public interface ITimeValue : IReadonlyTimeValue
    {
        new DateTime        Time { get; set; }
    }

    public interface ITimeValueSeries : IReadOnlyList<IReadonlyTimeValue>
    {

    }
}
