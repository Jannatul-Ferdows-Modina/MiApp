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
    public class LGVWPortController : BaseController<LGVWPortDTO, LGVWPortRepository, LG_VW_Port>
    {


        public LGVWPortController(LGVWPortRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGVWPort;
            KeyField = "PortId";
        }

        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                int count = 0;
                string condition = "";
                condition = Utility.GetWhere2(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<LGVWPortDTO> list = _context.ExecuteQuery<LGVWPortDTO>("EXEC dbo.LG_VW_PortList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@Condition", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key),
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
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWPortDTO dto)
        {
            try
            {

                IEnumerable<LGVWPortDTO> objResult = _context.ExecuteQuery<LGVWPortDTO>("EXEC dbo.LG_VWPort_IUD @PortID,@Name,@IsActive,@CreatedBy,@ModifiedBy,@fkCountryID,@Port_Code,@ActionType",
                              new SqlParameter("PortID", dto.PortId ?? Convert.DBNull),
                               new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                               new SqlParameter("fkCountryID", dto.fkCountryId ?? Convert.DBNull),
                               new SqlParameter("Port_Code", dto.port_code ?? Convert.DBNull),
                               new SqlParameter("ActionType", dto.PortId==null|| dto.PortId ==0? "insert":"update")).ToList();
                List<LGVWPortDTO> objList = objResult.ToList();
                var result = objList[0];
                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);

                //return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, LGVWPortDTO dto)
        {
            try
            {

                IEnumerable<LGVWPortDTO> objResult = _context.ExecuteQuery<LGVWPortDTO>("EXEC dbo.LG_VWPort_IUD @PortID,@Name,@IsActive,@CreatedBy,@ModifiedBy,@fkCountryID,@Port_Code,@ActionType",
                new SqlParameter("PortID", dto.PortId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("fkCountryID", dto.fkCountryId ?? Convert.DBNull),
                new SqlParameter("Port_Code", dto.port_code ?? Convert.DBNull),
                new SqlParameter("ActionType", "update")).ToList();
                List<LGVWPortDTO> objList = objResult.ToList();
                var result = objList[0];
                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(LGVWPortDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_VWPort_IUD @PortID,@Name,@IsActive,@CreatedBy,@ModifiedBy,@fkCountryID,@Port_Code,@ActionType",
                new SqlParameter("PortID", dto.PortId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("fkCountryID", dto.fkCountryId ?? Convert.DBNull),
                new SqlParameter("Port_Code", dto.port_code ?? Convert.DBNull),
                new SqlParameter("ActionType", "delete")).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
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
                    new SqlParameter("@PortId", id)
                };
                List<LGVWPortDTO> PortDetail = _context.ExecuteQuery<LGVWPortDTO>("GET_Port_DetailById @PortId", param.ToArray()).ToList();
                var result = PortDetail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Lookup(ListParams listParams)
        {
            try
            {
                int count = 0;
                string condition = "";
                condition = Utility.GetWhere3(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<LGVWPortDTO> result = _context.ExecuteQuery<LGVWPortDTO>("EXEC dbo.LG_VW_PortList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@Condition", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("Condition", condition)
                }).ToList();
                if (result.Count > 0)
                {
                    count = result[0].TotalCount;
                }

                return AppResult(result, count);
              
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult CountrySearch(ListParams listParams)
        {
            try
            {
                int count = 0;
                string condition = "";
                condition = Utility.GetWhere3(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<LGVWPortDTO> result = _context.ExecuteQuery<LGVWPortDTO>("EXEC dbo.LG_VW_PortCountryList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@Condition", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("Condition", condition)
                }).ToList();
                if (result.Count > 0)
                {
                    count = result[0].TotalCount;
                }

                return AppResult(result, count);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
}