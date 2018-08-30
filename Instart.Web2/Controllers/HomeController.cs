using Instart.Common;
using Instart.Models;
using Instart.Models.Enums;
using Instart.Service;
using Instart.Service.Base;
using Instart.Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instart.Web2.Infrastructures;

namespace Instart.Web2.Controllers
{
    public class HomeController : ControllerBase
    {
        IPartnerService _partnerService = AutofacService.Resolve<IPartnerService>();
        ISchoolService _schoolService = AutofacService.Resolve<ISchoolService>();
        ITeacherService _teacherService = AutofacService.Resolve<ITeacherService>();
        IStudentService _studentService = AutofacService.Resolve<IStudentService>();
        IBannerService _bannerService = AutofacService.Resolve<IBannerService>();
        IRecruitService _recruitService = AutofacService.Resolve<IRecruitService>();
        ICampusService _campusService = AutofacService.Resolve<ICampusService>();
        ICopysService _copysService = AutofacService.Resolve<ICopysService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        IHereMoreService _hereMoreService = AutofacService.Resolve<IHereMoreService>();
        IMailInfoService _mailInfoService = AutofacService.Resolve<IMailInfoService>();

        public HomeController() {
            this.AddDisposableObject(_partnerService);
            this.AddDisposableObject(_schoolService);
            this.AddDisposableObject(_teacherService);
            this.AddDisposableObject(_studentService);
            this.AddDisposableObject(_bannerService);
            this.AddDisposableObject(_recruitService);
            this.AddDisposableObject(_campusService);
            this.AddDisposableObject(_copysService);
            this.AddDisposableObject(_majorService);
            this.AddDisposableObject(_hereMoreService);
            this.AddDisposableObject(_mailInfoService);
        }

        public  ActionResult Index() {
            ViewBag.PartnerList = ( _partnerService.GetRecommendListAsync(14)) ?? new List<Instart.Models.Partner>();
            ViewBag.SchoolList = (_schoolService.GetRecommendListAsync(9)) ?? new List<Instart.Models.School>();
            ViewBag.TeacherList = ( _teacherService.GetRecommendListAsync(8)) ?? new List<Instart.Models.Teacher>();
            ViewBag.StudentList = ( _studentService.GetRecommendListAsync(8)) ?? new List<Instart.Models.Student>();
            return View();
        }

        /// <summary>
        /// 招贤纳士
        /// </summary>
        /// <returns></returns>
        public  ActionResult Recruit() {
            Recruit model =  _recruitService.GetInfoAsync() ?? new Recruit();

            List<Banner> bannerList =  _bannerService.GetBannerListByPosAsync(Instart.Models.Enums.EnumBannerPos.Recruit);
            ViewBag.BannerUrl = "";
            if (bannerList != null && bannerList.Count() > 0)
            {
                ViewBag.BannerUrl = bannerList[0].ImageUrl;
            }
            return View(model);
        }

        /// <summary>
        /// 校区
        /// </summary>
        /// <returns></returns>
        public ActionResult Campus(int id = 0)
        {
            Campus model = _campusService.GetByIdAsync(id);
            if (model == null)
            {
                throw new Exception("校区不存在");
            }
            ViewBag.Imgs = _campusService.GetImgsByCampusIdAsync(id) ?? new List<CampusImg>();
            ViewBag.Student = _studentService.GetListByCampusAsync(id, 4);
            return View(model);
        }

        /// <summary>
        /// here&more
        /// </summary>
        /// <returns></returns>
        public ActionResult HereAndMore() 
        {
            Copys copys = _copysService.GetInfoAsync();
            ViewBag.Copy = copys == null ? "" : copys.HereMoreCopy;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("here&more提交")]
        public JsonResult SetHereMore(HereMore model)
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
            List<string> fileList = new List<string>();
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
                            fileList.Add(uploadResult);
                        }
                    }
                    if (i == 1)
                    {
                        string uploadResult = UploadHelper.Process(file.FileName, file.InputStream);
                        if (!string.IsNullOrEmpty(uploadResult))
                        {
                            model.ImgUrlB = uploadResult;
                            fileList.Add(uploadResult);
                        }
                    }
                    if (i == 2)
                    {
                        string uploadResult = UploadHelper.Process(file.FileName, file.InputStream);
                        if (!string.IsNullOrEmpty(uploadResult))
                        {
                            model.ImgUrlC = uploadResult;
                            fileList.Add(uploadResult);
                        }
                    }
                }
            }
            var result = new ResultBase();
            result.success = _hereMoreService.InsertAsync(model);
            if (result.success) {
                result.success = _mailInfoService.SendMail("here&more", model.Country.GetDescription(), model.MajorId, model.Name, model.Phone, model.Email, "",
                    fileList, System.Web.HttpContext.Current.Server.MapPath("/"));
            }
            return Json(result);
        }
    }
}