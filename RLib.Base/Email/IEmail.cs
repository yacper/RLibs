/********************************************************************
    created:	2018/11/15 19:40:59
    author:		rush
    email:		
	
    purpose:	Email接口
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLib.Base
{
    public class EmailDm
    {
        public string       mailSmtpHost { get; set; }
        public int          mailSmtpPort { get; set; }
        public string       mailFrom { get; set; }
        public string       mailFromDisplayName { get; set; }
        public string       mailPwd { get; set; }
        public bool         mailEnableSSl { get; set; }
    }


    public interface IEmail
    {
        /// 初始化一次
        string              SmtpHost { get; }                               // "smtp.mxhichina.com";
        int                 SmtpPort { get; }
        bool                SmtpEnableSsl { get; }                          // 
        string              From { get; }                                   /// 发送者 "service@NeoTrader.com";
        string              FromDisplayName { get; }                        /// 发送者显示的名字
        string              FromPwd { get; }                                /// 发送者pwd
                                                                            /// user

        // 默认值，随时修改
        IEnumerable<string> Tos { get; set; }                               // 发送到序列
        IEnumerable<string> Ccs { get; set; }                               // 抄送序列

        string              Subject { get; set; }                           // 题头

        string              Body { get; set; }                              // 正文
        bool                IsBodyHtml { get; set; }

        IEnumerable<string> AttachFiles { get; set; }                       // 附件文件
//        IEnumerable<Stream> AttachStreams { get; set; }                     // 附件流


        bool                Send(string subject, string body, IEnumerable<string> tos = null, IEnumerable<string> ccs = null);
        bool                Send(string subject= null, string body = null, bool? bodyIsHtml = null, IEnumerable<string> attachFiles = null, IEnumerable<string> tos = null, IEnumerable<string> ccs = null);
    }
}
