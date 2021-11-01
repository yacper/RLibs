/********************************************************************
    created:	2017/5/20 18:08:40
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
    public enum ELogLevel
    {
        Info            = 1,
        Warning         = 2,
        Error           = 4,
        All             = Info|Warning|Error 
    }

    public interface ILogger
    {
        bool                Enabled { get; set; }                           // 总开关


        ELogLevel           LevelEnabled { get; set; }                      // 
        List<string>        ModsEnabled { get; set; }                       // 模块
        List<string>        OwnerEnabled { get; set; }                      // 拥有者

    }
}
