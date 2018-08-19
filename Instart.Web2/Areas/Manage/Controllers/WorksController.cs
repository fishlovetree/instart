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
    public class WorksController : ManageControllerBase
    {
        IWorksService _worksService = AutofacService.Resolve<IWorksService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();

        public WorksController()
        {
            base.AddDisposableObject(_worksService);
            base.AddDisposableObject(_majorService);
        }

        public ActionResult Index(int page = 1, int major = -1, string keyword = null)
        {
            int pageSize = 10;
            var list = _worksService.GetListAsync(page, pageSize, major, keyword);
            ViewBag.Total = list.Total;
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = Math.Ceiling(list.Total * 1.0 / pageSize);

            ViewBag.Keyword = keyword;
            ViewBag.majorList = _majorService.GetAllAsync();
            ViewBag.major = major;
            return View(list.Data);
        }

        public ActionResult Edit(int id = 0)
        {
            Works model = new Works();
            string action = "添加作品";

            if (id > 0)
            {
                model = _worksService.GetByIdAsync(id);
                action = "修改作品";
            }
            ViewBag.Action = action;

            List<SelectListItem> majorList = new List<SelectListItem>();
            IEnumerable<Major> majors = _majorService.GetAllAsync();
            foreach (var item in majors)
            {
                majorList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.majorList = majorList;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("设置作品")]
        public JsonResult Set(Works model)
        {
            if (model == null)
            {
                return Error("参数错误。");
            }

            var fileWorks = Request.Files["fileWorks"];

            if (fileWorks != null)
            {
                string uploadResult = UploadHelper.Process(fileWorks.FileName, fileWorks.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.ImgUrl = uploadResult;
                }
            }
            if (String.IsNullOrEmpty(model.ImgUrl))
            {
                return Error("作品图片不能为空。");
            }
            var result = new ResultBase();

            if (model.Id > 0)
            {
                result.success = _worksService.UpdateAsync(model);
            }
            else
            {
                result.success = _worksService.InsertAsync(model);
            }

            return Json(result);
        }

        [HttpPost]
        [Operation("删除作品")]
        public JsonResult Delete(int id)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _worksService.DeleteAsync(id)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("WorksController.Delete异常", ex);
                return Error(ex.Message);
            }
        }
    }
}