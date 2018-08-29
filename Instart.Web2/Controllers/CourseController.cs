using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Instart.Models;
using Instart.Service;
using Instart.Service.Base;
using Instart.Common;
using Instart.Models.Enums;
using Instart.Web2.Models;

namespace Instart.Web2.Controllers
{
    /// <summary>
    /// 课程
    /// </summary>
    public class CourseController : ControllerBase
    {
        ICourseService _courseService = AutofacService.Resolve<ICourseService>();
        IStudentService _studentService = AutofacService.Resolve<IStudentService>();
        IWorksService _worksService = AutofacService.Resolve<IWorksService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        ICopysService _copysService = AutofacService.Resolve<ICopysService>();
        ICourseApplyService _courseApplyService = AutofacService.Resolve<ICourseApplyService>();
        ICourseOrderService _courseOrderService = AutofacService.Resolve<ICourseOrderService>();
        ICourseSystemService _courseSystemService = AutofacService.Resolve<ICourseSystemService>();

        public CourseController()
        {
            this.AddDisposableObject(_courseService);
            this.AddDisposableObject(_studentService);
            this.AddDisposableObject(_worksService);
            this.AddDisposableObject(_majorService);
            this.AddDisposableObject(_copysService);
            this.AddDisposableObject(_courseApplyService);
            this.AddDisposableObject(_courseOrderService);
            this.AddDisposableObject(_courseSystemService);
        }

        public ActionResult Index(int studentId = -1)
        {
            IEnumerable<Course> courseList;
            if (studentId == -1)
            {
                courseList = _courseService.GetAllAsync() ?? new List<Course>();
            }
            else 
            {
                courseList = _courseService.GetAllByStudentAsync(studentId) ?? new List<Course>();
            }

            //一行3个课程，两行
            List<List<Course>> courseMap = new List<List<Course>>();
            int courseIndex = 1;
            List<Course> clist = new List<Course>();
            foreach (Course course in courseList)
            {
                clist.Add(course);
                if (courseIndex % 6 == 0)
                {
                    courseMap.Add(new List<Course>(clist.ToArray()));
                    clist.Clear();
                }
                courseIndex++;
            }
            if (clist.Count > 0)
            {
                courseMap.Add(clist);
            }

            ViewBag.CourseList = courseMap;
            IEnumerable<Student> studentList = _studentService.GetAllAsync() ?? new List<Instart.Models.Student>(); //成功学员
            //一行4个学员
            List<List<Student>> studentMap = new List<List<Student>>();
            int studentIndex = 1;
            List<Student> slist = new List<Student>();
            foreach (Student item in studentList)
            {
                slist.Add(item);
                if (studentIndex % 8 == 0)
                {
                    studentMap.Add(new List<Student>(slist.ToArray()));
                    slist.Clear();
                }
                studentIndex++;
            }
            if (slist.Count > 0)
            {
                studentMap.Add(slist);
            }
            ViewBag.StudentMap = studentMap;
            ViewBag.SystemList = _courseSystemService.GetAllAsync() ?? new List<Instart.Models.CourseSystem>(); //课程体系
            return View();
        }

        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("课程不存在");
            }
            Course course = _courseService.GetByIdAsync(id);
            ViewBag.WorkList = _worksService.GetListByCourseIdAsync(id, 3) ?? new List<Instart.Models.Works>();
            List<Student> studentList = _studentService.GetListByCourseAsync(id) ?? new List<Instart.Models.Student>();
            //一行4个学员
            List<List<Student>> studentMap = new List<List<Student>>();
            int studentIndex = 1;
            List<Student> slist = new List<Student>();
            foreach (Student item in studentList)
            {
                slist.Add(item);
                if (studentIndex % 8 == 0)
                {
                    studentMap.Add(new List<Student>(slist.ToArray()));
                    slist.Clear();
                }
                studentIndex++;
            }
            if (slist.Count > 0)
            {
                studentMap.Add(slist);
            }
            ViewBag.StudentMap = studentMap;
            return View(course ?? new Course());
        }

        /// <summary>
        /// 课程咨询
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseApply(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("课程不存在");
            }
            ViewBag.CourseId = id;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("课程咨询")]
        public JsonResult SubmitApply(CourseApply model)
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
            result.success = _courseApplyService.InsertAsync(model);
            return Json(result);
        }

        /// <summary>
        /// 课程预约
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseOrder(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("课程不存在");
            }
            //课程预约文案
            Copys copys = _copysService.GetInfoAsync();
            ViewBag.Copy = copys == null ? "" : copys.CourseApplyCopy;
            ViewBag.CourseId = id;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("课程预约")]
        public JsonResult SubmitOrder(CourseOrder model)
        {
            if (model == null)
            {
                return Error("参数错误");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("请选择您计划去的国家");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("请选择您计划学的专业");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("请输入您的姓名");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("请输入您的手机号");
            }
            var result = new ResultBase();
            result.success = _courseOrderService.InsertAsync(model);
            return Json(result);
        }

        public ActionResult CourseSystem(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("课程体系不存在");
            }
            CourseSystem model = _courseSystemService.GetByIdAsync(id) ?? new CourseSystem();
            IEnumerable<Course> courseList = _courseService.GetListBySystemId(id) ?? new List<Course>();
            //一行3个课程，两行
            List<List<Course>> courseMap = new List<List<Course>>();
            int courseIndex = 1;
            List<Course> clist = new List<Course>();
            foreach (Course course in courseList)
            {
                clist.Add(course);
                if (courseIndex % 6 == 0)
                {
                    courseMap.Add(new List<Course>(clist.ToArray()));
                    clist.Clear();
                }
                courseIndex++;
            }
            if (clist.Count > 0)
            {
                courseMap.Add(clist);
            }

            ViewBag.CourseList = courseMap;
            IEnumerable<Student> studentList = _studentService.GetAllAsync() ?? new List<Instart.Models.Student>(); //成功学员
            //一行4个学员
            List<List<Student>> studentMap = new List<List<Student>>();
            int studentIndex = 1;
            List<Student> slist = new List<Student>();
            foreach (Student item in studentList)
            {
                slist.Add(item);
                if (studentIndex % 8 == 0)
                {
                    studentMap.Add(new List<Student>(slist.ToArray()));
                    slist.Clear();
                }
                studentIndex++;
            }
            if (slist.Count > 0)
            {
                studentMap.Add(slist);
            }
            ViewBag.StudentMap = studentMap;
            return View(model);
        }
    }
}