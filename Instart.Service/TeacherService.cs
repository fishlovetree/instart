using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Repository;

namespace Instart.Service
{
    public class TeacherService : ServiceBase, ITeacherService
    {
        ITeacherRepository _teacherRepository = AutofacRepository.Resolve<ITeacherRepository>();

        public TeacherService()
        {
            base.AddDisposableObject(_teacherRepository);
        }

        public Teacher GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _teacherRepository.GetByIdAsync(id);
        }

        public PageModel<Teacher> GetListAsync(int pageIndex, int pageSize, int division = -1, string name = null)
        {
            return _teacherRepository.GetListAsync(pageIndex, pageSize, division, name);
        }

        public IEnumerable<Teacher> GetAllAsync()
        {
            return _teacherRepository.GetAllAsync();
        }

        public bool InsertAsync(Teacher model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Name不能为null");
            }

            return _teacherRepository.InsertAsync(model);
        }

        public bool UpdateAsync(Teacher model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Name不能为null");
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Id错误");
            }

            return _teacherRepository.UpdateAsync(model);
        }

        public bool DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _teacherRepository.DeleteAsync(id);
        }

        public List<Teacher> GetRecommendListAsync(int topCount)
        {
            if (topCount == 0)
            {
                return null;
            }

            return _teacherRepository.GetRecommendListAsync(topCount);
        }

        public bool SetRecommendAsync(int id, bool isRecommend)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _teacherRepository.SetRecommend(id, isRecommend);
        }

        public IEnumerable<Course> GetCoursesByIdAsync(int id)
        {
            return _teacherRepository.GetCoursesByIdAsync(id);
        }

        public bool SetCourses(int teacherId, string courseIds)
        {
            return _teacherRepository.SetCourses(teacherId, courseIds);
        }

        public IEnumerable<Major> GetMajorsByIdAsync(int id)
        {
            return _teacherRepository.GetMajorsByIdAsync(id);
        }

        public bool SetMajors(int teacherId, string majorIds)
        {
            return _teacherRepository.SetMajors(teacherId, majorIds);
        }

        public PageModel<Teacher> GetListByDivsionAsync(int divisionId, int pageIndex, int pageSize)
        {
            return _teacherRepository.GetListByDivsionAsync(divisionId, pageIndex, pageSize);
        }

        public List<SchoolMajor> GetSchoolListByTeacher(int teacherId) 
        {
            return _teacherRepository.GetSchoolListByTeacher(teacherId);
        }
    }
}
