using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models.Enums
{
    /// <summary>
    /// banner图片显示位置
    /// </summary>
    public enum EnumBannerPos
    {
        /// <summary>
        /// 首页
        /// </summary>
        [Description("首页")]
        Index = 0,
        /// <summary>
        /// 艺术课程
        /// </summary>
        [Description("艺术课程")]
        Course = 1,
        /// <summary>
        /// 艺术导师
        /// </summary>
        [Description("艺术导师")]
        Teacher = 2,
        /// <summary>
        /// 成功学员
        /// </summary>
        [Description("成功学员")]
        Student = 3,
        /// <summary>
        /// 艺术院校
        /// </summary>
        [Description("艺术院校")]
        School = 4,
        /// <summary>
        /// 艺术专业
        /// </summary>
        [Description("艺术专业")]
        Major = 5,
        /// <summary>
        /// 关于Instart
        /// </summary>
        [Description("关于Instart")]
        About = 6,
        /// <summary>
        /// 招贤纳士
        /// </summary>
        [Description("招贤纳士")]
        Recruit = 7,
        /// <summary>
        /// 校区
        /// </summary>
        [Description("校区")]
        Campus = 8,
        /// <summary>
        /// 艺术资讯
        /// </summary>
        [Description("艺术资讯")]
        Info = 9,
    }
}
