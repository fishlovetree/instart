using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Instart.Models;
using Instart.Service;
using Instart.Service.Base;
using Instart.Models.Enums;
using Instart.Common;
using Instart.Web2.Models;
using Instart.Web2.Infrastructures;

namespace Instart.Web2.Controllers
{
    /// <summary>
    /// 关于Instart
    /// </summary>
    public class AboutController : ControllerBase
    {
        IAboutInstartService _aboutService = AutofacService.Resolve<IAboutInstartService>();
        IProgramService _programService = AutofacService.Resolve<IProgramService>();
        ICompanyService _companyService = AutofacService.Resolve<ICompanyService>();
        IStudioService _studioService = AutofacService.Resolve<IStudioService>();
        IDivisionService _divisionService = AutofacService.Resolve<IDivisionService>();
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        IProgramApplyService _programApplyService = AutofacService.Resolve<IProgramApplyService>();
        ICopysService _copysService = AutofacService.Resolve<ICopysService>();
        ICompanyApplyService _companyApplyService = AutofacService.Resolve<ICompanyApplyService>();
        IMailInfoService _mailInfoService = AutofacService.Resolve<IMailInfoService>();

        public AboutController()
        {
            this.AddDisposableObject(_aboutService);
            this.AddDisposableObject(_programService);
            this.AddDisposableObject(_companyService);
            this.AddDisposableObject(_studioService);
            this.AddDisposableObject(_divisionService);
            this.AddDisposableObject(_majorService);
            this.AddDisposableObject(_programApplyService);
            this.AddDisposableObject(_copysService);
            this.AddDisposableObject(_companyApplyService);
            this.AddDisposableObject(_mailInfoService);
        }

        public ActionResult Index()
        {
            AboutInstart model = (_aboutService.GetInfoAsync()) ?? new AboutInstart();
            return View(model);
        }

        public ActionResult program(EnumProgramType type)
        {
            var list = _programService.GetListByTypeAsync((int)type) ?? new List<Program>();
            ViewBag.Type = type.ToString();
            return View(list);
        }

        public ActionResult Show(int id)
        {
            var detail = _programService.GetByIdAsync(id);
            if (detail == null)
            {
                throw new Exception("内容不存在");
            }

            ViewBag.Type = detail.Type.ToString();
            return View(detail);
        }

        public ActionResult Division()
        {
            ViewBag.Type = "Division";

            ViewBag.DivisionList = _divisionService.GetAllAsync() ?? new List<Division>();           

            return View();
        }

        public ActionResult Company()
        {
            ViewBag.Type = "Company";
            var list = _companyService.GetAllAsync();
            return View(list);
        }

        public ActionResult CompanyDetail(int id)
        {
            var model = _companyService.GetByIdAsync(id);
            if (model == null)
            {
                throw new Exception("内容不存在");
            }
            ViewBag.Type = "Company";
            return View(model);
        }

        public ActionResult Studio()
        {
            ViewBag.Type = "Studio";
            var model = _studioService.GetInfoAsync() ?? new Studio();
            ViewBag.ImageList = _studioService.GetImgsAsync();
            return View(model);
        }

        public ActionResult Teach()
        {
            ViewBag.Type = "Teach";
            var model = _aboutService.GetInfoAsync() ?? new AboutInstart();
            ViewBag.PassLearning = model.PassLearning;
            return View();
        }

        /// <summary>
        /// 项目咨询
        /// </summary>
        /// <returns></returns>
        public ActionResult ProgramApply(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("项目不存在");
            }
            ViewBag.ProgramId = id;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("项目咨询")]
        public JsonResult SubmitApply(ProgramApply model)
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
            result.success = _programApplyService.InsertAsync(model);
            if (result.success)
            {
                result.success = _mailInfoService.SendMail("项目咨询", model.Country.GetDescription(), model.MajorId, model.Name, model.Phone, model.Email, model.Question, 
                    new List<string>(), System.Web.HttpContext.Current.Server.MapPath("/"));
            }
            return Json(result);
        }

        /// <summary>
        /// 实习预约
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyApply(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("实习单位不存在");
            }
            ViewBag.CompanyId = id;
            Copys copys = _copysService.GetInfoAsync();
            ViewBag.Copy = copys == null ? "" : copys.CompanyApplyCopy;
            ViewBag.CountryList = EnumberHelper.EnumToList<EnumCountry>();
            ViewBag.MajorList = _majorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("实习预约提交")]
        public JsonResult SetCompany(CompanyApply model)
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
            result.success = _companyApplyService.InsertAsync(model);
            if (result.success)
            {
                result.success = _mailInfoService.SendMail("实习预约", model.Country.GetDescription(), model.MajorId, model.Name, model.Phone, model.Email, "", 
                    fileList, System.Web.HttpContext.Current.Server.MapPath("/"));
            }
            return Json(result);
        }
        public ActionResult Feature(int type = 1)
        {
            ViewBag.Type = type;
            return View();
        }
    }
}