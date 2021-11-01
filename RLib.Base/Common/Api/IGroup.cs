/********************************************************************
    created:	2017/5/20 16:38:23
    author:		rush
    email:		
	
    purpose:	一个组，可以存放GroupITem<T>

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{

    public interface IGroup<T, TID> where T:IHaveIDAndObservable<TID>
    {
#region Properties
        int                 Count { get; }
        List<T>             Items { get; }

        T                   this[TID index] { get; }

        ObservableCollection<T> ObservableCollection { get; }               // 用于wpf ui binding
#endregion

#region Maniplulators
        bool                Contains(T val);
        bool                Contains(TID id);
        T                   Get(TID id);

        T                   Add(T val);
        void                Add(IEnumerable<T> val);
        bool                Remove(T val);
        bool                Remove(IEnumerable<T> l);                              // 全部移除成功则返回true，否则返回false
        bool                Remove(TID id);                                 // 如果不存在，返回false
        bool                Remove(IEnumerable<TID> id);
        void                RemoveAll();
#endregion

#region virtuals
        void                _OnAdding(T val);                               // 开始添加
        void                _OnAdded(T val);                                // 添加完毕
        void                _OnRemoving(T val);                             // 正要删除
        void                _OnRemoved(T val);                              // 删除完毕
#endregion

#region Events
        event EventHandler<T> OnAddingEvent;
        event EventHandler<T> OnAddedEvent;
        event EventHandler<T> OnRemovingEvent;      
        event EventHandler<T> OnRemovedEvent;       
#endregion
    }
}
