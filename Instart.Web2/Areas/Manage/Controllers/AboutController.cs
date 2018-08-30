using Instart.Web2.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instart.Service;
using Instart.Service.Base;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Common;
using Instart.Web2.Models;
using System.IO;

namespace Instart.Web2.Areas.Manage.Controllers
{
    [AdminValidation]
    public class AboutController : ManageControllerBase
    {
        IAboutInstartService _aboutInstartService = AutofacService.Resolve<IAboutInstartService>();

        public AboutController()
        {
            base.AddDisposableObject(_aboutInstartService);
        }

        public ActionResult Index()
        {
            AboutInstart model = _aboutInstartService.GetInfoAsync();
            if (model == null) model = new AboutInstart();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Operation("设置关于我们")]
        public JsonResult Set(AboutInstart model)
        {
            if (model == null)
            {
                return Error("参数错误。");
            }

            var fileImg = Request.Files["fileImg"];
            if (fileImg != null)
            {
                string uploadResult = UploadHelper.Process(fileImg.FileName, fileImg.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.ImgUrl = uploadResult;
                }
            }

            var fileVideo = Request.Files["fileVideo"];
            if (fileVideo != null)
            {
                string uploadResult = UploadHelper.Process(fileVideo.FileName, fileVideo.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.VideoUrl = uploadResult;
                }
            }
            var filePreToPro = Request.Files["filePreToPro"];
            if (filePreToPro != null)
            {
                string uploadResult = UploadHelper.Process(filePreToPro.FileName, filePreToPro.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.PreToProImg = uploadResult;
                }
            }
            var fileDivision = Request.Files["fileDivision"];
            if (fileDivision != null)
            {
                string uploadResult = UploadHelper.Process(fileDivision.FileName, fileDivision.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.DivisionImg = uploadResult;
                }
            }
            var filePassLearning = Request.Files["filePassLearning"];
            if (filePassLearning != null)
            {
                string uploadResult = UploadHelper.Process(filePassLearning.FileName, filePassLearning.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.PassLearningImg = uploadResult;
                }
            }
            var fileWorkShop = Request.Files["fileWorkShop"];
            if (fileWorkShop != null)
            {
                string uploadResult = UploadHelper.Process(fileWorkShop.FileName, fileWorkShop.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.WorkShopImg = uploadResult;
                }
            }
            var fileStudio = Request.Files["fileStudio"];
            if (fileStudio != null)
            {
                string uploadResult = UploadHelper.Process(fileStudio.FileName, fileStudio.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.StudioImg = uploadResult;
                }
            }
            var fileCompany = Request.Files["fileCompany"];
            if (fileCompany != null)
            {
                string uploadResult = UploadHelper.Process(fileCompany.FileName, fileCompany.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.CompanyImg = uploadResult;
                }
            }
            var fileActor = Request.Files["fileActor"];
            if (fileActor != null)
            {
                string uploadResult = UploadHelper.Process(fileActor.FileName, fileActor.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.ActorImg = uploadResult;
                }
            }
            var filePrograms = Request.Files["filePrograms"];
            if (filePrograms != null)
            {
                string uploadResult = UploadHelper.Process(filePrograms.FileName, filePrograms.InputStream);
                if (!string.IsNullOrEmpty(uploadResult))
                {
                    model.ProgramsImg = uploadResult;
                }
            }
            var result = new ResultBase();

            int count = _aboutInstartService.GetCountAsync();
            if (count > 0)
            {
                result.success = _aboutInstartService.UpdateAsync(model);
            }
            else
            {
                result.success = _aboutInstartService.InsertAsync(model);
            }

            return Json(result);
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