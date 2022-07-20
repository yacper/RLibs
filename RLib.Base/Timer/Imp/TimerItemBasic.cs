/********************************************************************
    created:	2015/11/27 14:45:06
    author:		donghuiqi
    email:		
  
    purpose:	TimerItem 基类
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public abstract class TimerItemBasic : ITimerItem
    {
        public TimerItemBasic(object id, Action<object[]> callback, object[] paras, int times)
        {
            m_id = id;
            m_callback = callback;
            m_paras = paras;
            m_times = times;
        }

        public object ID
        {
            get { return m_id; }
        }

        public DateTime NextTime
        {
            get 
            {
                if (m_nextTime == null)
                    throw new Exception("Timer NextTime 未赋初始值。");

                return m_nextTime.Value; 
            }
        }

        public bool Complete
        {
            get
            {
                if (m_times > 0 && m_executeTimes >= m_times)
                    return true;

                return false;
            }
        }

        public void Init()
        {
            m_nextTime = GenNextTime();
            OnInit();
        }

        protected virtual void OnInit() { }

        public void Call()
        {
            if (m_callback != null)
            {

                List<object> callParas = null;

                if (m_paras != null && m_paras.Length > 0)
                {
                    if (callParas == null)
                        callParas = new List<object>();

                    callParas.AddRange(m_paras);
                }

                object parasAddition = CallParasAddition();
                if (parasAddition != null)
                {
                    if (callParas == null)
                        callParas = new List<object>();
                    callParas.Add(parasAddition);
                }


                if (m_times > 0)
                {
                    if (m_executeTimes >= m_times)
                    {
                        return;
                    }

                    if (m_executeTimes + 1 == m_times)//如果是最后一次执行，则参数列表中加一项true
                    {
                        if (callParas == null)
                            callParas = new List<object>();

                        callParas.Add(true);
                    }
                }
                m_nextTime = GenNextTime();
                if (m_times > 0)
                {
                    m_executeTimes++;
                }
                m_callback(callParas == null ? null : callParas.ToArray());
            }
        }

        protected virtual object CallParasAddition()
        {
            return null;
        }

        protected abstract DateTime GenNextTime();

        protected object        m_id;
        protected Action<object[]> m_callback;
        protected object[]      m_paras;
        protected int           m_times;                                    //需执行次数(小于等于0则执行无数次)
        protected uint          m_executeTimes = 0;                         //已经执行的次数
        protected DateTime?     m_nextTime;                                 //下一次执行时间


    }
}
