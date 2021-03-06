﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Instart.Repository;

namespace Instart.Service
{
    public class CampusService : ServiceBase, ICampusService
    {
        ICampusRepository _campusRepository = AutofacRepository.Resolve<ICampusRepository>();

        public CampusService()
        {
            base.AddDisposableObject(_campusRepository);
        }

        public Campus GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }
            return _campusRepository.GetByIdAsync(id);
        }

        public PageModel<Campus> GetListAsync(int pageIndex, int pageSize, string name = null)
        {
            return _campusRepository.GetListAsync(pageIndex, pageSize, name);
        }

        public IEnumerable<Campus> GetAllAsync()
        {
            return _campusRepository.GetAllAsync();
        }

        public bool InsertAsync(Campus model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Name不能为null");
            }

            return _campusRepository.InsertAsync(model);
        }

        public bool UpdateAsync(Campus model)
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

            return _campusRepository.UpdateAsync(model);
        }

        public bool DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _campusRepository.DeleteAsync(id);
        }

        public IEnumerable<CampusImg> GetImgsByCampusIdAsync(int campusId)
        {
            return _campusRepository.GetImgsByCampusIdAsync(campusId);
        }

        public bool InsertImgAsync(CampusImg model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model不能为null");
            }
            return _campusRepository.InsertImgAsync(model);
        }

        public bool DeleteImgAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id错误");
            }

            return _campusRepository.DeleteImgAsync(id);
        }
    }
}
