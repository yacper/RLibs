/********************************************************************
    created:	2020/1/10 15:26:42
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RLib.Base
{
    public static class ThreadEx
    {
        public static void ExecuteInUIThread(this Action action) // 在ui线程中运行，wpf/winform有效，console错误
        {
            // for Console Application Task will be scheduled to any available thread in ThreadPool ,
            // but for WPF and Winform it will be 1 if Managed ID of Thread is 1

            var synchronization = SynchronizationContext.Current;
            if (synchronization != null)
            {
                synchronization.Send(_ => action(), null); //sync
                //OR
                synchronization.Post(_ => action(), null); //async
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

        public static void SleepRandom(int minMill, int maxMill)
        {
            if(maxMill<minMill ||
               minMill<=0)
                throw new ArgumentException(string.Format("Bad Args [%0,%1]:%2", minMill, maxMill));

            int delay = MathEx.Random(minMill, maxMill);
            System.Threading.Thread.Sleep(delay);
        }


        #region affinity/ core pin
		[DllImport("kernel32.dll")]
		public static extern int GetCurrentThreadId();

        [DllImport("libc.so.6", EntryPoint = "syscall", SetLastError = true)]
        public static extern long syscall0(long number);

		// 该函数测试多次无法成功
        [DllImport("libc.so.6", SetLastError = true)]
        public static extern int sched_getaffinity(int pid, IntPtr cpusetsize, ref ulong  cpuset);

        [DllImport("libc.so.6", SetLastError = true)]
        public static extern int sched_setaffinity(int pid, IntPtr cpusetsize, ref ulong cpuset);

		// 获取native thread id
        public static int GetNativeThreadId()			
        {
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				int id = GetCurrentThreadId();
				return id;
			}
			else if (Environment.OSVersion.Platform == PlatformID.Unix) // linux 也会被认为unix
			{
				var sysGetId = (System.Runtime.InteropServices.RuntimeInformation.OSArchitecture == Architecture.Arm ||
									System.Runtime.InteropServices.RuntimeInformation.OSArchitecture == Architecture.Arm64)
										? 224
										: 186;
				var threadId = (int)syscall0(sysGetId);
				return threadId;
			}

			throw new NotImplementedException();
		}

		public static bool PinThreadToCore(int nativeThreadId, int cpunum)  // 设置thread affinity
		{
			ulong affinity = GetAffinityMask(cpunum);

			try
			{

				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					ProcessThread pt =
						(from ProcessThread th in Process.GetCurrentProcess().Threads
						 where th.Id == nativeThreadId
						 select th).Single();

					pt.ProcessorAffinity = new IntPtr((int) affinity);
					return true;
				}
				else if (Environment.OSVersion.Platform == PlatformID.Unix) // linux 也会被认为unix
				{
					
					int ret = sched_setaffinity(nativeThreadId, new IntPtr(sizeof(ulong)), ref affinity);
					return ret == 0;
				}

			}
			catch (Exception e)
			{
				return false;
			}

			throw new NotImplementedException();

		}

		static ulong GetAffinityMask(int cpunum)  // cpu 从0开始
		{// cpunum从1开始
			/* mask 可以|, 这里只支持单个cpu
				cpu0 - 1 - 0x0001
				cpu1 - 2 - 0x0010
				cpu2 - 4 - 0x0100
				cpu3 - 8 - 0x1000
				 */
			return 1UL << cpunum;
		}
        #endregion

	}
}
