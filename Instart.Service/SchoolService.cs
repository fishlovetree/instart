using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Repository;

namespace Instart.Service
{
    public class SchoolService : ServiceBase, ISchoolService
    {
        ISchoolRepository _schoolRepository = AutofacRepository.Resolve<ISchoolRepository>();

        public SchoolService()
        {
            base.AddDisposableObject(_schoolRepository);
        }

        public School GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id不能为空");
            }

            return _schoolRepository.GetByIdAsync(id);
        }

        public PageModel<School> GetListAsync(int pageIndex, int pageSize, string name = null)
        {
            return _schoolRepository.GetListAsync(pageIndex, pageSize, name);
        }

        public IEnumerable<School> GetAllAsync()
        {
            return _schoolRepository.GetAllAsync();
        }

        public bool InsertAsync(School model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Name不能为null");
            }
            return _schoolRepository.InsertAsync(model);
        }

        public bool UpdateAsync(School model)
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

            return _schoolRepository.UpdateAsync(model);
        }

        public bool DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _schoolRepository.DeleteAsync(id);
        }

        public List<School> GetRecommendListAsync(int topCount)
        {
            if (topCount == 0)
            {
                return null;
            }

            return _schoolRepository.GetRecommendListAsync(topCount);
        }

        public bool SetRecommendAsync(int id, bool isRecommend)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _schoolRepository.SetRecommendAsync(id, isRecommend);
        }

        public List<School> GetHotListAsync(int topCount)
        {
            if (topCount == 0)
            {
                return null;
            }

            return _schoolRepository.GetHotListAsync(topCount);
        }

        public bool SetHotAsync(int id, bool isHot)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _schoolRepository.SetHotAsync(id, isHot);
        }

        public PageModel<School> GetListAsync(int pageIndex, int pageSize, string name = null, int country = -1, int major = -1, int level = -1)
        {
            return _schoolRepository.GetListAsync(pageIndex, pageSize, name, country, major, level);
        }

        public IEnumerable<SchoolMajor> GetMajorsByIdAsync(int id)
        {
            return _schoolRepository.GetMajorsByIdAsync(id);
        }

        public bool SetMajors(int schoolId, string bkmajors, string yjsmajors, string bkintroduces, string yjsintroduces)
        {
            return _schoolRepository.SetMajors(schoolId, bkmajors, yjsmajors, bkintroduces, yjsintroduces);
        }

        public List<School> GetListByMajorAsync(int majorId = 0, int topCount = 6) 
        {
            return _schoolRepository.GetListByMajorAsync(majorId, topCount);
        }
    }
}
