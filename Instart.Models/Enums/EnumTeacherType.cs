using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Models.Enums
{
    public enum EnumTeacherType
    {
        /// <summary>
        /// 艺术导师
        /// </summary>
        [Description("艺术导师")]
        Internal = 1,
        /// <summary>
        /// 海外审核官
        /// </summary>
        [Description("海外审核官")]
        Overseas = 2
    }
}
