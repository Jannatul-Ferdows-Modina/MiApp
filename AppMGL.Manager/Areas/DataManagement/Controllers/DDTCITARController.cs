using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class DDTCITARController : BaseController<DDTCITARDTO, LGVWChtscodeRepository, LG_VW_SIPLCity>
    {
        

        public DDTCITARController(LGVWChtscodeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.DDTCITAR;
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
                condition = condition.Replace("Name", "DDTCITARCODE");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<DDTCITARDTO> list = _context.ExecuteQuery<DDTCITARDTO>("EXEC dbo.LG_ddtcitarList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@DDTCITARCODE", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key.Replace("Name", "DDTCITARCODE")),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("DDTCITARCODE", condition)
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
                               new SqlParameter("ddtcitarcode", ""),
                               new SqlParameter("ddtcitardesc",""),
                               new SqlParameter("isActive", ""),
                               new SqlParameter("CreatedBy", ""),
                               new SqlParameter("ActionType", "detail")
                };
                List<DDTCITARDTO> Detail = _context.ExecuteQuery<DDTCITARDTO>("USP_LG_DDTCITAR @Id,@ddtcitarcode,@ddtcitardesc,@isActive,@CreatedBY,@ActionType", param.ToArray()).ToList();
                var result = Detail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(DDTCITARDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_DDTCITAR @Id,@ddtcitarcode,@ddtcitardesc,@isActive,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("ddtcitarcode", dto.ddtcitarcode ?? Convert.DBNull),
                               new SqlParameter("ddtcitardesc", dto.ddtcitardesc ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive == true ? 1 : 0),
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
        public override ActionResult Update(long id, DDTCITARDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_DDTCITAR @Id,@ddtcitarcode,@ddtcitardesc,@isActive,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("ddtcitarcode", dto.ddtcitarcode ?? Convert.DBNull),
                               new SqlParameter("ddtcitardesc", dto.ddtcitardesc ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive == true ? 1 : 0),
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
        public override ActionResult Delete(DDTCITARDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_DDTCITAR @Id,@ddtcitarcode,@ddtcitardesc,@isActive,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("ddtcitarcode", dto.ddtcitarcode ?? Convert.DBNull),
                               new SqlParameter("ddtcitardesc", dto.ddtcitardesc ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive == true ? 1 : 0),
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
    }
}