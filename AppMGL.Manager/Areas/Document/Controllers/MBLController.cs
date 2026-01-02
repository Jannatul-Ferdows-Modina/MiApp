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
	public class MBLController : BaseController<DocumentCommonDTO, DocumentCommonRepository, SIPL_DocumentCommon>
	{
		public MBLController(DocumentCommonRepository context)
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
		public ActionResult GetMBL(Dictionary<string, string> listParams)
		{
			try
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("SystemRefNo", listParams["SystemRefNo"]),
					new SqlParameter("BookingNo", listParams["BookingNo"]),
					new SqlParameter("SitId", listParams["SitId"])
				};
				MBLDTO mBLDTO = _context.ExecuteQuery<MBLDTO>(DocumentCommonQuery.GetMBL, list.ToArray()).SingleOrDefault();
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "MBL"),
					new SqlParameter("DocumentCommonId", mBLDTO.DocumentCommonId)
				};
				List<CertificationOfOriginODDTO> list3 = _context.ExecuteQuery<CertificationOfOriginODDTO>(DocumentCommonQuery.GetCommodityDetail, list2.ToArray()).ToList();
				mBLDTO.CommodityDetail = list3.ToArray();
				List<SqlParameter> list4 = new List<SqlParameter>
				{
					new SqlParameter("Type", "MBL"),
					new SqlParameter("HOID", mBLDTO.HouseBLID)
				};
				List<FreightDTO> list5 = _context.ExecuteQuery<FreightDTO>(DocumentCommonQuery.GetFreightDetail, list4.ToArray()).ToList();
				mBLDTO.FreightDetail = list5.ToArray();
				return AppResult(mBLDTO, "");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult SaveMBL(MBLDTO document)
		{
			try
			{
				List<SqlParameter> list = PrepareParameters<MBLDTO>(document);
				list.Add(new SqlParameter("Type", "MBL"));
				int num = _context.ExecuteCommand(DocumentCommonQuery.SaveMBL, list.ToArray());
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "MBL"),
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
						Utility.SetParamValue(list3, "Type", "MBL");
						_context.ExecuteCommand(DocumentCommonQuery.InsertCommodityDetail, list3.ToArray());
					}
				}
				list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "MBL"),
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
						Utility.SetParamValue(list4, "Type", "MBL");
						_context.ExecuteCommand(DocumentCommonQuery.InsertFreightDetail, list4.ToArray());
					}
				}
				return AppResult(null, "MBL is saved successfully.");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult saveMBLPdf(DockReceiptDTO document)
		{
			try
			{
				LGShipmentDocsDTO lGShipmentDocsDTO = new LGShipmentDocsDTO();
				lGShipmentDocsDTO.DocumentCommonID = Convert.ToInt32(document.DocumentCommonId);
				lGShipmentDocsDTO.sdDocType = "MBL";
				lGShipmentDocsDTO.SdDocName = "MBL_" + document.DocumentCommonId + ".pdf";
				lGShipmentDocsDTO.SdCreatedTs = DateTime.Now;
				lGShipmentDocsDTO.SdUpdatedTs = DateTime.Now;
				lGShipmentDocsDTO.SdCreatedBy = document.CreatedBy;
				lGShipmentDocsDTO.SdUpdatedBy = document.ModifiedBy;
				List<SqlParameter> list = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsDTO);
				if (lGShipmentDocsDTO.DocumentCommonID != 0)
				{
					List<SqlParameter> list2 = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsDTO);
					Utility.SetParamValue(list2, "DocumentCommonID", lGShipmentDocsDTO.DocumentCommonID);
					Utility.SetParamValue(list2, "sdDocType", lGShipmentDocsDTO.sdDocType);
					_context.ExecuteCommand(DocumentCommonQuery.SaveDocuments, list2.ToArray());
				}
				return AppResult(null, "MBL Document saved successfully.");
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
				string reportPath = "/AppMGL.Report/MBLDocument";
				//List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				//list.Add(new KeyValuePair<string, string>("DocCommonId", exportParams["DocCommonId"]));
				//string value = list[0].Value;
				List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
				//list2.Add(new KeyValuePair<string, string>("MiamiRefNo", "1"));
				list2.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"]));
				list2.Add(new KeyValuePair<string, string>("BookingNo", exportParams["BookingNo"]));
				list2.Add(new KeyValuePair<string, string>("SitId", exportParams["SitId"]));


                List<SqlParameter> listV = new List<SqlParameter>
                {
                    new SqlParameter("SystemRefNo", exportParams["SystemRefNo"]),
                    new SqlParameter("BookingNo", exportParams["BookingNo"]),
                    new SqlParameter("SitId", exportParams["SitId"])
                };
                HBLDTO hBLDTO = _context.ExecuteQuery<HBLDTO>(DocumentCommonQuery.GetHBL, listV.ToArray()).SingleOrDefault();
                string value = hBLDTO.DocumentCommonId;


                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("DocCommonId", value));
                ReportServerProxy reportServerProxy = new ReportServerProxy();
				byte[] array = reportServerProxy.Render(reportPath, list2, ReportFormat.PDF);
				if (array.Length != 0)
				{
					string text = "MBL_" + value + ".pdf";
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
				string reportPath = "/AppMGL.Report/MBLDocument";
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("MiamiRefNo", listParams["MiamiRefNo"]));
				list.Add(new KeyValuePair<string, string>("SystemRefNo", listParams["SystemRefNo"]));
				list.Add(new KeyValuePair<string, string>("BookingNo", listParams["BookingNo"]));
				list.Add(new KeyValuePair<string, string>("SitId", listParams["SitId"]));
				ReportServerProxy reportServerProxy = new ReportServerProxy();
				byte[] array = reportServerProxy.Render(reportPath, list, ReportFormat.PDF);
				if (array.Length != 0)
				{
					string name = "MBL_" + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf";
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
				string text2 = text + "\\MBL";
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
