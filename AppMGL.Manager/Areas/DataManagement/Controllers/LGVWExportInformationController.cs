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
    public class LGVWExportInformationController : BaseController<LGVWExportInformationDTO, LGVWPortRepository, LG_VW_Port>
    {
        

        public LGVWExportInformationController(LGVWPortRepository context)
        {
            _context = context;
            BaseModule = EnumModule.GVWModeofTransport;
            KeyField = "Id";
        }
        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                int count = 0;
                string condition = "";
                condition= Utility.GetWhere1(listParams.Filter);
                condition = condition.Replace("Name", "Eic");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<LGVWExportInformationDTO> list = _context.ExecuteQuery<LGVWExportInformationDTO>("EXEC dbo.LG_ExportInformationList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@Condition", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key.Replace("Name", "Eic")),
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
                               
                               new SqlParameter("Id",id ),
                               new SqlParameter("Eic", ""),
                               new SqlParameter("Description", ""),
                               new SqlParameter("IsActive", ""),
                               new SqlParameter("ActionType", "detail")
                };
                List<LGVWExportInformationDTO> Detail = _context.ExecuteQuery<LGVWExportInformationDTO>("USP_LG_ExportInformation @Id,@Eic,@Description,@IsActive,@ActionType", param.ToArray()).ToList();
                var result = Detail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWExportInformationDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ExportInformation @Id,@Eic,@Description,@IsActive,@ActionType",
                               new SqlParameter("Id", dto.Id ?? Convert.DBNull),
                               new SqlParameter("Eic", dto.Eic ?? Convert.DBNull),
                               new SqlParameter("Description", dto.Description ?? Convert.DBNull),
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
        public override ActionResult Update(long id, LGVWExportInformationDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ExportInformation @Id,@Eic,@Description,@IsActive,@ActionType",
                               new SqlParameter("Id", dto.Id ?? Convert.DBNull),
                               new SqlParameter("Eic", dto.Eic ?? Convert.DBNull),
                               new SqlParameter("Description", dto.Description ?? Convert.DBNull),
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
        public override ActionResult Delete(LGVWExportInformationDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ExportInformation @Id,@Eic,@Description,@IsActive,@ActionType",
                               new SqlParameter("Id", dto.Id ?? Convert.DBNull),
                               new SqlParameter("Eic", dto.Eic ?? Convert.DBNull),
                               new SqlParameter("Description", dto.Description ?? Convert.DBNull),
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