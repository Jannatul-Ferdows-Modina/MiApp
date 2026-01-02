using AppMGL.DAL.Helper;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Document;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Document;
using AppMGL.Manager.Infrastructure;
using AppMGL.Manager.Infrastructure.Helper;
using AppMGL.Manager.Infrastructure.Report;
using AppMGL.Manager.Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AppMGL.Manager.Areas.Document.Controllers
{
	public class HBLController : BaseController<DocumentCommonDTO, DocumentCommonRepository, SIPL_DocumentCommon>
	{
		public HBLController(DocumentCommonRepository context)
		{
			_context = context;
			base.BaseModule = EnumModule.DocumentCommon;
			base.KeyField = "DocumentCommonID";
		}

		[System.Web.Http.HttpPost]
		public ActionResult GetDocumentList(Dictionary<string, string> listParams)
		{
			try
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("PageIndex", listParams["PageIndex"]),
					new SqlParameter("PageSize", listParams["PageSize"]),
					new SqlParameter("Sort", Utility.GetSort(listParams["Sort"])),
					new SqlParameter("SitId", listParams["SitId"]),
					new SqlParameter("DocumentStatus", listParams["DocumentStatus"]),
					new SqlParameter("Count", SqlDbType.Int)
					{
						Direction = ParameterDirection.Output
					},
					new SqlParameter("FileNo", listParams["FileNo"])
				};
				List<HBLDTO> entity = _context.ExecuteQuery<HBLDTO>(DocumentCommonQuery.GetDocumentList, list.ToArray()).ToList();
				int count = Utility.GetParamValue(list, "Count", typeof(int));
				return AppResult(entity, count);
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult GetHBL(Dictionary<string, string> listParams)
		{
			try
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("SystemRefNo", listParams["SystemRefNo"]),
					new SqlParameter("BookingNo", listParams["BookingNo"]),
					new SqlParameter("SitId", listParams["SitId"])
				};
				HBLDTO hBLDTO = _context.ExecuteQuery<HBLDTO>(DocumentCommonQuery.GetHBL, list.ToArray()).SingleOrDefault();
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "HBL"),
					new SqlParameter("DocumentCommonId", hBLDTO.DocumentCommonId)
				};
				List<CertificationOfOriginODDTO> list3 = _context.ExecuteQuery<CertificationOfOriginODDTO>(DocumentCommonQuery.GetCommodityDetail, list2.ToArray()).ToList();
				hBLDTO.CommodityDetail = list3.ToArray();
				List<SqlParameter> list4 = new List<SqlParameter>
				{
					new SqlParameter("Type", "HBL"),
					new SqlParameter("HOID", hBLDTO.HouseBLID)
				};
				List<FreightDTO> list5 = _context.ExecuteQuery<FreightDTO>(DocumentCommonQuery.GetFreightDetail, list4.ToArray()).ToList();
				hBLDTO.FreightDetail = list5.ToArray();
				return AppResult(hBLDTO, "");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult SaveHBL(HBLDTO document)
		{
			try
			{

        
    
    
  
   


                //List<SqlParameter> list = PrepareParameters<HBLDTO>(document);
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("DocumentCommonId", document.DocumentCommonId==null?"": document.DocumentCommonId));
                list.Add(new SqlParameter("ExporterId", document.ExporterId==null?0: document.ExporterId));
                list.Add(new SqlParameter("ExporterAddress", document.ExporterAddress==null?"": document.ExporterAddress));
                list.Add(new SqlParameter("DocumentNumber", document.DocumentNumber==null?"": document.DocumentNumber));
                list.Add(new SqlParameter("BLNumber", document.BLNumber==null?"": document.BLNumber));
                list.Add(new SqlParameter("FileNo", document.FileNo==null?"": document.FileNo));
                list.Add(new SqlParameter("ExportRef", document.ExportRef==null?"": document.ExportRef));
                list.Add(new SqlParameter("ConsignedToId", document.ConsignedToId==null?0: document.ConsignedToId));
				list.Add(new SqlParameter("ConsignedToAddress", document.ConsignedToAddress == null ? "" : document.ConsignedToAddress));
				list.Add(new SqlParameter("ConsignedToType", document.ConsignedToType == null?"": document.ConsignedToType));
                list.Add(new SqlParameter("IsPrintAddress", document.IsPrintAddress));
                list.Add(new SqlParameter("FwdAgentId", document.FwdAgentId==null?0: document.FwdAgentId));
				list.Add(new SqlParameter("FwdAgentAddress", document.FwdAgentAddress == null ? "" : document.FwdAgentAddress));
				list.Add(new SqlParameter("FTZNumber", document.FTZNumber==null?"": document.FTZNumber));
                list.Add(new SqlParameter("ConsigneeId", document.ConsigneeId==null?0: Convert.ToInt32( document.ConsigneeId)));
				list.Add(new SqlParameter("ConsigneeAddress", document.ConsigneeAddress == null ? "" : document.ConsigneeAddress));
				list.Add(new SqlParameter("ExportInstruction", document.ExportInstruction==null?"": document.ExportInstruction));
                list.Add(new SqlParameter("CarriageBy", document.CarriageBy==null?"": document.CarriageBy));
                list.Add(new SqlParameter("PlaceOfReceipt", document.PlaceOfReceipt==null?"": document.PlaceOfReceipt));
                list.Add(new SqlParameter("Vessel", document.Vessel==null?"": document.Vessel));
                list.Add(new SqlParameter("LandingPortId", document.LandingPortId==null?"": document.LandingPortId));
                list.Add(new SqlParameter("LoadingPert", document.LoadingPert==null?"": document.LoadingPert));
                list.Add(new SqlParameter("Voyage", document.Voyage==null?"": document.Voyage));
                
                list.Add(new SqlParameter("ForeignPortId", document.ForeignPortId==null?"": document.ForeignPortId));
                list.Add(new SqlParameter("Transshipment", document.Transshipment==null?"": document.Transshipment));
                list.Add(new SqlParameter("MoveType", document.MoveType==null?"": document.MoveType));
                list.Add(new SqlParameter("Place", document.Place==null?"": document.Place));
                list.Add(new SqlParameter("ExportingCarrier", document.ExportingCarrier==null?"" :document.ExportingCarrier));
                list.Add(new SqlParameter("SignDate", document.SignDate==null?DateTime.Now: document.SignDate));
                list.Add(new SqlParameter("Type", "HBL"));
                list.Add(new SqlParameter("AirId", document.AirId==null?0: document.AirId));
                list.Add(new SqlParameter("SitId", document.SitId==null?0: document.SitId));
                list.Add(new SqlParameter("HouseBLID", document.HouseBLID==null?0:document.HouseBLID));
                list.Add(new SqlParameter("ReturnCommonId", document.ReturnCommonId==null?0: document.ReturnCommonId));
				list.Add(new SqlParameter("IsHBLTemplate", document.IsHBLTemplate));

                int num = _context.ExecuteCommand(DocumentCommonQuery.SaveHBL, list.ToArray());
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "HBL"),
					new SqlParameter("DocumentCommonId", document.DocumentCommonId)
				};
				_context.ExecuteCommand(DocumentCommonQuery.DeleteAllCommodityDetail, list2.ToArray());
				CertificationOfOriginODDTO[] commodityDetail = document.CommodityDetail;
				foreach (CertificationOfOriginODDTO certificationOfOriginODDTO in commodityDetail)
				{
					if (!string.IsNullOrEmpty(certificationOfOriginODDTO.Number) || !string.IsNullOrEmpty(certificationOfOriginODDTO.Commodity))
					{
						List<SqlParameter> list3 = PrepareParameters<CertificationOfOriginODDTO>(certificationOfOriginODDTO);
						Utility.SetParamValue(list3, "DocumentCommonId", document.DocumentCommonId);
						Utility.SetParamValue(list3, "Type", "HBL");
						_context.ExecuteCommand(DocumentCommonQuery.InsertCommodityDetail, list3.ToArray());
					}
				}
				list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "HBL"),
					new SqlParameter("HOID", document.HouseBLID)
				};
				_context.ExecuteCommand(DocumentCommonQuery.DeleteAllFreightDetail, list2.ToArray());
				FreightDTO[] freightDetail = document.FreightDetail;
				foreach (FreightDTO freightDTO in freightDetail)
				{
					if (!string.IsNullOrEmpty(freightDTO.FREIGHTCharge))
					{
						List<SqlParameter> list4 = PrepareParameters<FreightDTO>(freightDTO);
						Utility.SetParamValue(list4, "HOID", document.HouseBLID);
						Utility.SetParamValue(list4, "Type", "HBL");
						_context.ExecuteCommand(DocumentCommonQuery.InsertFreightDetail, list4.ToArray());
					}
				}
				return AppResult(null, "HBL is saved successfully.");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult saveHBLPdf(HBLDTO document)
		{
			try
			{
                


                LGShipmentDocsDTO lGShipmentDocsDTO = new LGShipmentDocsDTO();
				lGShipmentDocsDTO.DocumentCommonID = Convert.ToInt32(document.DocumentCommonId);
				lGShipmentDocsDTO.sdDocType = "HBL";
				lGShipmentDocsDTO.SdDocName = "HBL_" + document.DocumentCommonId + ".pdf";
				lGShipmentDocsDTO.SdCreatedTs = DateTime.Now;
				lGShipmentDocsDTO.SdUpdatedTs = DateTime.Now;
				List<SqlParameter> list = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsDTO);
				if (lGShipmentDocsDTO.DocumentCommonID != 0)
				{
					List<SqlParameter> list2 = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsDTO);
					Utility.SetParamValue(list2, "DocumentCommonID", lGShipmentDocsDTO.DocumentCommonID);
					Utility.SetParamValue(list2, "sdDocType", lGShipmentDocsDTO.sdDocType);
					_context.ExecuteCommand(DocumentCommonQuery.SaveDocuments, list2.ToArray());
				}
                // to create blanck document by vikas solanki

                LGShipmentDocsDTO lGShipmentDocsBlankDTO = new LGShipmentDocsDTO();
                lGShipmentDocsBlankDTO.DocumentCommonID = Convert.ToInt32(document.DocumentCommonId);
                lGShipmentDocsBlankDTO.sdDocType = "HBLWithoutLogo";
                lGShipmentDocsBlankDTO.SdDocName = "HBLWithoutLogo_" + document.DocumentCommonId + ".pdf";
                lGShipmentDocsBlankDTO.SdCreatedTs = DateTime.Now;
                lGShipmentDocsBlankDTO.SdUpdatedTs = DateTime.Now;
                List<SqlParameter> listHBLBlank = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsBlankDTO);
                if (lGShipmentDocsBlankDTO.DocumentCommonID != 0)
                {
                    List<SqlParameter> list2 = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsBlankDTO);
                    Utility.SetParamValue(list2, "DocumentCommonID", lGShipmentDocsBlankDTO.DocumentCommonID);
                    Utility.SetParamValue(list2, "sdDocType", lGShipmentDocsBlankDTO.sdDocType);
                    _context.ExecuteCommand(DocumentCommonQuery.SaveDocuments, list2.ToArray());
                }

                return AppResult(null, "HBL Document saved successfully.");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public HttpResponseMessage ExportPdf(Dictionary<string, string> exportParams)
		{
			StandardJsonResult standardJsonResult = new StandardJsonResult
			{
				ResultId = Convert.ToInt32(EnumResult.Failed)
			};
			try
			{
                List<SqlParameter> listV = new List<SqlParameter>
                {
                    new SqlParameter("SystemRefNo", exportParams["SystemRefNo"]),
                    new SqlParameter("BookingNo", exportParams["BookingNo"]),
                    new SqlParameter("SitId", exportParams["SitId"])
                };
                HBLDTO hBLDTO = _context.ExecuteQuery<HBLDTO>(DocumentCommonQuery.GetHBL, listV.ToArray()).SingleOrDefault();




                string reportPath = "/AppMGL.Report/HBLDocument";
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("DocCommonId", hBLDTO.DocumentCommonId));
				string value = hBLDTO.DocumentCommonId;
				List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
				list2.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"]));
				list2.Add(new KeyValuePair<string, string>("BookingNo", exportParams["BookingNo"]));
				list2.Add(new KeyValuePair<string, string>("SitId", exportParams["SitId"]));
				ReportServerProxy reportServerProxy = new ReportServerProxy();
				byte[] array = reportServerProxy.Render(reportPath, list2, ReportFormat.PDF);
				if (array.Length != 0)
				{
					string text = "HBL_" + value + ".pdf";
					HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.OK);
					httpResponseMessage.Content = new StreamContent(new MemoryStream(array));
					httpResponseMessage.Content.Headers.Add("X-FileName", text);
					httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
					httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
					httpResponseMessage.Content.Headers.ContentDisposition.FileName = text;
					httpResponseMessage.StatusCode = HttpStatusCode.OK;
					UploadFile(text, array);
                    try
                    {
                        // create duplicate report
                        createBlankReport(exportParams, hBLDTO.DocumentCommonId);
                    }
                    catch(Exception ex)
                    {
                        Logger.WriteError(ex);
                    }


                    return httpResponseMessage;
				}
				standardJsonResult.AddMessage("System cannot generated report, please contact administrator.", clearLastMessages: true);
				return base.Request.CreateResponse(HttpStatusCode.BadRequest, new
				{
					output = standardJsonResult
				});
			}
			catch (Exception ex)
			{
				Logger.WriteError(ex);
				standardJsonResult.AddMessage(ex.Message, clearLastMessages: true);
				return base.Request.CreateResponse(HttpStatusCode.BadRequest, new
				{
					output = standardJsonResult
				});
			}
		}

        private void createBlankReport(Dictionary<string, string> exportParams,string documentid)
        {
            string reportPath = "/AppMGL.Report/HBLWithoutLogo";
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("DocCommonId", documentid));
            string value = documentid;
            List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
            list2.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"]));
            list2.Add(new KeyValuePair<string, string>("BookingNo", exportParams["BookingNo"]));
            list2.Add(new KeyValuePair<string, string>("SitId", exportParams["SitId"]));
            ReportServerProxy reportServerProxy = new ReportServerProxy();
            byte[] array = reportServerProxy.Render(reportPath, list2, ReportFormat.PDF);
            string text = "HBLWithoutLogo_" + value + ".pdf";
            UploadFile(text, array);
        }




        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportPdfBlank(Dictionary<string, string> exportParams)
        {
            StandardJsonResult standardJsonResult = new StandardJsonResult
            {
                ResultId = Convert.ToInt32(EnumResult.Failed)
            };
            try
            {
                string reportPath = "/AppMGL.Report/HBLDocument";
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("DocCommonId", exportParams["DocCommonId"]));
                string value = list[0].Value;
                List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
                list2.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"]));
                list2.Add(new KeyValuePair<string, string>("BookingNo", exportParams["BookingNo"]));
                list2.Add(new KeyValuePair<string, string>("SitId", exportParams["SitId"]));
                ReportServerProxy reportServerProxy = new ReportServerProxy();
                byte[] array = reportServerProxy.Render(reportPath, list2, ReportFormat.PDF);
                if (array.Length != 0)
                {
                    string text = "HBLBlank_" + value + ".pdf";
                    HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.OK);
                    httpResponseMessage.Content = new StreamContent(new MemoryStream(array));
                    httpResponseMessage.Content.Headers.Add("X-FileName", text);
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = text;
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                    UploadFile(text, array);
                    return httpResponseMessage;
                }
                standardJsonResult.AddMessage("System cannot generated report, please contact administrator.", clearLastMessages: true);
                return base.Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    output = standardJsonResult
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                standardJsonResult.AddMessage(ex.Message, clearLastMessages: true);
                return base.Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    output = standardJsonResult
                });
            }
        }
        [System.Web.Http.HttpPost]
		public ActionResult SendEmail(Dictionary<string, string> listParams)
		{
			try
			{
                string withoutlogo = listParams["WithoutLogo"];
                string Iswaywill  = listParams["Iswaywill"];
                string reportPath = "/AppMGL.Report/HBLDocument_WAYWILL";
                if (!string.IsNullOrEmpty(withoutlogo) && withoutlogo == "true")
                {
                    reportPath = "/AppMGL.Report/HBLWithoutLogo_WAYWILL";
                }
                if (!string.IsNullOrEmpty(Iswaywill) && Iswaywill == "true")
                {
                    Iswaywill = "yes";
                }
                else
                {
                    Iswaywill = "no";
                }
                Logger.WriteWarning("withoutlogo" + withoutlogo, false);
                Logger.WriteWarning(reportPath, false);
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("SystemRefNo", listParams["SystemRefNo"]));
				list.Add(new KeyValuePair<string, string>("BookingNo", listParams["BookingNo"]));
				list.Add(new KeyValuePair<string, string>("SitId", listParams["SitId"]));
                list.Add(new KeyValuePair<string, string>("wll", Iswaywill));

                ReportServerProxy reportServerProxy = new ReportServerProxy();
				byte[] array = reportServerProxy.Render(reportPath, list, ReportFormat.PDF);
				if (array.Length != 0)
				{
                    string name = "HBL_" + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf";
                                     
					Attachment item = new Attachment(new MemoryStream(array), name, "application/pdf");
					MailMessage mailMessage = new MailMessage();
                    var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
                                  new SqlParameter("UserID", listParams["CreatedBy"])).FirstOrDefault();
                    userDetail.UsrSmtpPassword = SecurityHelper.DecryptString(userDetail.UsrSmtpPassword, "sblw-3hn8-sqoy19");

                    mailMessage.From = new MailAddress(userDetail.UsrSmtpUsername); // MailAddress("Noreply@miamigloballines.com");

                    //	mailMessage.From = new MailAddress("Noreply@miamigloballines.com");
                    mailMessage.To.Add(listParams["EmailTo"]);
					if (listParams.ContainsKey("EmailCc"))
					{
						mailMessage.CC.Add(listParams.ContainsKey("EmailCc") ? listParams["EmailCc"] : "");
					}
					if (listParams.ContainsKey("EmailBcc"))
					{
						mailMessage.Bcc.Add(listParams["EmailBcc"] ?? "");
					}
					mailMessage.Subject = listParams["EmailSubject"];
					if (listParams.ContainsKey("EmailBody"))
					{
						mailMessage.Body = listParams["EmailBody"];
					}
					else
					{
						mailMessage.Body = "PFA";
					}
					mailMessage.IsBodyHtml = true;
					mailMessage.Attachments.Add(item);
					ClaimsPrincipal principal = base.RequestContext.Principal as ClaimsPrincipal;
					EmailHelper.Send(principal, mailMessage);
				}
				return AppResult(null, "Email has been sent successfully.");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		private void UploadFile(string fileName, byte[] fileContent)
		{
			StandardJsonResult standardJsonResult = new StandardJsonResult
			{
				ResultId = Convert.ToInt32(EnumResult.Failed)
			};
			try
			{
				string path = ConfigurationManager.AppSettings["DocumentsPath"];
				string text = HttpContext.Current.Server.MapPath(path);
				string text2 = text + "\\HBL";
				MultipartFormDataStreamProvider multipartFormDataStreamProvider = new MultipartFormDataStreamProvider(text);
				if (fileContent.Length != 0)
				{
					if (Directory.Exists(text2))
					{
						File.WriteAllBytes(text2 + "\\" + fileName, fileContent);
					}
					else
					{
						Directory.CreateDirectory(text2);
						File.WriteAllBytes(text2 + "\\" + fileName, fileContent);
					}
					standardJsonResult.AddMessage("File is uploaded successfully.", clearLastMessages: true);
				}
				else
				{
					standardJsonResult.AddMessage("Some error found, please re generate the file.", clearLastMessages: true);
				}
			}
			catch (Exception ex)
			{
				Logger.WriteError(ex);
				standardJsonResult.AddMessage(ex.Message, clearLastMessages: true);
			}
		}
		[System.Web.Http.HttpPost]
		public ActionResult GetHblTemplateList(Dictionary<string, string> listParams)
		{
			try
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("PageIndex", listParams["PageIndex"]),
					new SqlParameter("PageSize", listParams["PageSize"]),
					new SqlParameter("Sort", Utility.GetSort(listParams["Sort"])),
					new SqlParameter("SitId", listParams["SitId"]),
					new SqlParameter("DocumentStatus", listParams["DocumentStatus"]),
					new SqlParameter("Count", SqlDbType.Int)
					{
						Direction = ParameterDirection.Output
					},
					new SqlParameter("FileNo", listParams["FileNo"])
				};
				List<DocumentCommonDTO> entity = _context.ExecuteQuery<DocumentCommonDTO>("EXEC dbo.LG_GET_HBLTEMPLATE_LIST @PageIndex, @PageSize, @Sort, @SitId, @DocumentStatus,@Count OUT,@FileNo", list.ToArray()).ToList();
				int count = Utility.GetParamValue(list, "Count", typeof(int));
				return AppResult(entity, count);
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}
		[System.Web.Http.HttpPost]
		public ActionResult HBLRefrenceSearch(ListParams listParams)
		{
			try
			{

				var result = _context.ExecuteQuery<HBLDTO>("EXEC dbo.SP_Search_HBL @SEARCHVALUE,@SiteId",
					new SqlParameter("SEARCHVALUE", listParams.Filter),
					new SqlParameter("SiteId", listParams.SiteId)).ToList();
				return AppResult(result, result.Count);
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}
	}
	public class SearchItem
	{
		public int HouseBLId { get; set; }
		public int DocumentCommonId { get; set; }
		public string DocumentNumber { get; set; }
		public string BLNumber { get; set; }
		public string FileNo { get; set; }
	}
}
