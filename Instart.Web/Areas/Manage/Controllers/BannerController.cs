﻿using Instart.Common;
using Instart.Models;
using Instart.Service;
using Instart.Service.Base;
using Instart.Web.Areas.Manage.Models;
using Instart.Web.Attributes;
using Instart.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Instart.Web.Areas.Manage.Controllers
{
    [AdminValidation]
    public class BannerController : ManageControllerBase
    {
        IBannerService _bannserService = AutofacService.Resolve<IBannerService>();

        public BannerController()
        {
            base.AddDisposableObject(_bannserService);
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ResultBase> Insert(BannerRequest request)
        {
            try
            {
                string msg = this.Validate(request, false);

                if (!string.IsNullOrEmpty(msg))
                {
                    return Error(msg);
                }

                var model = request.Mapper<Banner>();

                return new ResultBase
                {
                    success = await _bannserService.UpdateAsync(model)
                };
            }
            catch(Exception ex)
            {
                LogHelper.Error($"BannerController.Insert异常", ex);
                return Error(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ResultBase> Update(BannerRequest request)
        {
            try
            {
                string msg = this.Validate(request, true);
                if (!string.IsNullOrEmpty(msg))
                {
                    return Error(msg);
                }

                var model = request.Mapper<Banner>();

                return new ResultBase
                {
                    success = await _bannserService.UpdateAsync(model)
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"BannerController.Update异常", ex);
                return Error(ex.Message);
            }
        }

        [NonAction]
        private string Validate(BannerRequest request, bool isUpdate = false)
        {
            if (request == null)
            {
                return "参数错误";
            }

            if (isUpdate && request.Id <= 0)
            {
                return "Id不正确";
            }

            if (string.IsNullOrEmpty(request.Title))
            {
                return "标题不能为空";
            }

            if (string.IsNullOrEmpty(request.ImageUrl))
            {
                return "图片不能为空";
            }

            return string.Empty;
        }
    }
}