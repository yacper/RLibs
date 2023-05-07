///********************************************************************
//    created:	2020/6/15 22:53:30
//    author:		rush
//    email:		
	
//    purpose:	只是在vs中显示信息
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using log4net.Appender;
//using log4net.Core;

//namespace RLib.Base
//{
//    public class VsAppender:AppenderSkeleton
//    {
//        override protected void Append(LoggingEvent loggingEvent)
//        {

//#if DEBUG
//			loggingEvent.Fix = FixFlags.All;
//	        System.Diagnostics.Debug.WriteLine(loggingEvent.MessageObject);
//#else

//   // console.writeline("release mode");

//#endif

//        }

//        override protected bool RequiresLayout
//		{
//			get { return false; }
//		}

//    }
//}
