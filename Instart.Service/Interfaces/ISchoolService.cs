using Instart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Service
{
    public interface ISchoolService
    {
         School GetByIdAsync(int id);

         PageModel<School> GetListAsync(int pageIndex, int pageSize, string name = null);

         IEnumerable<School> GetAllAsync(bool sortByName = true);

         bool InsertAsync(School model);

         bool UpdateAsync(School model);

         bool DeleteAsync(int id);

         List<School> GetRecommendListAsync(int topCount);

         bool SetRecommendAsync(int id, bool isRecommend);

         List<School> GetHotListAsync(int topCount);

         bool SetHotAsync(int id, bool isHot);

         PageModel<School> GetListAsync(int pageIndex, int pageSize, string name = null, int country = -1, int major = -1, int level = -1);

         IEnumerable<SchoolMajor> GetMajorsByIdAsync(int id);

         bool SetMajors(int schoolId, string bkmajors, string yjsmajors, string bkintroduces, string yjsintroduces);

         List<School> GetListByMajorAsync(int majorId = 0, int topCount = 6);
    }
}
