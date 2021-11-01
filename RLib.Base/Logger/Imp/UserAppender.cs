/********************************************************************
    created:	2018/7/31 9:52:43
    author:		rush
    email:		
	
    purpose:	用于UI的事件，需要主线程
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Core;

namespace RLib.Base
{
    public class UserAppender: AppenderSkeleton
    {
        public IReadonlyObservableCollection<LoggingEvent> Events
        {
            get { return _LoggingEvents; }
        }
        public EventHandler<LoggingEvent> OnEvent;


		override protected void Append(LoggingEvent loggingEvent)
		{
			loggingEvent.Fix = FixFlags.All;

            App.Instance.RunInMainThread(() =>
            {
                // todo: 这里可能需要lock, 暂时不锁
                _LoggingEvents.Add(loggingEvent);

                if (OnEvent != null)
                    OnEvent(this, loggingEvent);


//#if DEBUG
//                System.Diagnostics.Debug.WriteLine(loggingEvent.MessageObject);
//#else

//   // console.writeline("release mode");

//#endif


            });
        }



        override protected bool RequiresLayout
		{
			get { return false; }
		}

        public RObservableCollection<LoggingEvent> _LoggingEvents = new RObservableCollection<LoggingEvent>();
    }
}
