using Instart.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// 艺术专业
    /// </summary>
    public class Major
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }        

        /// <summary>
        /// 专业名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 专业英文名称
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 专业图片
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 专业介绍
        /// </summary>
        public string Introduce { get; set; }
        
        /// <summary>
        /// 就业前景
        /// </summary>
        public string Prospect { get; set; }

        /// <summary>
        /// 申请要求
        /// </summary>
        public string Apply { get; set; }

        /// <summary>
        /// 所属学部
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// 学部名称
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// 学部英文名称
        /// </summary>
        public string DivisionNameEn { get; set; }

        /// <summary>
        /// 学部颜色
        /// </summary>
        public string DivisionColor { get; set; }

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
        /// 被艺术院校选中
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 院校专业简介
        /// </summary>
        public string SchoolInfo { get; set; }
    }
}
