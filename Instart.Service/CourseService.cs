using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Repository;

namespace Instart.Service
{
    public class CourseService : ServiceBase, ICourseService
    {
        ICourseRepository _courseRepository = AutofacRepository.Resolve<ICourseRepository>();

        public CourseService()
        {
            base.AddDisposableObject(_courseRepository);
        }

        public Course GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _courseRepository.GetByIdAsync(id);
        }

        public PageModel<Course> GetListAsync(int pageIndex, int pageSize, int systemId = -1, string name = null)
        {
            return _courseRepository.GetListAsync(pageIndex, pageSize, systemId, name);
        }

        public IEnumerable<Course> GetAllAsync()
        {
            return _courseRepository.GetAllAsync();
        }

        public IEnumerable<Course> GetListBySystemId(int systemId)
        {
            return _courseRepository.GetListBySystemId(systemId);
        }

        public IEnumerable<Course> GetAllByStudentAsync(int studentId) 
        {
            return _courseRepository.GetAllByStudentAsync(studentId);
        }

        public bool InsertAsync(Course model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Name不能为null");
            }

            return _courseRepository.InsertAsync(model);
        }

        public bool UpdateAsync(Course model)
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

            return _courseRepository.UpdateAsync(model);
        }

        public bool DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _courseRepository.DeleteAsync(id);
        }

        public List<Course> GetRecommendListAsync(int topCount)
        {
            if (topCount == 0)
            {
                return null;
            }

            return _courseRepository.GetRecommendListAsync(topCount);
        }

        public bool SetRecommendAsync(int id, bool isRecommend)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _courseRepository.SetRecommend(id, isRecommend);
        }

        public int GetInfoCountAsync()
        {
            return _courseRepository.GetInfoCountAsync();
        }

        public CourseInfo GetInfoAsync()
        {
            return _courseRepository.GetInfoAsync();
        }

        public bool InsertInfoAsync(CourseInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            return _courseRepository.InsertInfoAsync(model);
        }

        public bool UpdateInfoAsync(CourseInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            return _courseRepository.UpdateInfoAsync(model);
        }

        public IEnumerable<int> GetTeachersByIdAsync(int id)
        {
            return _courseRepository.GetTeachersByIdAsync(id);
        }

        public bool SetTeachers(int courseId, string teacherIds)
        {
            return _courseRepository.SetTeachers(courseId, teacherIds);
        }
    }
}
