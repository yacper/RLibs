///********************************************************************
//    created:	2020/5/14 19:09:30
//    author:		rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using log4net.Appender;
//using log4net.Layout;

//namespace RLib.Base
//{
//    public static class Log4NetHelper
//    {

//        public static RollingFileAppender CreateRollingFileAppender(string name, string file)
//        {
//            var patternLayout = new PatternLayout();
//            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger [%property{NDC}] - %message%newline";
//            patternLayout.ActivateOptions();

//            RollingFileAppender rfa = new RollingFileAppender();
//            //rfa.File = "Logs/Name/";
//            rfa.File = file;
//            rfa.DatePattern = $"'{name}' yyyy_MM_dd'.log'";
//            rfa.AppendToFile = true;
//            rfa.RollingStyle = RollingFileAppender.RollingMode.Composite;
//            rfa.MaxSizeRollBackups = 10;
//            rfa.MaximumFileSize = "100MB";
//            rfa.StaticLogFileName = false;
//            rfa.Layout = patternLayout;
//            rfa.ActivateOptions();

//            return rfa;
//        }
            



//    }
//}
