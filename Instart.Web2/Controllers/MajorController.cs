﻿using Instart.Models;
using Instart.Service;
using Instart.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Instart.Web2.Controllers
{
    /// <summary>
    /// 艺术专业
    /// </summary>
    public class MajorController : ControllerBase
    {
        IMajorService _majorService = AutofacService.Resolve<IMajorService>();
        IDivisionService _divisionService = AutofacService.Resolve<IDivisionService>();
        IWorksService _workService = AutofacService.Resolve<IWorksService>();
        ISchoolService _schoolService = AutofacService.Resolve<ISchoolService>();
        IStudentService _studentService = AutofacService.Resolve<IStudentService>();

        public MajorController()
        {
            this.AddDisposableObject(_majorService);
            this.AddDisposableObject(_divisionService);
            this.AddDisposableObject(_workService);
            this.AddDisposableObject(_schoolService);
            this.AddDisposableObject(_studentService);
        }

        public  ActionResult Index(int id = 0)
        {
            var divisionList =  _divisionService.GetAllAsync();

            if (divisionList == null || divisionList.Count() == 0)
            {
                throw new Exception("请先创建学部");
            }

            if (id == 0)
            {
                id = divisionList.First().Id;
            }

            ViewBag.DivisionList = divisionList;
            ViewBag.DivisionId = id;
            return View();
        }

        [HttpPost]
        public  JsonResult GetMajorList(int divisionId, int pageIndex, int pageSize = 8)
        {
            var result =  _majorService.GetListByDivsionAsync(divisionId, pageIndex, pageSize);
            return Success(data: new
            {
                total = result.Total,
                pageSize = pageSize,
                totalPage = (int)Math.Ceiling(result.Total * 1.0 / pageSize),
                list = result.Data
            });
        }

        public  ActionResult Details(int id)
        {
            var major =  _majorService.GetByIdAsync(id);
            if (major == null)
            {
                throw new Exception("专业不存在");
            }

            ViewBag.WorkList = ( _workService.GetListByMajorIdAsync(id, 3)) ?? new List<Instart.Models.Works>();
            List<School> schoolList = _schoolService.GetListByMajorAsync(id) ?? new List<Instart.Models.School>();
            //计算录取比例
            IEnumerable<Student> studentList = (_studentService.GetAllAsync()) ?? new List<Student>();
            foreach (School school in schoolList)
            {
                int count = 0;
                foreach (Student student in studentList)
                {
                    if (student.SchoolId == school.Id)
                    {
                        count++;
                    }
                }
                school.AcceptRate = "0";
                if (schoolList.Count() > 0)
                {
                    decimal rate = (decimal)count / schoolList.Count();
                    school.AcceptRate = (rate * 100).ToString("f2");
                }
            }
            ViewBag.SchoolList = schoolList;
            return View(major);
        }
    }
}