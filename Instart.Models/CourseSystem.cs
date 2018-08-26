using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// 课程体系
    /// </summary>
    public class CourseSystem
    {
        /// <summary>
        /// 课程体系Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 课程体系名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 课程体系英文名
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 课程体系封面
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 课程体系Banner
        /// </summary>
        public string BannerImg { get; set; }

        /// <summary>
        /// 课程体系介绍
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 状态，1：正常，0：删除
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}
