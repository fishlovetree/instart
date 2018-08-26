using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Course
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 课程英文名
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 课程封面
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 课程介绍
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 课程过程
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 争对人群
        /// </summary>
        public string ForPeople { get; set; }

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

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 所属课程体系Id
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// 所属课程体系名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 所属课程体系英文名称
        /// </summary>
        public string SystemNameEn { get; set; }
    }
}
