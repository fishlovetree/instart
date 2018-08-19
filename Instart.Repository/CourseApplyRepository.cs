using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Instart.Models;
using Instart.Models.Enums;

namespace Instart.Repository
{
    public class CourseApplyRepository : ICourseApplyRepository
    {
        public  List<string> GetApplyCourseNameListAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "select CourseName from CourseApply group by CourseName;";
                var list = conn.Query<string>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public  PageModel<CourseApply> GetListAsync(int pageIndex, int pageSize, string courseName, EnumAccept accept)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where 1=1 ";
                if (!string.IsNullOrEmpty(courseName))
                {
                    where += string.Format(" and c.Name like '%{0}%'",courseName);
                }
                if(accept != EnumAccept.All)
                {
                    where += string.Format(" and a.IsAccept = {0}",(int)accept);
                }
                #endregion

                string countSql = string.Format("select count(1) from [CourseApply] as a left join [Course] as c on c.Id = a.CourseId {0};", where);
                int total =  conn.ExecuteScalar<int>(countSql);
                if (total == 0)
                {
                    return new PageModel<CourseApply>();
                }

                string sql = string.Format(@"select * from ( select a.*, m.Name as MajorName, m.NameEn as MajorNameEn, 
                    c.Name as CourseName, c.NameEn as CourseNameEn, ROW_NUMBER() over (Order by a.Id desc) as RowNumber from [CourseApply] as a
                left join [Course] as c on c.Id = a.CourseId 
                left join [Major] m on m.Id = a.MajorId {0} ) as b where RowNumber between {1} and {2};", where, ((pageIndex - 1) * pageSize) + 1, pageIndex * pageSize);
                var list =  conn.Query<CourseApply>(sql);

                return new PageModel<CourseApply>
                {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }

        public  bool InsertAsync(CourseApply model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                var fields = model.ToFields(removeFields: new List<string> { "Id", "MajorName", "MajorNameEn", "CourseName", "CourseNameEn" });
                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                model.CreateTime = DateTime.Now;

                string sql = string.Format("insert into [CourseApply] ({0}) values ({1});", string.Join(",", fields), string.Join(",", fields.Select(n => "@" + n)));
                return  conn.Execute(sql, model) > 0;
            }
        }

        public  bool SetAcceptAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [CourseApply] set IsAccept=1,AcceptTime=GETDATE() where Id=@Id;";
                return  conn.Execute(sql, new { Id = id }) > 0;
            }
        }

        public  List<CourseApply> GetTopListAsync(int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select top {0} a.*, c.Name as CourseName, c.NameEn as CourseNameEn from CourseApply as a left join [Course] as c on c.Id = a.CourseId order by a.Id Desc;", topCount);
                var list = conn.Query<CourseApply>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }
    }
}
