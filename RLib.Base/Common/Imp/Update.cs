/********************************************************************
    created:	2017/11/13 18:14:43
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public class Update
    {
#region IUpdate
        public virtual void OnUpdate(float delta)
        {
            __FireUpdate(delta);
        }

        protected void  __FireUpdate(float delta)
        {
            if (OnUpdateEvent != null)
            {
                var e = OnUpdateEvent;
                e(this, delta);
            }
        }

        public event EventHandler<float> OnUpdateEvent;
#endregion
    }
}
