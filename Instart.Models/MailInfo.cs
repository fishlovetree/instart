using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// 邮箱信息
    /// </summary>
    public class MailInfo
    {
        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// 发件人邮箱密码
        /// </summary>
        public string FromPwd { get; set; }

        /// <summary>
        /// 发件人邮件服务器地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 发件人邮件服务器端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 收件人邮箱
        /// </summary>
        public string ToAddress { get; set; }
    }
}
