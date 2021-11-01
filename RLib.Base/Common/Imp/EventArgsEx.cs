/********************************************************************
    created:	2017/5/20 15:47:34
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public class EventArgs<T> : EventArgs
    {
        public T            Value { get { return m_tValue; }}

        public              EventArgs(T val)
        {
            m_tValue = val;
        }

        protected T         m_tValue;
    }

    public class EventArgs<T1, T2> : EventArgs
    {
        public T1            Value1 { get { return m_pValue1; }}
        public T2            Value2 { get { return m_pValue2; }}

        public              EventArgs(T1 val1, T2 val2)
        {
            m_pValue1 = val1;
            m_pValue2 = val2;
        }

        protected T1         m_pValue1;
        protected T2         m_pValue2;
    }
    public class EventArgs<T1, T2, T3> : EventArgs
    {
        public T1            Value1 { get { return m_pValue1; }}
        public T2            Value2 { get { return m_pValue2; }}
        public T3            Value3 { get { return m_pValue3; }}

        public              EventArgs(T1 val1, T2 val2, T3 val3)
        {
            m_pValue1 = val1;
            m_pValue2 = val2;
            m_pValue3 = val3;
        }

        protected T1         m_pValue1;
        protected T2         m_pValue2;
        protected T3         m_pValue3;
    }

    public class ChangedEventArgs<T>:EventArgs                              // 值发生了改变
    {
        public T            Before { get { return m_tBefore; }}
        public T            After { get { return m_tAfter; }}

        public              ChangedEventArgs(T before, T after)
        {
            m_tBefore = before;
            m_tAfter = after;
        }

        protected T m_tBefore;
        protected T m_tAfter;
    }
}
