using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models.Enums
{
    public enum EnumProgramType
    {
        /// <summary>
        /// ProToPro
        /// </summary>
        [Description("你好！面试官")]
        ProToPro = 1,
        /// <summary>
        /// Workshop
        /// </summary>
        [Description("Workshop")]
        Workshop = 2,
        /// <summary>
        /// 艺术家孵化平台
        /// </summary>
        [Description("艺术家孵化平台")]
        Actor = 3,
        /// <summary>
        /// 公共艺术类
        /// </summary>
        [Description("驻地项目")]
        Area = 4
    }
}
