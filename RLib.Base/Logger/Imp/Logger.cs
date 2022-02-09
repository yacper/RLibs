/********************************************************************
    created:	2017/5/20 18:09:31
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
    public partial class LoggerImp:ILogger
    {
        public void Info(object msg, string tag = null)
        {

        }

        public void Warn(object msg, string tag = null)
        {

        }

        public void Error(object msg, string tag = null)
        {

        }


        public IReadonlyObservableCollection<string> Tags { get; } // 额外tag

        public IReadonlyObservableCollection<ILogMsg> Msgs { get; } // msgs


    }

    public class LogMsg:ILogMsg
    {
        public DateTime Time { get; set; }
        public ELogLevel Level { get; set; }
        public string Tag { get; set; }
        public object Msg { get; set; }

        public override string ToString()
        {
            return $"{Time}[{Level}][{Tag}]:{Msg}";
        }
    }

}

