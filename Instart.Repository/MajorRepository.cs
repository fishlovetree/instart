using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instart.Models;
using Dapper;

namespace Instart.Repository
{
    public class MajorRepository : IMajorRepository
    {
        public Major GetByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = @"select a.*, b.Name as DivisionName,b.NameEn as DivisionNameEn,b.BgColor as DivisionColor 
                    from [Major] as a left join [Division] as b on b.Id = a.DivisionId where a.Id = @Id and a.Status=1;";
                return conn.QueryFirstOrDefault<Major>(sql, new { Id = id });
            }
        }

        public PageModel<Major> GetListAsync(int pageIndex, int pageSize, int division = -1, string name = null)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where a.Status=1";
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and a.Name like '%{0}%'",name);
                }
                if (division != -1)
                {
                    where += string.Format(" and a.DivisionId = {0}",division);
                }
                #endregion

                string countSql = string.Format("select count(1) from [Major] as a {0};",where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0)
                {
                    return new PageModel<Major>();
                }

                string sql = string.Format(@"select * from (
                     select a.*, b.Name as DivisionName,b.NameEn as DivisionNameEn,b.BgColor as DivisionColor, ROW_NUMBER() over (Order by a.Id desc) as RowNumber from [Major] as a
                     left join [Division] as b on b.Id = a.DivisionId {0}
                     ) as c
                     where RowNumber between {1} and {2};",where,((pageIndex - 1) * pageSize) + 1, pageIndex * pageSize);
                var list = conn.Query<Major>(sql);

                return new PageModel<Major>
                {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }

        public IEnumerable<Major> GetAllAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where Status=1";
                #endregion

                string sql = string.Format(@"select * from [Major] {0} order by GroupIndex;", where);
                return conn.Query<Major>(sql);
            }
        }

        public bool InsertAsync(Major model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                var fields = model.ToFields(removeFields: new List<string> { "Id", "DivisionName", "DivisionNameEn", 
                    "DivisionColor", "IsSelected", "SchoolInfo" });
                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                model.CreateTime = DateTime.Now;
                model.ModifyTime = DateTime.Now;
                model.Status = 1;

                string sql = string.Format("insert into [Major] ({0}) values ({1});", string.Join(",", fields), string.Join(",", fields.Select(n => "@" + n)));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool UpdateAsync(Major model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                List<string> removeFields = new List<string>
                {
                    "Id",
                    "DivisionName",
                    "CreateTime",
                    "Status",
                    "DivisionNameEn",
                    "DivisionColor",
                    "IsSelected",
                    "SchoolInfo"
                };
                if (String.IsNullOrEmpty(model.ImgUrl))
                {
                    removeFields.Add("ImgUrl");
                }
                var fields = model.ToFields(removeFields: removeFields);

                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields)
                {
                    fieldList.Add(string.Format("{0}=@{0}", field));
                }

                model.ModifyTime = DateTime.Now;

                string sql = string.Format("update [Major] set {0} where Id=@Id;", string.Join(",", fieldList));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool DeleteAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [Major] set Status=0,ModifyTime=GETDATE() where Id=@Id;";
                return conn.Execute(sql, new { Id = id }) > 0;
            }
        }

        public PageModel<Major> GetListByDivsionAsync(int divisionId, int pageIndex, int pageSize)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where a.Status=1";
                if (divisionId != -1)
                {
                    where += string.Format(" and a.DivisionId = {0}", divisionId);
                }
                #endregion

                string countSql = string.Format("select count(1) from [Major] as a {0};", where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0)
                {
                    return new PageModel<Major>();
                }

                string sql = string.Format(@"select * from (
                     select a.Id,a.Name,a.NameEn,a.Introduce,a.CreateTime,a.DivisionId,b.Name as DivisionName,b.NameEn as DivisionNameEn,
                     b.BgColor as DivisionColor,a.ImgUrl, ROW_NUMBER() over (order by a.GroupIndex) as RowNumber from [Major] as a
                     left join [Division] as b on b.Id = a.DivisionId {0}
                     ) as c
                     where RowNumber between {1} and {2};", where,((pageIndex - 1) * pageSize) + 1,pageIndex * pageSize);
                var list = conn.Query<Major>(sql);

                return new PageModel<Major>
                {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }
    }
}
