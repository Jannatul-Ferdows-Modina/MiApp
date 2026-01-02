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
    public class DDTCUSMLController : BaseController<DDTCUSMLDTO, LGVWChtscodeRepository, LG_VW_SIPLCity>
    {
        

        public DDTCUSMLController(LGVWChtscodeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.DDTCUSML;
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
                condition = condition.Replace("Name", "DDTCUSMLCODE");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<DDTCUSMLDTO> list = _context.ExecuteQuery<DDTCUSMLDTO>("EXEC dbo.LG_ddtcusmlList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@DDTCUSMLCODE", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key.Replace("Name", "DDTCUSMLCODE")),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("DDTCUSMLCODE", condition)
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
                               new SqlParameter("ddtcusmlcode", ""),
                               new SqlParameter("ddtcusmldesc",""),
                               new SqlParameter("isActive", ""),
                               new SqlParameter("CreatedBy", ""),
                               new SqlParameter("ActionType", "detail")
                };
                List<DDTCUSMLDTO> Detail = _context.ExecuteQuery<DDTCUSMLDTO>("USP_LG_DDTCUSML @Id,@ddtcusmlcode,@ddtcusmldesc,@isActive,@CreatedBY,@ActionType", param.ToArray()).ToList();
                var result = Detail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(DDTCUSMLDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_DDTCUSML @Id,@ddtcusmlcode,@ddtcusmldesc,@isActive,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("ddtcusmlcode", dto.ddtcusmlcode ?? Convert.DBNull),
                               new SqlParameter("ddtcusmldesc", dto.ddtcusmldesc ?? Convert.DBNull),
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
        public override ActionResult Update(long id, DDTCUSMLDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_DDTCUSML @Id,@ddtcusmlcode,@ddtcusmldesc,@isActive,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("ddtcusmlcode", dto.ddtcusmlcode ?? Convert.DBNull),
                               new SqlParameter("ddtcusmldesc", dto.ddtcusmldesc ?? Convert.DBNull),
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
        public override ActionResult Delete(DDTCUSMLDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_DDTCUSML @Id,@ddtcusmlcode,@ddtcusmldesc,@isActive,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("ddtcusmlcode", dto.ddtcusmlcode ?? Convert.DBNull),
                               new SqlParameter("ddtcusmldesc", dto.ddtcusmldesc ?? Convert.DBNull),
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