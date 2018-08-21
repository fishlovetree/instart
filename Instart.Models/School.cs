using Instart.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// 艺术院校
    /// </summary>
    public class School
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称--
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 英文名称--
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// LOGO--
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 学费--
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// 托福
        /// </summary>
        public string Tuofu { get; set; }

        /// <summary>
        /// 雅思
        /// </summary>
        public string Yasi { get; set; }

        /// <summary>
        /// 研
        /// </summary>
        public string Yan { get; set; }

        /// <summary>
        /// SAT
        /// </summary>
        public string SAT { get; set; }

        /// <summary>
        /// 推荐专业--
        /// </summary>
        public string RecommendMajor { get; set; }

        /// <summary>
        /// 学校类型--
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 地址--
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 学校图片--
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 申请难度--
        /// </summary>
        public string Difficult { get; set; }

        /// <summary>
        /// 学校介绍--
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 校友
        /// </summary>
        public string OldBoy { get; set; }

        /// <summary>
        /// 学科
        /// </summary>
        public string Lesson { get; set; }

        /// <summary>
        /// 申请要求
        /// </summary>
        public string ApplyCondition { get; set; }

        /// <summary>
        /// 小贴士
        /// </summary>
        public string ApplyHelp { get; set; }

        /// <summary>
        /// 学校印象（图文json）
        /// </summary>
        public string Impress { get; set; }

        /// <summary>
        /// 国家--
        /// </summary>
        public EnumCountry Country { get; set; }       

        /// <summary>
        /// 网址--
        /// </summary>
        public string WebSite { get; set; }     

        /// <summary>
        /// 申请开始日期
        /// </summary>
        public DateTime? ApplyStartDate { get; set; }

        /// <summary>
        /// 申请结束日期
        /// </summary>
        public DateTime? ApplyEndDate { get; set; }        

        /// <summary>
        /// 状态，1：正常，0：删除
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 奖学金--
        /// </summary>
        public string Scholarship { get; set; }

        /// <summary>
        /// 录取比例
        /// </summary>
        public String AcceptRate { get; set; }

        /// <summary>
        /// 是否热搜
        /// </summary>
        public bool IsHot { get; set; }

        /// <summary>
        /// 申请截止日期
        /// </summary>
        public string LimitDate { get; set; }

        /// <summary>
        /// 语言要求
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 学费明细
        /// </summary>
        public string Moneys { get; set; }

        /// <summary>
        /// 建校时间
        /// </summary>
        public string CreateYear { get; set; }

        /// <summary>
        /// Banner图片
        /// </summary>
        public string BannerImg { get; set; }

        /// <summary>
        /// 学历，逗号拼接
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 被学员模块选中
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
