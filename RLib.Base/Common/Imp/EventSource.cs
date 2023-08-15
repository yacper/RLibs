// created: 2023/08/15 16:23
// author:  rush
// email:   yacper@gmail.com
// 
// purpose: 带source的event，为了实现，可以删除所以from source的event handler
// modifiers:

using System;
using System.Collections.Generic;
using System.Linq;

namespace RLib.Base;

public class EventSource<T>
{

    public void Invoke(object? sender, T args)
    {
        OnEvent?.Invoke(sender, args);
    }

    public void Sub(EventHandler<T> handler, object source = null)
    {
        if (source != null)
        {
            if (!SourceHandlers_.ContainsKey(source))
                SourceHandlers_[source] = new();

            SourceHandlers_[source].Add(handler);
        }

        OnEvent += handler;
    }
    public void UnSub(EventHandler<T> handler, object source = null)
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

        if (SourceHandlers_.TryGetValue(source, out List<EventHandler<T>> l))
        {
            foreach (EventHandler<T> handler in l)
            {
                OnEvent -= handler;
            }

            SourceHandlers_.Remove(source);
        }
    }

    
    protected event EventHandler<T> OnEvent;
    protected Dictionary<object, List<EventHandler<T>>> SourceHandlers_ = new();
}

public class EventSource
{

    public void Invoke(object? sender, EventArgs args)
    {
        OnEvent?.Invoke(sender, args);
    }

    public void Sub(EventHandler handler, object source = null)
    {
        if (source != null)
        {
            if (!SourceHandlers_.ContainsKey(source))
                SourceHandlers_[source] = new();

            SourceHandlers_[source].Add(handler);
        }

        OnEvent += handler;
    }
    public void UnSub(EventHandler handler, object source = null)
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

        if (SourceHandlers_.TryGetValue(source, out List<EventHandler> l))
        {
            foreach (EventHandler handler in l)
            {
                OnEvent -= handler;
            }

            SourceHandlers_.Remove(source);
        }
    }

    
    protected event EventHandler OnEvent;
    protected Dictionary<object, List<EventHandler>> SourceHandlers_ = new();
}