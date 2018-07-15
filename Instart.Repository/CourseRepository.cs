﻿using Dapper;
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
        public async Task<Course> GetByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "select * from [Course] where Id = @Id and Status=1;";
                return await conn.QueryFirstOrDefaultAsync<Course>(sql, new { Id = id });
            }
        }

        public async Task<PageModel<Course>> GetListAsync(int pageIndex, int pageSize, string name = null)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where a.Status=1";
                if (!string.IsNullOrEmpty(name))
                {
                    where += $" and a.Name like '%{name}%'";
                }
                #endregion

                string countSql = $"select count(1) from [Course] as a {where};";
                int total = await conn.ExecuteScalarAsync<int>(countSql);
                if (total == 0)
                {
                    return new PageModel<Course>();
                }

                string sql = $@"select * from (
                     select a.*, ROW_NUMBER() over (Order by a.Id desc) as RowNumber from [Course] as a {where}
                     ) as c
                     where RowNumber between {((pageIndex - 1) * pageSize) + 1} and {pageIndex * pageSize};";
                var list = await conn.QueryAsync<Course>(sql);

                return new PageModel<Course>
                {
                    Total = total,
                    Data = list?.ToList()
                };
            }
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                #region generate condition
                string where = "where Status=1";
                #endregion

                string sql = $@"select * from [Course] {where};";
                return await conn.QueryAsync<Course>(sql);
            }
        }

        public async Task<bool> InsertAsync(Course model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                var fields = model.ToFields(removeFields: new List<string> { nameof(model.Id), nameof(model.IsSelected) });
                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                model.CreateTime = DateTime.Now;
                model.ModifyTime = DateTime.Now;
                model.Status = 1;

                string sql = $"insert into [Course] ({string.Join(",", fields)}) values ({string.Join(",", fields.Select(n => "@" + n))});";
                return await conn.ExecuteAsync(sql, model) > 0;
            }
        }

        public async Task<bool> UpdateAsync(Course model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                List<string> removeFields = new List<string>
                {
                    nameof(model.Id),
                    nameof(model.CreateTime),
                    nameof(model.Status),
                    nameof(model.IsSelected)
                };
                var fields = model.ToFields(removeFields: removeFields);

                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields)
                {
                    fieldList.Add($"{field}=@{field}");
                }

                model.ModifyTime = DateTime.Now;

                string sql = $"update [Course] set {string.Join(",", fieldList)} where Id=@Id;";
                return await conn.ExecuteAsync(sql, model) > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "update [Course] set Status=0,ModifyTime=GETDATE() where Id=@Id;";
                return await conn.ExecuteAsync(sql, new { Id = id }) > 0;
            }
        }

        public async Task<List<Course>> GetRecommendListAsync(int topCount)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = $@"select top {topCount} Id,Name,NameEn,Picture,Introduce from Course where Status=1 and IsRecommend=1 order by Id desc;";
                return (await conn.QueryAsync<Course>(sql, null))?.ToList();
            }
        }

        public async Task<bool> SetRecommend(int id, bool isRecommend)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = $"update [Course] set IsRecommend=@IsRecommend where Id=@Id;";
                return await conn.ExecuteAsync(sql, new { IsRecommend = isRecommend, Id = id }) > 0;
            }
        }

        public async Task<int> GetInfoCountAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "select count(1) from [CourseInfo];";
                return await conn.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<CourseInfo> GetInfoAsync()
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = "select * from [CourseInfo];";
                return await conn.QueryFirstOrDefaultAsync<CourseInfo>(sql);
            }
        }

        public async Task<bool> InsertInfoAsync(CourseInfo model)
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

                string sql = $"insert into [CourseInfo] ({string.Join(",", fields)}) values ({string.Join(",", fields.Select(n => "@" + n))});";
                return await conn.ExecuteAsync(sql, model) > 0;
            }
        }

        public async Task<bool> UpdateInfoAsync(CourseInfo model)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                var fields = model.ToFields(removeFields: new List<string>
                {
                    nameof(model.CreateTime)
                });

                if (fields == null || fields.Count == 0)
                {
                    return false;
                }

                var fieldList = new List<string>();
                foreach (var field in fields)
                {
                    fieldList.Add($"{field}=@{field}");
                }

                model.ModifyTime = DateTime.Now;

                string sql = $"update [CourseInfo] set {string.Join(",", fieldList)};";
                return await conn.ExecuteAsync(sql, model) > 0;
            }
        }

        public async Task<IEnumerable<int>> GetTeachersByIdAsync(int id)
        {
            using (var conn = DapperFactory.GetConnection())
            {
                string sql = $"select TeacherId from [TeacherCourse] where CourseId={id};";
                return await conn.QueryAsync<int>(sql); ;
            }
        }

        public async Task<bool> SetTeachers(int courseId, string teacherIds)
        {
            var result = 0;
            using (var conn = DapperFactory.GetConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();

                string sql = $"delete from [TeacherCourse] where CourseId = @CourseId; ";

                var insertImg = @" INSERT INTO [TeacherCourse] ([TeacherId],[CourseId]) VALUES(@TeacherId,@CourseId)";
                try
                {

                    result = await conn.ExecuteAsync(sql, new { CourseId = courseId }, tran);
                    if (!String.IsNullOrEmpty(teacherIds))
                    {
                        string[] ids = teacherIds.Split(',');
                        foreach (var item in ids)
                        {
                            result = await conn.ExecuteAsync(insertImg, new { CourseId = courseId, TeacherId = item }, tran);
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
