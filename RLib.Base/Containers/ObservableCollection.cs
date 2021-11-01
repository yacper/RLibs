/********************************************************************
    created:	2018/1/26 11:32:01
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public class  RObservableCollection<T>:ObservableCollection<T>, IReadonlyObservableCollection, IReadonlyObservableCollection<T>, IObservableCollection<T>
    {
        object              IReadonlyObservableCollection.this[int index] { get { return this[index]; } }


        public void         AddRange(IEnumerable<T> co)
        {
            foreach (T v in co)
            {
                Add(v);
            }
        }

        public void         AddRange(IEnumerable co)
        {
            foreach (T v in co)
            {
                Add(v);
            }
        }

        public void         RemoveRange(IEnumerable<T> co)
        {
            foreach (T v in co)
            {
                Remove(v);
            }
        }

        public void         RemoveRange(IEnumerable co)
        {
            foreach (T v in co)
            {
                Remove(v);
            }
        }

        public T                   Front()
        {
            if (Count == 0)
                return default(T);

            return this[0];
        }
        public T                   Back()
        {
            if (Count == 0)
                return default(T);

            return this[Count - 1];
        }

        object                   IReadonlyObservableCollection.Front()
        {
            return this.Front();
        }
        object                   IReadonlyObservableCollection.Back()
        {
            return this.Back();
        }
    }


    public static class ExtendObservableCollection
    {

        public static void  AddRange<T>(this ObservableCollection<T> p, IEnumerable<T> co)
        {
            foreach (T v in co)
            {
                p.Add(v);
            }
        }

        public static void  RemoveRange<T>(this ObservableCollection<T> p, IEnumerable<T> co)
        {
            foreach (T v in co)
            {
                p.Remove(v);
            }
        }

        public static T     Front<T>(this ObservableCollection<T> p) 
        {
            DebuggingEx.EnsureNonempty(p);

            return p[0];
        }
        public static T     Back<T>(this ObservableCollection<T> p)
        {
            DebuggingEx.EnsureNonempty(p);

            return p[p.Count -1];
        }


    }

}
