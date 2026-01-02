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
using AppMGL.Manager.Infrastructure;
using AppMGL.Manager.Infrastructure.Report;
using AppMGL.Manager.Infrastructure.Results;

namespace AppMGL.Manager.Areas.Report.Controllers
{
    public class BookingStatusReportController : BaseController<BookingStatusReportDTO, BookingStatusReportRepository, RPT_GET_BOOKING_STATUS_Result>
    {
        #region Constructor

        public BookingStatusReportController(BookingStatusReportRepository context)
        {
            _context = context;
            BaseModule = EnumModule.BookingReport;
            KeyField = "SerialNo";
        }

        #endregion

        #region Public Methods

        [System.Web.Http.HttpPost]
        public ActionResult GetBookingStatusList(Dictionary<string, string> listParams)
        {
            try
            {
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("PageIndex", listParams["PageIndex"]),
                    new SqlParameter("PageSize", listParams["PageSize"]),
                    new SqlParameter("Sort", Utility.GetSort(listParams["Sort"])),
                    new SqlParameter("BookingStatusId", listParams["BookingStatusId"]),
                    new SqlParameter("EnquiryNo", listParams["EnquiryNo"]),
                    new SqlParameter("CompanyName", listParams["CompanyName"]),
                    new SqlParameter("QuotationNo", listParams["QuotationNo"]),
                    new SqlParameter("SystemRefNo", listParams["SystemRefNo"]),
                    new SqlParameter("MiamiRefNo", listParams["MiamiRefNo"]),
                    new SqlParameter("BookingNo", listParams["BookingNo"]),
                    new SqlParameter("SitId", listParams["SitId"]),
                    new SqlParameter("Count", SqlDbType.Int) {Direction = ParameterDirection.Output}
                };

                List<BookingStatusReportDTO> result = _context.ExecuteQuery<BookingStatusReportDTO>((new BookingStatusReportQuery()).List, parameters.ToArray()).ToList();
                int count = Utility.GetParamValue(parameters, "Count", typeof(int));
                return AppResult(result, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/BookingStatusReport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("BookingStatusId", exportParams["BookingStatusId"]));
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
                    string fileName = "BookingStatus_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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

        #endregion
    }
}
