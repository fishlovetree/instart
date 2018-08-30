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
    public class StudentController : ManageControllerBase
    {
        IStudentService _studentService = AutofacService.Resolve<IStudentService>();
        ISchoolService _schoolService = AutofacService.Resolve<ISchoolService>();
        ITeacherService _teacherService = AutofacService.Resolve<ITeacherService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        IDivisionService _divisionService = AutofacService.Resolve<IDivisionService>();
        ICourseService _courseService = AutofacService.Resolve<ICourseService>();
        ICampusService _campusService = AutofacService.Resolve<ICampusService>();

        public StudentController()
        {
            base.AddDisposableObject(_studentService);
            base.AddDisposableObject(_schoolService);
            base.AddDisposableObject(_teacherService);
            base.AddDisposableObject(_majorService);
            base.AddDisposableObject(_divisionService);
            base.AddDisposableObject(_courseService);
            base.AddDisposableObject(_campusService);
        }

        public ActionResult Index(int page = 1, int division = -1, string keyword = null)
        {
            int pageSize = 10;
            var list = _studentService.GetListAsync(page, pageSize, division, keyword);
            ViewBag.Total = list.Total;
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = Math.Ceiling(list.Total * 1.0 / pageSize);
            ViewBag.Keyword = keyword;

            ViewBag.divisionList = _divisionService.GetAllAsync();
            ViewBag.division = division;
            return View(list.Data);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Student model = new Student();
            string action = "添加学员";

            if (id > 0)
            {
                model = _studentService.GetByIdAsync(id);
                action = "修改学员";
            }

            ViewBag.Action = action;

            List<SelectListItem> teacherList = new List<SelectListItem>();
            IEnumerable<Teacher> teachers = _teacherService.GetAllAsync();
            foreach (var item in teachers)
            {
                teacherList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.TeacherList = teacherList;

            List<SelectListItem> majorList = new List<SelectListItem>();
            IEnumerable<Major> majors = _majorService.GetAllAsync();
            foreach (var item in majors)
            {
                majorList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.MajorList = majorList;

            List<SelectListItem> divisionList = new List<SelectListItem>();
            IEnumerable<Division> divisions = _divisionService.GetAllAsync();
            foreach (var item in divisions)
            {
                divisionList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.DivisionList = divisionList;

            List<SelectListItem> campusList = new List<SelectListItem>();
            IEnumerable<Campus> campuses = _campusService.GetAllAsync();
            foreach (var item in campuses)
            {
                campusList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.CampusList = campusList;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("设置学员")]
        public JsonResult Set(Student model)
        {
            if (model == null)
            {
                return Error("参数错误。");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("学员名称不能为空。");
            }

            model.Name = model.Name.Trim();

            var avatarFile = Request.Files["fileAvatar"];

            if (avatarFile != null)
            {
                string uploadResult = UploadHelper.Process(avatarFile.FileName, avatarFile.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.Avatar = uploadResult;
                }
            }

            var bannerImgFile = Request.Files["fileBannerImg"];
            if (bannerImgFile != null)
            {
                string uploadResult = UploadHelper.Process(bannerImgFile.FileName, bannerImgFile.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.BannerImg = uploadResult;
                }
            }

            var bannerVideoFile = Request.Files["fileBannerVideo"];
            if (bannerVideoFile != null)
            {
                string uploadResult = UploadHelper.Process(bannerVideoFile.FileName, bannerVideoFile.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.BannerVideo = uploadResult;
                }
            }

            var videoImgFile = Request.Files["fileImg"];
            if (videoImgFile != null)
            {
                string uploadResult = UploadHelper.Process(videoImgFile.FileName, videoImgFile.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.ImgUrl = uploadResult;
                }
            }

            var videoFile = Request.Files["fileVideo"];
            if (videoFile != null)
            {
                string uploadResult = UploadHelper.Process(videoFile.FileName, videoFile.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.VideoUrl = uploadResult;
                }
            }

            var result = new ResultBase();

            if (model.Id > 0)
            {
                result.success = _studentService.UpdateAsync(model);
            }
            else
            {
                result.success = _studentService.InsertAsync(model);
            }

            return Json(result);
        }


        [HttpPost]
        [Operation("推荐学员")]
        public JsonResult Delete(int id)
        {
            if (id <= 0)
            {
                return Error("id错误");
            }

            try
            {
                return Json(new ResultBase
                {
                    success = _studentService.DeleteAsync(id)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("SchoolController.Delete异常", ex);
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [Operation("推荐学员")]
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
                    success = _studentService.SetRecommend(id, isRecommend)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("StudentController.SetRecommend异常", ex);
                return Error(ex.Message);
            }
        }

        public ActionResult CourseSelect(int id = 0)
        {
            IEnumerable<Course> courseList = _courseService.GetAllAsync();
            IEnumerable<int> selectedList = _studentService.GetCoursesByIdAsync(id);
            if (courseList != null)
            {
                foreach (var course in courseList)
                {
                    if (selectedList != null && selectedList.Contains(course.Id))
                    {
                        course.IsSelected = true;
                    }
                    else
                    {
                        course.IsSelected = false;
                    }
                }
            }

            var student = _studentService.GetByIdAsync(id);
            if (student == null)
            {
                throw new Exception("学员不存在");
            }

            ViewBag.CourseList = courseList;
            ViewBag.StudentId = id;
            ViewBag.StudentName = student.Name;
            return View();
        }

        [HttpPost]
        [Operation("设置学员OFFER")]
        public JsonResult SetCourses(int studentId, string courseIds)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _studentService.SetCourses(studentId, courseIds)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("StudentController.SetCourses异常", ex);
                return Error(ex.Message);
            }
        }

        public ActionResult SchoolSelect(int id = 0)
        {
            IEnumerable<School> schoolList = _schoolService.GetAllAsync();
            IEnumerable<int> selectedList = _studentService.GetSchoolsByIdAsync(id);
            if (schoolList != null)
            {
                foreach (var school in schoolList)
                {
                    if (selectedList != null && selectedList.Contains(school.Id))
                    {
                        school.IsSelected = true;
                    }
                    else
                    {
                        school.IsSelected = false;
                    }
                }
            }

            var student = _studentService.GetByIdAsync(id);
            if (student == null)
            {
                throw new Exception("学员不存在");
            }

            ViewBag.SchoolList = schoolList;
            ViewBag.StudentId = id;
            ViewBag.StudentName = student.Name;
            return View();
        }

        [HttpPost]
        [Operation("设置学员OFFER")]
        public JsonResult SetSchools(int studentId, string schoolIds)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _studentService.SetSchools(studentId, schoolIds)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("StudentController.SetSchools异常", ex);
                return Error(ex.Message);
            }
        }

        #region 文件上传
        [HttpPost]
        public ActionResult Upload()
        {
            string fileName = Request["name"];
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("/content/upload/videos/");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            //if (data != null)//为null可能是暂停的那一瞬间
            //{
            data.SaveAs(filePath);//报错
            //}
            return Json(new { erron = 0 });//Demo，随便返回了个值，请勿参考
        }

        public ActionResult Merge()
        {
            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("/content/upload/videos/");//Upload 文件夹
            var fileName = Request["fileName"];//文件名
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));
            var dir = Path.Combine(uploadDir, fileRelName);//临时文件夹          
            var files = System.IO.Directory.GetFiles(dir);//获得下面的所有文件
            var finalPath = Path.Combine(uploadDir, fileName);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
            var fs = new FileStream(finalPath, FileMode.Create);
            foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            fs.Flush();
            fs.Close();
            System.IO.Directory.Delete(dir);//删除文件夹
            return Json(new { filePath = Path.Combine("/content/upload/videos/", fileName) });//随便返回个值，实际中根据需要返回
        }
        #endregion
    }
}