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
    /// 艺术院校
    /// </summary>
    public class SchoolController : ControllerBase
    {
        ISchoolService _schoolService = AutofacService.Resolve<ISchoolService>();
        IStudentService _studentService = AutofacService.Resolve<IStudentService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        ISchoolApplyService _schoolApplyService = AutofacService.Resolve<ISchoolApplyService>();

        public SchoolController()
        {
            this.AddDisposableObject(_schoolService);
            this.AddDisposableObject(_studentService);
            this.AddDisposableObject(_majorService);
            this.AddDisposableObject(_schoolApplyService);
        }

        public ActionResult Index()
        {

            //热门搜索
            IEnumerable<School> hotList = (_schoolService.GetHotListAsync(4)) ?? new List<School>();
            ViewBag.HotList = hotList;
            
            //专业列表
            IEnumerable<Major> majorList = (_majorService.GetAllAsync()) ?? new List<Major>();
            ViewBag.MajorList = majorList;

            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();

            return View();
        }

        [HttpPost]
        public JsonResult GetSchoolList(int pageIndex, int pageSize = 6, string keyword = null, int country = -1, int major = -1, int level = -1)
        {
            PageModel<School> schoolList = _schoolService.GetListAsync(pageIndex, pageSize, keyword, country, major, level);
            ViewBag.SchoolList = schoolList;

            return Success(data: new
            {
                total = schoolList.Total,
                pageSize = pageSize,
                totalPage = (int)Math.Ceiling(schoolList.Total * 1.0 / pageSize),
                list = schoolList.Data
            });
        }

        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                throw new Exception("艺术院校不存在");
            }

            var school = _schoolService.GetByIdAsync(id);

            if (school == null)
            {
                throw new Exception("艺术院校不存在");
            }

            IEnumerable<Student> studentList = (_studentService.GetAllAsync()) ?? new List<Student>();
            List<Student> schoolStudents = new List<Student>();
            foreach (Student student in studentList)
            {
                if (student.SchoolIds != null)
                {
                    string[] ids = student.SchoolIds.Split(',');
                    if (ids.Contains(school.Id.ToString()))
                    {
                        schoolStudents.Add(student);
                    }
                }
            }
            ViewBag.SchoolStudents = schoolStudents;
            //院校专业
            IEnumerable<SchoolMajor> schoolMajorList = (_schoolService.GetMajorsByIdAsync(id)) ?? new List<SchoolMajor>();
            List<SchoolMajor> majorBkList = new List<SchoolMajor>(); //本科专业
            List<SchoolMajor> majorYjsList = new List<SchoolMajor>(); //研究生专业
            foreach (var item in schoolMajorList)
            {
                if (item.Type == 0)
                {
                    majorBkList.Add(item);
                }
                else if (item.Type == 1)
                {
                    majorYjsList.Add(item);
                }
            }
            ViewBag.MajorBkList = majorBkList;
            ViewBag.MajorYjsList = majorYjsList;
            return View(school);
        }

        /// <summary>
        /// 申请咨询
        /// </summary>
        /// <returns></returns>
        public ActionResult SchoolApply(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("艺术院校不存在");
            }
            ViewBag.SchoolId = id;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("申请咨询")]
        public JsonResult SubmitApply(SchoolApply model)
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
            result.success = _schoolApplyService.InsertAsync(model);
            return Json(result);
        }
    }
}