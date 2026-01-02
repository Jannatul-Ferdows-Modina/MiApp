using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Operation;
using AppMGL.Manager.Infrastructure;
using AppMGL.Manager.Infrastructure.Report;
using Newtonsoft.Json;

using System.Net;
using System.Net.Http;
using AppMGL.DAL.Helper.Logging;
using System.Data.Entity.Infrastructure;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure.Results;

using System.Net.Http.Headers;
using System.Security.Claims;

using AppMGL.Manager.Infrastructure.Helper;

namespace AppMGL.Manager.Areas.Operation.Controllers
{
    public class DocumentationController : BaseController<DocumentationListDTO, DocumentationListRepository, USP_GET_ENQUIRY_LIST_Result>
    {
        
        #region Constructor


        public DocumentationController(DocumentationListRepository context, EnquiryListRepository enquiryListRepository)
        {
            _context = context;
            BaseModule = EnumModule.Documentation;
            KeyField = "EnquiryNo";
        }

        #endregion

        #region Public Method

        [System.Web.Http.HttpPost]
        public ActionResult GetDocumentationList(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<DocumentationListDTO> result = _context.ExecuteQuery<DocumentationListDTO>("EXEC dbo.USP_LG_DOCUMENTATION_GET_LIST @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@DOS_ID,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("DOS_ID", searchCriteria["dos_Id"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("DASHBOARD_FILTER", searchCriteria["dashboardOption"]),
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
        public virtual ActionResult getDocumentationDetail(DocumentationDetailDTO objDocument)
        {
            try
            {
                List<DocumentationDetailDTO> result = _context.ExecuteQuery<DocumentationDetailDTO>("EXEC dbo.USP_LG_DOCUMENTATION_GET_DETAILS @DocumentCommonID",                                        
                                        new SqlParameter("DocumentCommonID", objDocument.DocumentCommonID)).ToList();
                DocumentationDetailDTO objDocumentationDetailDTO = result[0];               
                ////get all Next Action remarks
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                     new SqlParameter("QUOTATIONID", objDocument.QuotationID),
                     new SqlParameter("ENQUIRYID", objDocument.EnquiryID),
                     new SqlParameter("BookingID", objDocument.DocumentCommonID),
                     new SqlParameter("RefType", "Documentation")).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                if (RemarksResultList.Count > 0)
                {
                    objDocumentationDetailDTO.NextActionRemarksDTOList = RemarksResult.ToArray();
                    objDocumentationDetailDTO.LastRemarks = RemarksResultList[0].Remarks;
                    objDocumentationDetailDTO.LastRemarkDate = RemarksResultList[0].NextActionDate;
                }
                
                return AppResult(objDocumentationDetailDTO, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveDocumentationDetail(DocumentationDetailDTO objDocument)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_DOCUMENTATION_INSERT_UPDATE @DocumentCommonID, @Doc_Id, @Dos_Id, @Doc_ReceiptDate, @Doc_IS_AES_ITN_REQ, @Doc_AES_ITN, @Doc_ReceiptBLInstructionDate,@Doc_BLInstructionLineDate, @Doc_DraftBLLineDate, @Doc_DraftBLCustomerDate, @Doc_DraftBLApprovalDate, @Doc_ApprovedDraftBLToLineDate, @Doc_BLReleaseAwaitedFromLineDate, @CreatedBy,@UpdatedBy",
                new SqlParameter("DocumentCommonID", objDocument.DocumentCommonID),
                new SqlParameter("Doc_Id", objDocument.Doc_Id),
                new SqlParameter("Dos_Id", objDocument.Dos_Id ?? Convert.DBNull),
                new SqlParameter("Doc_ReceiptDate", objDocument.Doc_ReceiptDate ?? Convert.DBNull),
                new SqlParameter("Doc_IS_AES_ITN_REQ", objDocument.Doc_IS_AES_ITN_REQ ?? Convert.DBNull),
                new SqlParameter("Doc_AES_ITN", objDocument.Doc_AES_ITN ?? Convert.DBNull),
                new SqlParameter("Doc_ReceiptBLInstructionDate", objDocument.Doc_ReceiptBLInstructionDate ?? Convert.DBNull),
                new SqlParameter("Doc_BLInstructionLineDate", objDocument.Doc_BLInstructionLineDate ?? Convert.DBNull),
                new SqlParameter("Doc_DraftBLLineDate", objDocument.Doc_DraftBLLineDate ?? Convert.DBNull),
                new SqlParameter("Doc_DraftBLCustomerDate", objDocument.Doc_DraftBLCustomerDate ?? Convert.DBNull),
                new SqlParameter("Doc_DraftBLApprovalDate", objDocument.Doc_DraftBLApprovalDate ?? Convert.DBNull),
                new SqlParameter("Doc_ApprovedDraftBLToLineDate", objDocument.Doc_ApprovedDraftBLToLineDate ?? Convert.DBNull),
                new SqlParameter("Doc_BLReleaseAwaitedFromLineDate", objDocument.Doc_BLReleaseAwaitedFromLineDate ?? Convert.DBNull),                
                new SqlParameter("CreatedBy", objDocument.CreatedBy),
                new SqlParameter("UpdatedBy", objDocument.UpdatedBy)
                ).ToList();
                List<int> objList = objResult.ToList();
                int DocumentCommonID = objDocument.DocumentCommonID;
                IEnumerable<int> objDcosResult;
                if (objDocument.ShipmentDocsDTOList != null)
                {
                    foreach (ShipmentDocsDTO objShipmentDocsDTO in objDocument.ShipmentDocsDTOList)
                    {
                        objDcosResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPMENT_DOC_INSERT @DocumentCommonID,@DocName,@DocType,@CreatedBy,@IsSysGenerated",
                            new SqlParameter("DocumentCommonID", objDocument.DocumentCommonID),
                            new SqlParameter("DocName", objShipmentDocsDTO.DocName),
                            new SqlParameter("DocType", "Documentation"),
                            new SqlParameter("CreatedBy", objDocument.CreatedBy),
                            new SqlParameter("IsSysGenerated", false)).ToList();
                    }
                }


                if (objDocument.NextActionRemarks != "" && objDocument.NextActionDueDate != null)
                {
                    IEnumerable<int> objRemarksResult;
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                    new SqlParameter("RefID", objDocument.DocumentCommonID),
                    new SqlParameter("NextActionDate", objDocument.NextActionDueDate),
                    new SqlParameter("Type", "Documentation"),
                    new SqlParameter("ActivityID", 7),
                    new SqlParameter("Remarks", objDocument.NextActionRemarks)).ToList();
                }
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SendEmail(QuotationEmailData emailData)
        {
            try
            {
                //int defaultValue = 0;
                //IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_DOCUMENTATION_INSERT_UPDATE @DocumentCommonID, @Doc_Id, @Dos_Id, @Doc_ReceiptDate, @Doc_IS_AES_ITN_REQ, @Doc_AES_ITN, @Doc_ReceiptBLInstructionDate,@Doc_BLInstructionLineDate, @Doc_DraftBLLineDate, @Doc_DraftBLCustomerDate, @Doc_DraftBLApprovalDate, @Doc_ApprovedDraftBLToLineDate, @Doc_BLReleaseAwaitedFromLineDate, @CreatedBy,@UpdatedBy",
                //new SqlParameter("DocumentCommonID", emailData.DocumentCommonID),
                //new SqlParameter("Doc_Id", Convert.ToDecimal(defaultValue)),
                //new SqlParameter("Dos_Id", 7),
                //new SqlParameter("Doc_ReceiptDate", Convert.DBNull),
                //new SqlParameter("Doc_IS_AES_ITN_REQ", Convert.DBNull),
                //new SqlParameter("Doc_AES_ITN", Convert.DBNull),
                //new SqlParameter("Doc_ReceiptBLInstructionDate", Convert.DBNull),
                //new SqlParameter("Doc_BLInstructionLineDate",Convert.DBNull),
                //new SqlParameter("Doc_DraftBLLineDate", Convert.DBNull),
                //new SqlParameter("Doc_DraftBLCustomerDate",  Convert.DBNull),
                //new SqlParameter("Doc_DraftBLApprovalDate", Convert.DBNull),
                //new SqlParameter("Doc_ApprovedDraftBLToLineDate",  Convert.DBNull),
                //new SqlParameter("Doc_BLReleaseAwaitedFromLineDate",  Convert.DBNull),
                //new SqlParameter("CreatedBy", emailData.CreatedBy),
                //new SqlParameter("UpdatedBy", emailData.UpdatedBy)
                //).ToList();
                //List<int> objList = objResult.ToList();  

                //IEnumerable<int> objDcosResult;
                //if (emailData.FileAttachementDTOList != null)
                //{
                //    {
                //        foreach (FileAttachementDTO objFileAttachement in emailData.FileAttachementDTOList)
                //        {
                //            objDcosResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPMENT_DOC_INSERT @DocumentCommonID,@DocName,@DocType,@CreatedBy,@IsSysGenerated",
                //                new SqlParameter("DocumentCommonID", emailData.DocumentCommonID),
                //                new SqlParameter("DocName", objFileAttachement.FileName),
                //                new SqlParameter("DocType", "Documentation"),
                //                new SqlParameter("CreatedBy", emailData.CreatedBy),
                //                new SqlParameter("IsSysGenerated", false)).ToList();
                //        }
                //    }
                //}


                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("Noreply@miamigloballines.com");
                
                var BookingFolder = ConfigurationManager.AppSettings["BookingDocPath"];                
                string root = HttpContext.Current.Server.MapPath(BookingFolder);
                System.Net.Mail.Attachment attachment;
                DirectoryInfo dir1 = new DirectoryInfo(root);
                
                if (emailData.FileAttachementDTOList != null)
                {
                    foreach (FileAttachementDTO objFileAttachement in emailData.FileAttachementDTOList)
                    {
                        if (System.IO.File.Exists(dir1.FullName + "\\" + objFileAttachement.FileName))
                        {
                            attachment = new System.Net.Mail.Attachment(dir1.FullName + "\\" + objFileAttachement.FileName);
                            mailMessage.Attachments.Add(attachment);
                        }
                    }
                }
                
                List<string> EmailList = null;
                EmailList = new List<string>();

                //mailMessage.To.Add(new MailAddress(emailData.EmailTo));
                if (emailData.EmailTo.Contains(";"))
                {
                    string[] strUser = emailData.EmailTo.Split(';');
                    for (int i = 0; i < strUser.Length; i++)
                    {
                        if (strUser[i] != "")
                        {
                            mailMessage.To.Add(new MailAddress(strUser[i]));
                            EmailList.Add(strUser[i]);
                        }
                    }
                }
                else
                {
                    mailMessage.To.Add(new MailAddress(emailData.EmailTo));
                    EmailList.Add(emailData.EmailTo);
                }
                if (emailData.Emailcc != null)
                {
                    if (emailData.Emailcc.Contains(";"))
                    {
                        string[] strUser = emailData.Emailcc.Split(';');
                        for (int i = 0; i < strUser.Length; i++)
                        {
                            if (strUser[i] != "")
                            {
                                mailMessage.CC.Add(new MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.CC.Add(new MailAddress(emailData.Emailcc));
                        EmailList.Add(emailData.Emailcc);
                    }
                }
                if (emailData.EmailBcc != null)
                {
                    if (emailData.EmailBcc.Contains(";"))
                    {
                        string[] strUser = emailData.EmailBcc.Split(';');
                        for (int i = 0; i < strUser.Length; i++)
                        {
                            if (strUser[i] != "")
                            {
                                mailMessage.Bcc.Add(new MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.Bcc.Add(new MailAddress(emailData.EmailBcc));
                        EmailList.Add(emailData.EmailBcc);
                    }
                }
                IEnumerable<int> objEmailResult;
                foreach (string email in EmailList)
                {
                    objEmailResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_EMAIL_INSERT @EMAIL",
                                    new SqlParameter("EMAIL", email)).ToList();
                }


                mailMessage.Subject = emailData.EmailSubject;
                mailMessage.Body = emailData.EmailBody;
                mailMessage.IsBodyHtml = true;

                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                EmailHelper.Send(principal, mailMessage);
                mailMessage.Attachments.Dispose();               
                return AppResult(EmailList, 1);
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
                //IEnumerable<EmailDTO> objResult = null;
                var result = _context.ExecuteQuery<EmailDTO>("EXEC dbo.USP_LG_EMAIL_GET @SEARCH_VALUE",
                                        new SqlParameter("SEARCH_VALUE", id)).ToList();

                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            if (!Request.Content.IsMimeMultipartContent())
            {
                output.AddMessage(HttpContext.GetGlobalResourceObject("CommonTerms", "ErrFileTypeNotSupported").ToString(), true);
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, new { output });
            }

            try
            {
                var uploadFolder = ConfigurationManager.AppSettings["BookingDocPath"];

                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                var provider = new MultipartFormDataStreamProvider(root);

                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData.HasKeys())
                {
                    var fileName = GetUnescapeData(result, "DisplayName").ToString();
                    var DocumentId = GetUnescapeData(result, "DocumentCommonID").ToString();
                    var DocumentType = GetUnescapeData(result, "DocumentType").ToString();                    
                    root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["BookingDocPath"]);

                    DirectoryInfo dir = new DirectoryInfo(root);
                    
                    if (System.IO.File.Exists(dir.FullName + "\\" + fileName))
                    {
                        System.IO.File.Delete(dir.FullName + "\\" + fileName);
                    }
                    var attachedDate = DateTime.Now;
                    var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

                    uploadedFileInfo.MoveTo(root + "\\" + fileName);                    

                    output.AddMessage("File is uploaded successfully.", true);
                    return Request.CreateResponse(HttpStatusCode.OK, new { output });
                }

                output.AddMessage("Some error found, please contact administrator.", true);
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