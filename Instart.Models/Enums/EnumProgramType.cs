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
        /// iN实习内推
        /// </summary>
        [Description("iN实习内推")]
        Practice = 1,
        /// <summary>
        /// iN申请干货
        /// </summary>
        [Description("iN申请干货")]
        Apply = 2,
        /// <summary>
        /// iN院校分享
        /// </summary>
        [Description("iN院校分享")]
        School = 3,
        /// <summary>
        /// iN设计资讯
        /// </summary>
        [Description("iN设计资讯")]
        Actor = 4
    }
}
