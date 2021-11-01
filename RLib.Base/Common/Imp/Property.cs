/********************************************************************
    created:	2017/6/29 18:23:21
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public class Property<T>:IProperty<T> 
    {

        public override string ToString(){ return Value.ToString(); }

        public object       Holder { get { return m_pHolder; } }

        public T Value
        {
            get
            {
                return m_value;
            }
            set
            {
                if (m_value.Equals(value))
                    return;

                T before = m_value;
                m_value = value;

                if (OnChangedEvenet != null)
                {
                    var eve = OnChangedEvenet;
                    eve(Holder, new ChangedEventArgs<T>(before, value));
                }
                
            }
        }

        public static implicit operator T(Property<T> v)
        {
            return v.Value;
        }
      

        public event EventHandler<ChangedEventArgs<T>> OnChangedEvenet;


        public              Property(object holder, T value = default(T))
        {
            m_pHolder = holder;
            m_value = value;
        }


        protected T         m_value;
        protected object    m_pHolder;
    }

    public class ReadOnlyProperty<T>:IReadOnlyProperty<T> 
    {
        public override string ToString(){ return Value.ToString(); }

        public object       Holder { get { return m_pHolder; } }
        public T Value
        {
            get
            {
                return m_value;
            }
        }

        public static implicit operator T(ReadOnlyProperty<T> v)
        {
            return v.Value;
        }

        public void         _Set(T val)
        {
            if (m_value.Equals(val))
                return;

            T before = m_value;
            m_value = val;

            if (OnChangedEvenet != null)
            {
                var eve = OnChangedEvenet;
                eve(Holder, new ChangedEventArgs<T>(before, val));
            }
        }

        public event EventHandler<ChangedEventArgs<T>> OnChangedEvenet;


        public              ReadOnlyProperty(object holder, T value = default (T))
        {
            m_pHolder = holder;
            m_value = value;
        }


        protected T         m_value;
        protected object    m_pHolder;
    }
}
