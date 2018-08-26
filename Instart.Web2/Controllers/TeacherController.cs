using Instart.Common;
using Instart.Models;
using Instart.Models.Enums;
using Instart.Service;
using Instart.Service.Base;
using Instart.Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Instart.Web2.Controllers
{
    /// <summary>
    /// 艺术导师
    /// </summary>
    public class TeacherController : ControllerBase
    {
        ITeacherService _teacherService = AutofacService.Resolve<ITeacherService>();
        IDivisionService _divisionService = AutofacService.Resolve<IDivisionService>();
        IStudentService _studentService = AutofacService.Resolve<IStudentService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        ITeacherQuestionService _teacherQuestionService = AutofacService.Resolve<ITeacherQuestionService>();

        public TeacherController()
        {
            this.AddDisposableObject(_teacherService);
            this.AddDisposableObject(_divisionService);
            this.AddDisposableObject(_studentService);
        }

        public  ActionResult Index(int id = 0)
        {
            var divisionList =  _divisionService.GetAllAsync();

            if (divisionList == null || divisionList.Count() == 0)
            {
                throw new Exception("请先创建学部");
            }

            if(id == 0)
            {
                id = divisionList.First().Id;
            }

            ViewBag.DivisionList = divisionList;
            ViewBag.DivisionId = id;
            return View();
        }

        [HttpPost]
        public  JsonResult GetTeacherList(int divisionId, int pageIndex, int pageSize = 8)
        {
            var result =  _teacherService.GetListByDivsionAsync(divisionId, pageIndex, pageSize);
            return Success(data: new
            {
                total = result.Total,
                pageSize = pageSize,
                totalPage = (int)Math.Ceiling(result.Total * 1.0 / pageSize),
                list = result.Data
            });
        }

        public  ActionResult Details(int id)
        {
            var teacher =  _teacherService.GetByIdAsync(id);
            if(teacher == null)
            {
                throw new Exception("导师不存在");
            }

            ViewBag.CourseList = _teacherService.GetCoursesByIdAsync(id) ?? new List<Instart.Models.Course>();
            ViewBag.SchoolList = _teacherService.GetSchoolListByTeacher(id) ?? new List<SchoolMajor>();
            return View(teacher);
        }

        /// <summary>
        /// TeacherQuestion
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherQuestion(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("导师不存在");
            }
            ViewBag.TeacherId = id;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("向导师提问")]
        public JsonResult SubmitQuestion(TeacherQuestion model)
        {
            if (model == null)
            {
                return Error("参数错误");
            }
            if (model.MajorId == 0)
            {
                return Error("请选择您计划学的专业");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("请输入您的姓名");
            }
            if (string.IsNullOrEmpty(model.Phone))
            {
                return Error("请输入您的微信号");
            }
            var result = new ResultBase();
            result.success = _teacherQuestionService.InsertAsync(model);
            return Json(result);
        }
    }
}