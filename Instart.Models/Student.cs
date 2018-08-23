using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    /// <summary>
    /// 成功学员
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 背景院校
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// 录取专业Id
        /// </summary>
        public int MajorId { get; set; }

        /// <summary>
        /// 录取专业名称
        /// </summary>
        public string MajorName { get; set; }

        /// <summary>
        /// 录取专业英文名称
        /// </summary>
        public string MajorNameEn { get; set; }

        /// <summary>
        /// 导师Id
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// 导师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 导师英文名称
        /// </summary>
        public string TeacherNameEn { get; set; }

        /// <summary>
        /// 校区Id
        /// </summary>
        public int CampusId { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string CampusName { get; set; }

        /// <summary>
        /// 校区英文名称
        /// </summary>
        public string CampusNameEn { get; set; }

        /// <summary>
        /// 学员介绍
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 视频链接
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// Banner图片
        /// </summary>
        public string BannerImg { get; set; }

        /// <summary>
        /// Banner视频
        /// </summary>
        public string BannerVideo { get; set; }

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
        ///是否推荐 
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 座右铭
        /// </summary>
        public string Motto { get; set; }

        /// <summary>
        /// OFFER ID列表，逗号拼接
        /// </summary>
        public string SchoolIds { get; set; }

        /// <summary>
        /// OFFER Name列表，逗号拼接
        /// </summary>
        public string SchoolNames { get; set; }
    }
}
