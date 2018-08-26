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
    public class SchoolRepository : ISchoolRepository
    {     
        public School GetByIdAsync(int id) {
            using (var conn = DapperFactory.GetConnection()) {
                string sql = "select * from [School] where Id = @Id and Status=1;";
                return conn.QueryFirstOrDefault<School>(sql, new { Id = id });
            }
        }

        public PageModel<School> GetListAsync(int pageIndex, int pageSize, string name = null) {
            using (var conn = DapperFactory.GetConnection()) {
                #region generate condition
                string where = "where Status=1";
                if (!string.IsNullOrEmpty(name)) {
                    where += string.Format(" and Name like '%{0}%'",name);
                }
                #endregion

                string countSql = string.Format("select count(1) from [School] {0};",where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0) {
                    return new PageModel<School>();
                }

                string sql = string.Format(@"select * from ( select *, ROW_NUMBER() over (Order by Id) as RowNumber from [School] {0} ) as b  
                                where RowNumber between {1} and {2};", where, ((pageIndex - 1) * pageSize) + 1, pageIndex * pageSize);
                var list = conn.Query<School>(sql);

                return new PageModel<School> {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }

        public IEnumerable<School> GetAllAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where Status=1";
                #endregion

                string sql = string.Format(@"select * from [School] {0};",where);
                return conn.Query<School>(sql);
            }
        }

        public bool InsertAsync(School model) {
            using (var conn = DapperFactory.GetConnection()) {
                var fields = model.ToFields(removeFields: new List<string> { "Id", "AcceptRate", "IsRecommend", "IsHot", "IsSelected" });
                if (fields == null || fields.Count == 0) {
                    return false;
                }

                model.CreateTime = DateTime.Now;
                model.ModifyTime = DateTime.Now;
                model.Status = 1;

                string sql = string.Format("insert into [School] ({0}) values ({1});", string.Join(",", fields), string.Join(",", fields.Select(n => "@" + n)));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool UpdateAsync(School model) {
            using (var conn = DapperFactory.GetConnection()) {
                var fields = model.ToFields(removeFields: new List<string>
                {
                    "Id",
                    "CreateTime",
                    "Status",
                    "AcceptRate",
                    "IsRecommend",
                    "IsHot",
                    "IsSelected"
                });

                if (fields == null || fields.Count == 0) {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields) {
                    fieldList.Add(string.Format("{0}=@{0}",field));
                }

                model.ModifyTime = DateTime.Now;

                string sql = string.Format("update [School] set {0} where Id=@Id;", string.Join(",", fieldList));
                return conn.Execute(sql, model) > 0;
            }
        }

        public bool DeleteAsync(int id) {
            using (var conn = DapperFactory.GetConnection()) {
                string sql = "update [School] set Status=0,ModifyTime=GETDATE() where Id=@Id;";
                return conn.Execute(sql, new { Id = id }) > 0;
            }
        }

        public List<School> GetRecommendListAsync(int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select top {0} * from [School] where Status=1 and IsRecommend = 1;",topCount);
                var result = conn.Query<School>(sql, null);
                return result != null ? result.ToList() : null;
            }
        }

        public bool SetRecommendAsync(int id, bool isRecommend)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [School] set IsRecommend=@IsRecommend where Id=@Id;";
                return conn.Execute(sql, new { IsRecommend =isRecommend, Id = id}) > 0;
            }
        }

        public List<School> GetHotListAsync(int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select top {0} Id,Name,NameEn,Difficult,Avatar,Country,Fee,Scholarship,LimitDate,Language from [School] where Status=1 and IsHot = 1;",topCount);
                var result = conn.Query<School>(sql, null);
                return result != null ? result.ToList() : null;
            }
        }

        public bool SetHotAsync(int id, bool isHot)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [School] set isHot=@isHot where Id=@Id;";
                return conn.Execute(sql, new { isHot = isHot, Id = id }) > 0;
            }
        }

        public PageModel<School> GetListAsync(int pageIndex, int pageSize, string name = null, int country = -1, int major = -1, int level = -1)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where k.Status=1";
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and k.Name like '%{0}%'",name);
                }
                if (country != -1)
                {
                    where += string.Format(" and k.Country = {0}",country);
                }
                if (major != -1)
                {
                    where += string.Format(" and exists (select * from [SchoolMajor] as a where a.SchoolId = k.Id and a.MajorId = {0})", major);
                }
                if (level != -1)
                {
                    where += string.Format(" and CHARINDEX('{0}', k.Education) > 0", level);
                }
                #endregion

                string countSql = string.Format("select count(1) from [School] as k {0};",where);
                int total = conn.ExecuteScalar<int>(countSql);
                if (total == 0)
                {
                    return new PageModel<School>
                    {
                        Total = 0,
                        Data = new List<School>()
                    };
                }

                string sql = string.Format(@"select * from ( select k.*, ROW_NUMBER() over (Order by k.Id) as RowNumber from [School] as k {0} ) as b  
                                where RowNumber between {1} and {2};",where,((pageIndex - 1) * pageSize) + 1, pageIndex * pageSize);
                var list = conn.Query<School>(sql);

                return new PageModel<School>
                {
                    Total = total,
                    Data = list != null ? list.ToList() : null
                };
            }
        }

        public IEnumerable<SchoolMajor> GetMajorsByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format("select s.SchoolId, s.MajorId, s.Introduce, s.Type, m.Name as MajorName from [SchoolMajor] as s left join [Major] as m on m.Id = s.MajorId where s.SchoolId={0};",id);
                return conn.Query<SchoolMajor>(sql); ;
            }
        }

        public bool SetMajors(int schoolId, string bkmajors, string yjsmajors, string bkintroduces, string yjsintroduces)
        {
            var result = 0;
            using (var conn = DapperFactory.GetConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();

                string sql = "delete from [SchoolMajor] where SchoolId = @SchoolId; ";

                var insertImg = @" INSERT INTO [SchoolMajor] ([MajorId],[SchoolId],[Introduce],[Type]) VALUES(@MajorId,@SchoolId,@Introduce,@Type)";
                try
                {

                    result = conn.Execute(sql, new { SchoolId = schoolId }, tran);
                    if (!String.IsNullOrEmpty(bkmajors))
                    {
                        string[] ids = bkmajors.Split(',');
                        string[] introducearr = bkintroduces.Split('|');
                        for (int i = 0; i < ids.Length; i++)
                        {
                            result = conn.Execute(insertImg, new { SchoolId = schoolId, MajorId = ids[i], Introduce = introducearr[i], Type = 0 }, tran);
                        }
                    }
                    if (!String.IsNullOrEmpty(yjsmajors))
                    {
                        string[] ids = yjsmajors.Split(',');
                        string[] introducearr = yjsintroduces.Split('|');
                        for (int i = 0; i < ids.Length; i++)
                        {
                            result = conn.Execute(insertImg, new { SchoolId = schoolId, MajorId = ids[i], Introduce = introducearr[i], Type = 1 }, tran);
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

        public List<School> GetListByMajorAsync(int majorId = 0, int topCount = 6)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = string.Format(@"select top {0} t.* from (select distinct s.Id, s.Name, s.NameEn, s.Logo, s.Fee, s.Avatar, s.Difficult, s.Country, s.LimitDate, s.Language, s.RecommendMajor 
                    from [SchoolMajor] sm left join [School] s on s.Id = sm.SchoolId where sm.MajorId = {1} and s.Status=1) as t;", topCount, majorId);
                var result = conn.Query<School>(sql, null);
                return result != null ? result.ToList() : null;
            }
        }
    }
}
