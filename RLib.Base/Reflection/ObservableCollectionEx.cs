// created: 2023/03/02 18:59
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace RLib.Base
{


public static class ObservableCollectionEx
{
    // 将col的内容，设置为跟other一样的
    public static void Apply<T>(this ObservableCollection<T> col, ObservableCollection<T> other)
    {
        col.RemoveRange(col.Except(other).ToList());
        col.AddRange(other.Except(col).ToList());
    }

    public static void AddRange<T>(this ObservableCollection<T> p, IEnumerable<T> co)
    {
        foreach (T v in co) { p.Add(v); }
    }

    public static void RemoveRange<T>(this ObservableCollection<T> p, IEnumerable<T> co)
    {
        foreach (T v in co) { p.Remove(v); }
    }


}
}