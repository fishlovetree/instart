﻿using Instart.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// here&more
    /// </summary>
    public class HereMore
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public EnumCountry Country { get; set; }

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
        /// 申请人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 申请人电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 作品1
        /// </summary>
        public string ImgUrlA { get; set; }

        /// <summary>
        /// 作品2
        /// </summary>
        public string ImgUrlB { get; set; }

        /// <summary>
        /// 作品3
        /// </summary>
        public string ImgUrlC { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否沟通受理
        /// </summary>
        public bool IsAccept { get; set; }

        /// <summary>
        /// 受理时间
        /// </summary>
        public DateTime? AcceptTime { get; set; }
    }
}
