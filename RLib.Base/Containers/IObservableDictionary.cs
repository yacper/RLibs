/********************************************************************
    created:	2018/1/26 11:10:25
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public interface IObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
        ICollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        System.Runtime.Serialization.ISerializable,
        IDeserializationCallback,
        INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        bool                IsHaveObservableCollection { get; }
        IReadonlyObservableCollection<TValue> ObservableCollection { get; } // 不可操作，只有展示
    }

    public interface IReadonlyObservableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>,
        System.Runtime.Serialization.ISerializable,
        IDeserializationCallback,
        INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        
        bool                IsHaveObservableCollection { get; }
        IReadonlyObservableCollection<TValue> ObservableCollection { get; }// 不可操作，只有展示
    }

}
