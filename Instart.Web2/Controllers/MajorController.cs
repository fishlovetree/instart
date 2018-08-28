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
    /// 艺术专业
    /// </summary>
    public class MajorController : ControllerBase
    {
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        IDivisionService _divisionService = AutofacService.Resolve<IDivisionService>();
        IWorksService _workService = AutofacService.Resolve<IWorksService>();
        ISchoolService _schoolService = AutofacService.Resolve<ISchoolService>();
        IStudentService _studentService = AutofacService.Resolve<IStudentService>();
        IMajorApplyService _majorApplyService = AutofacService.Resolve<IMajorApplyService>();
        IWorksCommentService _worksCommentService = AutofacService.Resolve<IWorksCommentService>();
        ICourseService _courseService = AutofacService.Resolve<ICourseService>();

        public MajorController()
        {
            this.AddDisposableObject(_majorService);
            this.AddDisposableObject(_divisionService);
            this.AddDisposableObject(_workService);
            this.AddDisposableObject(_schoolService);
            this.AddDisposableObject(_studentService);
            this.AddDisposableObject(_majorApplyService);
            this.AddDisposableObject(_worksCommentService);
            this.AddDisposableObject(_courseService);
        }

        public  ActionResult Index(int id = -1)
        {
            var divisionList =  _divisionService.GetAllAsync();

            if (divisionList == null || divisionList.Count() == 0)
            {
                throw new Exception("请先创建学部");
            }

            ViewBag.DivisionList = divisionList;
            ViewBag.DivisionId = id;
            return View();
        }

        [HttpPost]
        public  JsonResult GetMajorList(int divisionId, int pageIndex, int pageSize = 8)
        {
            var result =  _majorService.GetListByDivsionAsync(divisionId, pageIndex, pageSize);
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
            var major =  _majorService.GetByIdAsync(id);
            if (major == null)
            {
                throw new Exception("专业不存在");
            }

            ViewBag.WorkList = ( _workService.GetListByMajorIdAsync(id, 3)) ?? new List<Instart.Models.Works>();
            ViewBag.CourseList = _courseService.GetListByMajor(3, id) ?? new List<Course>();
            return View(major);
        }

        /// <summary>
        /// 申请咨询
        /// </summary>
        /// <returns></returns>
        public ActionResult MajorApply(int id = 0, string name = null)
        {
            if (id == 0)
            {
                throw new Exception("艺术专业不存在。");
            }
            ViewBag.MajorId = id;
            ViewBag.MajorName = name;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("申请咨询")]
        public JsonResult SubmitApply(MajorApply model)
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
            result.success = _majorApplyService.InsertAsync(model);
            return Json(result);
        }

        /// <summary>
        /// WorksComment
        /// </summary>
        /// <returns></returns>
        public ActionResult WorksComment()
        {
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("作品评析")]
        public JsonResult Comment(WorksComment model)
        {
            if (model == null)
            {
                return Error("参数错误。");
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
            if (string.IsNullOrEmpty(model.Email))
            {
                return Error("请输入您的邮箱地址");
            }
            HttpFileCollectionBase files = Request.Files;
            if (files != null)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    //1-3个作品
                    if (i == 0)
                    {
                        string uploadResult = UploadHelper.Process(file.FileName, file.InputStream);
                        if (!string.IsNullOrEmpty(uploadResult))
                        {
                            model.ImgUrlA = uploadResult;
                        }
                    }
                    if (i == 1)
                    {
                        string uploadResult = UploadHelper.Process(file.FileName, file.InputStream);
                        if (!string.IsNullOrEmpty(uploadResult))
                        {
                            model.ImgUrlB = uploadResult;
                        }
                    }
                    if (i == 2)
                    {
                        string uploadResult = UploadHelper.Process(file.FileName, file.InputStream);
                        if (!string.IsNullOrEmpty(uploadResult))
                        {
                            model.ImgUrlC = uploadResult;
                        }
                    }
                }
            }
            var result = new ResultBase();
            result.success = _worksCommentService.InsertAsync(model);
            return Json(result);
        }
    }
}