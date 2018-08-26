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
    public class CourseSystemController : ManageControllerBase
    {
        ICourseSystemService _courseSystemService = AutofacService.Resolve<ICourseSystemService>();

        public CourseSystemController()
        {
            base.AddDisposableObject(_courseSystemService);
        }

        public ActionResult Index(int page = 1, string keyword = null)
        {
            int pageSize = 10;
            var list = _courseSystemService.GetListAsync(page, pageSize, keyword);
            ViewBag.Total = list.Total;
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = Math.Ceiling(list.Total * 1.0 / pageSize);
            ViewBag.Keyword = keyword;
            return View(list.Data);
        }

        public ActionResult Edit(int id = 0)
        {
            CourseSystem model = new CourseSystem();
            string action = "添加课程体系";

            if (id > 0)
            {
                model = _courseSystemService.GetByIdAsync(id);
                action = "修改课程课程体系";
            }

            ViewBag.Action = action;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("设置课程体系")]
        public JsonResult Set(CourseSystem model)
        {
            if (model == null)
            {
                return Error("参数错误。");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("课程名称不能为空。");
            }
            var fileAvatar = Request.Files["fileAvatar"];

            if (fileAvatar != null)
            {
                string uploadResult = UploadHelper.Process(fileAvatar.FileName, fileAvatar.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.Picture = uploadResult;
                }
            }

            var fileBanner = Request.Files["fileBanner"];

            if (fileBanner != null)
            {
                string uploadResult = UploadHelper.Process(fileBanner.FileName, fileBanner.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.BannerImg = uploadResult;
                }
            }
            var result = new ResultBase();

            if (model.Id > 0)
            {
                result.success = _courseSystemService.UpdateAsync(model);
            }
            else
            {
                result.success = _courseSystemService.InsertAsync(model);
            }

            return Json(result);
        }

        [HttpPost]
        [Operation("删除课程体系")]
        public JsonResult Delete(int id)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _courseSystemService.DeleteAsync(id)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("CourseSystemController.Delete异常", ex);
                return Error(ex.Message);
            }
        }
    }
}