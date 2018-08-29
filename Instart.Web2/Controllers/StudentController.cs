using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Instart.Models;
using Instart.Service;
using Instart.Service.Base;

namespace Instart.Web2.Controllers
{
    /// <summary>
    /// 成功学员
    /// </summary>
    public class StudentController : ControllerBase
    {
        IStudentService _studentService = AutofacService.Resolve<IStudentService>();
        ISchoolService _schoolService = AutofacService.Resolve<ISchoolService>();

        public StudentController()
        {
            this.AddDisposableObject(_studentService);
            this.AddDisposableObject(_schoolService);
        }

        public  ActionResult Index() {
            IEnumerable<Student> studentList = ( _studentService.GetAllAsync()) ?? new List<Student>();
            IEnumerable<School> schoolList = ( _schoolService.GetAllAsync()) ?? new List<School>();

            //一行4个学员
            List<List<Student>> studentMap = new List<List<Student>>();
            int studentIndex = 1;
            List<Student> slist = new List<Student>();
            foreach (Student student in studentList)
            {
                slist.Add(student);
                if (studentIndex % 8 == 0)
                {
                    studentMap.Add(new List<Student>(slist.ToArray()));
                    slist.Clear();
                }
                studentIndex++;
            }
            if (slist.Count > 0)
            {
                studentMap.Add(slist);
            }

            //一行6个学校
            List<List<School>> schoolMap = new List<List<School>>();
            int schoolIndex = 1;
            List<School> tlist = new List<School>();
            foreach (School school in schoolList)
            {
                tlist.Add(school);
                if (schoolIndex % 12 == 0)
                {
                    schoolMap.Add(new List<School>(tlist.ToArray()));
                    tlist.Clear();
                }
                schoolIndex++;
            }
            if (tlist.Count > 0)
            {
                schoolMap.Add(tlist);
            }

            ViewBag.StudentMap = studentMap;
            ViewBag.SchoolMap = schoolMap;
            ViewBag.VideoList = ( _studentService.GetStarStudentsAsync()) ?? new List<Instart.Models.Student>();
            return View();
        }

        public  ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                throw new Exception("成功学员不存在。");
            }
            Student student =  _studentService.GetByIdAsync(id);
            IEnumerable<Student> studentList = ( _studentService.GetListByDivisionAsync(student.DivisionId)) ?? new List<Student>();
            //一行4个学员
            List<List<Student>> studentMap = new List<List<Student>>();
            int studentIndex = 1;
            List<Student> slist = new List<Student>();
            foreach (Student item in studentList)
            {
                slist.Add(item);
                if (studentIndex % 8 == 0)
                {
                    studentMap.Add(new List<Student>(slist.ToArray()));
                    slist.Clear();
                }
                studentIndex++;
            }
            if (slist.Count > 0)
            {
                studentMap.Add(slist);
            }
            ViewBag.StudentMap = studentMap;
            return View(student ?? new Student());
        }
    }
}