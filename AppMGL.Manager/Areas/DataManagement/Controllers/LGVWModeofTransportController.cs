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
    public class LGVWModeofTransportController : BaseController<LGVWModeofTransportDTO, LGVWPortRepository, LG_VW_Port>
    {
        

        public LGVWModeofTransportController(LGVWPortRepository context)
        {
            _context = context;
            BaseModule = EnumModule.GVWModeofTransport;
            KeyField = "TransportModeID";
        }
        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                int count = 0;
                string condition = "";
                condition= Utility.GetWhere1(listParams.Filter);
                condition = condition.Replace("Name", "TransportMode");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<LGVWModeofTransportDTO> list = _context.ExecuteQuery<LGVWModeofTransportDTO>("EXEC dbo.LG_ModeofTransportList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@Condition", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key.Replace("Name", "TransportMode")),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("Condition", condition)
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
                               
                               new SqlParameter("TransportModeID",id ),
                               new SqlParameter("TransportMode", ""),
                               new SqlParameter("TransportationCode", ""),
                               new SqlParameter("TransCode", ""),
                               new SqlParameter("IsActive", ""),
                               new SqlParameter("ActionType", "detail")
                };
                List<LGVWModeofTransportDTO> Detail = _context.ExecuteQuery<LGVWModeofTransportDTO>("USP_LG_ModeofTransport @TransportModeID,@TransportMode,@TransportationCode,@TransCode,@IsActive,@ActionType", param.ToArray()).ToList();
                var result = Detail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWModeofTransportDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ModeofTransport @TransportModeID,@TransportMode,@TransportationCode,@TransCode,@IsActive,@ActionType",
                               new SqlParameter("TransportModeID", dto.TransportModeID ?? Convert.DBNull),
                               new SqlParameter("TransportMode", dto.TransportMode ?? Convert.DBNull),
                               new SqlParameter("TransportationCode", dto.TransportationCode ?? Convert.DBNull),
                               new SqlParameter("TransCode", dto.TransCode ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive == true ? 1 : 0),
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
        public override ActionResult Update(long id, LGVWModeofTransportDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ModeofTransport @TransportModeID,@TransportMode,@TransportationCode,@TransCode,@IsActive,@ActionType",
                               new SqlParameter("TransportModeID", dto.TransportModeID ?? Convert.DBNull),
                               new SqlParameter("TransportMode", dto.TransportMode ?? Convert.DBNull),
                               new SqlParameter("TransportationCode", dto.TransportationCode ?? Convert.DBNull),
                               new SqlParameter("TransCode", dto.TransCode ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive == true ? 1 : 0),
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
        public override ActionResult Delete(LGVWModeofTransportDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ModeofTransport @TransportModeID,@TransportMode,@TransportationCode,@TransCode,@IsActive,@ActionType",
                               new SqlParameter("TransportModeID", dto.TransportModeID ?? Convert.DBNull),
                               new SqlParameter("TransportMode", dto.TransportMode ?? Convert.DBNull),
                               new SqlParameter("TransportationCode", dto.TransportationCode ?? Convert.DBNull),
                               new SqlParameter("TransCode", dto.TransCode ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive == true ? 1 : 0),
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