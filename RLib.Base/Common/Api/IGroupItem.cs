/********************************************************************
    created:	2017/5/20 16:36:00
    author:		rush
    email:		
	
    purpose:	组中间的一个项
 *              必须有一个标识ID，以放到组里区分

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{

    public interface IGroupItem<T>:IHaveIDAndObservable<T>
    {
    }
}
