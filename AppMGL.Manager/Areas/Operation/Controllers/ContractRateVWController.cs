using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using AppMGL.Manager.Infrastructure;
using AutoMapper;
using AppMGL.DAL.Helper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Linq;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DTO.Operation;
using System.Net.Http;
using AppMGL.Manager.Infrastructure.Results;
using AppMGL.Manager.Infrastructure.Report;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using AppMGL.DAL.Helper.Logging;

namespace AppMGL.Manager.Areas.Operation.Controllers
{
    public class ContractRateVWController : BaseController<LGVWContractRateDTO, ContractRateVWRepository, LG_VW_Contract>
    {


        public ContractRateVWController(ContractRateVWRepository context)
        {
            _context = context;
            BaseModule = EnumModule.ContractRateVW;
            KeyField = "ContractID";
        }


        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWContractRateDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SAVE_CONTRACT_RATE @ContractID,@ContractNo,@StartDate,@EndDate,@IsValid,@CarrierID,@ContractDate,@MQCTYPE,@MQC,@ContractType,@fkOriginID,@fkDestinationID,@Remark,@Attachments,@ActionType,@TradeLane,@SCTermNo,@AmedementNo,@AmedementDate",

                new SqlParameter("ContractID", dto.ContractID),
                new SqlParameter("ContractNo", dto.ContractNo),
                new SqlParameter("StartDate", dto.StartDate),
                new SqlParameter("EndDate", dto.EndDate),
                new SqlParameter("IsValid", dto.IsValid),
                new SqlParameter("CarrierID", dto.CarrierID),
                new SqlParameter("ContractDate", dto.ContractDate),
                new SqlParameter("MQCTYPE", dto.MQCTYPE),
                new SqlParameter("MQC", Convert.ToDecimal(dto.mqcvalue)),
                new SqlParameter("ContractType", dto.ContractTypeVal ?? Convert.DBNull),
                new SqlParameter("fkOriginID", dto.fkOriginID ?? Convert.DBNull),
                new SqlParameter("fkDestinationID", dto.fkDestinationID ?? Convert.DBNull),
                new SqlParameter("Remark", dto.Remark ?? Convert.DBNull),
                new SqlParameter("Attachments", dto.Attachments ?? Convert.DBNull),
                new SqlParameter("ActionType", "insert"),
                new SqlParameter("TradeLane", dto.TradeLane?? Convert.DBNull),
                new SqlParameter("SCTermNo", dto.SCTermNo ?? Convert.DBNull),
                new SqlParameter("AmedementNo", dto.AmedementNo ?? Convert.DBNull),
                new SqlParameter("AmedementDate", dto.AmedementDate ?? Convert.DBNull)
                ).ToList();
                List<int> objList = objResult.ToList();
                long vcontactcategoryid = objList[0];
                var result = _context.Detail(vcontactcategoryid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, LGVWContractRateDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SAVE_CONTRACT_RATE @ContractID,@ContractNo,@StartDate,@EndDate,@IsValid,@CarrierID,@ContractDate,@MQCTYPE,@MQC,@ContractType,@fkOriginID,@fkDestinationID,@Remark,@Attachments,@ActionType,@TradeLane,@SCTermNo,@AmedementNo,@AmedementDate",
                new SqlParameter("ContractID", dto.ContractID),
                new SqlParameter("ContractNo", dto.ContractNo),
                new SqlParameter("StartDate", dto.StartDate),
                new SqlParameter("EndDate", dto.EndDate),
                new SqlParameter("IsValid", dto.IsValid),
                new SqlParameter("CarrierID", dto.CarrierID),
                new SqlParameter("ContractDate", dto.ContractDate),
                new SqlParameter("MQCTYPE", dto.MQCTYPE),
                new SqlParameter("MQC", Convert.ToDecimal(dto.mqcvalue)),
                new SqlParameter("ContractType", dto.ContractTypeVal ?? Convert.DBNull),
                new SqlParameter("fkOriginID", dto.fkOriginID ?? Convert.DBNull),
                new SqlParameter("fkDestinationID", dto.fkDestinationID ?? Convert.DBNull),
                new SqlParameter("Remark", dto.Remark ?? Convert.DBNull),
                new SqlParameter("Attachments", dto.Attachments ?? Convert.DBNull),
                new SqlParameter("ActionType", "update"),
                new SqlParameter("TradeLane", dto.TradeLane ?? Convert.DBNull),
                new SqlParameter("SCTermNo", dto.SCTermNo ?? Convert.DBNull),
                new SqlParameter("AmedementNo", dto.AmedementNo ?? Convert.DBNull),
                new SqlParameter("AmedementDate", dto.AmedementDate ?? Convert.DBNull)
                ).ToList();
                List<int> objList = objResult.ToList();
                long vcontactcategoryid = objList[0];
                var result = _context.Detail(vcontactcategoryid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(LGVWContractRateDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SAVE_CONTRACT_RATE @ContractID,@ContractNo,@StartDate,@EndDate,@IsValid,@CarrierID,@ContractDate,@MQCTYPE,@MQC,@ContractType,@fkOriginID,@fkDestinationID,@Remark,@Attachments,@ActionType,@TradeLane,@SCTermNo,@AmedementNo,@AmedementDate",
               new SqlParameter("ContractID", dto.ContractID),
               new SqlParameter("ContractNo", dto.ContractNo),
               new SqlParameter("StartDate", dto.StartDate),
               new SqlParameter("EndDate", dto.EndDate),
               new SqlParameter("IsValid", dto.IsValid),
               new SqlParameter("CarrierID", dto.CarrierID),
               new SqlParameter("ContractDate", dto.ContractDate),
               new SqlParameter("MQCTYPE", dto.MQCTYPE),
               new SqlParameter("MQC", Convert.ToDouble(dto.mqcvalue)),
               new SqlParameter("ContractType", dto.ContractTypeVal ?? Convert.DBNull),
               new SqlParameter("fkOriginID", dto.fkOriginID ?? Convert.DBNull),
               new SqlParameter("fkDestinationID", dto.fkDestinationID ?? Convert.DBNull),
               new SqlParameter("Remark", dto.Remark ?? Convert.DBNull),
               new SqlParameter("Attachments", dto.Attachments ?? Convert.DBNull),
               new SqlParameter("ActionType", "delete"),
               new SqlParameter("TradeLane", dto.TradeLane ?? Convert.DBNull),
               new SqlParameter("SCTermNo", dto.SCTermNo ?? Convert.DBNull),
               new SqlParameter("AmedementNo", dto.AmedementNo ?? Convert.DBNull),
               new SqlParameter("AmedementDate", dto.AmedementDate ?? Convert.DBNull)
               ).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]
        // public  ActionResult GetContractList(Dictionary<string, string> listParams)
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                //var filter = JsonConvert.DeserializeObject<List<Filter>>(listParams.Filter);
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<LG_VW_Contract> result = _context.ExecuteQuery<LG_VW_Contract>("EXEC dbo.USP_LG_VW_Contract_LIST @PAGENO, @PAGESIZE,@Carrier,@ContractNo,@ValidFrom,@ValidTo,@SORTCOLUMN,@SORTORDER,@SIT_ID,@USERID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("Carrier", searchCriteria["Carrier"]),
                        new SqlParameter("ContractNo", searchCriteria["ContractNo"]),
                        new SqlParameter("ValidFrom", searchCriteria["fromDate"]),
                        new SqlParameter("ValidTo", searchCriteria["toDate"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("SIT_ID", listParams.SiteId),
                        new SqlParameter("USERID", "0")
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                }
                
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return AppResult(ex);
            }

        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReport(ListParams listParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                string reportPath = "/AppMGL.Report/ContractReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("PAGENO", listParams.PageIndex.ToString()));
                reportParams.Add(new KeyValuePair<string, string>("PAGESIZE", listParams.PageSize.ToString()));

                if (string.IsNullOrEmpty(searchCriteria["Carrier"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("Carrier", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("Carrier", searchCriteria["Carrier"]));
                }

                if (string.IsNullOrEmpty(searchCriteria["ContractNo"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("ContractNo", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("ContractNo", searchCriteria["ContractNo"]));
                }

                reportParams.Add(new KeyValuePair<string, string>("ValidFrom", searchCriteria["fromDate"]));


                reportParams.Add(new KeyValuePair<string, string>("ValidTo", searchCriteria["toDate"]));
                reportParams.Add(new KeyValuePair<string, string>("SORTCOLUMN", sortOrder.First().Key));
                reportParams.Add(new KeyValuePair<string, string>("SORTORDER", sortOrder.First().Value));
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", listParams.SiteId.ToString()));
                reportParams.Add(new KeyValuePair<string, string>("USERID", "0"));
                

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "Contract_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

                    HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new MemoryStream(reportBuffer));
                    result.Content.Headers.Add("X-FileName", fileName);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                    result.StatusCode = HttpStatusCode.OK;

                    return result;
                }

                output.AddMessage("System cannot generated report, please contact administrator.", true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { output });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                output.AddMessage(ex.Message, true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { output });
            }
        }

    }
}
