using Dapper;
using Instart.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Repository
{
    public class CourseRepository: ICourseRepository
    {
        public Course GetByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "select * from [Course] where Id = @Id and Status=1;";
                return conn.QueryFirstOrDefault<Course>(sql, new { Id = id });
            }
        }

        public PageModel<Course> GetListAsync(int pageIndex, int pageSize, int systemId = -1, string name = null)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where a.Status=1";
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and a.Name like '%{0}%'",name);
                }
                if (systemId != -1)
                {
                    where += string.Format(" and a.SystemId = {0}", systemId);
                }
                #endregion

                string countSql = string.Format("select count(1) from [Course] as a {0};",where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0)
                {
                    return new PageModel<Course>();
                }

                string sql = string.Format(@"select * from (
                     select a.*, s.Name as SystemName, s.NameEn as SystemNameEn, ROW_NUMBER() over (Order by a.Id desc) as RowNumber 
                     from [Course] as a left join [CourseSystem] s on a.SystemId = s.Id {0} ) as c
                     where RowNumber between {1} and {2};", where,((pageIndex - 1) * pageSize) + 1,pageIndex * pageSize);
                var list = conn.Query<Course>(sql);

                return new PageModel<Course>
                {
                    Total = total,
                    Data = list != null ? list.ToList(): null
                };
            }
        }

        public IEnumerable<Course> GetAllAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where a.Status=1";
                #endregion

                string sql = string.Format(@"select a.*, s.Name as SystemName, s.NameEn as SystemNameEn from [Course] as a left join [CourseSystem] s on a.SystemId = s.Id {0};", where);
                return conn.Query<Course>(sql);
            }
        }

        public IEnumerable<Course> GetListBySystemId(int systemId)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select a.*, s.Name as SystemName, s.NameEn as SystemNameEn from [Course] as a left join [CourseSystem] s on a.SystemId = s.Id where a.SystemId = {0} and a.Status=1;", systemId);
                return conn.Query<Course>(sql);
            }
        }

        public IEnumerable<Course> GetAllByStudentAsync(int studentId)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select c.* from [StudentCourse] as s left join [Course] as c on s.CourseId = c.Id where c.Status=1 and s.StudentId = {0};", studentId);
                return conn.Query<Course>(sql);
            }
        }

        public bool InsertAsync(Course model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                var fields = model.ToFields(removeFields: new List<string> { "Id", "IsSelected", "SystemName", "SystemNameEn", "MajorName", "MajorNameEn"});
                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                model.CreateTime = DateTime.Now;
                model.ModifyTime = DateTime.Now;
                model.Status = 1;

                string sql = string.Format("insert into [Course] ({0}) values ({1});", string.Join(",", fields), string.Join(",", fields.Select(n => "@" + n)));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool UpdateAsync(Course model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                List<string> removeFields = new List<string>
                {
                    "Id",
                    "CreateTime",
                    "Status",
                    "IsSelected",
                    "IsRecommend", 
                    "SystemName", 
                    "SystemNameEn", 
                    "MajorName", 
                    "MajorNameEn"
                };
                var fields = model.ToFields(removeFields: removeFields);

                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields)
                {
                    fieldList.Add(string.Format("{0}=@{0}",field));
                }

                model.ModifyTime = DateTime.Now;

                string sql = string.Format("update [Course] set {0} where Id=@Id;", string.Join(",", fieldList));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool DeleteAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [Course] set Status=0,ModifyTime=GETDATE() where Id=@Id;";
                return conn.Execute(sql, new { Id = id }) > 0;
            }
        }

        public List<Course> GetRecommendListAsync(int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select top {0} Id,Name,NameEn,Picture,Introduce from Course where Status=1 and IsRecommend=1 order by Id;", topCount);
                var list = conn.Query<Course>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public List<Course> GetListByMajor(int topCount, int majorId)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select top {0} Id,Name,NameEn,Picture,Introduce from Course where Status=1 and MajorId={1} order by Id;", topCount, majorId);
                var list = conn.Query<Course>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public bool SetRecommend(int id, bool isRecommend)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [Course] set IsRecommend=@IsRecommend where Id=@Id;";
                return conn.Execute(sql, new { IsRecommend = isRecommend, Id = id }) > 0;
            }
        }

        public int GetInfoCountAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "select count(1) from [CourseInfo];";
                return conn.ExecuteScalar<int>(sql);
            }
        }

        public CourseInfo GetInfoAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "select * from [CourseInfo];";
                return conn.QueryFirstOrDefault<CourseInfo>(sql);
            }
        }

        public bool InsertInfoAsync(CourseInfo model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                var fields = model.ToFields();
                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                model.CreateTime = DateTime.Now;
                model.ModifyTime = DateTime.Now;

                string sql = string.Format("insert into [CourseInfo] ({0}) values ({1});", string.Join(",", fields), string.Join(",", fields.Select(n => "@" + n)));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool UpdateInfoAsync(CourseInfo model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                var fields = model.ToFields(removeFields: new List<string>
                {
                    "CreateTime"
                });

                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields)
                {
                    fieldList.Add(string.Format("{0}=@{0}",field));
                }

                model.ModifyTime = DateTime.Now;

                string sql = string.Format("update [CourseInfo] set {0};", string.Join(",", fieldList));
                return conn.Execute(sql, model) > 0;
            }
        }

        public IEnumerable<int> GetTeachersByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select TeacherId from [TeacherCourse] where CourseId={0};",id);
                return conn.Query<int>(sql); ;
            }
        }

        public bool SetTeachers(int courseId, string teacherIds)
        {
            var result = 0;
            using (var conn = DapperFactory.GetConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();

                string sql = "delete from [TeacherCourse] where CourseId = @CourseId; ";

                var insertImg = @" INSERT INTO [TeacherCourse] ([TeacherId],[CourseId]) VALUES(@TeacherId,@CourseId)";
                try
                {

                    result = conn.Execute(sql, new { CourseId = courseId }, tran);
                    if (!String.IsNullOrEmpty(teacherIds))
                    {
                        string[] ids = teacherIds.Split(',');
                        foreach (var item in ids)
                        {
                            result = conn.Execute(insertImg, new { CourseId = courseId, TeacherId = item }, tran);
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
    }
}
