using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Repository;

namespace Instart.Service
{
    public class WorksService : ServiceBase, IWorksService
    {
        IWorksRepository _worksRepository = AutofacRepository.Resolve<IWorksRepository>();

        public WorksService()
        {
            base.AddDisposableObject(_worksRepository);
        }

        public Works GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _worksRepository.GetByIdAsync(id);
        }

        public PageModel<Works> GetListAsync(int pageIndex, int pageSize, int major = -1, string name = null)
        {
            return _worksRepository.GetListAsync(pageIndex, pageSize, major, name);
        }

        public bool InsertAsync(Works model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            return _worksRepository.InsertAsync(model);
        }

        public bool UpdateAsync(Works model)
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

            return _worksRepository.UpdateAsync(model);
        }

        public bool DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _worksRepository.DeleteAsync(id);
        }

        public List<Works> GetListByMajorIdAsync(int majorId, int topCount)
        {
            if (majorId <= 0)
            {
                throw new ArgumentException("majorId错误");
            }

            return _worksRepository.GetListByMajorIdAsync(majorId, topCount);
        }

        public List<Works> GetListByCourseIdAsync(int courseId, int topCount)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("courseId错误");
            }

            return _worksRepository.GetListByCourseIdAsync(courseId, topCount);
        }
    }
}
