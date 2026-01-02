using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Report;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Report;
using AppMGL.DTO.Operation;
using AppMGL.Manager.Infrastructure;
using AppMGL.Manager.Infrastructure.Report;
using AppMGL.Manager.Infrastructure.Results;
using Newtonsoft.Json;

namespace AppMGL.Manager.Areas.Report.Controllers
{
    public class QuotationReportController : BaseController<QuotationReportDTO, QuotationReportRepository, RPT_GET_QUOTATION_Result>
    {
        #region Constructor

        public QuotationReportController(QuotationReportRepository context)
        {
            _context = context;
            BaseModule = EnumModule.QuotationReport;
            KeyField = "QuotationID";
        }

        #endregion

        #region Public Methods

        [System.Web.Http.HttpPost]
        public ActionResult GetQuotationList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<QuotationReportDTO> result = _context.ExecuteQuery<QuotationReportDTO>("EXEC dbo.USP_LG_RPT_QUOTATION_LIST @PAGENO, @PAGESIZE,@Type,@EnquiryDateFrom,@EnquiryDateTo,@OPTIONVALUE,@SEARCHVALUE,@CompanyName,@SORTCOLUMN,@SORTORDER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("Type", searchCriteria["Type"]),
                        new SqlParameter("EnquiryDateFrom", searchCriteria["EnquiryDateFrom"]),
                        new SqlParameter("EnquiryDateTo", searchCriteria["EnquiryDateTo"]),
                        new SqlParameter("OPTIONVALUE", searchCriteria["OptionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["SeachValue"]),
                        new SqlParameter("CompanyName", searchCriteria["CompanyName"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),                        
                        new SqlParameter("SIT_ID", listParams.SiteId)
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                }
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReport1(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/QuotationReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("Type", exportParams["Type"]));

                if (string.IsNullOrEmpty(exportParams["EnquiryDateFrom"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("EnquiryDateFrom", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("EnquiryDateFrom", exportParams["EnquiryDateFrom"]));
                }

                if (string.IsNullOrEmpty(exportParams["EnquiryDateTo"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("EnquiryDateTo", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("EnquiryDateTo", exportParams["EnquiryDateTo"]));
                }

                reportParams.Add(new KeyValuePair<string, string>("EnquiryNo", exportParams["EnquiryNo"]));
                reportParams.Add(new KeyValuePair<string, string>("CompanyName", exportParams["CompanyName"]));
                reportParams.Add(new KeyValuePair<string, string>("QuotationNo", exportParams["QuotationNo"]));
                reportParams.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"]));
                reportParams.Add(new KeyValuePair<string, string>("MiamiRefNo", exportParams["MiamiRefNo"]));
                reportParams.Add(new KeyValuePair<string, string>("BookingNo", exportParams["BookingNo"]));
                reportParams.Add(new KeyValuePair<string, string>("SitId", exportParams["SitId"]));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "Quotation_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/QuotationReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();

                reportParams.Add(new KeyValuePair<string, string>("Type", exportParams["Type"]));
                if (string.IsNullOrEmpty(exportParams["EnquiryDateFrom"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("EnquiryDateFrom", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("EnquiryDateFrom", exportParams["EnquiryDateFrom"]));
                }

                if (string.IsNullOrEmpty(exportParams["EnquiryDateTo"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("EnquiryDateTo", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("EnquiryDateTo", exportParams["EnquiryDateTo"]));
                }
                if (string.IsNullOrEmpty(exportParams["OptionValue"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", exportParams["OptionValue"]));
                }
                if (string.IsNullOrEmpty(exportParams["SearchValue"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", exportParams["SearchValue"]));
                }
                if (string.IsNullOrEmpty(exportParams["CompanyName"]))
                {
                    reportParams.Add(new KeyValuePair<string, string>("CompanyName", null));
                }
                else
                {
                    reportParams.Add(new KeyValuePair<string, string>("CompanyName", exportParams["CompanyName"]));
                }
                

                //reportParams.Add(new KeyValuePair<string, string>("EnquiryDateFrom", exportParams["EnquiryDateFrom"]));
                //reportParams.Add(new KeyValuePair<string, string>("EnquiryDateTo", exportParams["EnquiryDateTo"]));
                //reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", exportParams["OptionValue"]));
                //reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", exportParams["SearchValue"]));
                //reportParams.Add(new KeyValuePair<string, string>("CompanyName", exportParams["CompanyName"]));               
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SitId"]));

                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "Quotation_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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

        [System.Web.Http.HttpPost]
        public ActionResult GetQuotationEnquiry(Dictionary<string, string> listParams)
        {
            try
            {
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("QuotationId", listParams["QuotationId"])
                };

                List<QuotationEnquiryDTO> result = _context.ExecuteQuery<QuotationEnquiryDTO>(QuotationReportQuery.GetQuotationEnquiry, parameters.ToArray()).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetQuotationCharges(Dictionary<string, string> listParams)
        {
            try
            {
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("QuotationId", listParams["QuotationId"]),
                    new SqlParameter("QuotationCarrierId", listParams["QuotationCarrierId"]),
                    new SqlParameter("PickupTypeId", listParams["PickupTypeId"])
                };

                List<QuotationChargesDTO> result = _context.ExecuteQuery<QuotationChargesDTO>(QuotationReportQuery.GetQuotationCharges, parameters.ToArray()).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetQuotaionChargesList(Dictionary<string, string> listParams)
        {
            try
            {
                List<CarrierChargesDTO> result = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_CARRIERS_RATES @QUOTATIONID",                                        
                                        new SqlParameter("QUOTATIONID", decimal.Parse(listParams["QuotationId"]))).ToList();

                return AppResult(result, 0);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        #endregion
    }
}
