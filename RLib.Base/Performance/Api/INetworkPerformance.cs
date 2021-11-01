/********************************************************************
    created:	2018/8/15 10:59:20
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
	public interface INetworkPerformance:IPerformance
	{
		float				BytesSent { get; }
		float				BytesReceived { get; }

		float				BytesSentPerSec { get; }
		float				BytesReceivedPerSec { get; }

	}
}
