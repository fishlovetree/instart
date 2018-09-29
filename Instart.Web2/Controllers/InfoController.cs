using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Instart.Models;
using Instart.Service;
using Instart.Service.Base;
using Instart.Models.Enums;
using Instart.Common;
using Instart.Web2.Models;
using Instart.Web2.Infrastructures;

namespace Instart.Web2.Controllers
{
    /// <summary>
    /// 艺术资讯
    /// </summary>
    public class InfoController : ControllerBase
    {
        IProgramService _programService = AutofacService.Resolve<IProgramService>();

        public InfoController()
        {
            this.AddDisposableObject(_programService);
        }

        public ActionResult Index()
        {
            IEnumerable<Program> progaramList = (_programService.GetAllAsync()) ?? new List<Program>();
            ViewBag.programList = progaramList;
            return View();
        }

        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("项目不存在。");
            }
            Program program = _programService.GetByIdAsync(id);
            return View(program ?? new Program());
        }
    }
}