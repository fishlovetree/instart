using Instart.Service;
using Instart.Service.Base;
using Instart.Web2.Attributes;
using Instart.Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Instart.Web2.Areas.Manage.Controllers
{
    [AdminValidation]
    public class HomeController : ManageControllerBase
    {
        ICourseApplyService _courseApplyService = AutofacService.Resolve<ICourseApplyService>();
        ISchoolApplyService _schoolApplyService = AutofacService.Resolve<ISchoolApplyService>();
        IMajorApplyService _majorApplyService = AutofacService.Resolve<IMajorApplyService>();
        ITeacherQuestionService _teacherQuestionService = AutofacService.Resolve<ITeacherQuestionService>();
        IStatisticsService _statisticsService = AutofacService.Resolve<IStatisticsService>();
        IUserService _userService = AutofacService.Resolve<IUserService>();
        ILogService _logService = AutofacService.Resolve<ILogService>();

        public HomeController()
        {
            this.AddDisposableObject(_courseApplyService);
            this.AddDisposableObject(_schoolApplyService);
            this.AddDisposableObject(_majorApplyService);
            this.AddDisposableObject(_teacherQuestionService);
            this.AddDisposableObject(_statisticsService);
            this.AddDisposableObject(_userService);
            this.AddDisposableObject(_logService);
        }

        public ActionResult Index()
        {
            ViewBag.CourseApplyList = (_courseApplyService.GetTopListAsync(3)) ?? new List<Instart.Models.CourseApply>();
            ViewBag.SchoolApplyList = (_schoolApplyService.GetTopListAsync(3)) ?? new List<Instart.Models.SchoolApply>();
            ViewBag.MajorApplyList = (_majorApplyService.GetTopListAsync(3)) ?? new List<Instart.Models.MajorApply>();
            ViewBag.TeacherQuestionList = (_teacherQuestionService.GetTopListAsync(3)) ?? new List<Instart.Models.TeacherQuestion>();
            ViewBag.Statistics = (_statisticsService.GetAsync()) ?? new Instart.Models.Statistics();
            ViewBag.LogList = (_logService.GetTopListAsync(20)) ?? new List<Instart.Models.Log>();
            return View();
        } 
    }
}