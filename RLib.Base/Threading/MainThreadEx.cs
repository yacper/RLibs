// created: 2022/07/22 11:54
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using System.Threading;
using System.Threading.Tasks;

namespace RLib.Base;

public static class MainThreadEx
{
    public static void ExecuteInUIThread(this Action action)        // 在ui线程中运行，wpf/winform有效，console错误
    {
        // for Console Application Task will be scheduled to any available thread in ThreadPool ,
        // but for WPF and Winform it will be 1 if Managed ID of Thread is 1

        var synchronization = SynchronizationContext.Current;
        if (synchronization != null)
        {
            synchronization.Send(_ => action(), null);//sync
            //OR
            synchronization.Post(_ => action(), null);//async
        }
        else
            Task.Factory.StartNew(action); // console下的勉强之策

        //var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        //Task task = new Task(action);
        //if (scheduler != null)
        //    task.Start(scheduler);
        //else
        //    task.Start();
    }
}