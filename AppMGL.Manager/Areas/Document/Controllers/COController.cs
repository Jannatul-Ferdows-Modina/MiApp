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
	public class COController : BaseController<DocumentCommonDTO, DocumentCommonRepository, SIPL_DocumentCommon>
	{
		public COController(DocumentCommonRepository context)
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
					}
				};
				List<DocumentCommonDTO> entity = _context.ExecuteQuery<DocumentCommonDTO>(DocumentCommonQuery.GetDocumentList, list.ToArray()).ToList();
				int count = Utility.GetParamValue(list, "Count", typeof(int));
				return AppResult(entity, count);
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult GetCO(Dictionary<string, string> listParams)
		{
			try
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("SystemRefNo", listParams["SystemRefNo"]),
					new SqlParameter("BookingNo", listParams["BookingNo"]),
					new SqlParameter("SitId", listParams["SitId"])
				};
				CODTO cODTO = _context.ExecuteQuery<CODTO>(DocumentCommonQuery.GetCO, list.ToArray()).SingleOrDefault();
                if (cODTO != null)
                {
                    List<SqlParameter> list2 = new List<SqlParameter>
                {
                    new SqlParameter("Type", "CO"),
                    new SqlParameter("DocumentCommonId", cODTO.DocumentCommonId)
                };
                    List<CertificationOfOriginODDTO> list3 = _context.ExecuteQuery<CertificationOfOriginODDTO>(DocumentCommonQuery.GetCommodityDetail, list2.ToArray()).ToList();
                    cODTO.CommodityDetail = list3.ToArray();
                }
                    return AppResult(cODTO, "");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult SaveCO(CODTO document)
		{
			try
			{
                List<SqlParameter> list = new List<SqlParameter>();// PrepareParameters<CODTO>(document);
				list.Add(new SqlParameter("DocumentCommonId", document.DocumentCommonId));
                list.Add(new SqlParameter("ExporterId", document.ExporterId==null?0: document.ExporterId));
                list.Add(new SqlParameter("ExporterAddress", document.ExporterAddress==null?"": document.ExporterAddress));
                list.Add(new SqlParameter("DocumentNumber", document.DocumentNumber==null?"": document.DocumentNumber));
                list.Add(new SqlParameter("BLNumber", document.BLNumber==null?"": document.BLNumber));
                list.Add(new SqlParameter("FileNo", document.FileNo==null?"": document.FileNo));
                list.Add(new SqlParameter("ExportRef", document.ExportRef==null?"": document.ExportRef));
                list.Add(new SqlParameter("ConsignedToId", document.ConsignedToId==null?0: document.ConsignedToId));
				list.Add(new SqlParameter("ConsignedToAddres", document.ConsignedToAddress == null ? "" : document.ConsignedToAddress));
				list.Add(new SqlParameter("ConsignedToType", (document.ConsignedToType == null ? "" : document.ConsignedToType)));
				list.Add(new SqlParameter("IsPrintAddress", document.IsPrintAddress));
				list.Add(new SqlParameter("FwdAgentId", document.FwdAgentId==null?0: document.FwdAgentId));
                list.Add(new SqlParameter("FwdAgentAddress", document.FwdAgentAddress == null ? "" : document.FwdAgentAddress));
                list.Add(new SqlParameter("FTZNumber", document.FTZNumber==null?"": document.FTZNumber));
                list.Add(new SqlParameter("ConsigneeId", document.ConsigneeId==null?0: document.ConsigneeId));
				list.Add(new SqlParameter("ConsigneeAddress", document.ConsigneeAddress == null ? "" : document.ConsigneeAddress));
                list.Add(new SqlParameter("ExportInstruction", document.ExportInstruction==null?"": document.ExportInstruction));
                list.Add(new SqlParameter("CarriageBy", document.CarriageBy==null?"": document.CarriageBy));
                list.Add(new SqlParameter("PlaceOfReceipt", document.PlaceOfReceipt==null?"": document.PlaceOfReceipt));
                list.Add(new SqlParameter("LandingPortId", document.LandingPortId==null?"": document.LandingPortId));
                list.Add(new SqlParameter("LoadingPert", document.LoadingPert==null?"": document.LoadingPert));
                list.Add(new SqlParameter("ForeignPortId", document.ForeignPortId==null?"":document.ForeignPortId));
                list.Add(new SqlParameter("Transshipment", document.Transshipment==null?"": document.Transshipment));
                list.Add(new SqlParameter("MoveType", document.MoveType==null?"": document.MoveType));
                list.Add(new SqlParameter("Containerized", document.Containerized));
                list.Add(new SqlParameter("ExportingCarrier", document.ExportingCarrier==null?"": document.ExportingCarrier));
                list.Add(new SqlParameter("Undersigned", document.DatedAt==null?"": document.DatedAt));
				list.Add(new SqlParameter("DatedAt", document.DatedAt == null ? "" : document.DatedAt));
				list.Add(new SqlParameter("Dayof1", document.Dayof1==null?"": document.Dayof1));
                list.Add(new SqlParameter("Dayof2", document.Dayof2==null?"": document.Dayof2));
                list.Add(new SqlParameter("SignDayof1", document.SignDayof1==null?"": document.SignDayof1));
                list.Add(new SqlParameter("SignDayof2", document.SignDayof2==null?"": document.SignDayof2));
                list.Add(new SqlParameter("Chamber", document.Chamber==null?"": document.Chamber));
                list.Add(new SqlParameter("Stateof", document.Stateof==null?"": document.Stateof));
                list.Add(new SqlParameter("Secretary", document.Secretary==null?"": document.Secretary));
				list.Add(new SqlParameter("Type", "CO"));
				list.Add(new SqlParameter("AirId", document.AirId==null?0: document.AirId));
                list.Add(new SqlParameter("SitId", document.SitId==null?0: document.SitId));
                list.Add(new SqlParameter("COID", document.COID==null?0: document.COID));
                list.Add(new SqlParameter("ReturnCommonId", document.ReturnCommonId==null?0: document.ReturnCommonId));
                
                list.Add(new SqlParameter("Voyage", document.Voyage == null ? "" : document.Voyage));
                list.Add(new SqlParameter("Vessel", document.Vessel == null ? "" : document.Vessel));
                int num = _context.ExecuteCommand(DocumentCommonQuery.SaveCO, list.ToArray());
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "CO"),
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
						Utility.SetParamValue(list3, "Type", "CO");
						_context.ExecuteCommand(DocumentCommonQuery.InsertCommodityDetail, list3.ToArray());
					}
				}
				return AppResult(null, "Certification of Origin Receipt is saved successfully.");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult saveCOPdf(CODTO document)
		{
			try
			{
				LGShipmentDocsDTO lGShipmentDocsDTO = new LGShipmentDocsDTO();
				lGShipmentDocsDTO.DocumentCommonID = Convert.ToInt32(document.DocumentCommonId);
				lGShipmentDocsDTO.sdDocType = "CO";
				lGShipmentDocsDTO.SdDocName = "CO_" + document.DocumentCommonId + ".pdf";
				lGShipmentDocsDTO.SdCreatedTs = DateTime.Now;
				lGShipmentDocsDTO.SdUpdatedTs = DateTime.Now;
				//lGShipmentDocsDTO.SdCreatedBy = document.;
				//lGShipmentDocsDTO.SdUpdatedBy = document.ModifiedBy;
				List<SqlParameter> list = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsDTO);
				if (lGShipmentDocsDTO.DocumentCommonID != 0)
				{
					List<SqlParameter> list2 = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsDTO);
					Utility.SetParamValue(list2, "DocumentCommonID", lGShipmentDocsDTO.DocumentCommonID);
					Utility.SetParamValue(list2, "sdDocType", lGShipmentDocsDTO.sdDocType);
					_context.ExecuteCommand(DocumentCommonQuery.SaveDocuments, list2.ToArray());
				}
				return AppResult(null, "CO Document saved successfully.");
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
				string reportPath = "/AppMGL.Report/CODocument";
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("DocCommonId", exportParams["DocCommonId"]));
				string value = list[0].Value;
				List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
				
                
                list2.Add(new KeyValuePair<string, string>("MiamiRefNo", exportParams["SystemRefNo"]));
				list2.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"]));
				list2.Add(new KeyValuePair<string, string>("BookingNo", exportParams["BookingNo"]));
				list2.Add(new KeyValuePair<string, string>("SitId", exportParams["SitId"]));
				ReportServerProxy reportServerProxy = new ReportServerProxy();
				byte[] array = reportServerProxy.Render(reportPath, list2, ReportFormat.PDF);
				if (array.Length != 0)
				{
					string text = "CO_" + value + ".pdf";
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
				string reportPath = "/AppMGL.Report/CODocument";
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("MiamiRefNo", listParams["SystemRefNo"]));
				list.Add(new KeyValuePair<string, string>("SystemRefNo", listParams["SystemRefNo"]));
				list.Add(new KeyValuePair<string, string>("BookingNo", listParams["BookingNo"]));
				list.Add(new KeyValuePair<string, string>("SitId", listParams["SitId"]));
				ReportServerProxy reportServerProxy = new ReportServerProxy();
				byte[] array = reportServerProxy.Render(reportPath, list, ReportFormat.PDF);
				if (array.Length != 0)
				{
					string name = "CO_" + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf";
					Attachment item = new Attachment(new MemoryStream(array), name, "application/pdf");
					MailMessage mailMessage = new MailMessage();
                    var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
                              new SqlParameter("UserID", listParams["CreatedBy"])).FirstOrDefault();
                    userDetail.UsrSmtpPassword = SecurityHelper.DecryptString(userDetail.UsrSmtpPassword, "sblw-3hn8-sqoy19");

                    mailMessage.From = new MailAddress(userDetail.UsrSmtpUsername); // MailAddress("Noreply@miamigloballines.com");

                    //mailMessage.From = new MailAddress("Noreply@miamigloballines.com");
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
				string text2 = text + "\\CO";
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
	}
}
