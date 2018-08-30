using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;

namespace Instart.Service
{
    public interface IMailInfoService
    {
        int GetCountAsync();

        MailInfo GetInfoAsync();

        bool InsertAsync(MailInfo model);

        bool UpdateAsync(MailInfo model);

        bool SendMail(string title, string country, int majorId, string name, string phone, 
            string email, string question, List<string> files, string serverPath);
    }
}
