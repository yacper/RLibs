/********************************************************************
    created:	2017/11/23 16:31:07
    author:	rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentFTP;
using log4net;
using RLib.Base;

namespace RLib.Base
{
    public static class RLibBase
    {
        public static ILog Logger { get; set; }

        public static FtpClient  FtpClient { get; set; }                              // ftpclient

        public static LanguageManager  LanguageManager { get; set; }                              // ftpclient
    }
}
