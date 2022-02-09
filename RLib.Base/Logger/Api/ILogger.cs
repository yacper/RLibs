/********************************************************************
    created:	2017/5/20 18:08:40
    author:		rush
    email:		
	
    purpose:	

*********************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public enum ELogLevel
    {
        Info            = 1,
        Warn            = 2,
        Error           = 4,
        All             = Info|Warn|Error 
    }

    public static class Logger
    {
        public static void Info(object o, string tag= null)
        {
            DefaultLogger?.Info(o, tag);
        }

        public static void Warn(object o, string tag= null)
        {
            DefaultLogger?.Warn(o, tag);
        }
        public static void Error(object o, string tag= null)
        {
            DefaultLogger?.Error(o, tag);
        }

        public static ILogger DefaultLogger;
    }


    public interface ILogger
    {
        void                Info(object msg, string tag = null);    
        void                Warn(object msg, string tag = null);    
        void                Error(object msg, string tag = null);    


        IReadonlyObservableCollection<string> Tags { get; }          // 额外tag

        IReadonlyObservableCollection<ILogMsg> Msgs { get; }          // msgs
    }

    public interface ILogMsg
    {
        DateTime            Time { get; }
        ELogLevel           Level { get; }
        string              Tag { get; }
        object              Msg { get; }
    }

}
