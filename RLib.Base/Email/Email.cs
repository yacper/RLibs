/********************************************************************
    created:	2018/11/15 19:40:59
    author:		rush
    email:		
	
    purpose:	Email
*********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using DataModel;

namespace RLib.Base
{
    public class Email:IEmail
    {
        public string       SmtpHost { get; protected set; }
        public int          SmtpPort { get; protected set; }
        public bool         SmtpEnableSsl { get; protected set; }                          // 
        public string       From { get; protected set; }                               /// 发送者
        public string       FromDisplayName { get; protected set; }                        /// 发送者显示的名字
        public string       FromPwd { get; protected set; }                            /// 发送者pwd

        // 随时修改
        public IEnumerable<string> Tos { get; set; }                               // 发送到序列
        public IEnumerable<string> Ccs { get; set; }                               // 抄送序列

        public string       Subject { get; set; }                              // 正文

        public string       Body { get; set; }                              // 正文
        public bool         IsBodyHtml { get; set; }

        public IEnumerable<string> AttachFiles { get; set; }                       // 附件文件
//        public IEnumerable<Stream> AttachStreams { get; set; }                     // 附件流



        public bool         Send(string subject, string body, IEnumerable<string> tos = null, IEnumerable<string> ccs = null)
        {
            return Send(subject, body, IsBodyHtml, null, tos, Ccs);
        }

        public bool         Send(string subject = null, string body = null, bool? bodyIsHtml = null, IEnumerable<string> attachFiles = null,
                                 IEnumerable<string> tos = null, IEnumerable<string> ccs = null)
        {
            MailMessage mail = new MailMessage();
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.Default;
            mail.Priority = MailPriority.High;
            mail.From = new MailAddress(From, FromDisplayName);

            if (tos == null)
                tos = Tos;
            if (tos == null || !tos.Any())
                throw new ArgumentNullException("没有tos");
            foreach (string s in tos)
                mail.To.Add(s);

            if (ccs == null)
                ccs = Ccs;
            if (ccs != null && ccs.Any())
                ccs.ForEach(p=>mail.CC.Add(p));

            if (subject == null)
                subject = Subject;
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException("没有subject");
            mail.Subject = subject;

            if (body == null)
                body = Body;
            mail.Body = body;  // body 可以为空字符串

            if (bodyIsHtml == null)
                bodyIsHtml = IsBodyHtml;
            mail.IsBodyHtml = bodyIsHtml != null ? bodyIsHtml.Value : false;

            try
            {
                if (attachFiles == null)
                    attachFiles = AttachFiles;
                if (attachFiles != null && 
                    attachFiles.Any())
                    attachFiles.ForEach(p => mail.Attachments.Add(new Attachment(p)));
            }
            catch (Exception e) //添加附件出错
            {
                RLibBase.Logger.Error("添加附件出错:" + e);  // 虽然添加附件出错，还是继续发送
            }

            SmtpClient smtp = new SmtpClient(SmtpHost);
            smtp.EnableSsl = SmtpEnableSsl;
	        smtp.Port = SmtpPort;
            smtp.UseDefaultCredentials = false;
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.Credentials = new System.Net.NetworkCredential(From, FromPwd);

            try
            {
                smtp.Send(mail); //将邮件发送到SMTP邮件服务器
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                RLibBase.Logger.Error("[Email.Send] SmtpException : " + ex.Message);
                return false;
            }
        }


#region C&D
        public              Email(string smtphost, int smtpPort,  string from, string fromPwd, string  fromDisplayerName = null, bool smtpEnableSsl = true)
        {
            SmtpHost = smtphost;
            SmtpPort = smtpPort;
            From = from;
            FromPwd = fromPwd;
            FromDisplayName = fromDisplayerName != null ? fromDisplayerName : from;
            SmtpEnableSsl = smtpEnableSsl;
        }

        public              Email(EmailDm dm)
        :this(dm.mailSmtpHost, dm.mailSmtpPort, dm.mailFrom, dm.mailPwd, dm.mailFromDisplayName, dm.mailEnableSSl)
        {

        }
#endregion
    }
}
