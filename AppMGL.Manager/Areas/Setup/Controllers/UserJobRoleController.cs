using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
//using AppMGL.DTO.DataManagement;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class UserJobRoleController : BaseController<UserJobRoleDTO, LGVWChtscodeRepository, LG_VW_SIPLCity>
    {
        

        public UserJobRoleController(LGVWChtscodeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.UserJobRole;
            KeyField = "ID";
        }
        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                int count = 0;
                string condition = "";
                condition= Utility.GetWhere1(listParams.Filter);
                condition = condition.Replace("Name", "JobRole");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<UserJobRoleDTO> list = _context.ExecuteQuery<UserJobRoleDTO>("EXEC dbo.LG_UserJobRoleList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@JobRole", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key.Replace("Name", "JobRole")),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("JobRole", condition)
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPut]
        public override ActionResult Detail(long id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                               
                               new SqlParameter("ID",id ),
                               new SqlParameter("JobRole", ""),
                               new SqlParameter("JobRoleDesc", ""),
                               new SqlParameter("IsDel", ""),
                               new SqlParameter("CreatedBy", ""),
                               new SqlParameter("ActionType", "detail")
                };
                
                List <UserJobRoleDTO> Detail = _context.ExecuteQuery<UserJobRoleDTO>("USP_LG_UserJobRole @Id,@JobRole,@JobRoleDesc,@IsDel,@CreatedBY,@ActionType", param.ToArray()).ToList();
                var result = Detail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(UserJobRoleDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_UserJobRole @Id,@JobRole,@JobRoleDesc,@IsDel,@CreatedBY,@ActionType",
                                new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("JobRole", dto.JobRole ?? Convert.DBNull),
                               new SqlParameter("JobRoleDesc", dto.JobRoleDesc ?? Convert.DBNull),
                               new SqlParameter("IsDel", dto.IsDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "insert")).ToList();
                List<int> objList = objResult.ToList();
                long htsid = objList[0];
                var result = _context.Detail(htsid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, UserJobRoleDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_UserJobRole @Id,@JobRole,@JobRoleDesc,@IsDel,@CreatedBY,@ActionType",
                                new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                              new SqlParameter("JobRole", dto.JobRole ?? Convert.DBNull),
                               new SqlParameter("JobRoleDesc", dto.JobRoleDesc ?? Convert.DBNull),
                               new SqlParameter("IsDel", dto.IsDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "update")).ToList();
                List<int> objList = objResult.ToList();
                long htsid = objList[0];

                var result = _context.Detail(htsid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Delete(UserJobRoleDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_UserJobRole @Id,@JobRole,@JobRoleDesc,@IsDel,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("JobRole", dto.JobRole ?? Convert.DBNull),
                               new SqlParameter("JobRoleDesc", dto.JobRoleDesc ?? Convert.DBNull),
                               new SqlParameter("isDel", dto.IsDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "delete")).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public override ActionResult Lookup(ListParams listParams)
        {
            try
            {
                var result = _context.ExecuteQuery<UserJobRoleDTO>("EXEC dbo.SP_UserJobRole_Search").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
}