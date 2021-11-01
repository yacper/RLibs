/*
 static void Main(string[] args)
		{
			AffinityThread t1 = new AffinityThread(PrintNumbers, 1);
			t1.ManagedThread.Name = "t1";
			t1.Start();

			AffinityThread t2 = new AffinityThread(PrintNumbersWithParam, 2);
			t2.ManagedThread.Name = "t2";
			t2.Start(10);

			Console.ReadLine();
		}

		static void PrintNumbers()
		{
			PrintNumbersWithParam(5);
		}
		static void PrintNumbersWithParam(object number)
		{
			for (int i = 0; i != (int)number; ++i)
			{
				Console.WriteLine(i);
				Thread.Sleep(1000);
			}
		}
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RLib.Base
{
	public interface IAffinityThread
	{
		Thread ManagedThread { get; }
		int ProcessorAffinity { get; }

		void Start();
		void Start(object parameter);
	}

	public class AffinityThread : IAffinityThread
	{
		public Thread ManagedThread { get; protected set; }
		public int ProcessorAffinity { get; protected set; } // cpu 从1开始


		private AffinityThread(int affinity = 0)
		{
			ProcessorAffinity = affinity;
			ManagedThread     = new Thread(AffinityThreadStart);
		}

		public AffinityThread(ThreadStart threadStart, int affinity = 0)
			: this(affinity)
		{
			this.threadStart = threadStart;
		}

		public AffinityThread(ParameterizedThreadStart threadStart, int affinity = 0)
			: this(affinity)
		{
			this.parameterizedThreadStart = threadStart;
		}

		public void Start()
		{
			if (this.threadStart == null)
				throw new InvalidOperationException();

			ManagedThread.Start(null);
		}

		public void Start(object parameter)
		{
			if (this.parameterizedThreadStart == null)
				throw new InvalidOperationException();

			ManagedThread.Start(parameter);
		}

		private void AffinityThreadStart(object parameter)
		{
			try
			{
				// fix to OS thread
				Thread.BeginThreadAffinity();

				// set affinity
				if (ProcessorAffinity != 0)
				{
					ThreadEx.PinThreadToCore(ThreadEx.GetNativeThreadId(), ProcessorAffinity);
				}

				// call real thread
				if (this.threadStart != null)
				{
					this.threadStart();
				}
				else if (this.parameterizedThreadStart != null)
				{
					this.parameterizedThreadStart(parameter);
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
			finally
			{
				// reset affinity
				//CurrentThread.ProcessorAffinity = new IntPtr(0xFFFF);
				Thread.EndThreadAffinity();
			}
		}

		private ThreadStart threadStart;
		private ParameterizedThreadStart parameterizedThreadStart;
	}
}
