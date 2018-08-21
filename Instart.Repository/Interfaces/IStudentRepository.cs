using Instart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Repository
{
    public interface IStudentRepository
    {
        Student GetByIdAsync(int id);        

        PageModel<Student> GetListAsync(int pageIndex, int pageSize, int division = -1, string name = null);

        bool InsertAsync(Student model);

        bool UpdateAsync(Student model);

        bool DeleteAsync(int id);

        List<Student> GetRecommendListAsync(int topCount);

        bool SetRecommend(int id, bool isRecommend);

        IEnumerable<Student> GetAllAsync();

        IEnumerable<Student> GetStarStudentsAsync();

        IEnumerable<int> GetCoursesByIdAsync(int id);

        bool SetCourses(int studentId, string courseIds);

        IEnumerable<int> GetSchoolsByIdAsync(int id);

        bool SetSchools(int studentId, string schoolIds);

        List<Student> GetListByCourseAsync(int courseId = -1);

        List<Student> GetListByTeacherAsync(int teacherId = -1);

        List<Student> GetListByCampusAsync(int campusId = -1, int topCount = 4);

        List<School> GetStudentSchools(int studentId = -1);
    }
}
