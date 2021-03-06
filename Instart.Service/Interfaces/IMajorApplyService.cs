﻿using Instart.Models;
using Instart.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Service
{
    public interface IMajorApplyService
    {
        PageModel<MajorApply> GetListAsync(int pageIndex, int pageSize, string name, EnumAccept accept);

        bool SetAcceptAsync(int id);

        bool InsertAsync(MajorApply model);

        List<MajorApply> GetTopListAsync(int topCount);
    }
}
