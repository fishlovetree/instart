using Instart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Service
{
    public interface ITeacherService
    {
         Teacher GetByIdAsync(int id);

         PageModel<Teacher> GetListAsync(int pageIndex, int pageSize, int type = -1, int division = -1, string name = null);

         IEnumerable<Teacher> GetAllAsync(int type = 1);

         bool InsertAsync(Teacher model);

         bool UpdateAsync(Teacher model);

         bool DeleteAsync(int id);

         List<Teacher> GetRecommendListAsync(int topCount);

         bool SetRecommendAsync(int id, bool isRecommend);

         IEnumerable<Course> GetCoursesByIdAsync(int id);

         bool SetCourses(int teacherId, string courseIds);

         IEnumerable<Major> GetMajorsByIdAsync(int id);

         bool SetMajors(int teacherId, string majorIds);

         PageModel<Teacher> GetListByDivsionAsync(int divisionId, int pageIndex, int pageSize);

         List<SchoolMajor> GetSchoolListByTeacher(int teacherId);
    }
}
