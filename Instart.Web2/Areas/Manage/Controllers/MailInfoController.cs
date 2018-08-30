using Instart.Web2.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instart.Service;
using Instart.Service.Base;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Common;
using Instart.Web2.Models;
using System.IO;

namespace Instart.Web2.Areas.Manage.Controllers
{
    [AdminValidation]
    public class MailInfoController : ManageControllerBase
    {
        IMailInfoService _mailInfoService = AutofacService.Resolve<IMailInfoService>();

        public MailInfoController()
        {
            base.AddDisposableObject(_mailInfoService);
        }

        public ActionResult Index()
        {
            MailInfo model = _mailInfoService.GetInfoAsync();
            if (model == null) model = new MailInfo();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("设置关于我们")]
        public JsonResult Set(MailInfo model)
        {
            if (model == null)
            {
                return Error("参数错误。");
            }

            var result = new ResultBase();

            int count = _mailInfoService.GetCountAsync();
            if (count > 0)
            {
                result.success = _mailInfoService.UpdateAsync(model);
            }
            else
            {
                result.success = _mailInfoService.InsertAsync(model);
            }

            return Json(result);
        }
    }
}