/********************************************************************
    created:	2017/5/20 16:10:14
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
    public class HaveID<T>:IHaveID<T>
    {
#region IHaveID<T>
#region properties
        public T            ID { get { return m_tID; } set { m_tID = value; }}
#endregion

#region C&D
        public              HaveID(T id)
        {
            m_tID = id;
        }
#endregion

#region Members
        protected T         m_tID;
#endregion
#endregion
    }

    public class HaveIDAndObservable<T>:ObservableObject,IHaveIDAndObservable<T>
    {
#region IHaveID<T>
#region properties
        public T            ID { get { return m_tID; } set { m_tID = value; }}
#endregion

#region C&D
        public              HaveIDAndObservable(T id)
        {
            m_tID = id;
        }
#endregion

#region Members
        protected T         m_tID;
#endregion
#endregion
    }
}
