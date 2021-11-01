/********************************************************************
    created:	2016/11/9 16:48:21
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RLib.Base
{
    public interface IHaveID<T>
    {
        T                   ID { get; }
    }

    public interface IHaveIDAndObservable<T>:INotifyPropertyChanged
    {
        T                   ID { get; }
    }
}
