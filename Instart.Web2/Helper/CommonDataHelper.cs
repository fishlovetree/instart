using Instart.Models;
using Instart.Models.Enums;
using Instart.Service;
using Instart.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instart.Web2.Helper
{
    public class CommonDataHelper
    {
        public static List<Banner> GetBannerList(EnumBannerPos pos)
        {
            var list = AutofacService.Resolve<IBannerService>().GetBannerListByPosAsync(pos);
            if (list != null && list.Count > 0)
            {
                return list;
            }

            list = AutofacService.Resolve<IBannerService>().GetBannerListByPosAsync(EnumBannerPos.Index);   // 如果页面没有banner配置，默认给index的banner配置
            return list ?? new List<Instart.Models.Banner>();
        }

        public static List<Course> GetCourseList()
        {
            return (AutofacService.Resolve<ICourseService>().GetRecommendListAsync(5)) ?? new List<Instart.Models.Course>();
        }
    }
}