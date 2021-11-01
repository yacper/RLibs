/********************************************************************
    created:	2018/8/15 15:01:38
    author:		rush
    email:		
	
    purpose:	简单网络监控, 使用的PerformanceCounter
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
	public interface INetworkMeter:IDisposable, INotifyPropertyChanged
	{
		bool				Run { get; set; }

		TimeSpan			Duration { get; }								// 过了多久

		float				BytesSent { get; }
		float				BytesReceived { get; }

		float				BytesSentPerSec { get; }
		float				BytesReceivedPerSec { get; }
	}
}
