using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Dapper;
using System.Data.SqlClient;

namespace Instart.Repository
{
    public class StudentRepository : IStudentRepository
    {
        public Student GetByIdAsync(int id) {
            using (var conn = DapperFactory.GetConnection()) {
                string sql = @"select t.*, b.Name as MajorName, b.NameEn as MajorNameEn, c.Name as TeacherName, 
                     c.NameEn as TeacherNameEn, d.Name as DivisionName, d.NameEn as DivisionNameEn,d.BgColor as DivisionColor, 
                     f.Name as CampusName, f.NameEn as CampusNameEn from Student t 
                     left join [Major] as b on b.Id = t.MajorId 
                     left join [Teacher] as c on c.Id = t.TeacherId 
                     left join [Division] as d on d.Id = t.DivisionId 
                     left join [Campus] as f on f.Id = t.CampusId
                     where t.Id = @Id and t.Status=1;";
                return conn.QueryFirstOrDefault<Student>(sql, new { Id = id });
            }
        }

        public PageModel<Student> GetListAsync(int pageIndex, int pageSize, int division = -1, string name = null) {
            using (var conn = DapperFactory.GetConnection()) {
                #region generate condition
                string where = "where a.Status=1";
                if (!string.IsNullOrEmpty(name)) {
                    where += string.Format(" and a.Name like '%{0}%'",name);
                }
                if (division != -1)
                {
                    where += string.Format(" and a.DivisionId = {0}",division);
                }
                #endregion

                string countSql = string.Format("select count(1) from [Student] as a {0};",where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0) {
                    return new PageModel<Student>();
                }

                string sql = string.Format(@"select * from (
                     select a.*, b.Name as MajorName, b.NameEn as MajorNameEn, c.Name as TeacherName, c.NameEn as TeacherNameEn, 
                     d.Name as DivisionName, d.NameEn as DivisionNameEn,d.BgColor as DivisionColor, 
                     f.Name as CampusName, f.NameEn as CampusNameEn, ROW_NUMBER() over (Order by a.Id desc) as RowNumber from [Student] as a
                     left join [Major] as b on b.Id = a.MajorId 
                     left join [Teacher] as c on c.Id = a.TeacherId 
                     left join [Division] as d on d.Id = a.DivisionId
                     left join [Campus] as f on f.Id = a.CampusId {0}
                     ) as d
                     where RowNumber between {1} and {2};", where,((pageIndex - 1) * pageSize) + 1,pageIndex * pageSize);
                var list = conn.Query<Student>(sql.Trim());

                return new PageModel<Student> {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }

        public IEnumerable<Student> GetAllAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where t.Status=1";
                #endregion

                string sql = string.Format(@"select t.*, b.Name as MajorName, b.NameEn as MajorNameEn, c.Name as TeacherName, 
                     c.NameEn as TeacherNameEn, d.Name as DivisionName, d.NameEn as DivisionNameEn, d.BgColor as DivisionColor,
                     f.Name as CampusName, f.NameEn as CampusNameEn from Student t 
                     left join [Major] as b on b.Id = t.MajorId 
                     left join [Teacher] as c on c.Id = t.TeacherId 
                     left join [Division] as d on d.Id = t.DivisionId
                     left join [Campus] as f on f.Id = t.CampusId {0} order by t.Id;", where);
                return conn.Query<Student>(sql);
            }
        }

        public IEnumerable<Student> GetStarStudentsAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where Status=1 and ImgUrl is not null and VideoUrl is not null";
                #endregion

                string sql = string.Format(@"select * from [Student] {0};",where);
                return conn.Query<Student>(sql);
            }
        }

        public bool InsertAsync(Student model) {
            using (var conn = DapperFactory.GetConnection()) {
                var fields = model.ToFields(removeFields: new List<string> { "Id","MajorName", "MajorNameEn", "TeacherName", "TeacherNameEn",
                    "DivisionName", "DivisionNameEn", "DivisionColor", "IsRecommend", "CampusName", "CampusNameEn", "SchoolList"});
                if (fields == null || fields.Count == 0) {
                    return false;
                }

                model.CreateTime = DateTime.Now;
                model.ModifyTime = DateTime.Now;
                model.Status = 1;

                string sql = string.Format("insert into [Student] ({0}) values ({1});",string.Join(",", fields),string.Join(",", fields.Select(n => "@" + n)));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool UpdateAsync(Student model) {
            using (var conn = DapperFactory.GetConnection()) {
                var fields = model.ToFields(removeFields: new List<string>
                {
                    "Id", "MajorName", "MajorNameEn", "TeacherName", "TeacherNameEn",
                    "DivisionName", "DivisionNameEn", "DivisionColor", "IsRecommend", "Status", "CreateTime", "CampusName", "CampusNameEn", "SchoolList"
                });

                if (fields == null || fields.Count == 0) {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields) {
                    fieldList.Add(string.Format("{0}=@{0}",field));
                }

                model.ModifyTime = DateTime.Now;

                string sql = string.Format("update [Student] set {0} where Id=@Id;",string.Join(",", fieldList));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool DeleteAsync(int id) {
            using (var conn = DapperFactory.GetConnection()) {
                string sql = "update [Student] set Status=0,ModifyTime=GETDATE() where Id=@Id;";
                return conn.Execute(sql, new { Id = id }) > 0;
            }
        }

        public List<Student> GetRecommendListAsync(int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select top {0} t.*, b.Name as MajorName, b.NameEn as MajorNameEn, c.Name as TeacherName, 
                     c.NameEn as TeacherNameEn, d.Name as DivisionName, d.NameEn as DivisionNameEn, d.BgColor as DivisionColor,
                     f.Name as CampusName, f.NameEn as CampusNameEn from Student t 
                     left join [Major] as b on b.Id = t.MajorId 
                     left join [Teacher] as c on c.Id = t.TeacherId 
                     left join [Division] as d on d.Id = t.DivisionId
                     left join [Campus] as f on f.Id = t.CampusId
                     where t.Status=1 and t.IsRecommend=1
                     order by t.Id Desc;", topCount);
                var list = conn.Query<Student>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public bool SetRecommend(int id, bool isRecommend)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [Student] set IsRecommend=@IsRecommend where Id=@Id;";
                return conn.Execute(sql, new { IsRecommend = isRecommend, Id = id }) > 0;
            }
        }

        public IEnumerable<int> GetCoursesByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select CourseId from [StudentCourse] where StudentId={0};",id);
                return conn.Query<int>(sql); ;
            }
        }

        public bool SetCourses(int studentId, string courseIds)
        {
            var result = 0;
            using (var conn = DapperFactory.GetConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();

                string sql = "delete from [StudentCourse] where StudentId = @StudentId; ";

                var insertImg = @" INSERT INTO [StudentCourse] ([StudentId],[CourseId]) VALUES(@StudentId,@CourseId)";
                try
                {

                    result = conn.Execute(sql, new { StudentId = studentId }, tran);
                    if (!String.IsNullOrEmpty(courseIds))
                    {
                        string[] ids = courseIds.Split(',');
                        foreach (var item in ids)
                        {
                            result = conn.Execute(insertImg, new { StudentId = studentId, CourseId = item }, tran);
                        }
                    }
                    tran.Commit();
                }
                catch (SqlException ex)
                {
                    result = 0;
                    tran.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    result = 0;
                    tran.Rollback();
                    return false;
                }
            }//end using
            return result > 0;
        }

        public IEnumerable<int> GetSchoolsByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select SchoolId from [StudentSchool] where StudentId={0};", id);
                return conn.Query<int>(sql); ;
            }
        }

        public bool SetSchools(int studentId, string schoolIds)
        {
            var result = 0;
            using (var conn = DapperFactory.GetConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();

                string sql = "delete from [StudentSchool] where StudentId = @StudentId; ";

                var insertImg = @" INSERT INTO [StudentSchool] ([StudentId],[SchoolId]) VALUES(@StudentId,@SchoolId)";
                try
                {

                    result = conn.Execute(sql, new { StudentId = studentId }, tran);
                    if (!String.IsNullOrEmpty(schoolIds))
                    {
                        string[] ids = schoolIds.Split(',');
                        foreach (var item in ids)
                        {
                            result = conn.Execute(insertImg, new { StudentId = studentId, SchoolId = item }, tran);
                        }
                    }
                    tran.Commit();
                }
                catch (SqlException ex)
                {
                    result = 0;
                    tran.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    result = 0;
                    tran.Rollback();
                    return false;
                }
            }//end using
            return result > 0;
        }

        public List<Student> GetListByCourseAsync(int courseId = -1)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select t.*, b.Name as MajorName, b.NameEn as MajorNameEn, c.Name as TeacherName, 
                     c.NameEn as TeacherNameEn, d.Name as DivisionName, d.NameEn as DivisionNameEn, d.BgColor as DivisionColor,
                     f.Name as CampusName, f.NameEn as CampusNameEn from StudentCourse s 
                     left join [STUDENT] as t on s.StudentId = t.Id
                     left join [Major] as b on b.Id = t.MajorId 
                     left join [Teacher] as c on c.Id = t.TeacherId 
                     left join [Division] as d on d.Id = t.DivisionId
                     left join [Campus] as f on f.Id = t.CampusId
                     where s.CourseId = {0} and t.Status=1
                     order by t.Id Desc;", courseId);
                var list = conn.Query<Student>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public List<Student> GetListByTeacherAsync(int teacherId = -1)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select t.*, b.Name as MajorName, b.NameEn as MajorNameEn, c.Name as TeacherName, 
                     c.NameEn as TeacherNameEn, d.Name as DivisionName, 
                     d.NameEn as DivisionNameEn, d.BgColor as DivisionColor, f.Name as CampusName, f.NameEn as CampusNameEn from [STUDENT] t 
                     left join [Major] as b on b.Id = t.MajorId 
                     left join [Teacher] as c on c.Id = t.TeacherId 
                     left join [Division] as d on d.Id = t.DivisionId
                     left join [Campus] as f on f.Id = t.CampusId
                     where t.TeacherId = {0} and t.Status=1
                     order by t.Id Desc;", teacherId);
                var list = conn.Query<Student>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public List<Student> GetListByCampusAsync(int campusId = -1, int topCount = 4)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select top {0} t.*, b.Name as MajorName, b.NameEn as MajorNameEn, c.Name as TeacherName, 
                     c.NameEn as TeacherNameEn, d.Name as DivisionName, 
                     d.NameEn as DivisionNameEn, d.BgColor as DivisionColor, f.Name as CampusName, f.NameEn as CampusNameEn from [STUDENT] t 
                     left join [Major] as b on b.Id = t.MajorId 
                     left join [Teacher] as c on c.Id = t.TeacherId 
                     left join [Division] as d on d.Id = t.DivisionId
                     left join [Campus] as f on f.Id = t.CampusId
                     where t.CampusId = {1} and t.Status=1
                     order by t.Id Desc;", topCount, campusId);
                var list = conn.Query<Student>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public List<School> GetStudentSchools(int studentId = -1)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select s.* from [StudentSchool] as t
                     left join [School] as s on s.Id = t.SchoolId
                     where t.StudentId = {0} and s.Status=1
                     order by s.Id Desc;", studentId);
                var list = conn.Query<School>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }
    }
}
