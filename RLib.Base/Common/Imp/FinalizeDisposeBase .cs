// created: 2022/09/15 14:26
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:Finalize Dispose 基类 https://www.cnblogs.com/dotnet261010/p/12330981.html
// modifiers: 

using System;

namespace RLib.Base;

public class FinalizeDisposeBase : IDisposable
{
#region FinalizeDispose
    // Finalize方法
    ~FinalizeDisposeBase() { Dispose(false); }

    /// <summary>
    /// 这里实现了IDisposable中的Dispose方法
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        // 告诉GC此对象的Finalize方法不再需要调用
        GC.SuppressFinalize(true);
    }

    /// <summary>
    /// 在这里做实际的析构工作
    /// 声明为虚方法以供子类在必要时重写
    /// </summary>
    /// <param name="isDisposing"></param>
    protected virtual void Dispose(bool isDisposing)
    {
        // 当对象已经被析构时，不在执行
        if (_disposed) { return; }

        if (isDisposing)
        {
            // 在这里释放托管资源
            // 只在用户调用Dispose方法时执行
        }

        // 在这里释放非托管资源
        // 标记对象已被释放
        _disposed = true;
    }

    // 标记对象是否已被释放
    private bool _disposed = false;
#endregion
}