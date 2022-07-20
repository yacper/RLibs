/********************************************************************
    created:	2015/11/27 14:45:06
    author:		donghuiqi
    email:		
  
    purpose:	TimerItem接口
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
public enum EWeek
{
    Monday    = 1,
    Tuesday   = 2,
    Wednesday = 4,
    Thursday  = 8,
    Friday    = 16,
    Saturday  = 32,
    Sunday    = 64,
    Workday   = 31,
    DayOff    = 96,
    Everyday  = 127,
}


    public interface ITimerItem
    {
        object              ID { get; }                                     //ID
        DateTime            NextTime { get; }                               //下次执行时间
        bool                Complete { get; }                               //完成状态

        void                Init();                                         //初始化
        void                Call();                                         //调用

    }
}
