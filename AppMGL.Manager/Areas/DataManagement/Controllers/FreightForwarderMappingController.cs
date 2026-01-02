using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Operation;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using AppMGL.Manager.Infrastructure.Results;
using System.Web;
using System.Net;
using System.Configuration;
using System.IO;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DTO.Report;
using System.Net.Http.Headers;
using AppMGL.DAL.Repository.Report;
using AppMGL.Manager.Infrastructure.Report;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class FreightForwarderMappingController : BaseController<FreightForwarderMappingDTO, CustomerContactRepository, USP_GET_CUSTOMERCONTACT_LIST_Result>
    {
        public FreightForwarderMappingController(CustomerContactRepository context)
        {
            _context = context;
            base.BaseModule = EnumModule.FreightForwarderMapping;
            base.KeyField = "NetworkMapId";
        }
        
        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int count = 0;
                List<FreightForwarderMappingDTO> list = _context.ExecuteQuery<FreightForwarderMappingDTO>("EXEC dbo.USP_GET_FFMAPPING_LIST @PAGENO, @PAGESIZE,@FFNetworkName,@CompanyName,@SITID,@SORTCOLUMN,@SORTORDER", new object[7]
                {
                    new SqlParameter("PAGENO", listParams.PageIndex),
                    new SqlParameter("PAGESIZE", listParams.PageSize),
                    new SqlParameter("FFNetworkName", dictionary.ContainsKey("companyName") ? dictionary["companyName"] : string.Empty),
                    new SqlParameter("CompanyName", dictionary.ContainsKey("customerCode") ? dictionary["customerCode"] : string.Empty),
                    new SqlParameter("SITID", dictionary["siteId"]),
                    new SqlParameter("SORTCOLUMN", source.First().Key),
                    new SqlParameter("SORTORDER", source.First().Value)
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
        [System.Web.Http.HttpGet]
        public virtual ActionResult getForwarderMapping(int id)
        {
            try
            {
                List<FreightForwarderMappingDTO> list = _context.ExecuteQuery<FreightForwarderMappingDTO>("EXEC dbo.USP_LG_FFMAPPING_GET_DETAILBYID @NetworkMapId", new object[1]
                {
                    new SqlParameter("NetworkMapId", id)
                }).ToList();
                
                return AppResult(list, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult saveForwarderMapping(FreightForwarderMappingDTO objMappingDTO)
        {
            try
            {
                string text = "";
                FreightForwarderMappingDTO data = null;
                text = ((objMappingDTO.NetworkMapId != 0) ? "update" : "insert");
                CustomerContactRepository context = _context;
                object[] obj = new object[9]
                {
                    new SqlParameter("NetworkMapId", objMappingDTO.NetworkMapId),
                    new SqlParameter("FFNetworkId", objMappingDTO.FFNetworkId),
                    new SqlParameter("ContactId", objMappingDTO.ContactId),
                    new SqlParameter("Remarks", objMappingDTO.Remarks ?? Convert.DBNull),
                    new SqlParameter("CreatedBy", objMappingDTO.CreatedBy ?? Convert.DBNull),
                    new SqlParameter("ModifiedBy", objMappingDTO.ModifiedBy ?? Convert.DBNull),
                    new SqlParameter("ModifiedOn", objMappingDTO.ModifiedOn ?? Convert.DBNull),
                    new SqlParameter("SiteId",  objMappingDTO.SiteId),
                    new SqlParameter("Type", text)

                };
                IEnumerable<int> source = context.ExecuteQuery<int>("EXEC dbo.USP_LG_FFMAPPING_INSERT_UPDATE @NetworkMapId,@FFNetworkId,@ContactId,@Remarks,@CreatedBy, @ModifiedBy, @ModifiedOn,@SiteId,@Type", obj).ToList();
                List<int> list = source.ToList();
                int num = list[0];
                if (num > 0)
                {
                   
                }
                return AppResult(data, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Delete(FreightForwarderMappingDTO dto)
        {
            try
            {
                IEnumerable<int> source = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_FFMAPPING_DELETE @NetworkMapId", new object[1]
                {
                    new SqlParameter("NetworkMapId", dto.NetworkMapId)
                }).ToList();
                List<int> entity = source.ToList();
                return AppResult(entity, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        
        [System.Web.Http.HttpGet]
        public virtual ActionResult getLatestMappingCode()
        {
            try
            {
                FreightForwarderMappingDTO fFMappingCode = new FreightForwarderMappingDTO();
                IEnumerable<string> source = _context.ExecuteQuery<string>("EXEC dbo.USP_LG_FREIGHTFORWARDER_GET_CODE", Array.Empty<object>()).ToList();
                List<string> list = source.ToList();
                if (list.Count > 0)
                {
                    fFMappingCode.FFNetworkCode = list[0].ToString();
                }
                return AppResult(fFMappingCode, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        
       
        [System.Web.Http.HttpGet]
        public override ActionResult Lookup(ListParams listParams)
        {
            try
            {
                var result = _context.ExecuteQuery<FreightForwarderNetworkDTO>("EXEC dbo.SP_ForwarderNetwork_Search @SEARCHVALUE",
                   new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
}