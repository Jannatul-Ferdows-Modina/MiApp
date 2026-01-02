using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using AppMGL.DTO.Report;
using AppMGL.DAL.Helper;
using Newtonsoft.Json;
using System.Data;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class CommodityController : BaseController<CommodityDTO, CommodityRepository,SIPL_Commodity>
    {
         

        public CommodityController(CommodityRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Commodity;
            KeyField = "CommodityId";
        }
        [System.Web.Http.HttpPut]
        public override ActionResult Detail(long id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@CommondityId", id)
                };
                List<SIPL_CommodityDetail> CommodityDetail = _context.ExecuteQuery<SIPL_CommodityDetail>("GET_COMMODITY_DETAILBYID @CommondityId", param.ToArray()).ToList();
                var result = CommodityDetail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(CommodityDTO objcom)
        {
            try
            {
                var parameters = new[]
{
    new SqlParameter("@CommodityId", SqlDbType.Int) { Value = "0" },
    new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = (object)(objcom.Name ?? string.Empty) },
    new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object)(objcom.Description ?? (object)DBNull.Value) },
    new SqlParameter("@IsActive", SqlDbType.Int) { Value = objcom.IsActive }, // INT in SP
    new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 100) { Value = objcom.CreatedBy },
    new SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 100) { Value = (objcom.ModifiedBy==null?0:objcom.ModifiedBy) },
    new SqlParameter("@IsDeleted", SqlDbType.Int) { Value = 0 },
    new SqlParameter("@Flag", SqlDbType.NVarChar, 50) { Value = "1" }, // must be string
    new SqlParameter("@CommodityTypeID", SqlDbType.Int) { Value = (object)(objcom.CommodityTypeID ?? (object)DBNull.Value) },
    new SqlParameter("@LOW_Value_High_Risk_Remark", SqlDbType.NVarChar, 500) { Value = (objcom.LOW_Value_High_Risk_Remark ) },
    new SqlParameter("@CommodityGroupId", SqlDbType.Int) { Value = (object)(objcom.CommodityGroupId ?? (object)DBNull.Value) }
};

                IEnumerable<int> objResult = _context.ExecuteQuery<int>(
                    "usp_InsertUpdate_SIPL_Commodity @CommodityId,@Name,@Description,@IsActive,@CreatedBy,@ModifiedBy,@IsDeleted,@Flag,@CommodityTypeID,@LOW_Value_High_Risk_Remark,@CommodityGroupId",
                    parameters
                ).ToList();

                int commodityId = objResult.FirstOrDefault();
                return AppResult(commodityId, null);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Update(long id ,CommodityDTO objcom)
        {
            try
            {
                var parameters = new[]
{
    new SqlParameter("@CommodityId", SqlDbType.Int) { Value = objcom.CommodityId },
    new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = (object)(objcom.Name ?? string.Empty) },
    new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object)(objcom.Description ?? (object)DBNull.Value) },
    new SqlParameter("@IsActive", SqlDbType.Int) { Value = objcom.IsActive }, // INT in SP
    new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 100) { Value = objcom.CreatedBy },
    new SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 100) { Value = (objcom.ModifiedBy==null?0:objcom.ModifiedBy) },
    new SqlParameter("@IsDeleted", SqlDbType.Int) { Value = 0 },
    new SqlParameter("@Flag", SqlDbType.NVarChar, 50) { Value = "1" }, // must be string
    new SqlParameter("@CommodityTypeID", SqlDbType.Int) { Value = (object)(objcom.CommodityTypeID ?? (object)DBNull.Value) },
    new SqlParameter("@LOW_Value_High_Risk_Remark", SqlDbType.NVarChar, 500) { Value = (objcom.LOW_Value_High_Risk_Remark ) },
    new SqlParameter("@CommodityGroupId", SqlDbType.Int) { Value = (object)(objcom.CommodityGroupId ?? (object)DBNull.Value) }
};

                IEnumerable<int> objResult = _context.ExecuteQuery<int>(
                    "usp_InsertUpdate_SIPL_Commodity @CommodityId,@Name,@Description,@IsActive,@CreatedBy,@ModifiedBy,@IsDeleted,@Flag,@CommodityTypeID,@LOW_Value_High_Risk_Remark,@CommodityGroupId",
                    parameters
                ).ToList();

                int commodityId = objResult.FirstOrDefault();
                return AppResult(commodityId, null);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                var filters = JsonConvert.DeserializeObject<List<FilterModel>>(listParams.Filter);

              //  var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                string name = "";
                string desc = "";
                string comptype = "";

                if (filters.Count>0)
                {
                    name = filters.FirstOrDefault(f => f.name == "Name")?.value;
                    desc = filters.FirstOrDefault(f => f.name == "Description")?.value;
                    comptype = filters.FirstOrDefault(f => f.name == "CommodityType")?.value;
                }
                   
                int count = 0;
                string condition = "";
                condition = Utility.GetWhere1(listParams.Filter);
               // condition = condition.Replace("Name", "DDTCITARCODE");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<CommodityDTO> list = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.Get_commodity @PAGENO,@PAGESIZE,@SORTCOLUMN,@SORTORDER,@Name,@Desc,@Comtype", new object[7]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("Name", (name==null?"":name)),
                    new SqlParameter("Desc", (desc==null?"":name)),
                     new SqlParameter("Comtype", (comptype==null?"":comptype))
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount.Value;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public  ActionResult GetCommodityGroup(AppMGL.DAL.Helper.ListParams listParams)
        {
            try
            {
               
                List<CommodityGroup_DDL_DTO> CommodityDetail = _context.ExecuteQuery<CommodityGroup_DDL_DTO>("Get_CommodityGrop_ddl").ToList();
                
                return AppResult(CommodityDetail, CommodityDetail.Count);


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
}