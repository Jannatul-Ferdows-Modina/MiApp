using AppMGL.DAL.Helper;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Document;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Document;
using AppMGL.DTO.Operation;
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
	public class DocumentCommonController : BaseController<DocumentCommonDTO, DocumentCommonRepository, SIPL_DocumentCommon>
	{
		public DocumentCommonController(DocumentCommonRepository context)
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
				List<DocumentCommonDTO> entity = _context.ExecuteQuery<DocumentCommonDTO>(DocumentCommonQuery.GetDockReceiptList, list.ToArray()).ToList();
				int count = Utility.GetParamValue(list, "Count", typeof(int));
				return AppResult(entity, count);
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult GetDockReceipt(Dictionary<string, string> listParams)
		{
			try
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("MiamiRefNo", listParams["MiamiRefNo"]),
					new SqlParameter("SystemRefNo", listParams["SystemRefNo"]),
					new SqlParameter("BookingNo", listParams["BookingNo"]),
					new SqlParameter("SitId", listParams["SitId"])
				};
				DockReceiptDTO dockReceiptDTO = _context.ExecuteQuery<DockReceiptDTO>(DocumentCommonQuery.GetDockReceipt, list.ToArray()).SingleOrDefault();
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "DO"),
					new SqlParameter("DocumentCommonId", dockReceiptDTO.DocumentCommonId)
				};
				List<CertificationOfOriginODDTO> list3 = _context.ExecuteQuery<CertificationOfOriginODDTO>(DocumentCommonQuery.GetCommodityDetail, list2.ToArray()).ToList();
				dockReceiptDTO.CommodityDetail = list3.ToArray();
				return AppResult(dockReceiptDTO, "");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult SaveDockReceipt(DockReceiptDTO document)
		{
			try
			{
				List<SqlParameter> list = PrepareParameters<DockReceiptDTO>(document);
				int num = _context.ExecuteCommand(DocumentCommonQuery.SaveDockReceipt, list.ToArray());
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("Type", "DO"),
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
						Utility.SetParamValue(list3, "Type", "DO");
						_context.ExecuteCommand(DocumentCommonQuery.InsertCommodityDetail, list3.ToArray());
					}
				}
				return AppResult(null, "Dock Receipt is saved successfully.");
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}

		[System.Web.Http.HttpPost]
		public ActionResult saveDockRecPdf(DockReceiptDTO document)
		{
			try
			{
				LGShipmentDocsDTO lGShipmentDocsDTO = new LGShipmentDocsDTO();
				lGShipmentDocsDTO.DocumentCommonID = Convert.ToInt32(document.DocumentCommonId);
				lGShipmentDocsDTO.sdDocType = "DockReceipt";
				lGShipmentDocsDTO.SdDocName = "DockReceipt_" + document.DocumentCommonId + ".pdf";
				lGShipmentDocsDTO.SdCreatedTs = DateTime.Now;
				lGShipmentDocsDTO.SdCreatedBy = document.CreatedBy;
				lGShipmentDocsDTO.SdUpdatedTs = DateTime.Now;
				lGShipmentDocsDTO.SdUpdatedBy = document.ModifiedBy;
				List<SqlParameter> list = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsDTO);
				if (lGShipmentDocsDTO.DocumentCommonID != 0)
				{
					List<SqlParameter> list2 = PrepareParameters<LGShipmentDocsDTO>(lGShipmentDocsDTO);
					Utility.SetParamValue(list2, "DocumentCommonID", lGShipmentDocsDTO.DocumentCommonID);
					Utility.SetParamValue(list2, "sdDocType", lGShipmentDocsDTO.sdDocType);
					_context.ExecuteCommand(DocumentCommonQuery.SaveDocuments, list2.ToArray());
				}
				return AppResult(null, "Dock Receipt is saved successfully.");
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
				string reportPath = "/AppMGL.Report/DockReceiptDocument";
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("DocCommonId", exportParams["DocCommonId"]));
				string value = list[0].Value;
				List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
				list2.Add(new KeyValuePair<string, string>("MiamiRefNo", exportParams["MiamiRefNo"]));
				list2.Add(new KeyValuePair<string, string>("SystemRefNo", exportParams["SystemRefNo"]));
				list2.Add(new KeyValuePair<string, string>("BookingNo", exportParams["BookingNo"]));
				list2.Add(new KeyValuePair<string, string>("SitId", exportParams["SitId"]));
				ReportServerProxy reportServerProxy = new ReportServerProxy();
				byte[] array = reportServerProxy.Render(reportPath, list2, ReportFormat.PDF);
				if (array.Length != 0)
				{
					string text = "DockReceipt_" + value + ".pdf";
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
				string reportPath = "/AppMGL.Report/DockReceiptDocument";
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("MiamiRefNo", listParams["MiamiRefNo"]));
				list.Add(new KeyValuePair<string, string>("SystemRefNo", listParams["SystemRefNo"]));
				list.Add(new KeyValuePair<string, string>("BookingNo", listParams["BookingNo"]));
				list.Add(new KeyValuePair<string, string>("SitId", listParams["SitId"]));
				ReportServerProxy reportServerProxy = new ReportServerProxy();
				byte[] array = reportServerProxy.Render(reportPath, list, ReportFormat.PDF);
				if (array.Length != 0)
				{
					string name = "DockReceipt_" + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf";
					Attachment item = new Attachment(new MemoryStream(array), name, "application/pdf");
					MailMessage mailMessage = new MailMessage();

                    var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
                                new SqlParameter("UserID", listParams["CreatedBy"])).FirstOrDefault();
                    userDetail.UsrSmtpPassword = SecurityHelper.DecryptString(userDetail.UsrSmtpPassword, "sblw-3hn8-sqoy19");

                    mailMessage.From = new MailAddress(userDetail.UsrSmtpUsername); // MailAddress("Noreply@miamigloballines.com");

                 //   mailMessage.From = new MailAddress("Noreply@miamigloballines.com");
					List<string> list2 = null;
					list2 = new List<string>();
					if (listParams["EmailTo"].Contains(";"))
					{
						string[] array2 = listParams["EmailTo"].Split(';');
						for (int i = 0; i < array2.Length; i++)
						{
							if (array2[i] != "")
							{
								mailMessage.To.Add(new MailAddress(array2[i]));
								list2.Add(array2[i]);
							}
						}
					}
					else
					{
						mailMessage.To.Add(new MailAddress(listParams["EmailTo"]));
						list2.Add(listParams["EmailTo"]);
					}
					if (listParams.ContainsKey("EmailCc") && (listParams["EmailCc"] != null || listParams["EmailCc"] != ""))
					{
						if (listParams["EmailCc"].Contains(";"))
						{
							string[] array3 = listParams["EmailCc"].Split(';');
							for (int j = 0; j < array3.Length; j++)
							{
								if (array3[j] != "")
								{
									mailMessage.CC.Add(new MailAddress(array3[j]));
									list2.Add(array3[j]);
								}
							}
						}
						else
						{
							mailMessage.CC.Add(new MailAddress(listParams["EmailCc"]));
							list2.Add(listParams["EmailCc"]);
						}
					}
					if (listParams.ContainsKey("EmailBcc") && (listParams["EmailBcc"] != null || listParams["EmailBcc"] != ""))
					{
						if (listParams["EmailBcc"].Contains(";"))
						{
							string[] array4 = listParams["EmailBcc"].Split(';');
							for (int k = 0; k < array4.Length; k++)
							{
								if (array4[k] != "")
								{
									mailMessage.Bcc.Add(new MailAddress(array4[k]));
									list2.Add(array4[k]);
								}
							}
						}
						else
						{
							mailMessage.Bcc.Add(new MailAddress(listParams["EmailBcc"]));
							list2.Add(listParams["EmailBcc"]);
						}
					}
					foreach (string item2 in list2)
					{
						IEnumerable<int> enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_EMAIL_INSERT @EMAIL", new object[1]
						{
							new SqlParameter("EMAIL", item2)
						}).ToList();
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

		[System.Web.Http.HttpGet]
		public virtual ActionResult GetEmailIds(string id)
		{
			try
			{
				List<EmailDTO> list = _context.ExecuteQuery<EmailDTO>("EXEC dbo.USP_LG_EMAIL_GET @SEARCH_VALUE", new object[1]
				{
					new SqlParameter("SEARCH_VALUE", id)
				}).ToList();
				return AppResult(list, list.Count);
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
				string text2 = text + "\\DockReceipt";
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
		public ActionResult GetDockTemplateList(Dictionary<string, string> listParams)
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
				List<DocumentCommonDTO> entity = _context.ExecuteQuery<DocumentCommonDTO>("EXEC dbo.LG_GET_DOCKTEMPLATE_LIST @PageIndex, @PageSize, @Sort, @SitId, @DocumentStatus,@Count OUT,@FileNo", list.ToArray()).ToList();
				int count = Utility.GetParamValue(list, "Count", typeof(int));
				return AppResult(entity, count);
			}
			catch (Exception ex)
			{
				return AppResult(ex);
			}
		}
		[System.Web.Http.HttpPost]
		public ActionResult DockReceiptSearch(ListParams listParams)
		{
			try
			{

				var result = _context.ExecuteQuery<DockReceiptDTO>("EXEC dbo.SP_Search_Dock @SEARCHVALUE,@SiteId",
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
}
