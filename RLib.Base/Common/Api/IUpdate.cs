/********************************************************************
    created:	2016/11/5 19:21:32
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
    public interface IUpdate
    {
        void            OnUpdate(float deltaTime);

        event EventHandler<float> OnUpdateEvent;							// 参数为DeltaTime
    }
    
}
