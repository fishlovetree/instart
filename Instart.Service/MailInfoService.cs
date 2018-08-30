using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Repository;
using System.Net.Mail;
using Instart.Models.Enums;
using System.Net;
using System.IO;

namespace Instart.Service
{
    public class MailInfoService : ServiceBase, IMailInfoService
    {
        IMailInfoRepository _mailInfoRepository = AutofacRepository.Resolve<IMailInfoRepository>();
        IMajorRepository _majorRepository = AutofacRepository.Resolve<IMajorRepository>();

        public MailInfoService()
        {
            base.AddDisposableObject(_mailInfoRepository);
            base.AddDisposableObject(_majorRepository);
        }

        public int GetCountAsync()
        {
            return _mailInfoRepository.GetCountAsync();
        }

        public MailInfo GetInfoAsync()
        {
            return _mailInfoRepository.GetInfoAsync();
        }

        public bool InsertAsync(MailInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            return _mailInfoRepository.InsertAsync(model);
        }

        public bool UpdateAsync(MailInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            return _mailInfoRepository.UpdateAsync(model);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="country"></param>
        /// <param name="majorId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool SendMail(string title, string country, int majorId, string name, string phone, 
            string email, string question, List<string> files, string serverPath) 
        {
            bool result = false;
            MailInfo mailInfo = _mailInfoRepository.GetInfoAsync() ?? new MailInfo();
            Major major = _majorRepository.GetByIdAsync(majorId) ?? new Major();
            if (!String.IsNullOrEmpty(mailInfo.FromAddress)
                && !String.IsNullOrEmpty(mailInfo.FromPwd) && !String.IsNullOrEmpty(mailInfo.Host)
                && !String.IsNullOrEmpty(mailInfo.Port) && !String.IsNullOrEmpty(mailInfo.ToAddress))
            {
                MailMessage mailMessage = new MailMessage
                {
                    //发件人
                    From = new MailAddress(mailInfo.FromAddress)
                };

                //收件人
                mailMessage.To.Add(new MailAddress(mailInfo.ToAddress));

                //邮件主题
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Subject = title;

                //邮件正文
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Body = "<p style='font-weight:bold;font-size:16px;margin:5px;'>计划去的国家：" + country + "</p>" +
                    "<p style='font-weight:bold;font-size:16px;margin:5px;'>计划学的专业：" + major.Name + "</p>" +
                    "<p style='font-weight:bold;font-size:16px;margin:5px;'>姓名：" + name + "</p>" +
                    "<p style='font-weight:bold;font-size:16px;margin:5px;'>微信号：" + phone + "</p>";
                if (!String.IsNullOrEmpty(email)) 
                {
                    mailMessage.Body += "<p style='font-weight:bold;font-size:16px;margin:5px;'>电子邮箱：" + email + "</p>";
                }
                if (!String.IsNullOrEmpty(question)) 
                {
                    mailMessage.Body += "<p style='font-weight:bold;font-size:16px;margin:5px;'>问题：" + question + "</p>";
                }

                //如果要发送html格式的消息，需要设置这个属性
                mailMessage.IsBodyHtml = true;

                //发送附件 指明附件的绝对地址
                if (files != null && files.Count > 0) 
                {
                    foreach (var file in files) 
                    {
                        Attachment attachment = new Attachment(serverPath + file);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                //创建邮件发送客户端
                try
                {
                    //这里使用qq邮箱 需要在设置->账户下开启POP3/SMTP服务 和 IMAP / SMTP服务
                    //qq邮箱的发件服务器smtp.qq.com  端口25
                    SmtpClient sendClient = new SmtpClient(mailInfo.Host, Int32.Parse(mailInfo.Port))
                    {
                        //指定邮箱账号和密码
                        //在第三方客户端登陆qq邮箱时，密码是授权码
                        //登陆qq邮箱在设置->账户下可以生成授权码
                        Credentials = new NetworkCredential(mailInfo.FromAddress, mailInfo.FromPwd)
                    };

                    //指定如何发送电子邮件
                    sendClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    sendClient.EnableSsl = false;
                    sendClient.Send(mailMessage);
                    result = true;
                }
                catch
                {
                    throw;
                }
            }
            return result;
        }
    }
}
