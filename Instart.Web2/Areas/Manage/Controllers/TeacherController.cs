using Instart.Common;
using Instart.Models;
using Instart.Service;
using Instart.Service.Base;
using Instart.Web2.Attributes;
using Instart.Web2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Instart.Web2.Areas.Manage.Controllers
{
    [AdminValidation]
    public class TeacherController : ManageControllerBase
    {
        ITeacherService _teacherService = AutofacService.Resolve<ITeacherService>();
        ICourseService _courseService = AutofacService.Resolve<ICourseService>();
        IDivisionService _divisionService = AutofacService.Resolve<IDivisionService>();
        ISchoolService _schoolService = AutofacService.Resolve<ISchoolService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();

        public TeacherController()
        {
            base.AddDisposableObject(_teacherService);
            base.AddDisposableObject(_courseService);
            base.AddDisposableObject(_divisionService);
            base.AddDisposableObject(_schoolService);
            base.AddDisposableObject(_majorService);
        }

        public ActionResult Index(int page = 1, int division = -1, string keyword = null)
        {
            int pageSize = 10;
            var list = _teacherService.GetListAsync(page, pageSize, division, keyword);
            ViewBag.Total = list.Total;
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = Math.Ceiling(list.Total * 1.0 / pageSize);
            ViewBag.Keyword = keyword;

            ViewBag.divisionList = _divisionService.GetAllAsync();
            ViewBag.division = division;
            return View(list.Data);
        }

        public ActionResult Edit(int id = 0)
        {
            Teacher model = new Teacher();
            string action = "添加导师";

            if (id > 0)
            {
                model = _teacherService.GetByIdAsync(id);
                action = "修改导师";
            }

            ViewBag.Action = action;

            List<SelectListItem> divisionList = new List<SelectListItem>();
            IEnumerable<Division> divisions = _divisionService.GetAllAsync();
            foreach (var item in divisions)
            {
                divisionList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.divisionList = divisionList;

            List<SelectListItem> schoolList = new List<SelectListItem>();
            IEnumerable<School> schools = _schoolService.GetAllAsync();
            foreach (var item in schools)
            {
                schoolList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.SchoolList = schoolList;

            List<SelectListItem> majorList = new List<SelectListItem>();
            IEnumerable<Major> majors = _majorService.GetAllAsync();
            foreach (var item in majors)
            {
                majorList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.MajorList = majorList;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("设置导师")]
        public JsonResult Set(Teacher model)
        {
            if (model == null)
            {
                return Error("参数错误。");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("导师名称不能为空。");
            }
            var fileAvatar = Request.Files["fileAvatar"];

            if (fileAvatar != null)
            {
                string uploadResult = UploadHelper.Process(fileAvatar.FileName, fileAvatar.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.Avatar = uploadResult;
                }
            }
            var result = new ResultBase();

            if (model.Id > 0)
            {
                result.success = _teacherService.UpdateAsync(model);
            }
            else
            {
                result.success = _teacherService.InsertAsync(model);
            }

            return Json(result);
        }

        [HttpPost]
        [Operation("删除导师")]
        public JsonResult Delete(int id)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _teacherService.DeleteAsync(id)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("TeacherController.Delete异常", ex);
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [Operation("推荐导师")]
        public JsonResult SetRecommend(int id, bool isRecommend)
        {
            if (id <= 0)
            {
                return Error("id错误");
            }

            try
            {
                return Json(new ResultBase
                {
                    success = _teacherService.SetRecommendAsync(id, isRecommend)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("TeacherController.SetRecommend异常", ex);
                return Error(ex.Message);
            }
        }

        public ActionResult CourseSelect(int id = 0)
        {
            IEnumerable<Course> courseList = _courseService.GetAllAsync();
            IEnumerable<Course> selectedList = _teacherService.GetCoursesByIdAsync(id) ?? new List<Course>();
            if (courseList != null)
            {
                foreach (var course in courseList)
                {
                    course.IsSelected = false;
                    foreach (var item in selectedList) 
                    {
                        if (course.Id == item.Id) 
                        {
                            course.IsSelected = true;
                            break;
                        }
                    }
                }
            }

            var teacher = _teacherService.GetByIdAsync(id);
            if (teacher == null)
            {
                throw new Exception("导师不存在");
            }

            ViewBag.CourseList = courseList;
            ViewBag.TeacherId = id;
            ViewBag.TeacherName = teacher.Name;
            return View();
        }

        [HttpPost]
        [Operation("设置导师擅长课程")]
        public JsonResult SetCourses(int teacherId, string courseIds)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _teacherService.SetCourses(teacherId, courseIds)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("TeacherController.SetCourses异常", ex);
                return Error(ex.Message);
            }
        }

        public ActionResult MajorSelect(int id = 0)
        {
            IEnumerable<Major> majorList = _majorService.GetAllAsync() ?? new List<Major>();
            IEnumerable<Major> selectedList = _teacherService.GetMajorsByIdAsync(id) ?? new List<Major>();
            if (majorList != null)
            {
                foreach (var major in majorList)
                {
                    major.IsSelected = false;
                    foreach (var item in selectedList)
                    {
                        if (major.Id == item.Id)
                        {
                            major.IsSelected = true;
                            break;
                        }
                    }
                }
            }

            var teacher = _teacherService.GetByIdAsync(id);
            if (teacher == null)
            {
                throw new Exception("导师不存在");
            }

            ViewBag.MajorList = majorList;
            ViewBag.TeacherId = id;
            ViewBag.TeacherName = teacher.Name;
            return View();
        }

        [HttpPost]
        [Operation("设置导师擅长专业")]
        public JsonResult SetMajors(int teacherId, string majorIds)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _teacherService.SetMajors(teacherId, majorIds)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("TeacherController.SetMajors异常", ex);
                return Error(ex.Message);
            }
        }
    }
}