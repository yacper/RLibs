/********************************************************************
    created:	2017/5/20 18:31:49
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
    public class Product<T>:HaveIDAndObservable<T>, IProduct<T>
    {
#region IGottaInit
#region properties
        public EInitState   InitStat { get { return _InitStat; } set { Set("InitStat", ref _InitStat, value); } }                         // init 状态
#endregion

#region virtuals
	    public virtual void Init()
	    {
		    InitStat = EInitState.Initing;

			OnIniting();

		    InitStat = EInitState.Inited;
	    }

	    public virtual void UnInit()
	    {
		    InitStat = EInitState.Uniniting;

			OnUnIniting();

		    InitStat = EInitState.Uninited;
	    }

	    public virtual void	OnIniting(){}
	    public virtual void	OnUnIniting(){}
#endregion

#region Members
	    protected EInitState _InitStat;
#endregion

#endregion

#region
        public object       Manager { get { return m_pManager; } }
        public IFactory     Factory { get { return m_pFactory; } }

#region Dynamic factory 生成
        public void         _SetManager(object m)
        {
            m_pManager = m;
        }
        public virtual void _SetFactory(IFactory f)
        {
            m_pFactory = f;
        }
#endregion
#endregion

#region C&D
        public              Product(T id)
            :base(id)
        {
        }
#endregion

#region Members
        protected object    m_pManager;
        protected IFactory  m_pFactory;
#endregion

    }
    public class DynamicProduct<T> : Product<T>, IDynamicProduct<T>
    {
        public IParams      Params { get { return m_pParams; } }
        public void         _SetParams(IParams p)
        {
            m_pParams = p;
        }
//        public              Product(){}                                     // 需要后面填充

#region C&D
        public              DynamicProduct( T id)
            :base(id)
        {
        }
#endregion
        
        protected IParams   m_pParams;
    }
}
