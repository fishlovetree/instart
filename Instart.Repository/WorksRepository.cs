using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Dapper;

namespace Instart.Repository
{
    public class WorksRepository : IWorksRepository
    {
        public Works GetByIdAsync(int id) {
            using (var conn = DapperFactory.GetConnection()) {
                string sql = "select * from [Works] where Id = @Id and Status=1;";
                return conn.QueryFirstOrDefault<Works>(sql, new { Id = id });
            }
        }

        public PageModel<Works> GetListAsync(int pageIndex, int pageSize, int major = -1, string name = null) {
            using (var conn = DapperFactory.GetConnection()) {
                #region generate condition
                string where = "where a.Status=1";
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and a.Name like '%{0}%'", name);
                }
                if (major != -1)
                {
                    where += string.Format(" and a.MajorId = {0}", major);
                }
                #endregion

                string countSql = string.Format("select count(1) from [Works] as a {0};",where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0) {
                    return new PageModel<Works>();
                }

                string sql = string.Format(@"select * from (select a.*, b.Name as MajorName, ROW_NUMBER() over (Order by a.Id desc) as RowNumber from [Works] as a 
                    left join [Major] as b on b.Id = a.MajorId {0}) as c 
                    where RowNumber between {1} and {2};",where,((pageIndex - 1) * pageSize) + 1,pageIndex * pageSize);
                var list = conn.Query<Works>(sql);

                return new PageModel<Works> {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }

        public List<Works> GetListByMajorIdAsync(int majorId, int topCount)
        {
            using(var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select top {0} Id,Name,ImgUrl,Introduce,CreateTime from Works where MajorId=@MajorId and Status=1 order by Id desc",topCount);
                var list = conn.Query<Works>(sql, new { MajorId = majorId });
                return list != null ? list.ToList() : null;
            }
        }

        public List<Works> GetListByCourseIdAsync(int courseId, int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select top {0} t.* from (select distinct w.Id,w.Name,w.ImgUrl from Works w
                    left join Student s on s.MajorId = w.MajorId
                    left join StudentCourse sc on sc.StudentId = s.Id
                    where sc.CourseId=@CourseId and w.Status=1) as t order by t.Id desc", topCount);
                var list = conn.Query<Works>(sql, new { CourseId = courseId });
                return list != null ? list.ToList() : null;
            }
        }

        public bool InsertAsync(Works model) {
            using (var conn = DapperFactory.GetConnection()) {
                var fields = model.ToFields(removeFields: new List<string> { "Id", "MajorName" });
                if (fields == null || fields.Count == 0) {
                    return false;
                }

                model.CreateTime = DateTime.Now;
                model.ModifyTime = DateTime.Now;
                model.Status = 1;

                string sql = string.Format("insert into [Works] ({0}) values ({1});",string.Join(",", fields),string.Join(",", fields.Select(n => "@" + n)));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool UpdateAsync(Works model) {
            using (var conn = DapperFactory.GetConnection()) {
                List<string> removeFields = new List<string>
                {
                    "Id",
                    "MajorName",
                    "CreateTime",
                    "Status"
                };
                if (String.IsNullOrEmpty(model.ImgUrl))
                {
                    removeFields.Add("ImgUrl");
                }
                var fields = model.ToFields(removeFields: removeFields);

                if (fields == null || fields.Count == 0) {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields) {
                    fieldList.Add(string.Format("{0}=@{0}",field));
                }

                model.ModifyTime = DateTime.Now;

                string sql = string.Format("update [Works] set {0} where Id=@Id;", string.Join(",", fieldList));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool DeleteAsync(int id) {
            using (var conn = DapperFactory.GetConnection()) {
                string sql = "update [Works] set Status=0,ModifyTime=GETDATE() where Id=@Id;";
                return conn.Execute(sql, new { Id = id }) > 0;
            }
        }
    }
}
