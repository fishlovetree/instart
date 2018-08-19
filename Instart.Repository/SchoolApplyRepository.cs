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
    public class SchoolApplyRepository : ISchoolApplyRepository
    {
        public List<string> GetApplySchoolNameListAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "select SchoolName from SchoolApply group by SchoolName;";
                var list = conn.Query<string>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }

        public PageModel<SchoolApply> GetListAsync(int pageIndex, int pageSize, string schoolName, EnumAccept accept)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where 1=1 ";
                if (!string.IsNullOrEmpty(schoolName))
                {
                    where += string.Format(" and s.Name like '%{0}%'",schoolName);
                }
                if(accept != EnumAccept.All)
                {
                    where += string.Format(" and a.IsAccept = {0}",(int)accept);
                }
                #endregion

                string countSql = string.Format("select count(1) from [SchoolApply] as a left join [School] as s on s.Id = a.SchoolId {0};",where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0)
                {
                    return new PageModel<SchoolApply>();
                }

                string sql = string.Format(@"select * from ( select a.*, m.Name as MajorName, m.NameEn as MajorNameEn, 
                    s.Name as SchoolName, s.NameEn as SchoolNameEn, ROW_NUMBER() over (Order by a.Id desc) as RowNumber from [SchoolApply] as a 
                    left join  [School] as s on s.Id = a.SchoolId 
                    left join [Major] m on m.Id = a.MajorId 
                    {0} ) as b where RowNumber between {1} and {2};", where,((pageIndex - 1) * pageSize) + 1,pageIndex * pageSize);
                var list = conn.Query<SchoolApply>(sql);

                return new PageModel<SchoolApply>
                {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }

        public bool InsertAsync(SchoolApply model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                var fields = model.ToFields(removeFields: new List<string> { "Id", "MajorName", "MajorNameEn", "SchoolName", "SchoolNameEn" });
                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                model.CreateTime = DateTime.Now;

                string sql = string.Format("insert into [SchoolApply] ({0}) values ({1});",string.Join(",", fields),string.Join(",", fields.Select(n => "@" + n)));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool SetAcceptAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [SchoolApply] set IsAccept=1,AcceptTime=GETDATE() where Id=@Id;";
                return conn.Execute(sql, new { Id = id }) > 0;
            }
        }

        public List<SchoolApply> GetTopListAsync(int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select top {0} a.*, s.Name as SchoolName, s.NameEn as SchoolNameEn from SchoolApply as a left join  [School] as s on s.Id = a.SchoolId order by a.Id Desc;", topCount);
                var list = conn.Query<SchoolApply>(sql, null);
                return list != null ? list.ToList() : null;
            }
        }
    }
}
