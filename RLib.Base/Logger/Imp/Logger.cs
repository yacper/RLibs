/********************************************************************
    created:	2017/5/20 18:09:31
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
public class LoggerImp : ILogger
{
    public virtual void Info(object msg, string tag = null)
    {
        Msgs_.Add(new LogMsg(ELogLevel.Info, msg, tag));
        Tags_.Add(tag);
    }

    public virtual void Warn(object msg, string tag = null)
    {
        Msgs_.Add(new LogMsg(ELogLevel.Warn, msg, tag));
        Tags_.Add(tag);
    }

    public virtual void Error(object msg, string tag = null)
    {
        Msgs_.Add(new LogMsg(ELogLevel.Error, msg, tag));
        Tags_.Add(tag);
    }


    public IReadOnlyCollection<string>           Tags => Tags_;
    public ReadOnlyObservableCollection<ILogMsg> Msgs { get; protected set; } // msgs


    public LoggerImp()
    {
        Msgs = new ReadOnlyObservableCollection<ILogMsg>(Msgs_);
    }


    protected HashSet<string> Tags_ = new HashSet<string>();

    protected ObservableCollection<ILogMsg> Msgs_ = new ObservableCollection<ILogMsg>();
}

public class LogMsg : ILogMsg
{
    public DateTime  Time  { get; set; }
    public ELogLevel Level { get; set; }
    public string    Tag   { get; set; }
    public object    Msg   { get; set; }

    public override string ToString() { return $"{Time}[{Level}][{Tag}]:{Msg}"; }

    public LogMsg(ELogLevel level, object msg, string tag)
    {
        Time  = DateTime.Now;
        Level = level;
        Msg   = msg;
        Tag   = tag;
    }
}
}