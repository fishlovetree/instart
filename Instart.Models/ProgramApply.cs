﻿using Instart.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// 项目咨询
    /// </summary>
    public class ProgramApply
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 学校Id
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// 项目英文名
        /// </summary>
        public string ProgramNameEn { get; set; }

        /// <summary>
        /// 项目类型: 1-pretopro,2-workshop,3-艺术家孵化平台,4-驻地项目
        /// </summary>
        public EnumProgramType ProgramType { get; set; }

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
        /// 问题
        /// </summary>
        public string Question { get; set; }

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
