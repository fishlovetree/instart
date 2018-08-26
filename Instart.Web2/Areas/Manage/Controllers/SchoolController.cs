using Instart.Common;
using Instart.Models;
using Instart.Models.Enums;
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
    public class SchoolController : ManageControllerBase
    {
        ISchoolService _schoolService = AutofacService.Resolve<ISchoolService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();

        public SchoolController()
        {
            base.AddDisposableObject(_schoolService);
            base.AddDisposableObject(_majorService);
        }

        public ActionResult Index(int page = 1, string keyword = null)
        {
            int pageSize = 10;
            var list = _schoolService.GetListAsync(page, pageSize, keyword);
            ViewBag.Total = list.Total;
            ViewBag.PageIndex = page;
            ViewBag.TotalPages = Math.Ceiling(list.Total * 1.0 / pageSize);
            ViewBag.Keyword = keyword;
            return View(list.Data);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            School model = new School();
            string action = "添加院校";

            if (id > 0)
            {
                model = _schoolService.GetByIdAsync(id);
                action = "修改院校";
            }

            ViewBag.Action = action;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("设置学校")]
        public JsonResult Set(School model)
        {
            if (model == null)
            {
                return Error("参数错误");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return Error("院校名称不能为空");
            }

            model.Name = model.Name.Trim();

            var avatarFile = Request.Files["fileAvatar"];
            var logoFile = Request.Files["fileLogo"];
            var bannerFile = Request.Files["fileBanner"];

            if (avatarFile != null)
            {
                string uploadResult = UploadHelper.Process(avatarFile.FileName, avatarFile.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.Avatar = uploadResult;
                }
            }

            if (logoFile != null)
            {
                string uploadResult = UploadHelper.Process(logoFile.FileName, logoFile.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.Logo = uploadResult;
                }
            }

            if (bannerFile != null)
            {
                string uploadResult = UploadHelper.Process(bannerFile.FileName, bannerFile.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.BannerImg = uploadResult;
                }
            }

            var result = new ResultBase();

            if (model.Id > 0)
            {
                result.success = _schoolService.UpdateAsync(model);
            }
            else
            {
                result.success = _schoolService.InsertAsync(model);
            }

            return Json(result);
        }


        [HttpPost]
        [Operation("删除学校")]
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
                    success = _schoolService.DeleteAsync(id)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("SchoolController.Delete异常", ex);
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [Operation("设置推荐学校")]
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
                    success = _schoolService.SetRecommendAsync(id, isRecommend)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("SchoolController.SetRecommend异常", ex);
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [Operation("设置热门学校")]
        public JsonResult SetHot(int id, bool isHot)
        {
            if (id <= 0)
            {
                return Error("id错误");
            }

            try
            {
                return Json(new ResultBase
                {
                    success = _schoolService.SetHotAsync(id, isHot)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("SchoolController.SetHot异常", ex);
                return Error(ex.Message);
            }
        }

        public ActionResult MajorSelect(int id = 0)
        {
            IEnumerable<Major> majorBkList = (_majorService.GetAllAsync()) ?? new List<Major>();
            IEnumerable<Major> majorYjsList = (_majorService.GetAllAsync()) ?? new List<Major>();
            IEnumerable<SchoolMajor> selectedList = (_schoolService.GetMajorsByIdAsync(id)) ?? new List<SchoolMajor>();
            foreach (var major in majorBkList)
            {
                Major temp = major;
                temp.IsSelected = false;
                foreach (var item in selectedList)
                {
                    if (item.MajorId == major.Id && item.Type == 0)
                    {
                        temp.IsSelected = true;
                        temp.SchoolInfo = item.Introduce;
                        break;
                    }
                }
            }
            foreach (var major in majorYjsList)
            {
                Major temp = major;
                temp.IsSelected = false;
                foreach (var item in selectedList)
                {
                    if (item.MajorId == major.Id && item.Type == 1)
                    {
                        temp.IsSelected = true;
                        temp.SchoolInfo = item.Introduce;
                        break;
                    }
                }
            }
            ViewBag.MajorBkList = majorBkList;
            ViewBag.MajorYjsList = majorYjsList;
            var school = _schoolService.GetByIdAsync(id);
            if (school == null)
            {
                throw new Exception("艺术院校不存在");
            }
            ViewBag.SchoolId = id;
            ViewBag.SchoolName = school.Name;
            return View();
        }

        [HttpPost]
        [Operation("设置学校专业")]
        public JsonResult SetMajors(int schoolId, string bkmajorIds, string yjsmajorIds, string bkintroduces, string yjsintroduces)
        {
            try
            {
                return Json(new ResultBase
                {
                    success = _schoolService.SetMajors(schoolId, bkmajorIds, yjsmajorIds, bkintroduces, yjsintroduces)
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("SchoolController.SetMajors异常", ex);
                return Error(ex.Message);
            }
        }
    }
}