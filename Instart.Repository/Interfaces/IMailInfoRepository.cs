using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;

namespace Instart.Repository
{
    public interface IMailInfoRepository
    {
        int GetCountAsync();

        MailInfo GetInfoAsync();

        bool InsertAsync(MailInfo model);

        bool UpdateAsync(MailInfo model);
    }
}
