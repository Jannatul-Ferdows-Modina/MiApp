using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Home;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Home;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;
using AppMGL.DAL.Helper;
using Newtonsoft.Json;
using AppMGL.DAL.Helper.Logging;
using System.Net.Http;
using AppMGL.Manager.Infrastructure.Results;
using AppMGL.Manager.Infrastructure.Report;
using System.Net;
using System.IO;
using System.Net.Http.Headers;

namespace AppMGL.Manager.Areas.Home.Controllers
{
    public class DashboardController : BaseController<DashboardDTO, DashboardRepository, DashboardData_Job>
    {
        #region Constructor

        public DashboardController(DashboardRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Dashboard;
            KeyField = "Id";
        }

        #endregion

        #region Public Method
        [System.Web.Http.HttpGet]
        public virtual ActionResult GetDashBoardData(int id)
        {
            try
            {
                Logger.WriteWarning("Dasboard id " + id.ToString(), false);

                List<DashboardDTO> result = _context.ExecuteQuery<DashboardDTO>("EXEC dbo.USP_GET_DASHBOARD_DATA @SIT_ID",
                        new SqlParameter("SIT_ID", id)
                        ).ToList();
                DashboardDTO objDashboardDTO = result[0];

                Logger.WriteWarning("Dasboard id data" + objDashboardDTO.DashboardId, false);
                return AppResult(objDashboardDTO, 1, "", EnumResult.Success);
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
                List<DashboardDTO> result = _context.ExecuteQuery<DashboardDTO>("EXEC dbo.USP_GET_DASHBOARD_DATA @SIT_ID",
                        new SqlParameter("SIT_ID", listParams.SiteId)
                        ).ToList();
                DashboardDTO objDashboardDTO = result[0];
                return AppResult(objDashboardDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult ListTargetData(TargetFilter obj)
        {
            try
            {
                List<DashboardTargetDTO> result = _context.ExecuteQuery<DashboardTargetDTO>("EXEC dbo.USP_GET_DASHBOARD_TARGETDATA @SiteId,@TargetYear",
                        new SqlParameter("SiteId", obj.FrenchieId),
                        new SqlParameter("TargetYear", obj.TargetYear)).ToList();
                return AppResult(result, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetTotalActivity(int id)
        {
            try
            {
                //int TotalCount;
                IEnumerable<int> objCount = _context.ExecuteQuery<int>("EXEC dbo.USP_GET_DASHBOARD_TOTAL_AT @SIT_ID",
                   new SqlParameter("SIT_ID", id)).ToList();
                List <int> objList = objCount.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetSiteLocations()
        {
            try
            {
                //int TotalCount;                
                var result = _context.ExecuteQuery<LocationDTO>("EXEC dbo.USP_LG_GET_SITE_LOCATION").ToList();
                Logger.WriteWarning("GetSiteLocations data " + result.Count, false);
                return AppResult(result, result.Count);
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
                string reportPath = "/AppMGL.Report/dashboardreport";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SIT_ID"]));
                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "Dashboard_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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