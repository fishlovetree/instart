using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models
{
    public class SchoolMajor
    {
        /// <summary>
        /// 院校Id
        /// </summary>
        public int SchoolId { get; set; }

        /// <summary>
        /// 院校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 院校logo
        /// </summary>
        public string SchoolLogo { get; set; }

        /// <summary>
        /// 专业Id
        /// </summary>
        public int MajorId { get; set; }

        /// <summary>
        /// 专业名称
        /// </summary>
        public string MajorName { get; set; }

        /// <summary>
        /// 专业介绍
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 专业类型：0-本科，1-研究生
        /// </summary>
        public int Type { get; set; }
    }
}
