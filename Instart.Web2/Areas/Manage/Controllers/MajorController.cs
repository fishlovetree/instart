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
    public class MajorController : ManageControllerBase
    {
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        IDivisionService _divisionService = AutofacService.Resolve<IDivisionService>();

        public MajorController()
        {
            base.AddDisposableObject(_majorService);
            base.AddDisposableObject(_divisionService);
        }

        public ActionResult Index(int page = 1, int division = -1, string keyword = null)
        {
            int pageSize = 10;
            var list = _majorService.GetListAsync(page, pageSize, division, keyword);
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
            Major model = new Major();
            string action = "添加专业";

            if (id > 0)
            {
                model = _majorService.GetByIdAsync(id);
                action = "修改专业";
            }

            ViewBag.Action = action;
            List<SelectListItem> divisionList = new List<SelectListItem>();
            IEnumerable<Division> divisions = _divisionService.GetAllAsync();
            foreach (var item in divisions)
            {
                divisionList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.divisionList = divisionList;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("设置专业")]
        public JsonResult Set(Major model)
        {
            if (model == null)
            {
                return Error("参数错误。");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("专业名称不能为空。");
            }

            model.Name = model.Name.Trim();

            var fileMajor = Request.Files["fileMajor"];

            if (fileMajor != null)
            {
                string uploadResult = UploadHelper.Process(fileMajor.FileName, fileMajor.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.ImgUrl = uploadResult;
                }
            }
            var result = new ResultBase();

            if (model.Id > 0)
            {
                result.success = _majorService.UpdateAsync(model);
            }
            else
            {
                result.success = _majorService.InsertAsync(model);
            }

            return Json(result);
        }

        [HttpPost]
        [Operation("删除专业")]
        public JsonResult Delete(int id)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _majorService.DeleteAsync(id)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("MajorController.Delete异常", ex);
                return Error(ex.Message);
            }
        }
    }
}