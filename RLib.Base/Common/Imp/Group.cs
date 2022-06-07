///********************************************************************
//    created:	2017/5/20 16:47:53
//    author:		rush
//    email:		
	
//    purpose:	

//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace RLib.Base
//{
//    public class Group<T, TID>:ObservableObject, IGroup<T, TID> where T:IHaveIDAndObservable<TID>
//    {
//#region Properties
//        public int          Count { get { return m_dGroupItems.Count; } }
//        public List<T> Items { get { return m_dGroupItems.Values.ToList(); } }

//        public T            this[TID index]
//        {
//            get { return Get(index); }
//        }

//        public ObservableCollection<T> ObservableCollection { get { return m_pObservableCollection; } }               // 用于wpf ui binding

//        #endregion

//#region Maniplulators
//        public virtual bool Contains(T val)
//        {
//            return Contains(val.ID);
//        }
//        public virtual bool Contains(TID id)
//        {
//            return m_dGroupItems.ContainsKey(id);
//        }
//        public virtual T Get(TID id)
//        {
//            T i;
//            m_dGroupItems.TryGetValue(id, out i);

//            return i;
//        }

//        public virtual T Add(T val)
//        {
//            System.Diagnostics.Debug.Assert(val != null);

//            if (Contains(val.ID))
//                throw new Exception(this + " Already have:(" + val.ID + ")" + val.ToString());

//            _OnAdding(val);
//            _FireOnAddingEvent(val);

//            m_dGroupItems.Add(val.ID, val);
//            m_pObservableCollection.Add(val);

//            _OnAdded(val);
//            _FireOnAddedEvent(val);

//            return val;
//        }
//        public virtual void Add(IEnumerable<T> val)
//        {
//            System.Diagnostics.Debug.Assert(val != null);

//            foreach (T i in val)
//                Add(i);
//        }
//        public virtual bool Remove(T val)
//        {
//            System.Diagnostics.Debug.Assert(val != null);

//            return Remove(val.ID);
//        }
//        public virtual bool Remove(IEnumerable<T> val)
//        {
//            System.Diagnostics.Debug.Assert(val != null);

//            bool ret = true;
//            foreach (var i in val)
//            {
//                if(!Remove(i))
//                    ret = false;
//            }
//            return ret;
//        }
//        public virtual bool Remove(TID id)
//        {
//            if (!Contains(id))
//                return false;

//            T val = Get(id);

//            _OnRemoving(val);
//            _FireOnRemovingEvent(val);          // 移除前

//            m_dGroupItems.Remove(val.ID);
//            m_pObservableCollection.Remove(val);

//            _OnRemoved(val);
//            _FireOnRemovedEvent(val);           // 移除后

//            return true;
//        }
//        public virtual bool Remove(IEnumerable<TID> val)                             // 全部移除成功，才会true
//        {
//            System.Diagnostics.Debug.Assert(val != null);

//            bool ret = true;
//            foreach (var i in val)
//            {
//                if(!Remove(i))
//                    ret = false;
//            }
//            return ret;
//        }
//        public virtual void RemoveAll()
//        {
//            var keys = m_dGroupItems.Keys.ToArray(); // shouldn't just reference key list
//            foreach (TID k in keys)
//                Remove(k);
//        }


//#endregion

//#region virtuals
//        public virtual void _OnAdding(T val){}
//        public virtual void _OnAdded(T s){}
//        public virtual void _OnRemoving(T s){}
//        public virtual void _OnRemoved(T val){}                  // 删除完毕
//#endregion

//        #region Events
//        public event EventHandler<T> OnAddingEvent;
//        public event EventHandler<T> OnAddedEvent;
//        public event EventHandler<T> OnRemovingEvent;      
//        public event EventHandler<T> OnRemovedEvent;       

//        public void         _FireOnAddingEvent(T val)
//        {
//            if (OnAddingEvent != null)
//            {
//                var eve = OnAddingEvent;
//                eve(this, val);
//            }
//        }
//        public void         _FireOnAddedEvent(T val)
//        {
//            if (OnAddedEvent != null)
//            {
//                var eve = OnAddedEvent;
//                eve(this, val);
//            }
//        }
//        public void         _FireOnRemovingEvent(T val)
//        {
//            if (OnRemovingEvent!= null)
//            {
//                var eve = OnRemovingEvent;
//                eve(this, val);
//            }
//        }
//        public void         _FireOnRemovedEvent(T val)
//        {
//            if (OnRemovedEvent!= null)
//            {
//                var eve = OnRemovedEvent;
//                OnRemovedEvent(this, val);
//            }
//        }
//#endregion

//#region Members
//        protected Dictionary<TID, T> m_dGroupItems = new Dictionary<TID, T>();

//        protected ObservableCollection<T> m_pObservableCollection = new ObservableCollection<T>(); // 用于wpf ui binding

//        #endregion
//    }
//}
