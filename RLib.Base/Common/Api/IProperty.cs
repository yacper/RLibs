/********************************************************************
    created:	2017/6/29 18:23:07
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
    //public interface IPropertyHolder        
    //{
        
    //}

    public interface IProperty // todo: 加一些property公共的對象
    {
        string              ToString();

        object              Holder { get; }

    }
    public interface IProperty<T> :IProperty
    {
        T                   Value { get; set; }

        event EventHandler<ChangedEventArgs<T>> OnChangedEvenet;
    }

    public interface IReadOnlyProperty<T>:IProperty
    {
        T                   Value { get; }
        void                _Set(T val);

        event EventHandler<ChangedEventArgs<T>> OnChangedEvenet;
    }
}
