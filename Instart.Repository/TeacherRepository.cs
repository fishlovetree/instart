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
    public class TeacherRepository : ITeacherRepository
    {
        public Teacher GetByIdAsync(int id) {
            using (var conn = DapperFactory.GetConnection()) {
                string sql = @"select t.*, b.Name as MajorName, b.NameEn as MajorNameEn, 
                     d.Name as DivisionName, d.NameEn as DivisionNameEn, d.BgColor as DivisionColor, e.Name as SchoolName, e.NameEn as SchoolNameEn from [Teacher] t 
                     left join [Major] as b on b.Id = t.MajorId 
                     left join [Division] as d on d.Id = t.DivisionId 
                     left join [School] as e on e.Id = t.SchoolId where t.Id = @Id and t.Status=1;";
                return conn.QueryFirstOrDefault<Teacher>(sql, new { Id = id });
            }
        }

        public PageModel<Teacher> GetListAsync(int pageIndex, int pageSize, int division = -1, string name = null) {
            using (var conn = DapperFactory.GetConnection()) {
                #region generate condition
                string where = "where t.Status=1";
                if (!string.IsNullOrEmpty(name)) {
                    where += string.Format(" and t.Name like '%{0}%'",name);
                }
                if (division != -1)
                {
                    where += string.Format(" and t.DivisionId = {0}",division);
                }
                #endregion

                string countSql = string.Format("select count(1) from [Teacher] as t {0};",where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0) {
                    return new PageModel<Teacher>();
                }

                string sql = string.Format(@"select * from ( select t.*, d.Name as DivisionName, d.NameEn as DivisionNameEn, 
                    d.BgColor as DivisionColor, ROW_NUMBER() over (Order by t.Id desc) as RowNumber from [Teacher] as t
                    left join [Division] d on d.Id = t.DivisionId {0} ) as b 
                    where RowNumber between {1} and {2};",where,((pageIndex - 1) * pageSize) + 1,pageIndex * pageSize);
                var list = conn.Query<Teacher>(sql);

                return new PageModel<Teacher> {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }

        public IEnumerable<Teacher> GetAllAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where Status=1";
                #endregion

                string sql = string.Format(@"select * from [Teacher] {0};",where);
                return conn.Query<Teacher>(sql);
            }
        }

        public bool InsertAsync(Teacher model) {
            using (var conn = DapperFactory.GetConnection()) {
                var fields = model.ToFields(removeFields: new List<string> { "Id", "DivisionName", "DivisionNameEn",
                    "DivisionColor", "SchoolName", "SchoolNameEn", "MajorName", "MajorNameEn", "IsSelected"});
                if (fields == null || fields.Count == 0) {
                    return false;
                }

                model.CreateTime = DateTime.Now;
                model.ModifyTime = DateTime.Now;
                model.Status = 1;

                string sql = string.Format("insert into [Teacher] ({0}) values ({1});",string.Join(",", fields),string.Join(",", fields.Select(n => "@" + n)));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool UpdateAsync(Teacher model) {
            using (var conn = DapperFactory.GetConnection()) {
                var fields = model.ToFields(removeFields: new List<string>
                {
                    "Id",
                    "CreateTime",
                    "Status",
                    "DivisionName",
                    "DivisionNameEn",
                    "DivisionColor",
                    "SchoolName",
                    "SchoolNameEn",
                    "MajorName",
                    "MajorNameEn",
                    "IsSelected",
                    "IsRecommend"
                });

                if (fields == null || fields.Count == 0) {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields) {
                    fieldList.Add(string.Format("{0}=@{0}",field));
                }

                model.ModifyTime = DateTime.Now;

                string sql = string.Format("update [Teacher] set {0} where Id=@Id;",string.Join(",", fieldList));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool DeleteAsync(int id) {
            using (var conn = DapperFactory.GetConnection()) {
                string sql = "update [Teacher] set Status=0,ModifyTime=GETDATE() where Id=@Id;";
                return conn.Execute(sql, new { Id = id }) > 0;
            }
        }

        public List<Teacher> GetRecommendListAsync(int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select top {0} t.Id,t.Name,t.NameEn,t.Avatar,t.SchoolId,s.Name as SchoolName,s.NameEn as SchoolNameEn,t.MajorId,
                                m.Name as MajorName,m.NameEn as MajorNameEn,t.DivisionId,d.Name as DivisionName,d.NameEn as DivisionNameEn,d.BgColor as DivisionColor from Teacher t
                                left join School s on t.SchoolId = s.Id
                                left join Major m on t.MajorId = m.Id
                                left join Division d on t.DivisionId = d.Id
                                where t.Status = 1 and t.IsRecommend = 1
                                order by t.Id;", topCount);
                var list = conn.Query<Teacher>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public bool SetRecommend(int id, bool isRecommend)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [Teacher] set IsRecommend=@IsRecommend where Id=@Id;";
                return conn.Execute(sql, new { IsRecommend = isRecommend, Id = id }) > 0;
            }
        }

        public IEnumerable<Course> GetCoursesByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select c.* from [TeacherCourse] as t left join [Course] as c on c.Id = t.CourseId where t.TeacherId={0};", id);
                return conn.Query<Course>(sql); ;
            }
        }

        public bool SetCourses(int teacherId, string courseIds)
        {
            var result = 0;
            using (var conn = DapperFactory.GetConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();

                string sql = "delete from [TeacherCourse] where TeacherId = @TeacherId; ";

                var insertImg = @" INSERT INTO [TeacherCourse] ([TeacherId],[CourseId]) VALUES(@TeacherId,@CourseId)";
                try
                {

                    result = conn.Execute(sql, new { TeacherId = teacherId }, tran);
                    if (!String.IsNullOrEmpty(courseIds))
                    {
                        string[] ids = courseIds.Split(',');
                        foreach (var item in ids)
                        {
                            result = conn.Execute(insertImg, new { TeacherId = teacherId, CourseId = item }, tran);
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

        public PageModel<Teacher> GetListByDivsionAsync(int divisionId, int pageIndex, int pageSize)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string countSql = string.Format("select count(1) from [Teacher] where DivisionId={0} and Status=1;",divisionId);

                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0)
                {
                    return new PageModel<Teacher>();
                }

                string sql = string.Format(@"select * from (select TOP (100) PERCENT t.Id,t.Name,t.NameEn,t.Avatar,s.Name as SchoolName,s.NameEn as SchoolNameEn,
                                m.Name as MajorName,m.NameEn as MajorNameEn,d.Name as DivisionName,d.NameEn as DivisionNameEn,d.BgColor as DivisionColor, ROW_NUMBER() over (Order by t.Id desc) as RowNumber from Teacher t
                                left join School s on t.SchoolId = s.Id
                                left join Major m on t.MajorId = m.Id
                                left join Division d on t.DivisionId = d.Id
                                where t.DivisionId = @DivisionId and t.Status = 1 
                                order by t.Id) as b 
                                where RowNumber between {0} and {1};", ((pageIndex - 1) * pageSize) + 1,pageIndex * pageSize);

                var list = conn.Query<Teacher>(sql, new { DivisionId = divisionId });

                PageModel<Teacher> result = new PageModel<Teacher>
                {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };

                return result;
            }
        }

        public List<SchoolMajor> GetSchoolListByTeacher(int teacherId)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select distinct r.Id as SchoolId, r.Name as SchoolName, r.Logo as SchoolLogo, m.Id as MajorId, m.Name as MajorName 
                    from [Student] s left join StudentSchool ss on s.Id = ss.StudentId
                    left join School r on r.Id = ss.SchoolId
                    left join Major m on m.Id = s.MajorId
                    where s.TeacherId = {0} and r.Id is not null order by r.Id;", teacherId);
                var list = conn.Query<SchoolMajor>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }
    }
}
