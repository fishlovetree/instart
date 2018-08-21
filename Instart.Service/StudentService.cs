using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Repository;

namespace Instart.Service
{
    public class StudentService : ServiceBase, IStudentService
    {
        IStudentRepository _studentRepository = AutofacRepository.Resolve<IStudentRepository>();

        public StudentService() {
            base.AddDisposableObject(_studentRepository);
        }

        public  Student GetByIdAsync(int id) {
            if (id <= 0) {
                throw new ArgumentException("id不能为空");
            }

            return _studentRepository.GetByIdAsync(id);
        }

        public  PageModel<Student> GetListAsync(int pageIndex, int pageSize, int division = -1, string name = null) {
            return _studentRepository.GetListAsync(pageIndex, pageSize, division, name);
        }

        public  IEnumerable<Student> GetAllAsync()
        {
            IEnumerable<Student> list = _studentRepository.GetAllAsync();
            //获取学员OFFER
            foreach (Student item in list)
            {
                item.SchoolList = _studentRepository.GetStudentSchools(item.Id);
            }
            return list;
        }

        public  IEnumerable<Student> GetStarStudentsAsync()
        {
            return _studentRepository.GetStarStudentsAsync();
        }

        public  bool InsertAsync(Student model) {
            if (model == null) {
                throw new ArgumentNullException("model不能为null");
            }

            if (string.IsNullOrEmpty(model.Name)) {
                throw new ArgumentNullException("Name不能为null");
            }

            return _studentRepository.InsertAsync(model);
        }

        public  bool UpdateAsync(Student model) {
            if (model == null) {
                throw new ArgumentNullException("model不能为null");
            }

            if (string.IsNullOrEmpty(model.Name)) {
                throw new ArgumentNullException("Name不能为null");
            }

            if (model.Id <= 0) {
                throw new ArgumentException("Id错误");
            }
            
            return _studentRepository.UpdateAsync(model);
        }

        public  bool DeleteAsync(int id) {
            if (id <= 0) {
                throw new ArgumentException("id错误");
            }

            return _studentRepository.DeleteAsync(id);
        }

        public  List<Student> GetRecommendListAsync(int topCount)
        {
            if(topCount == 0)
            {
                return null;
            }

            List<Student> list = _studentRepository.GetRecommendListAsync(topCount);
            //获取学员OFFER
            foreach (Student item in list) 
            {
                item.SchoolList = _studentRepository.GetStudentSchools(item.Id);
            }
            return list;
        }

        public bool SetRecommend(int id, bool isRecommend)
        {
            return _studentRepository.SetRecommend(id, isRecommend);
        }

        public IEnumerable<int> GetCoursesByIdAsync(int id)
        {
            return _studentRepository.GetCoursesByIdAsync(id);
        }

        public bool SetCourses(int studentId, string courseIds)
        {
            return _studentRepository.SetCourses(studentId, courseIds);
        }

        public IEnumerable<int> GetSchoolsByIdAsync(int id)
        {
            return _studentRepository.GetSchoolsByIdAsync(id);
        }

        public bool SetSchools(int studentId, string schoolIds)
        {
            return _studentRepository.SetSchools(studentId, schoolIds);
        }

        public List<Student> GetListByCourseAsync(int courseId = -1)
        {
            List<Student> list = _studentRepository.GetListByCourseAsync(courseId);
            //获取学员OFFER
            foreach (Student item in list)
            {
                item.SchoolList = _studentRepository.GetStudentSchools(item.Id);
            }
            return list;
        }

        public List<Student> GetListByTeacherAsync(int teacherId = -1)
        {
            List<Student> list = _studentRepository.GetListByTeacherAsync(teacherId);
            //获取学员OFFER
            foreach (Student item in list)
            {
                item.SchoolList = _studentRepository.GetStudentSchools(item.Id);
            }
            return list;
        }

        public List<Student> GetListByCampusAsync(int campusId = -1, int topCount = 4) 
        {
            List<Student> list = _studentRepository.GetListByCampusAsync(campusId, topCount);
            //获取学员OFFER
            foreach (Student item in list)
            {
                item.SchoolList = _studentRepository.GetStudentSchools(item.Id);
            }
            return list;
        }
    }
}
