using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
using Intuit.Ipp.Core;
using Intuit.Ipp.DataService;
using System.Security.Claims;
using Intuit.Ipp.Security;
using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using System.Text.RegularExpressions;
using AppMGL.DTO.Operation;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace AppMGL.Manager.Areas.Report.Controllers
{
    public class MCSReportController : BaseController<MCSReportDTO, MCSReportRepository, SIPL_DocumentCommon>
    {
        #region Constructor

        public MCSReportController(MCSReportRepository context)
        {
            _context = context;
            BaseModule = EnumModule.MCSReport;
            KeyField = "ExportRef";
        }

        #endregion

        #region Public Methods

        [System.Web.Http.HttpPost]
        public ActionResult GetMCSList(Dictionary<string, string> listParams)
        {
            try
            {

                string startdate = Convert.ToDateTime(listParams["StartBookingDate"].ToString()).ToString("yyyy/MM/dd");
                string enddate = Convert.ToDateTime(listParams["EndBookingDate"].ToString()).ToString("yyyy/MM/dd");
                Logger.WriteWarning("Start " + listParams["StartBookingDate"].ToString());
                Logger.WriteWarning("startdate " + startdate);
                Logger.WriteWarning("enddate " + enddate);
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("SiteId", listParams["SiteId"]),
                    new SqlParameter("StartBookingDate", startdate),
                    new SqlParameter("EndBookingDate", enddate),
                    new SqlParameter("DeptId", listParams["DeptId"]),
                    new SqlParameter("isinvoiceready", listParams["isinvoiceready"]),
                    new SqlParameter("SystemRefNo", listParams["SystemRefNo"]),
                    new SqlParameter("QBStatus", listParams["QBStatus"]),
                    new SqlParameter("QBMaturedMonth", listParams["QBMaturedMonth"]),
                     new SqlParameter("UserId", listParams["UserId"])
                    
                };

                List<MCSReportDTO> result = _context.ExecuteQuery<MCSReportDTO>((new MCSReportQuery()).List, parameters.ToArray()).ToList();
                int count = result.Count;// Utility.GetParamValue(parameters, "Count", typeof(int));
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }



        public static T GetEntity<T>(DataRow row) where T : new()
        {
            var entity = new T();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                //Get the description attribute
                var descriptionAttribute = (DescriptionAttribute)property.GetCustomAttributes(typeof(DescriptionAttribute), true).SingleOrDefault();
                if (descriptionAttribute == null)
                    continue;

                property.SetValue(entity, row[descriptionAttribute.Description]);
            }

            return entity;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReport(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/MCSReportNew";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("SiteId", exportParams["SiteId"]));
                reportParams.Add(new KeyValuePair<string, string>("StartBookingDate", exportParams["StartBookingDate"]));
                reportParams.Add(new KeyValuePair<string, string>("EndBookingDate", exportParams["EndBookingDate"]));
                reportParams.Add(new KeyValuePair<string, string>("DeptId", exportParams["DeptId"]));
                reportParams.Add(new KeyValuePair<string, string>("isinvoiceready", exportParams["isinvoiceready"] == "" ? "1" : exportParams["isinvoiceready"]));
                reportParams.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"] == null ? "" : exportParams["SystemRefNo"].ToString()));
                reportParams.Add(new KeyValuePair<string, string>("QBStatus", exportParams["QBStatus"] == null ? "" : exportParams["QBStatus"].ToString()));
                reportParams.Add(new KeyValuePair<string, string>("QBMaturedMonth", exportParams["QBMaturedMonth"] == null ? "" : exportParams["QBMaturedMonth"].ToString()));
                reportParams.Add(new KeyValuePair<string, string>("UserId", exportParams["UserId"] == null ? "" : exportParams["UserId"].ToString()));
                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "MCS_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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
        public HttpResponseMessage ExportReport_Sepatara(Dictionary<string, string> exportParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/MCSReportNew_Sepatara";

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("SiteId", exportParams["SiteId"]));
                reportParams.Add(new KeyValuePair<string, string>("StartBookingDate", exportParams["StartBookingDate"]));
                reportParams.Add(new KeyValuePair<string, string>("EndBookingDate", exportParams["EndBookingDate"]));
                reportParams.Add(new KeyValuePair<string, string>("DeptId", exportParams["DeptId"]));
                reportParams.Add(new KeyValuePair<string, string>("isinvoiceready", exportParams["isinvoiceready"] == "" ? "1" : exportParams["isinvoiceready"]));
                reportParams.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"] == null ? "" : exportParams["SystemRefNo"].ToString()));
                reportParams.Add(new KeyValuePair<string, string>("QBStatus", exportParams["QBStatus"] == null ? "" : exportParams["QBStatus"].ToString()));
                reportParams.Add(new KeyValuePair<string, string>("QBMaturedMonth", exportParams["QBMaturedMonth"] == null ? "" : exportParams["QBMaturedMonth"].ToString()));
                reportParams.Add(new KeyValuePair<string, string>("UserId", exportParams["UserId"] == null ? "" : exportParams["UserId"].ToString()));               
                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "Sepatara_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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
        public ActionResult GetQBMCSList(Dictionary<string, string> listParams)
        {
            try
            {
                List<SqlParameter> list = new List<SqlParameter>
                {
                    new SqlParameter("Documentno", listParams["Documentno"]),
                    new SqlParameter("userid", listParams["userid"])
                };
                List<QuckbookInvoiceReceiptnew> list2 = _context.ExecuteQuery<QuckbookInvoiceReceiptnew>(new MCSQBQuery().List, list.ToArray()).ToList();
                int count = list2.Count;
                return AppResult(list2, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public ServiceContext IntializeContext(string realmId, string accessToken)
        {
            var principal = User as ClaimsPrincipal;
            OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(accessToken);
            ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
            //Enable minorversion 
            serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
            //Enable logging
            //serviceContext.IppConfiguration.Logger.RequestLog.EnableRequestResponseLogging = true;
            //serviceContext.IppConfiguration.Logger.RequestLog.ServiceRequestLoggingLocation = @"C:\IdsLogs";//Create a folder in your drive first
            return serviceContext;
        }
        private Customer CreateCustomer()
        {
            Random random = new Random();
            Customer customer = new Customer();

            customer.GivenName = "Vikas test MGL" + random;
            customer.FamilyName = "Serling";
            customer.DisplayName = "Vikas test MGL";
            return customer;
        }
        public string SentenceCase(string input)
        {
            if (input.Length < 1)
                return input;

            string sentence = input.ToLower();
            return sentence[0].ToString().ToUpper() +
               sentence.Substring(1);
        }
        private Invoice CreateInvoice(string realmId, Customer customer, string accessToken, MCSReportDTO mcobj)
        {



            if (mcobj.ModeOfService == "LCL" || mcobj.ModeOfService == "LCL")
            {

            }

            List<ExpenseDetailDTO> expenseresult = _context.ExecuteQuery<ExpenseDetailDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPENSES_REPORT_DETAILS @DOCUMENTCOMMONID,@SIT_ID",
                                          new SqlParameter("DOCUMENTCOMMONID", mcobj.DocumentCommonID),
                                          new SqlParameter("SIT_ID", mcobj.SitId)).ToList();



            IEnumerable<DispatchContainerDTO> ContainerResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_CONTAINER_DISPATCH_TABLE @CommonID",
                                       new SqlParameter("CommonID", mcobj.DocumentCommonID)).ToList();
            List<DispatchContainerDTO> DispatchContainerDTOList = ContainerResult.ToList();
            if (DispatchContainerDTOList.Count > 0)
            {
                //  objBookingListDTO.DispatchContainerDTOList = DispatchContainerDTOList.ToArray();
            }
            IEnumerable<DispatchContainerDTO> DispatchTableResult = _context.ExecuteQuery<DispatchContainerDTO>("EXEC dbo.USP_LG_BOOKING_GET_DISPATCH_TABLE_DATA @CommonID",
                                    new SqlParameter("CommonID", mcobj.DocumentCommonID)).ToList();
            List<DispatchContainerDTO> DispatchTableList = DispatchTableResult.ToList();
            if (DispatchContainerDTOList.Count > 0 && DispatchTableList.Count > 0)
            {
                //foreach (DispatchContainerDTO ContainerItem in DispatchContainerDTOList)
                //{
                //    foreach (DispatchContainerDTO DispatchItem in DispatchTableList)
                //    {
                //        if (DispatchItem.ContainerID == ContainerItem.ContainerID && DispatchItem.SeqNo == ContainerItem.SeqNo)
                //        {
                //            ContainerItem.CNTNo = DispatchItem.CNTNo;
                //            ContainerItem.SealNo = DispatchItem.SealNo;
                //            ContainerItem.SeqNo = DispatchItem.SeqNo;
                //            ContainerItem.DispatchRemarks = DispatchItem.DispatchRemarks;
                //            ContainerItem.IsSelected = true;
                //        }
                //    }
                //}
            }

            if (DispatchContainerDTOList.Count > 0)
            {
                foreach (var item in DispatchContainerDTOList)
                {
                    item.ContainerOwnerName = "LINE";
                }
                // objBookingListDTO.DispatchContainerDTOList = DispatchContainerDTOList.ToArray();
            }





            // https://gist.github.com/IntuitDeveloperRelations/6500373

            // Step 1: Initialize OAuth2RequestValidator and ServiceContext
            ServiceContext serviceContext = IntializeContext(realmId, accessToken);

            // Step 2: Initialize an Invoice object
            Invoice invoice = new Invoice();
            // invoice.Deposit = new Decimal(0.00);
            //invoice.DepositSpecified = true;


            // Step 3: Invoice is always created for a customer so lets retrieve reference to a customer and set it in Invoice
            /*QueryService<Customer> querySvc = new QueryService<Customer>(serviceContext);
            Customer customer = querySvc.ExecuteIdsQuery("SELECT * FROM Customer WHERE CompanyName like 'Amy%'").FirstOrDefault();*/
            invoice.CustomerRef = new ReferenceType()
            {
                Value = customer.Id,
                name = customer.CompanyName
            };

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            string strimport = Regex.Replace(mcobj.FileNo, "[@,ABCDEFGHIJKLMNOPQRSTUVYZ]", string.Empty);
            strimport = strimport.Substring(2, 2);
            invoice.DocNumber = mcobj.FileNo;

            // Step 4: Invoice is always created for an item so lets retrieve reference to an item and a Line item to the invoice
            /* QueryService<Item> querySvcItem = new QueryService<Item>(serviceContext);
            Item item = querySvcItem.ExecuteIdsQuery("SELECT * FROM Item WHERE Name = 'Lighting'").FirstOrDefault();*/
            List<Line> lineList = new List<Line>();
            Line line = new Line();
            Line line1 = new Line();
            Line line2 = new Line();
            Line line3 = new Line();
            Line line4 = new Line();

            var itemcount = Convert.ToInt32(mcobj.isContainer20Exits) + Convert.ToInt32(mcobj.isContainer40Exits) + Convert.ToInt32(mcobj.isContainer45Exits) + Convert.ToInt32(mcobj.VolumeInCB);
            double totalprice = Convert.ToDouble(mcobj.TOTAL_SELLING_RATE);
            double unitprice = Convert.ToDouble(mcobj.TOTAL_SELLING_RATE) / itemcount;

            string cno_20 = "";
            string cno_40 = "";
            string cno_45 = "";


            //invoice.
            string desc = "";// "Ocean freight/Inland Freight/other Income heads";



            if (!string.IsNullOrEmpty(Convert.ToString(mcobj.BLNumber)))
            {

                //  desc = desc + " B/L # " + mcobj.BLNumber.ToString();// + " in  some SP'S ";
            }


            string conno = "";
            if (Convert.ToString(mcobj.isContainer20Exits) != "0" && !string.IsNullOrEmpty(Convert.ToString(mcobj.isContainer20Exits)))
            {

                desc = desc + "" + mcobj.isContainer20Exits.ToString().PadLeft(2, '0') + " X20''" + " ";
                if (DispatchTableList.Count > 0)
                {
                    if (DispatchContainerDTOList.Count > 0)
                    {

                        var oo = DispatchContainerDTOList.Where(x => x.Name.Contains("20")).FirstOrDefault();
                        if (oo != null && oo.SeqNo != null)
                        {
                            var o = DispatchTableList.Where(x => x.SeqNo == oo.SeqNo).FirstOrDefault();
                            if (o != null && o.CNTNo != null)
                            {
                                //  desc = desc + " Container No.# " + o.CNTNo;
                                conno = conno + o.CNTNo + ",";
                            }
                        }
                    }

                }


            }
            if (Convert.ToString(mcobj.isContainer40Exits) != "0" && !string.IsNullOrEmpty(Convert.ToString(mcobj.isContainer40Exits)))
            {


                desc = desc + "" + mcobj.isContainer40Exits.ToString().PadLeft(2, '0') + " X40''" + " ";
                if (DispatchTableList.Count > 0)
                {
                    if (DispatchContainerDTOList.Count > 0)
                    {

                        var oo = DispatchContainerDTOList.Where(x => x.Name.Contains("40")).FirstOrDefault();
                        if (oo != null && oo.SeqNo != null)
                        {
                            var o = DispatchTableList.Where(x => x.SeqNo == oo.SeqNo).FirstOrDefault();
                            if (o != null && o.CNTNo != null)
                            {
                                //   desc = desc + " Container No. # " + o.CNTNo;
                                conno = conno + o.CNTNo + ",";
                            }
                        }
                    }
                }
            }
            if (Convert.ToString(mcobj.isContainer45Exits) != "0" && !string.IsNullOrEmpty(Convert.ToString(mcobj.isContainer45Exits)))
            {


                desc = desc + "" + mcobj.isContainer45Exits.ToString().PadLeft(2, '0') + " X45''" + " ";
                if (DispatchTableList.Count > 0)
                {

                    if (DispatchContainerDTOList.Count > 0)
                    {
                        var oo = DispatchContainerDTOList.Where(x => x.Name.Contains("40")).FirstOrDefault();
                        if (oo != null && oo.SeqNo != null)
                        {

                            var o = DispatchTableList.Where(x => x.SeqNo == oo.SeqNo).FirstOrDefault();
                            if (o != null && o.CNTNo != null)
                            {
                                //  desc = desc + " Container No. # " + o.CNTNo;
                                conno = conno + o.CNTNo + ",";
                            }
                        }
                    }
                }
            }
            if (Convert.ToString(mcobj.ModeOfService) == "LCL" && !string.IsNullOrEmpty(Convert.ToString(mcobj.VolumeInCB)))
            {
                itemcount = Convert.ToInt32(mcobj.isContainer20Exits) + Convert.ToInt32(mcobj.isContainer40Exits) + Convert.ToInt32(mcobj.isContainer45Exits) + Convert.ToInt32(mcobj.VolumeInCB);

                desc = desc + "" + mcobj.VolumeInCB.ToString().PadLeft(2, '0') + " X LCL''" + " ";
                if (DispatchTableList.Count > 0)
                {

                    if (DispatchContainerDTOList.Count > 0)
                    {
                        var oo = DispatchContainerDTOList.Where(x => x.Name.Contains("LCL")).FirstOrDefault();
                        if (oo != null && oo.SeqNo != null)
                        {

                            var o = DispatchTableList.Where(x => x.SeqNo == oo.SeqNo).FirstOrDefault();
                            if (o != null && o.CNTNo != null)
                            {
                                //  desc = desc + " Container No. # " + o.CNTNo;
                                conno = conno + o.CNTNo + ",";
                            }
                        }
                    }
                }
            }

            if (Convert.ToString(mcobj.ModeOfService) == "RORO" && !string.IsNullOrEmpty(Convert.ToString(mcobj.VolumeInCB)))
            {

                itemcount = Convert.ToInt32(mcobj.isContainer20Exits) + Convert.ToInt32(mcobj.isContainer40Exits) + Convert.ToInt32(mcobj.isContainer45Exits) + Convert.ToInt32(mcobj.VolumeInCB);
                desc = desc + "" + mcobj.VolumeInCB.ToString().PadLeft(2, '0') + " X RORO''" + " ";
                if (DispatchTableList.Count > 0)
                {

                    if (DispatchContainerDTOList.Count > 0)
                    {
                        var oo = DispatchContainerDTOList.Where(x => x.Name.Contains("LCL")).FirstOrDefault();
                        if (oo != null && oo.SeqNo != null)
                        {

                            var o = DispatchTableList.Where(x => x.SeqNo == oo.SeqNo).FirstOrDefault();
                            if (o != null && o.CNTNo != null)
                            {
                                //  desc = desc + " Container No. # " + o.CNTNo;
                                conno = conno + o.CNTNo + ",";
                            }
                        }
                    }
                }
            }


            desc = desc + " Exw ";
            if (!string.IsNullOrEmpty(Convert.ToString(mcobj.PortOfOrigin)))
            {

                desc = desc + "  " + mcobj.PortOfOrigin.ToString().ToUpper();
            }
            if (!string.IsNullOrEmpty(Convert.ToString(mcobj.PortOfDischarge)))
            {

                desc = desc + " to  " + mcobj.PortOfDischarge.ToString().ToUpper();
            }
            desc = desc + " Booking # " + mcobj.BookingNo;

            if (!string.IsNullOrEmpty(mcobj.BLNumber))
            {
                desc = desc + " B/L # " + mcobj.BLNumber + " ";
            }

            desc = desc + " Container # " + mcobj.ContainerNumber + "  ";

            decimal totalamtcase1 = 0;
            mcobj.isconsolidatedreport = (mcobj.isconsolidatedreport == null ? "0" : mcobj.isconsolidatedreport);

            if (mcobj.isconsolidatedreport == "1" || mcobj.isconsolidatedreport == "2" || mcobj.isconsolidatedreport == "3" || mcobj.isconsolidatedreport == "4")
            {

                string InsurnceId = ConfigurationManager.AppSettings["insurance"];
                string FreightDesc = "Ocean Freight Charges";
                string InlandDesc = "Inland Freight Charges";
                string CourierDesc = "Import Courier";
                string OtherDesc = "Import Other";
                string FreightId = ConfigurationManager.AppSettings["importfreightkey"];
                string InlandId = ConfigurationManager.AppSettings["importinlandkey"];
                string CourierId = ConfigurationManager.AppSettings["importcourierkey"];
                string OtherId = ConfigurationManager.AppSettings["importotherkey"];
                string ClassId = ConfigurationManager.AppSettings["exportClassId"];
                if (Convert.ToInt32(strimport) == 1)//export

                {
                    FreightDesc = "Ocean Freight Charges";
                    InlandDesc = "Inland Freight Charges";
                    CourierDesc = "Export Courier";
                    OtherDesc = "Export Other";
                    FreightId = ConfigurationManager.AppSettings["exportfreightkey"];
                    InlandId = ConfigurationManager.AppSettings["exportinlandkey"];
                    CourierId = ConfigurationManager.AppSettings["exportcourierkey"];
                    OtherId = ConfigurationManager.AppSettings["exportotherkey"];
                    ClassId = ConfigurationManager.AppSettings["exportClassId"];
                }

                // desc = "test";
                line.Description = (desc);
                line.Amount = Convert.ToDecimal("0");
                line.AmountSpecified = true;
                SalesItemLineDetail salesItemLineDetail = new SalesItemLineDetail();
                salesItemLineDetail.AnyIntuitObject = Convert.ToDecimal("0"); // +"/" + mcobj.FREIGHT_BUYING_RATE.ToString();
                salesItemLineDetail.ItemElementName = ItemChoiceType.UnitPrice;
                // salesItemLineDetail.uni
                salesItemLineDetail.Qty = Convert.ToDecimal("0");
                salesItemLineDetail.QtySpecified = false;
                salesItemLineDetail.ItemRef = new ReferenceType()
                {

                    Value = ConfigurationManager.AppSettings["desc"]
                };


                line.AnyIntuitObject = salesItemLineDetail;

                line.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                line.DetailTypeSpecified = true;

                if (mcobj.isconsolidatedreport == "1" || mcobj.isconsolidatedreport == "2")
                {



                    // line 1 
                    line1.Description = FreightDesc;

                    if (Convert.ToInt32(mcobj.isconsolidatedreport) == 1)
                        line1.Amount = Math.Round(Convert.ToDecimal(((Convert.ToDecimal(mcobj.FREIGHT_SELLING_RATE) + Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) + Convert.ToDecimal(mcobj.COURIER_SELLING_RATE) + Convert.ToDecimal(mcobj.OTHER_SELLING_RATE)))));
                    //line1.Amount = (Convert.ToDecimal(((Convert.ToDecimal(mcobj.FREIGHT_SELLING_RATE) + Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) + Convert.ToDecimal(0) + Convert.ToDecimal(0)))));
                    // else
                    //    line1.Amount = (Convert.ToDecimal(mcobj.FREIGHT_SELLING_RATE));
                    line1.AmountSpecified = true;
                    //rate


                    SalesItemLineDetail salesItemLineDetail1 = new SalesItemLineDetail();

                    // +"/" + mcobj.FREIGHT_BUYING_RATE.ToString();
                    salesItemLineDetail1.ItemElementName = ItemChoiceType.UnitPrice;
                    // salesItemLineDetail.uni

                    Logger.WriteWarning("isconsolidatedreport " + mcobj.isconsolidatedreport.ToString(), false);
                    Logger.WriteWarning("stritemcount " + itemcount, false);
                    if (Convert.ToInt32(mcobj.isconsolidatedreport) == 1)
                    {
                        //salesItemLineDetail1.Qty = Convert.ToDecimal(1);
                        //salesItemLineDetail1.AnyIntuitObject = Convert.ToDecimal(mcobj.FREIGHT_SELLING_RATE) + Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) + Convert.ToDecimal(mcobj.COURIER_SELLING_RATE) + Convert.ToDecimal(mcobj.OTHER_SELLING_RATE);

                        //  salesItemLineDetail1.Qty = 1;
                        //    salesItemLineDetail1.AnyIntuitObject = ( Convert.ToDecimal((Convert.ToDecimal(mcobj.FREIGHT_SELLING_RATE) + Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) + Convert.ToDecimal(0) + Convert.ToDecimal(0))));


                        salesItemLineDetail1.Qty = itemcount; ;
                        salesItemLineDetail1.AnyIntuitObject = Math.Round(Convert.ToDecimal(mcobj.FREIGHT_SELLING_RATE) + Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) + Convert.ToDecimal(mcobj.COURIER_SELLING_RATE) + Convert.ToDecimal(mcobj.OTHER_SELLING_RATE)) / itemcount;
                        totalamtcase1 = Convert.ToDecimal(Convert.ToDecimal(salesItemLineDetail1.AnyIntuitObject) * itemcount);

                        Logger.WriteWarning("FREIGHT_SELLING_RATE" + mcobj.FREIGHT_SELLING_RATE.ToString(), false);
                        Logger.WriteWarning("INLAND_SELLING_RATE" + mcobj.INLAND_SELLING_RATE.ToString(), false);
                        Logger.WriteWarning("COURIER_SELLING_RATE" + mcobj.COURIER_SELLING_RATE.ToString(), false);
                        Logger.WriteWarning("OTHER_SELLING_RATE" + mcobj.OTHER_SELLING_RATE.ToString(), false);

                        Logger.WriteWarning("line 1 Qty" + itemcount.ToString(), false);
                        Logger.WriteWarning("line 1 AnyIntuitObject" + salesItemLineDetail1.AnyIntuitObject.ToString(), false);
                        Logger.WriteWarning("line 1 Amt" + (totalamtcase1).ToString(), false);
                    }
                    else
                    {
                        salesItemLineDetail1.Qty = itemcount;
                        salesItemLineDetail1.AnyIntuitObject = Math.Round(Convert.ToDecimal(mcobj.FREIGHT_SELLING_RATE / itemcount));
                        line1.Amount = Convert.ToDecimal(salesItemLineDetail1.AnyIntuitObject) * itemcount;
                        line1.AmountSpecified = true;
                    }
                    salesItemLineDetail1.QtySpecified = true;
                    //salesItemLineDetail.


                    salesItemLineDetail1.ItemRef = new ReferenceType()
                    {
                        Value = FreightId
                        //Value = "1"
                    };
                    Logger.WriteWarning("strimport " + strimport, false);

                    Logger.WriteWarning("ClassRef " + ClassId, false);
                    salesItemLineDetail1.ClassRef = new ReferenceType()
                    {
                        Value = ClassId
                    };
                    line1.AnyIntuitObject = salesItemLineDetail1;

                    line1.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                    line1.DetailTypeSpecified = true;
                    //// line 1 


                    lineList.Add(line);
                    lineList.Add(line1);
                    if (mcobj.isconsolidatedreport == "2")
                    {
                        decimal camt = 0;

                        decimal amt = 0;

                        if (Convert.ToInt32(mcobj.isconsolidatedreport) == 1)
                        {
                            amt = Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) + Convert.ToDecimal(mcobj.COURIER_SELLING_RATE) + Convert.ToDecimal(mcobj.OTHER_SELLING_RATE);
                        }
                        else if (Convert.ToInt32(mcobj.isconsolidatedreport) == 2)
                            amt = Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) + Convert.ToDecimal(mcobj.OTHER_SELLING_RATE);
                        decimal line2amt = amt / itemcount;

                        Logger.WriteWarning("line2amt1 " + Math.Round(Convert.ToDecimal(line2amt)).ToString(), false);
                        decimal incamt = 0;
                        if (expenseresult.Count > 0)
                        {
                            var invoiceResult = expenseresult.Where(x => x.ExpenseHead == "Insurance charges").FirstOrDefault();
                            if (invoiceResult != null)
                            {
                                incamt = Convert.ToDecimal(invoiceResult.SellingAmount);
                                amt = amt - incamt;
                                line2amt = amt / itemcount;
                            }
                        }

                        line2.Description = InlandDesc;
                        Logger.WriteWarning("line2Amount " + Math.Round(Convert.ToDecimal(line2amt) * itemcount).ToString(), false);
                        Logger.WriteWarning("line21 " + Math.Round(Convert.ToDecimal(line2amt)).ToString(), false);
                        // amt
                        line2.AmountSpecified = true;
                        SalesItemLineDetail salesItemLineDetail2 = new SalesItemLineDetail();
                        salesItemLineDetail2.AnyIntuitObject = Math.Round(line2amt);     //Convert.ToDecimal((mcobj.OTHER_SELLING_RATE) / itemcount); // +"/" + mcobj.FREIGHT_BUYING_RATE.ToString();
                        line2.Amount = Convert.ToDecimal(Convert.ToDecimal(salesItemLineDetail2.AnyIntuitObject) * itemcount);
                        salesItemLineDetail2.ItemElementName = ItemChoiceType.UnitPrice;
                        // salesItemLineDetail.uni

                        salesItemLineDetail2.Qty = itemcount;
                        salesItemLineDetail2.QtySpecified = true;
                        //salesItemLineDetail.


                        salesItemLineDetail2.ItemRef = new ReferenceType()
                        {
                            Value = InlandId
                        };
                        line2.AnyIntuitObject = salesItemLineDetail2;

                        line2.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                        line2.DetailTypeSpecified = true;

                        // line 3
                        line3.Description = "Insurance";
                        line3.Amount = incamt;
                        line3.AmountSpecified = true;
                        SalesItemLineDetail salesItemLineDetail3 = new SalesItemLineDetail();
                        salesItemLineDetail3.AnyIntuitObject = incamt;

                        Logger.WriteWarning("Insurance " + Math.Round(Convert.ToDecimal(incamt)).ToString(), false);
                        salesItemLineDetail3.ItemElementName = ItemChoiceType.UnitPrice;
                        //  // salesItemLineDetail.uni
                        salesItemLineDetail3.Qty = 1;
                        salesItemLineDetail3.QtySpecified = true;
                        //  //salesItemLineDetail.


                        salesItemLineDetail3.ItemRef = new ReferenceType()
                        {
                            Value = InsurnceId
                        };
                        line3.AnyIntuitObject = salesItemLineDetail3;

                        line3.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                        line3.DetailTypeSpecified = true;

                        // line 4

                        line4.Description = "Courier";
                        line4.Amount = (Convert.ToDecimal(mcobj.COURIER_SELLING_RATE));
                        Logger.WriteWarning("Courier " + (Convert.ToDecimal(mcobj.COURIER_SELLING_RATE)).ToString(), false);
                        line4.AmountSpecified = true;
                        SalesItemLineDetail salesItemLineDetail4 = new SalesItemLineDetail();
                        salesItemLineDetail4.AnyIntuitObject = Convert.ToDecimal(Convert.ToDecimal(mcobj.COURIER_SELLING_RATE));
                        salesItemLineDetail4.ItemElementName = ItemChoiceType.UnitPrice;
                        //  salesItemLineDetail.uni
                        salesItemLineDetail4.Qty = 1;
                        salesItemLineDetail4.QtySpecified = true;
                        //salesItemLineDetail.


                        salesItemLineDetail4.ItemRef = new ReferenceType()
                        {
                            Value = CourierId
                        };
                        line4.AnyIntuitObject = salesItemLineDetail4;

                        line4.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                        line4.DetailTypeSpecified = true;
                        totalamtcase1 = 0;
                        totalamtcase1 = totalamtcase1 + line1.Amount;
                        totalamtcase1 = totalamtcase1 + line2.Amount;
                        lineList.Add(line2);
                        if (incamt > 0)
                        {
                            lineList.Add(line3);
                            totalamtcase1 = totalamtcase1 + line3.Amount;
                        }
                        if (Convert.ToDecimal(mcobj.COURIER_SELLING_RATE) > 0)
                        {
                            lineList.Add(line4);
                            totalamtcase1 = totalamtcase1 + line4.Amount;
                        }
                    }
                }
                else if (mcobj.isconsolidatedreport == "3" || mcobj.isconsolidatedreport == "4")
                {
                    lineList.Add(line);

                    List<CarrierChargesDTO> CarrierCharges = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_CARRIERS_RATES @QUOTATIONID",
                                       new SqlParameter("QUOTATIONID", decimal.Parse(mcobj.QUOTATIONID))).ToList();

                    List<QuotationCarrierDTO> objQuotationCarrierDTOList = new List<QuotationCarrierDTO>();
                    List<CarrierChargesDTO> objCarrierChargesDTOList = new List<CarrierChargesDTO>();
                    List<TruckingCharges> TruckingChargesList = new List<TruckingCharges>();
                    List<QuotationDetailDTO> resultQ = _context.ExecuteQuery<QuotationDetailDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ENQUIRY_DETAIL @ENQUIRYID,@QUOTATIONID",
                                       new SqlParameter("ENQUIRYID", mcobj.EnquiryID),
                                       new SqlParameter("QUOTATIONID", mcobj.QUOTATIONID)).ToList();

                    QuotationDetailDTO objQuotationDetailDTO = resultQ[0];

                    if (objQuotationDetailDTO.QuotationID > 0)
                    {
                        Logger.WriteWarning("get Quotation Carriers", false);

                        //get Quotation Carriers
                        objQuotationCarrierDTOList = _context.ExecuteQuery<QuotationCarrierDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIERS @QuotationID",
                                       new SqlParameter("QuotationID", objQuotationDetailDTO.QuotationID)).ToList();
                        if (objQuotationCarrierDTOList.Count > 0)
                        {
                            objQuotationDetailDTO.CarrierDTOList = objQuotationCarrierDTOList.ToArray();
                            foreach (QuotationCarrierDTO objQuotationCarrierDTO in objQuotationDetailDTO.CarrierDTOList)
                            {
                                //get Carrier Charges
                                objCarrierChargesDTOList = _context.ExecuteQuery<CarrierChargesDTO>("EXEC dbo.USP_LG_QUOTATION_GET_CARRIER_RATES @QuotationID,@CarrierID",
                                      new SqlParameter("QuotationID", objQuotationCarrierDTO.QuotationID),
                                      new SqlParameter("CarrierID", objQuotationCarrierDTO.CarrierID)).ToList();
                                //if (objCarrierChargesDTOList.Count > 0)
                                // {
                                //     objQuotationCarrierDTO.CarrierChargesDTOList = objCarrierChargesDTOList.ToArray();
                                //  }

                            }
                        }
                        //get Quotation Trucking Charges

                        TruckingChargesList = _context.ExecuteQuery<TruckingCharges>("EXEC dbo.USP_LG_QUOTATION_GET_TRUCKING_CHARGES @EnquiryId,@QuotationId",
                                        new SqlParameter("@EnquiryId", objQuotationDetailDTO.EnquiryID),
                                        new SqlParameter("@QuotationId", objQuotationDetailDTO.QuotationID)
                                       ).ToList();
                        //  if (TruckingChargesList.Count > 0)
                        // {
                        //    objQuotationDetailDTO.TruckingChargesList = TruckingChargesList.ToArray();
                        //}

                        Logger.WriteWarning("trucking", false);

                    }




                    decimal incamt = 0;
                    var invoiceResult = expenseresult.Where(x => x.ExpenseHead == "Insurance charges").FirstOrDefault();
                    if (invoiceResult != null)
                    {
                        incamt = Convert.ToDecimal(invoiceResult.SellingAmount);

                        Logger.WriteWarning("incamt", false);
                    }


                    if (DispatchContainerDTOList.Count > 0)
                    {
                        Logger.WriteWarning("in ", false);




                        //  foreach (DispatchContainerDTO ContainerItem in DispatchContainerDTOList)
                        // CarrierCharges = CarrierCharges.Where(x => x.RefName == "Ocean Frieght" || x.RefName == "Trucking").ToList();
                        foreach (CarrierChargesDTO itemc in CarrierCharges)
                        {
                            var dispachitem = DispatchContainerDTOList.Where(x => x.ContainerID == itemc.fkContainerID).FirstOrDefault();

                            // var itemc = CarrierCharges.Where(x => x.fkContainerID == ContainerItem.ContainerID);
                            string cno = "";
                            var c = DispatchTableList.Where(x => x.ContainerID == itemc.fkContainerID).ToList();
                            foreach (var item in c)
                            {
                                if (cno != item.CNTNo)
                                    cno = cno + item.CNTNo + ", ";
                            }

                            Logger.WriteWarning("for 2 ", false);

                            // foreach (var item1 in itemc)
                            // {
                            Line line11 = new Line();
                            //  Line line22 = new Line();
                            if (itemc.RefName == "Ocean Frieght" || itemc.RefName == "Ocean Freight")
                            {
                                line11.Description = "OCEAN FREIGHT CHARGES FOR CONTAINER # " + cno.Substring(0, cno.Length - 1) + "Exw ";
                            }
                            else
                            {
                                line11.Description = "INLAND FREIGHT CHARGES FOR CONTAINER #" + cno.Substring(0, cno.Length - 1) + "Exw ";
                            }


                            line11.Amount = Convert.ToDecimal(itemc.SellingRate * dispachitem.TotalQty);
                            line11.AmountSpecified = true;



                            SalesItemLineDetail salesItemLineDetail1 = new SalesItemLineDetail();
                            salesItemLineDetail1.ItemElementName = ItemChoiceType.UnitPrice;
                            salesItemLineDetail1.Qty = Convert.ToDecimal(dispachitem.TotalQty);
                            salesItemLineDetail1.AnyIntuitObject = Convert.ToDecimal(itemc.SellingRate);// Convert.ToDecimal(Carrieritem.SellingRate/DispatchItem.TotalQty);
                            salesItemLineDetail1.QtySpecified = true;

                            if (itemc.RefName == "Ocean Frieght" || itemc.RefName == "Ocean Freight")
                            {
                                salesItemLineDetail1.ItemRef = new ReferenceType()
                                {
                                    Value = FreightId

                                };
                            }
                            else if (itemc.RefName == "Trucking")
                            {
                                salesItemLineDetail1.ItemRef = new ReferenceType()
                                {
                                    Value = InlandId

                                };
                            }
                            line11.AnyIntuitObject = salesItemLineDetail1;
                            line11.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                            line11.DetailTypeSpecified = true;
                            //----------------------

                            // line22.Description = "INLAND FREIGHT CHARGES FOR CONTAINER #" + c.CNTNo;
                            // line22.Amount = Convert.ToDecimal(item1.SellingRate);
                            // line22.AmountSpecified = true;


                            //  SalesItemLineDetail salesItemLineDetail2 = new SalesItemLineDetail();
                            //  salesItemLineDetail2.ItemElementName = ItemChoiceType.UnitPrice;
                            //  salesItemLineDetail2.Qty = Convert.ToDecimal(ContainerItem.TotalQty);
                            //  salesItemLineDetail2.AnyIntuitObject = Convert.ToDecimal(item1.SellingRate / ContainerItem.TotalQty);
                            //  salesItemLineDetail2.QtySpecified = true;
                            //   salesItemLineDetail2.ItemRef = new ReferenceType()
                            //  {
                            //      Value = InlandId

                            //  };
                            //  line22.AnyIntuitObject = salesItemLineDetail2;
                            //  line22.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                            //  line22.DetailTypeSpecified = true;



                            //  line2.Description = InlandDesc;
                            // line2.Amount = Convert.ToDecimal(mcobj.OTHER_SELLING_RATE) - incamt;
                            //  line2.AmountSpecified = true;
                            //  SalesItemLineDetail salesItemLineDetail20 = new SalesItemLineDetail();
                            //   salesItemLineDetail20.AnyIntuitObject = Convert.ToDecimal(mcobj.OTHER_SELLING_RATE) - incamt;
                            //   salesItemLineDetail20.ItemElementName = ItemChoiceType.UnitPrice;
                            //  // salesItemLineDetail.uni
                            //   salesItemLineDetail20.Qty = Convert.ToDecimal(1);
                            //  salesItemLineDetail20.QtySpecified = true;
                            //  //salesItemLineDetail.


                            //  salesItemLineDetail20.ItemRef = new ReferenceType()
                            //  {
                            //      Value = InlandId
                            //  };
                            //  line2.AnyIntuitObject = salesItemLineDetail20;

                            //  line2.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                            // line2.DetailTypeSpecified = true;
                            lineList.Add(line11);
                            //  lineList.Add(line22);

                            // }




                            /*   foreach (DispatchContainerDTO DispatchItem in DispatchTableList)
                            {
                                  if (DispatchItem.ContainerID == ContainerItem.ContainerID && DispatchItem.SeqNo == ContainerItem.SeqNo)
                                  {
                                      Logger.WriteWarning("for e in ", false);

                                      var Carrieritem = objCarrierChargesDTOList.Where(x => x.fkContainerID == ContainerItem.ContainerID).FirstOrDefault();
                                      //   var connaame = ContainerItem.Name;
                                      var ind = Convert.ToInt32( DispatchItem.SeqNo - 1);

                                      if (ind <TruckingChargesList.Count())
                                      {

                                          Logger.WriteWarning("TruckingChargesList 1" + TruckingChargesList[ind], false);
                                          var item = TruckingChargesList[ind]; //.Where(x => x.ContainerName == ContainerItem.Name).FirstOrDefault();
                                          Logger.WriteWarning("TruckingChargesList" + TruckingChargesList[ind], false);
                                          Line line11 = new Line();
                                          Line line22 = new Line();


                                          line11.Description = "OCEAN FREIGHT CHARGES FOR CONTAINER # " + DispatchItem.CNTNo;
                                          line11.Amount = Convert.ToDecimal(Carrieritem.SellingRate);
                                          line11.AmountSpecified = true;



                                          SalesItemLineDetail salesItemLineDetail1 = new SalesItemLineDetail();
                                          salesItemLineDetail1.ItemElementName = ItemChoiceType.UnitPrice;
                                          salesItemLineDetail1.Qty = Convert.ToDecimal(ContainerItem.TotalQty);
                                          salesItemLineDetail1.AnyIntuitObject = Convert.ToDecimal(Carrieritem.SellingRate/ ContainerItem.TotalQty);// Convert.ToDecimal(Carrieritem.SellingRate/DispatchItem.TotalQty);
                                          salesItemLineDetail1.QtySpecified = true;
                                          salesItemLineDetail1.ItemRef = new ReferenceType()
                                          {
                                              Value = FreightId

                                          };
                                          line11.AnyIntuitObject = salesItemLineDetail1;
                                          line11.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                                          line11.DetailTypeSpecified = true;
                                          //----------------------

                                          line22.Description = "INLAND FREIGHT CHARGES FOR CONTAINER #" + DispatchItem.CNTNo;
                                          line22.Amount = Convert.ToDecimal(item.TrkSellingRate);
                                          line22.AmountSpecified = true;


                                          SalesItemLineDetail salesItemLineDetail2 = new SalesItemLineDetail();
                                          salesItemLineDetail2.ItemElementName = ItemChoiceType.UnitPrice;
                                          salesItemLineDetail2.Qty = Convert.ToDecimal(ContainerItem.TotalQty);
                                          salesItemLineDetail2.AnyIntuitObject = Convert.ToDecimal(item.TrkSellingRate/ ContainerItem.TotalQty);
                                          salesItemLineDetail2.QtySpecified = true;
                                          salesItemLineDetail2.ItemRef = new ReferenceType()
                                          {
                                              Value = InlandId

                                          };
                                          line22.AnyIntuitObject = salesItemLineDetail2;
                                          line22.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                                          line22.DetailTypeSpecified = true;



                                          line2.Description = InlandDesc;
                                          line2.Amount = Convert.ToDecimal(mcobj.OTHER_SELLING_RATE) - incamt;
                                          line2.AmountSpecified = true;
                                          SalesItemLineDetail salesItemLineDetail20 = new SalesItemLineDetail();
                                          salesItemLineDetail20.AnyIntuitObject = Convert.ToDecimal(mcobj.OTHER_SELLING_RATE) - incamt;
                                          salesItemLineDetail20.ItemElementName = ItemChoiceType.UnitPrice;
                                          //  // salesItemLineDetail.uni
                                          salesItemLineDetail20.Qty = Convert.ToDecimal(1);
                                          salesItemLineDetail20.QtySpecified = true;
                                          //  //salesItemLineDetail.


                                          salesItemLineDetail20.ItemRef = new ReferenceType()
                                          {
                                              Value = InlandId
                                          };
                                          line2.AnyIntuitObject = salesItemLineDetail20;

                                          line2.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                                          line2.DetailTypeSpecified = true;
                                          lineList.Add(line11);
                                          lineList.Add(line22);
                                          // lineList.Add(line2);
                                      }
                                  }
                              } */


                        }
                        line3.Description = "Insurance";
                        line3.Amount = incamt;
                        line3.AmountSpecified = true;
                        SalesItemLineDetail salesItemLineDetail3 = new SalesItemLineDetail();
                        salesItemLineDetail3.AnyIntuitObject = incamt;
                        salesItemLineDetail3.ItemElementName = ItemChoiceType.UnitPrice;
                        //  // salesItemLineDetail.uni
                        salesItemLineDetail3.Qty = 1;
                        salesItemLineDetail3.QtySpecified = true;
                        //  //salesItemLineDetail.


                        salesItemLineDetail3.ItemRef = new ReferenceType()
                        {
                            Value = InsurnceId
                        };
                        line3.AnyIntuitObject = salesItemLineDetail3;

                        line3.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                        line3.DetailTypeSpecified = true;
                        // line 4

                        line4.Description = "Courier";
                        line4.Amount = (Convert.ToDecimal(mcobj.COURIER_SELLING_RATE));
                        line4.AmountSpecified = true;
                        SalesItemLineDetail salesItemLineDetail4 = new SalesItemLineDetail();
                        salesItemLineDetail4.AnyIntuitObject = Convert.ToDecimal(Convert.ToDecimal(mcobj.COURIER_SELLING_RATE));
                        salesItemLineDetail4.ItemElementName = ItemChoiceType.UnitPrice;
                        //  salesItemLineDetail.uni
                        salesItemLineDetail4.Qty = 1;
                        salesItemLineDetail4.QtySpecified = true;
                        //salesItemLineDetail.


                        salesItemLineDetail4.ItemRef = new ReferenceType()
                        {
                            Value = CourierId
                        };
                        line4.AnyIntuitObject = salesItemLineDetail4;

                        line4.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                        line4.DetailTypeSpecified = true;






                        if (incamt > 0)
                        {
                            lineList.Add(line3);
                        }
                        if ((Convert.ToDecimal(mcobj.COURIER_SELLING_RATE)) > 0)
                        {
                            lineList.Add(line4);
                        }

                    }
                    if (mcobj.isconsolidatedreport == "4" && expenseresult.Count > 0)
                    {

                        expenseresult = expenseresult.Where(x => x.ExpenseHead != "Insurance charges" && x.ExpenseHead != "Courier Charges").ToList();

                        foreach (var item in expenseresult)
                        {
                            Line line11 = new Line();
                            line11.Description = item.ExpenseHead;
                            line11.Amount = Convert.ToDecimal(item.SellingAmount);
                            line11.AmountSpecified = true;


                            SalesItemLineDetail salesItemLineDetail1 = new SalesItemLineDetail();
                            salesItemLineDetail1.ItemElementName = ItemChoiceType.UnitPrice;
                            salesItemLineDetail1.Qty = Convert.ToDecimal(1);
                            salesItemLineDetail1.AnyIntuitObject = Convert.ToDecimal(item.SellingAmount);
                            salesItemLineDetail1.QtySpecified = true;
                            string inlandidobj = InlandId;
                            if (item.ExpenseHead == "Insurance charges")
                            {
                                inlandidobj = InsurnceId;
                            }
                            else if (item.ExpenseHead == "Courier Charges")
                            {
                                inlandidobj = CourierId;
                            }
                            else
                            {
                                inlandidobj = InlandId;
                            }
                            salesItemLineDetail1.ItemRef = new ReferenceType()
                            {

                                Value = inlandidobj


                            };
                            line11.AnyIntuitObject = salesItemLineDetail1;
                            line11.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                            line11.DetailTypeSpecified = true;
                            lineList.Add(line11);


                        }

                    }








                }


                // if (Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) > 0)
                //     lineList.Add(line2);
                // if (Convert.ToDecimal(mcobj.COURIER_SELLING_RATE) > 0)
                //     lineList.Add(line3);
                // if (Convert.ToDecimal(mcobj.OTHER_SELLING_RATE) > 0)
                //     lineList.Add(line4);
            }
            else
            {
                line.Description = desc;
                line.Amount = Convert.ToDecimal(totalprice);
                line.AmountSpecified = true;
                SalesItemLineDetail salesItemLineDetail = new SalesItemLineDetail();
                salesItemLineDetail.AnyIntuitObject = Convert.ToDecimal(unitprice); // +"/" + mcobj.FREIGHT_BUYING_RATE.ToString();
                salesItemLineDetail.ItemElementName = ItemChoiceType.UnitPrice;
                // salesItemLineDetail.uni
                salesItemLineDetail.Qty = Convert.ToDecimal(itemcount);
                salesItemLineDetail.QtySpecified = true;
                salesItemLineDetail.ItemRef = new ReferenceType()
                {

                    Value = "1"
                };
                line.AnyIntuitObject = salesItemLineDetail;

                line.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                line.DetailTypeSpecified = true;
                lineList.Add(line);
            }
            invoice.Line = lineList.ToArray();

            // Step 5: Set other properties such as Total Amount, Due Date, Email status and Transaction Date
            invoice.DueDate = Convert.ToDateTime(mcobj.ETS);
            invoice.DueDateSpecified = true;

            if (Convert.ToInt32(mcobj.isconsolidatedreport) == 1 || Convert.ToInt32(mcobj.isconsolidatedreport) == 2)
            {
                //(Math.Round(((Convert.ToDecimal(mcobj.FREIGHT_SELLING_RATE) + Convert.ToDecimal(mcobj.INLAND_SELLING_RATE) + Convert.ToDecimal(mcobj.COURIER_SELLING_RATE) + Convert.ToDecimal(mcobj.OTHER_SELLING_RATE)) / itemcount) * itemcount));

                invoice.TotalAmt = totalamtcase1;
                Logger.WriteWarning("invoice.TotalAmt case 1 " + (invoice.TotalAmt).ToString(), false);
            }
            else
            {
                invoice.TotalAmt = Convert.ToDecimal(mcobj.TOTAL_SELLING_RATE);
            }
            Logger.WriteWarning("invoice.TotalAmt " + invoice.TotalAmt.ToString(), false);
            invoice.TotalAmtSpecified = true;

            invoice.EmailStatus = EmailStatusEnum.NotSet;
            invoice.EmailStatusSpecified = true;

            //invoice.Balance = new Decimal(10.00);
            //invoice.BalanceSpecified = false;

            invoice.TxnDate = Convert.ToDateTime(mcobj.ETS); ;
            invoice.TxnDateSpecified = true;
            //invoice.TxnTaxDetail = new TxnTaxDetail()
            //{
            //    TotalTax = Convert.ToDecimal(10),
            //    TotalTaxSpecified = false,
            //};



            //  List<CustomField> culist = new List<CustomField>();
            //  CustomField c1 = new CustomField();
            //   c1.DefinitionId = "1";
            //  c1.Name = "Message on invoice";
            //  c1.Type = CustomFieldTypeEnum.StringType;
            //  c1.AnyIntuitObject = "test";

            //     culist.Add(c1);
            // invoice.CustomField = culist.ToArray();


            //Billing Address
            //PhysicalAddress billAddr = new PhysicalAddress();
            //billAddr.Line1 = "123 Main St.";
            //billAddr.Line2 = "Unit 506";
            //billAddr.City = "Brockton";
            //billAddr.CountrySubDivisionCode = "MA";
            //billAddr.Country = "United States";
            //billAddr.PostalCode = "02301";
            //billAddr.Note = "Billing Address Note";
            //invoice.BillAddr = billAddr;

            //Shipping Address
            PhysicalAddress shipAddr = new PhysicalAddress();
            shipAddr.Line1 = mcobj.PortOfDischarge.ToString();// mcobj.Destination;
            // shipAddr.Line2 = "Line2";
            // shipAddr.Line3 = "Line3";
            //shipAddr.Line4 = "Line4";
            //shipAddr.Line5 = "Line5";
            //shipAddr.City = "Waltham";
            //shipAddr.CountrySubDivisionCode = "MA";
            //shipAddr.Country = "United States";
            //shipAddr.PostalCode = "02452";
            //shipAddr.Note = "Shipping Address Note";
            invoice.ShipAddr = shipAddr;
            Logger.WriteWarning("ship via", false);


            if (mcobj.ShippingLine.Length > 30)
            {
                mcobj.ShippingLine = mcobj.ShippingLine.Substring(0, 30);
            }


            ReferenceType smf = new ReferenceType();
            smf.name = "ship via";
            smf.Value = mcobj.ShippingLine;
            // smf.
            invoice.ShipMethodRef = smf;
            if (mcobj.ContainerNumber.Length > 30)
                invoice.TrackingNum = mcobj.ContainerNumber.Substring(0, 30);
            else
                invoice.TrackingNum = mcobj.ContainerNumber;

            invoice.ShipDate = Convert.ToDateTime(mcobj.ETS);
            invoice.ShipDateSpecified = true;
            //  invoice.TrackingNum = conno.TrimEnd(',').ToString();  // mcobj.FileNo;
            // ReferenceType smff = new ReferenceType();
            //  smf.name = "Customer email";
            //  smf.Value = "solankivikas@gmail.com";
            //  invoice.CustomerRef = smff;

            // var customerQueryService = new QueryService<Customer>(context);
            // var customer1 = customerQueryService.ExecuteIdsQuery("query to get customer");
            QueryService<Customer> querySvc1 = new QueryService<Customer>(serviceContext);
            // string query = " SELECT * FROM Customer where lower(DisplayName) like =' " +  result[0].CompanyName.ToLower() +"'";
            string query = "SELECT * FROM Customer WHERE id = '" + mcobj.qbid + "'";//  and FamilyName LIKE '" + result[0].CompanyName + "%'";
            var q = querySvc1.ExecuteIdsQuery(query).ToList();
            Logger.WriteWarning("Customer id" + mcobj.qbid.ToString(), false);
            if (q.Count > 0)
            {
                // EmailAddress ee = new EmailAddress();
                // ee.Address = mcobj.Email;
                invoice.BillEmail = q[0].PrimaryEmailAddr;
            }
            MemoRef mf = new MemoRef();
            //   mf.Value = desc;
            mf.Value = "All payments by credit cards are subject to 4 % Approx.cash discount. For Check Deposit and Wire Transfer:Account Name: Miami Freight &Logistics Services, Inc.DBA, MIAMI GLOBAL LINES Bank : Bank of America Acc # : 3810 4344 6528 Wire Routing No.: 026009593 ACH Routing No.: 021200339 International SWIFT#: BOFAUS3N PAYMENT RECEIVED AFTER 7 DAYS OF VESSEL SAILING WILL ATTRACT LATE FEE $ 100, $ 150 OR MORE DEPENDS ON CARRIER AND PARTY TO BILL IS RESPONSIBLE ";
            invoice.CustomerMemo = mf;
            invoice.PrivateNote = desc;

            List<CustomField> culist = new List<CustomField>();
            CustomField c1 = new CustomField();
            c1.DefinitionId = ConfigurationManager.AppSettings["SalesRep"];
            c1.Name = "Sales Rep";
            c1.Type = CustomFieldTypeEnum.StringType;
            c1.AnyIntuitObject = Regex.Replace(mcobj.FileNo.ToString(), @"[\d-]", string.Empty);
            culist.Add(c1);
            CustomField c2 = new CustomField();
            c2.DefinitionId = ConfigurationManager.AppSettings["ETA"]; ;
            c2.Name = "ETA";
            c2.Type = CustomFieldTypeEnum.StringType;
            if (!string.IsNullOrEmpty(mcobj.ETA.ToString()))
            {
                c2.AnyIntuitObject = mcobj.ETA == null ? "" : ((DateTime)mcobj.ETA).ToString("MM/dd/yyyy");
            }
            else
            {
                c2.AnyIntuitObject = "";
            }
            culist.Add(c2);
            invoice.CustomField = culist.ToArray();
            ReferenceType TermRef = new ReferenceType();
            TermRef.Value = ConfigurationManager.AppSettings["TermsRefID"];
            invoice.SalesTermRef = TermRef;

            ReferenceType sales = new ReferenceType();
            sales.Value = Regex.Replace(mcobj.FileNo.ToString(), @"[\d-]", string.Empty);
            invoice.SalesRepRef = sales;
            return invoice;
        }

        [System.Web.Http.HttpGet]
        public ActionResult testmethod()
        {
            // https://developer.intuit.com/app/developer/qbo/docs/workflows/create-custom-fields#enable-custom-fields

            string realmId = ConfigurationManager.AppSettings["realmid"];
            string accessToken = "";
            try
            {
                Logger.WriteWarning("QB1", false);
                var obj = _context.ExecuteQuery<QBToken>("EXEC dbo.LG_QB_Access_TOKEN_GET").ToList();
                if (obj.Count > 0)
                {
                    accessToken = obj[0].AccessToken;
                    Logger.WriteWarning("accessToken" + accessToken, false);
                }
                Logger.WriteWarning("QB2", false);
                ServiceContext serviceContext = IntializeContext(realmId, accessToken);
                DataService dataService = new DataService(serviceContext);
                // Customer customer = CreateCustomer();
                // Customer customerCreated = dataService.Add<Customer>(customer);

                ProductAndServicesPrefs p = new ProductAndServicesPrefs();

                DataService commonServiceQBO = new DataService(serviceContext);

                // get all preferences
                QueryService<Preferences> prefService = new QueryService<Preferences>(serviceContext);
                Preferences pref = prefService.ExecuteIdsQuery("SELECT * FROM Preferences").First();
                /*

                                QueryService<Item> prefService1 = new QueryService<Item>(serviceContext);
                                List<Item> listpref = prefService1.ExecuteIdsQuery("SELECT * FROM Item").OrderBy(x => x.Id).ToList();
                                List<Item> listpref = prefService1.ExecuteIdsQuery("SELECT * FROM Class").OrderBy(x => x.Id).ToList();
                                foreach (var item in listpref)
                                {
                                    var parameters1 = new List<SqlParameter>()
                                {
                                    new SqlParameter("QBID", item.Id),
                                    new SqlParameter("Name", item.Name)

                                };
                                    _context.ExecuteQuery<int>(("EXEC dbo.USP_INSERTQBITEM @QBID,@Name"), parameters1.ToArray()).ToList();

                                } 
                                */
                List<CustomFieldDefinition> customFieldDefinitions = pref.SalesFormsPrefs.CustomField.ToList();
                //To retrieve PO custom field definition list use //pref.VendorAndPurchasesPrefs.POCustomField.ToList();

                //If size is 2, indicate CustomerField is enabled. The first CustomerField defintion is structure, //the second one is the actual value.
                if (customFieldDefinitions.Count() == 2)
                {

                    //loop through the 2nd list to get the name and value of custom field
                    List<CustomField> customFields = customFieldDefinitions[0].CustomField.ToList();

                    if (customFields.Count() > 0)
                    {

                        //Note - retrieving data only for one custom field for illustration, loop through the list to get data for others
                        String customFieldName = pref.SalesFormsPrefs.CustomField[0].CustomField[0].Name;

                        string customFieldDefinitionId = pref.SalesFormsPrefs.CustomField[0].CustomField[0].Name.Substring(pref.SalesFormsPrefs.CustomField[0].CustomField[0].Name.Length - 1);


                        string customFieldValue = pref.SalesFormsPrefs.CustomField[0].CustomField[0].Name;

                    }
                }



                string value = Request.Headers.GetValues("docnumber").ToList()[0];
                Logger.WriteWarning("QB3 " + value, false);
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("documentNumber", value),

                };


                Logger.WriteWarning("QB4", false);
                List<MCSReportDTO> result = _context.ExecuteQuery<MCSReportDTO>(("EXEC dbo.USP_LG_GET_MCS_BY_UNIT_Report_Detail @documentNumber"), parameters.ToArray()).ToList();
                if (string.IsNullOrEmpty(result[0].qbid))
                {
                    return AppResult(null, 0, "Company is not sync. with quickbook", EnumResult.Failed);
                    //  QueryService<Customer> querySvc1 = new QueryService<Customer>(serviceContext);
                    //  // string query = " SELECT * FROM Customer where lower(DisplayName) like =' " +  result[0].CompanyName.ToLower() +"'";

                    //string  query = "SELECT * FROM Customer WHERE GivenName LIKE '" + result[0].CompanyName + "%'";//  and FamilyName LIKE '" + result[0].CompanyName + "%'";
                    // // query = "SELECT count(*) FROM Customer";
                    //  var q = querySvc1.ExecuteIdsQuery(query).ToList();
                    //  if (q.Count > 0)
                    //  {
                    //      IEnumerable<int> objResult;
                    //      objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_UPDATEQBID @QBID,@CustomerID",
                    //         new SqlParameter("QBID", q[0].Id),
                    //         new SqlParameter("CustomerID", result[0].CustomerID == null ? "0" : result[0].CustomerID)
                    //        ).ToList();
                    //      if (string.IsNullOrEmpty(result[0].qbid))
                    //      {
                    //          result[0].qbid = q[0].Id;
                    //      }
                    //  }
                }
                Customer customer = new Customer();
                Customer customerCreated = new Customer();
                Random random = new Random();
                Logger.WriteWarning("QB6", false);

                if (string.IsNullOrEmpty(result[0].isconsolidatedreport))
                {
                    return AppResult(null, 0, "Please select invoice type for Company.", EnumResult.Failed);
                }


                if (string.IsNullOrEmpty(result[0].qbid))
                {
                    Logger.WriteWarning(result[0].CompanyName, false);
                    customer.GivenName = result[0].CompanyName + random.ToString();
                    customer.FamilyName = result[0].CompanyName + random.ToString();
                    customer.DisplayName = result[0].CompanyName + random.ToString();
                    Logger.WriteWarning("cust", false);
                    customerCreated = dataService.Add<Customer>(customer);
                    IEnumerable<int> objResult;
                    Logger.WriteWarning("cust1", false);
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_UPDATEQBID @QBID,@CustomerID",
                        new SqlParameter("QBID", customerCreated.Id),
                        new SqlParameter("CustomerID", result[0].CustomerID == null ? "0" : result[0].CustomerID)
                       ).ToList();
                }
                else
                {

                    Logger.WriteWarning("else", false);
                    customerCreated.Id = result[0].qbid.ToString();
                    customerCreated.CompanyName = result[0].QBCustomerName.ToString();
                    Logger.WriteWarning("elseend", false);
                }
                if (customerCreated.Id != "")
                {
                    //  Logger.WriteWarning("QB7", false);

                    Invoice objInvoice = CreateInvoice(realmId, customerCreated, accessToken, result[0]);

                    //  var jsonobjInvoice = new JavaScriptSerializer().Serialize(objInvoice).ToString();
                    //  string jsonobjInvoice = JsonConvert.SerializeObject(objInvoice);
                    //  Logger.WriteWarning("QB11 " + jsonobjInvoice, false);
                    Invoice addedInvoice = dataService.Add<Invoice>(objInvoice);
                    //  Logger.WriteWarning("QB12 ", false);

                    var objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_UPDATEINVOICEID @DocumentCommId,@INVOICEID",
                        new SqlParameter("DocumentCommId", result[0].DocumentCommonID == null ? "0" : result[0].DocumentCommonID),
                        new SqlParameter("INVOICEID", addedInvoice.Id)
                       ).ToList();



                }
                return AppResult(1, 1L, "Success", EnumResult.Success);
            }
            catch (Exception ex)
            {
                Logger.WriteWarning("Message" + ex.Message.ToString(), false);
                Logger.WriteWarning("Stack Trace " + ex.StackTrace.ToString(), false);
                if (!string.IsNullOrEmpty(ex.InnerException.ToString()))
                {
                    Logger.WriteWarning("InnerException" + ex.InnerException.ToString(), false);
                }
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult SearchCustomerQB()
        {
            string realmId = ConfigurationManager.AppSettings["realmid"];
            string accessToken = "";
            try
            {

                var obj = _context.ExecuteQuery<QBToken>("EXEC dbo.LG_QB_Access_TOKEN_GET").ToList();
                if (obj.Count > 0)
                {
                    accessToken = obj[0].AccessToken;
                    Logger.WriteWarning("accessToken" + accessToken, false);
                }

                ServiceContext serviceContext = IntializeContext(realmId, accessToken);
                DataService dataService = new DataService(serviceContext);
                string value = Request.Headers.GetValues("CustomerName").ToList()[0];
                QueryService<Customer> querySvc1 = new QueryService<Customer>(serviceContext);
                // string query = " SELECT * FROM Customer where lower(DisplayName) like =' " +  result[0].CompanyName.ToLower() +"'";
                string query = "SELECT * FROM Customer WHERE CompanyName LIKE '" + value + "%'";//  and FamilyName LIKE '" + result[0].CompanyName + "%'";
                var q = querySvc1.ExecuteIdsQuery(query).ToList();
                return AppResult(q, q.Count, "Success", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult LinkCustomerQB()
        {
            try
            {
                string custid = Request.Headers.GetValues("CustomerId").ToList()[0];
                string qbid = Request.Headers.GetValues("QBId").ToList()[0];
                string qbCustomerName = Request.Headers.GetValues("QBCustomerName").ToList()[0];
                IEnumerable<int> objResult;

                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_UPDATEQBID_WithName @QBID,@CustomerID,@CustomerName",
                    new SqlParameter("QBID", qbid),
                    new SqlParameter("CustomerID", custid == null ? "0" : custid),
                    new SqlParameter("CustomerName", qbCustomerName == null ? "" : qbCustomerName)
                   ).ToList();
                return AppResult(objResult, objResult.Count(), "Successfully linked", EnumResult.Success);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpGet]
        public virtual ActionResult QuckbookInvoiceReceiptnew(int id)
        {
            try
            {
                string text = base.Request.Headers.GetValues("docnumber").ToList()[0];
                List<QuckbookInvoiceReceiptnew> list = _context.ExecuteQuery<QuckbookInvoiceReceiptnew>("EXEC dbo.LG_QUICKBOOK_CUSTOMER_CREATE_NEW @CustID ,@userid", new object[2]
                {
                    new SqlParameter("CustID", 7105),
                    new SqlParameter("userid", 108)
                }).ToList();
                QuckbookInvoiceReceiptnew quckbookInvoiceReceiptnew = new QuckbookInvoiceReceiptnew();
                quckbookInvoiceReceiptnew.QBStatus = list[0].QBStatus;
                quckbookInvoiceReceiptnew.QBResult = list[0].QBResult;
                if (quckbookInvoiceReceiptnew.QBStatus == 200)
                {
                    return AppResult(list[0], 1L, quckbookInvoiceReceiptnew.QBResult, EnumResult.Success);
                }
                return AppResult(list[0], 1L, quckbookInvoiceReceiptnew.QBResult, EnumResult.Failed);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult ResetInvoice()
        {
            try
            {
                string value = Request.Headers.GetValues("docnumber").ToList()[0];
                var objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_RESETINVOICE @DocumentCommId",
                        new SqlParameter("DocumentCommId", value == null ? "0" : value)

                       ).ToList();
                return AppResult(1, 1L, "Success", EnumResult.Success);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }



        [System.Web.Http.HttpPost]
        public ActionResult SearchInvoiceQB()
        {
            string realmId = ConfigurationManager.AppSettings["realmid"];
            string accessToken = "";
            try
            {
               
                var obj = _context.ExecuteQuery<QBToken>("EXEC dbo.LG_QB_Access_TOKEN_GET").ToList();
                if (obj.Count > 0)
                {
                    accessToken = obj[0].AccessToken;
                    Logger.WriteWarning("accessToken" + accessToken, false);
                }

                ServiceContext serviceContext = IntializeContext(realmId, accessToken);
                DataService dataService = new DataService(serviceContext);
                string value = Request.Headers.GetValues("docno").ToList()[0];
                //value = "MUM24010137";
                QueryService<Invoice> querySvc1 = new QueryService<Invoice>(serviceContext);
                
                 string query = "SELECT * FROM Invoice WHERE DocNumber LIKE '" + value + "'";

                string qbstatus = string.Empty;
                string QBMaturedMonth = string.Empty;
                Invoice q =  querySvc1.ExecuteIdsQuery(query).ToList().FirstOrDefault();
                if(q !=null)
                {
                    foreach (var item in q.CustomField)
                    {
                        if(item.Name.ToLower()=="status")
                        {
                            qbstatus = item.AnyIntuitObject.ToString();
                        }
                        if (item.Name.ToUpper() == "MATURED MONTH")
                        {
                            QBMaturedMonth = item.AnyIntuitObject.ToString();

                        }

                    }

                   

                     if(qbstatus==string.Empty && QBMaturedMonth==string.Empty)
                    {
                        return AppResult(q, 1, "Status and Matured Month value are not set in QuickBook", EnumResult.Failed);
                    }
                    else
                    {
                        var objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_SETINVOICESTATUS @FileNo,@QBStatus,@QBMaturedMonth",
                        new SqlParameter("FileNo", value == null ? "0" : value),
                        new SqlParameter("QBStatus", qbstatus == string.Empty ? " " : qbstatus),
                        new SqlParameter("QBMaturedMonth", QBMaturedMonth == string.Empty ? "" : QBMaturedMonth)
                     ).ToList();
                        return AppResult(q, 1, "Success", EnumResult.Success);
                    }                  

                }

                return AppResult(q, 1, "Invoice is not genetared in Quickbook", EnumResult.Failed);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetMCSList_Sepatara(Dictionary<string, string> listParams)
        {
            try
            {

                string startdate = Convert.ToDateTime(listParams["StartBookingDate"].ToString()).ToString("yyyy/MM/dd");
                string enddate = Convert.ToDateTime(listParams["EndBookingDate"].ToString()).ToString("yyyy/MM/dd");
                Logger.WriteWarning("Start " + listParams["StartBookingDate"].ToString());
                Logger.WriteWarning("startdate " + startdate);
                Logger.WriteWarning("enddate " + enddate);
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("SiteId", listParams["SiteId"]),
                    new SqlParameter("StartBookingDate", startdate),
                    new SqlParameter("EndBookingDate", enddate),
                    new SqlParameter("DeptId", listParams["DeptId"]),
                    new SqlParameter("isinvoiceready", listParams["isinvoiceready"]),
                    new SqlParameter("SystemRefNo", listParams["SystemRefNo"]),
                    new SqlParameter("QBStatus", listParams["QBStatus"]),
                    new SqlParameter("QBMaturedMonth", listParams["QBMaturedMonth"]),
                     new SqlParameter("UserId", listParams["UserId"])
                };

                List<MCSReportDTO> result = _context.ExecuteQuery<MCSReportDTO>("EXEC dbo.USP_LG_GET_MCS_Sepatara @SiteId, @StartBookingDate, @EndBookingDate, @DeptId,@isinvoiceready,@SystemRefNo,@QBStatus,@QBMaturedMonth,@UserId", parameters.ToArray()).ToList();
                int count = result.Count;// Utility.GetParamValue(parameters, "Count", typeof(int));
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult AddComm()
        {
            try
            {
                decimal comm = 0;
                string docid = Request.Headers.GetValues("docid").ToList()[0];
                 comm = Convert.ToDecimal( Request.Headers.GetValues("comm").ToList()[0]);
               
                IEnumerable<int> objResult;

                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_Add_SepComm @docid,@comm",
                    new SqlParameter("docid", docid),
                    new SqlParameter("comm",  comm)
                    
                   ).ToList();
                return AppResult(objResult, objResult.Count(), "Successfully linked", EnumResult.Success);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }




        #endregion
    }



        public class QBToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
