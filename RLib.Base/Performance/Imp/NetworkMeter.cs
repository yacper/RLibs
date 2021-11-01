///********************************************************************
//    created:	2018/8/15 15:01:08
//    author:		rush
//    email:		
	
//    purpose:	网络简单监控

//			//NetworkMeter nm = new NetworkMeter();
//			//nm.PropertyChanged += (s, e) =>
//			//{
//			//	if (e.PropertyName == "BytesReceivedPerSec")
//			//		DownloadSpeed = (s as NetworkMeter).BytesReceivedPerSec;
//			//};
//			//nm.Run = true;
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Runtime.Versioning;
//using System.Text;
//using System.Threading.Tasks;


//namespace RLib.Base
//{
//	public class NetworkMeter:ObservableObject, INetworkMeter
//	{
//		public void			Dispose()
//		{
//			App.Instance.OnUpdateEvent -= OnUpdate;
//		}

//		public bool			Run { get { return _Run; } set { Set("Run", ref _Run, value); } }


//		public void			OnUpdate(object sender, float delta)
//		{
//			if(!Run)
//				return;

//			Duration+= new TimeSpan(0, 0, 0, (int)(delta*1000));

//			float deltaRecv = _RecvCounter.NextValue() - BytesReceived;
//			float deltaSent = _SentCounter.NextValue() - BytesSent;

//			_PerSecBytesReceived += deltaRecv;
//			_PerSecBytesSent += deltaSent;
//			_PerSecCountTime += delta;		

//			/// 满1s计算
//			if (_PerSecCountTime >= 1)
//			{
//				BytesReceivedPerSec = _PerSecBytesReceived / _PerSecCountTime/ 1024;
//				BytesSentPerSec = _PerSecBytesSent / _PerSecCountTime /1024;

//				_PerSecCountTime = 0;
//				_PerSecBytesReceived = 0;
//				_PerSecBytesSent = 0;
//			}
			
//			BytesReceived =_RecvCounter.NextValue();
//			BytesSent = _SentCounter.NextValue();

//			RaisePropertyChanged("Duration");
//			RaisePropertyChanged("BytesSent");
//			RaisePropertyChanged("BytesReceived");
//			RaisePropertyChanged("BytesSentPerSec");
//			RaisePropertyChanged("BytesReceivedPerSec");
//		}

//		public TimeSpan		Duration { get; protected set; }								// 过了多久

//		public float		BytesSent { get; protected set; }
//		public float		BytesReceived { get; protected set; }

//		public float		BytesSentPerSec { get; protected set; }
//		public float		BytesReceivedPerSec { get; protected set; }

//#region C&D
//		public				NetworkMeter()
//		{
//			_SentCounter = new PerformanceCounter();
//			_SentCounter.CategoryName = ".NET CLR Networking 4.0.0.0";
//			_SentCounter.CounterName = "Bytes Sent";
//			_SentCounter.InstanceName = GetInstanceName();
//			_SentCounter.ReadOnly = true;

//			_RecvCounter = new PerformanceCounter();
//			_RecvCounter.CategoryName = ".NET CLR Networking 4.0.0.0";
//			_RecvCounter.CounterName = "Bytes Received";
//			_RecvCounter.InstanceName = GetInstanceName();
//			_RecvCounter.ReadOnly = true;


//			App.Instance.OnUpdateEvent += OnUpdate;
//		}

//		private string		GetInstanceName()
//		{
//			var processFileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
//			var instanceName = VersioningHelper.MakeVersionSafeName(processFileName, ResourceScope.Machine, ResourceScope.AppDomain);
//			return instanceName;
//		}
//#endregion

//#region Members
//		protected PerformanceCounter _SentCounter;
//		protected PerformanceCounter _RecvCounter;


//		protected bool		_Run;

//		protected float		_PerSecCountTime;								// 每s计算的时间
//		protected float		_PerSecBytesSent;
//		protected float		_PerSecBytesReceived;

//#endregion
//	}
//}
