using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// 艺术导师
    /// </summary>
    public class Teacher
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 学部Id
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// 学部名
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// 学部英文名
        /// </summary>
        public string DivisionNameEn { get; set; }

        /// <summary>
        /// 学部颜色
        /// </summary>
        public string DivisionColor { get; set; }

        /// <summary>
        /// 学校Id
        /// </summary>
        public int SchoolId { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 学校英文名
        /// </summary>
        public string SchoolNameEn { get; set; }

        /// <summary>
        /// 专业Id
        /// </summary>
        public int MajorId { get; set; }

        /// <summary>
        /// 专业名称
        /// </summary>
        public string MajorName { get; set; }

        /// <summary>
        /// 专业英文名称
        /// </summary>
        public string MajorNameEn { get; set; }

        /// <summary>
        /// 导师姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 导师英文名
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 导师头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 导师介绍
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }

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
        /// 是否被选中
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 擅长专业 ID列表，逗号拼接
        /// </summary>
        public string MajorIds { get; set; }

        /// <summary>
        /// 擅长专业 Name列表，逗号拼接
        /// </summary>
        public string MajorNames { get; set; }
    }
}
