/********************************************************************
    created:	2015/11/27 14:45:06
    author:		donghuiqi
    email:		
  
    purpose:	TimerItem 间隔执行
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public class TimerItemInterval : TimerItemBasic
    {
        public TimerItemInterval(object id, Action<object[]> callback, object[] paras, TimeSpan timeSpan, int times)
            : base(id, callback, paras, times)
        {
            m_timeSpan = timeSpan;
        }

        protected override DateTime GenNextTime()
        {
            if (m_nextTime == null)
            {
                return DateTime.Now.Add(m_timeSpan);
            }
            else
            {
                return m_nextTime.Value.Add(m_timeSpan);
            }
        }

        TimeSpan m_timeSpan;                                 //执行间隔
    }
}
