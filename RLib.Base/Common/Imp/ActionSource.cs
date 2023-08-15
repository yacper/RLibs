// created: 2023/08/15 22:45
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using System.Collections.Generic;
using System.Linq;

namespace RLib.Base;

public class ActionSource<T>
{
    public void Invoke(T? sender)
    {
        OnEvent?.Invoke(sender);
    }

    public void Sub(Action<T> handler, object source = null)
    {
        if (source != null)
        {
            if (!SourceHandlers_.ContainsKey(source))
                SourceHandlers_[source] = new();

            SourceHandlers_[source].Add(handler);
        }

        OnEvent += handler;
    }
    public void UnSub(Action<T> handler, object source = null)
    {
        if (source != null)
        {
            // 如果多个相同，只需删除一个就行了
            if (SourceHandlers_.ContainsKey(source))
                SourceHandlers_[source].Remove(SourceHandlers_[source].FirstOrDefault(p=>p == handler));
        }

        OnEvent -= handler;
    }

    public void RemoveAll(object source)
    {
        if (source == null)
            return;

        if (SourceHandlers_.TryGetValue(source, out List<Action<T>> l))
        {
            foreach (Action<T> handler in l)
            {
                OnEvent -= handler;
            }

            SourceHandlers_.Remove(source);
        }
    }

    
    protected event Action<T> OnEvent;
    protected Dictionary<object, List<Action<T>>> SourceHandlers_ = new();
}

public class ActionSource
{
    public void Invoke()
    {
        OnEvent?.Invoke();
    }

    public void Sub(Action handler, object source = null)
    {
        if (source != null)
        {
            if (!SourceHandlers_.ContainsKey(source))
                SourceHandlers_[source] = new();

            SourceHandlers_[source].Add(handler);
        }

        OnEvent += handler;
    }
    public void UnSub(Action handler, object source = null)
    {
        if (source != null)
        {
            // 如果多个相同，只需删除一个就行了
            if (SourceHandlers_.ContainsKey(source))
                SourceHandlers_[source].Remove(SourceHandlers_[source].FirstOrDefault(p=>p == handler));
        }

        OnEvent -= handler;
    }

    public void RemoveAll(object source)
    {
        if (source == null)
            return;

        if (SourceHandlers_.TryGetValue(source, out List<Action> l))
        {
            foreach (Action handler in l)
            {
                OnEvent -= handler;
            }

            SourceHandlers_.Remove(source);
        }
    }

    
    protected event Action OnEvent;
    protected Dictionary<object, List<Action>> SourceHandlers_ = new();
}