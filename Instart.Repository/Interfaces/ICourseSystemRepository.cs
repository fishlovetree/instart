using Instart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Repository
{
    public interface ICourseSystemRepository
    {
        CourseSystem GetByIdAsync(int id);

        PageModel<CourseSystem> GetListAsync(int pageIndex, int pageSize, string name = null);

        IEnumerable<CourseSystem> GetAllAsync();

        bool InsertAsync(CourseSystem model);

        bool UpdateAsync(CourseSystem model);

        bool DeleteAsync(int id);
    }
}
