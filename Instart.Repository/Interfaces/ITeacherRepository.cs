using Instart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Repository
{
    public interface ITeacherRepository
    {
        Teacher GetByIdAsync(int id);        

        PageModel<Teacher> GetListAsync(int pageIndex, int pageSize, int division = -1, string name = null);

        IEnumerable<Teacher> GetAllAsync();

        bool InsertAsync(Teacher model);

        bool UpdateAsync(Teacher model);

        bool DeleteAsync(int id);

        List<Teacher> GetRecommendListAsync(int topCount);

        bool SetRecommend(int id, bool isRecommend);

        IEnumerable<Course> GetCoursesByIdAsync(int id);

        bool SetCourses(int teacherId, string courseIds);

        PageModel<Teacher> GetListByDivsionAsync(int divisionId, int pageIndex, int pageSize);

        List<SchoolMajor> GetSchoolListByTeacher(int teacherId);
    }
}
