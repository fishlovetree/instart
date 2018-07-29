﻿using Instart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Repository
{
    public interface ISchoolRepository
    {
        School GetByIdAsync(int id);        

        PageModel<School> GetListAsync(int pageIndex, int pageSize, string name = null);

        IEnumerable<School> GetAllAsync();

        bool InsertAsync(School model);

        bool UpdateAsync(School model);

        bool DeleteAsync(int id);

        List<School> GetRecommendListAsync(int topCount);

        bool SetRecommendAsync(int id, bool isRecommend);

        List<School> GetHotListAsync(int topCount);

        bool SetHotAsync(int id, bool isHot);

        PageModel<School> GetListAsync(int pageIndex, int pageSize, string name = null, int country = -1, int major = -1);

        IEnumerable<SchoolMajor> GetMajorsByIdAsync(int id);

        bool SetMajors(int schoolId, string majorIds, string introduces);

        List<School> GetListByMajorAsync(int majorId = 0);
    }
}
