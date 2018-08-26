using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Repository;

namespace Instart.Service
{
    public class CourseSystemService : ServiceBase, ICourseSystemService
    {
        ICourseSystemRepository _courseSystemRepository = AutofacRepository.Resolve<ICourseSystemRepository>();

        public CourseSystemService()
        {
            base.AddDisposableObject(_courseSystemRepository);
        }

        public CourseSystem GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _courseSystemRepository.GetByIdAsync(id);
        }

        public PageModel<CourseSystem> GetListAsync(int pageIndex, int pageSize, string name = null)
        {
            return _courseSystemRepository.GetListAsync(pageIndex, pageSize, name);
        }

        public IEnumerable<CourseSystem> GetAllAsync()
        {
            return _courseSystemRepository.GetAllAsync();
        }

        public bool InsertAsync(CourseSystem model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Name不能为null");
            }

            return _courseSystemRepository.InsertAsync(model);
        }

        public bool UpdateAsync(CourseSystem model)
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

            return _courseSystemRepository.UpdateAsync(model);
        }

        public bool DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _courseSystemRepository.DeleteAsync(id);
        }
    }
}
