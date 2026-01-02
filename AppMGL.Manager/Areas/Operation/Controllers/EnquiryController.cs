using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DAL.UDT;
using AppMGL.DAL.Helper;
using AppMGL.DTO.Operation;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;
//using Microsoft.Exchange.WebServices.Data;
using AutoMapper;

using System.Data.Entity.Infrastructure;
using AppMGL.DTO.DataManagement;
//using EAGetMail;
using System.Net.Mail;
using System.Configuration;
using System.Security.Claims;
using AppMGL.Manager.Infrastructure.Helper;
using System.Net.Http;
using AppMGL.Manager.Infrastructure.Results;
using AppMGL.Manager.Infrastructure.Report;
using System.Net;
using AppMGL.DAL.Helper.Logging;
using System.IO;
using System.Net.Http.Headers;
using EAGetMail;
using System.Data;
using ClosedXML.Excel;
using System.Text;
using EASendMail;
using OAuthResponseParser = EAGetMail.OAuthResponseParser;
using AppMGL.DTO.Report;

namespace AppMGL.Manager.Areas.Operation.Controllers
{

    public class EnquiryController : BaseController<EnquiryListDTO, EnquiryListRepository, USP_GET_ENQUIRY_LIST_Result>
    {


        #region Constructor
        public EnquiryController()
        {


        }

        public void OnSecuring(object sender, ref bool cancel)
        {

        }


        public void OnReceivingDataStream(object sender, MailInfo info, int received, int total, ref bool cancel)
        {

        }
        public EnquiryController(EnquiryListRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Enquiry;
            KeyField = "EnquiryNo";
        }

        #endregion

        #region Public Method

        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                List<EnquiryListDTO> result = _context.ExecuteQuery<EnquiryListDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LIST_New @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID,@isdraft",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("DASHBOARD_FILTER", searchCriteria["dashboardOption"]),
                        new SqlParameter("SIT_ID", listParams.SiteId),
                        new SqlParameter("isdraft", searchCriteria["isdraft"] == null ? 1 : Convert.ToInt32(searchCriteria["isdraft"].ToString()))

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
        public ActionResult ListCRM(ListParams listParams)
        {
            try
            {
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                string startdate = searchCriteria["startDate"].ToString();
                string enddate = searchCriteria["endDate"].ToString();
                List<EnquiryListDTO> result = _context.ExecuteQuery<EnquiryListDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LIST_New_CRM @PAGENO, @PAGESIZE,@OPTIONVALUE,@SEARCHVALUE,@SORTCOLUMN,@SORTORDER,@DASHBOARD_FILTER,@SIT_ID,@isdraft,@StartDate,@EndDate",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("OPTIONVALUE", string.IsNullOrWhiteSpace(searchCriteria["optionValue"]) ? (object)DBNull.Value : searchCriteria["optionValue"]),
                        new SqlParameter("SEARCHVALUE", string.IsNullOrWhiteSpace(searchCriteria["seachValue"]) ? (object)DBNull.Value : searchCriteria["seachValue"]),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("DASHBOARD_FILTER", string.IsNullOrWhiteSpace(searchCriteria["dashboardOption"]) ? (object)DBNull.Value : searchCriteria["dashboardOption"]),
                        new SqlParameter("SIT_ID", listParams.SiteId),
                        new SqlParameter("isdraft", searchCriteria["isdraft"] == null ? 1 : Convert.ToInt32(searchCriteria["isdraft"].ToString())),
                        new SqlParameter("StartDate", startdate),
                        new SqlParameter("EndDate", enddate)

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
        public virtual ActionResult GetEnquiryDetail(EnquiryDetailDTO objEnquiry)
        {
            try
            {
                List<EnquiryDetailDTO> result = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_DETAIL @ENQUIRYID,@IS_COMPLETE,@SIT_ID",
                                        new SqlParameter("ENQUIRYID", objEnquiry.EnquiryID),
                                        new SqlParameter("IS_COMPLETE", objEnquiry.IsComplete),
                                        new SqlParameter("SIT_ID", objEnquiry.SiteId)).ToList();
                EnquiryDetailDTO objEnquiryDetail = result[0];
                string RefType = "";
                if (objEnquiry.IsComplete == 0)
                {
                    RefType = "EnquiryDraft";
                }
                else
                {
                    RefType = "Enquiry";
                }
                if (result[0].EnquiryID > 0)
                {
                    //get commodity
                    List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", "Enquiry")).ToList();
                    if (CommodityDTOList.Count > 0)
                    {
                        objEnquiryDetail.CommodityDTOList = CommodityDTOList.ToArray();
                    }
                    ////get all Next Action remarks
                    int bookingid = 0;
                    int quotationID = 0;
                    IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                         new SqlParameter("QUOTATIONID", quotationID),
                         new SqlParameter("ENQUIRYID", result[0].EnquiryID),
                         new SqlParameter("BookingID", bookingid),
                         //new SqlParameter("RefType", "Enquiry")).ToList();  comment by vikas solanki on 12 Nov 2020
                         new SqlParameter("RefType", RefType)).ToList();
                    List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                    if (RemarksResultList.Count > 0)
                    {
                        objEnquiryDetail.NextActionRemarksDTOList = RemarksResult.ToArray();
                        objEnquiryDetail.LastRemarks = RemarksResultList[0].Remarks;

                        objEnquiryDetail.LastRemarkDate = RemarksResultList[0].CurrentDate.Value.Date;

                        if (RemarksResultList.Count > 1)
                        {
                            objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                        }
                        else
                        {
                            objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                        }
                    }
                    //get Container details / FCL                                     
                    List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType)).ToList();
                    if (ContainerServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                    }
                    //get Air
                    List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 7)).ToList();
                    if (AirServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                    }
                    //get Break Bulk
                    List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 5)).ToList();
                    if (BreakBulkServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                    }
                    //get LCL
                    List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 2)).ToList();
                    if (LCLServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                    }
                    //get RORO
                    List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 3)).ToList();
                    if (ROROServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                    }
                }
                return AppResult(objEnquiryDetail, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        private static Imap4Folder SearchFolder(Imap4Folder[] folders, string name)
        {
            int count = folders.Length;
            for (int i = 0; i < count; i++)
            {
                Imap4Folder folder = folders[i];
                Console.WriteLine(folder.FullPath);
                // Folder was found.
                if (String.Compare(folder.Name.ToLower(), name.ToLower()) == 0)
                    return folder;

                folder = SearchFolder(folder.SubFolders, name);
                if (folder != null)
                    return folder;
            }
            // No folder found
            return null;
        }


        //private List<EmailDetail> GetEmailsOutLook(string emailid, string password, int monthOlder, string enqno, string location)
        //{
        //    // Initialize the Exchange service

        //    List<EmailDetail> lstemail = new List<EmailDetail>();
        //    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
        //    service.Credentials = new WebCredentials(emailid, password);
        //    service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

        //    // List of folders to process
        //    List<WellKnownFolderName> folders = new List<WellKnownFolderName>
        //{
        //    WellKnownFolderName.Inbox,
        //    WellKnownFolderName.SentItems,
        //    WellKnownFolderName.DeletedItems
        //    // Add more folders if needed
        //};

        //    foreach (var folderId in folders)
        //    {
        //        // Bind to the folder
        //        Folder folder = Folder.Bind(service, folderId);
        //        int mailCount = folder.TotalCount;

        //        Console.WriteLine($"Folder: {folder.DisplayName}, Mail Count: {mailCount}");

        //        // Retrieve emails in the folder
        //        ItemView view = new ItemView(mailCount); // Fetch all emails in the folder
        //        FindItemsResults<Item> findResults = service.FindItems(folderId, view);

        //        foreach (Item item in findResults.Items)
        //        {
        //            Microsoft.Exchange.WebServices.Data.EmailMessage email = Microsoft.Exchange.WebServices.Data.EmailMessage.Bind(service, item.Id, new PropertySet(BasePropertySet.FirstClassProperties));

        //            // Display email details
        //            Console.WriteLine($"Subject: {email.Subject}");
        //            Console.WriteLine($"From: {email.From.Address}");
        //            Console.WriteLine($"Received: {email.DateTimeReceived}");
        //            Console.WriteLine("----------");
        //        }

        //        Console.WriteLine(); // Print a blank line between folders
        //    }

        //    return lstemail;
        //} 




        private List<EmailDetail> GetEmailS(string emailid, string password, int monthOlder, string enqno, string location)
        {

            List<EmailDetail> lstmail = new List<EmailDetail>();
            try
            {

                string client_id = ConfigurationManager.AppSettings["client_idemail"];
                string client_secret = ConfigurationManager.AppSettings["client_secretemail"];
                string tenant = ConfigurationManager.AppSettings["tenantemail"];
                string crmemail = ConfigurationManager.AppSettings["crmemail"];
                string EAGetEmailLicense = ConfigurationManager.AppSettings["EAGetEmailLicense"];
                string requestData =
                string.Format("client_id={0}&client_secret={1}&scope=https://graph.microsoft.com/.default&grant_type=client_credentials",
                        client_id, client_secret);
                string tokenUri = string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/token", tenant);
                string responseText = _postString(tokenUri, requestData);
                OAuthResponseParser parser = new OAuthResponseParser();
                parser.Load(responseText);
                // emailid = "CRM@miamigloballines.com";
                string officeUser = crmemail;

                MailServer oServer = new MailServer("graph.microsoft.com",
                        officeUser,
                        parser.AccessToken,
                        EAGetMail.ServerProtocol.MsGraphApi);

                oServer.AuthType = ServerAuthType.AuthXOAUTH2;
                oServer.SSLConnection = true;
                // oServer.Alias = crmemail;
                MailClient oClient = new MailClient(EAGetEmailLicense);
                oClient.Connect(oServer);
                Imap4Folder folder = new Imap4Folder();
                if (location == "Sent")

                    folder = SearchFolder(oClient.Imap4Folders, "Sent Items");
                else
                    folder = SearchFolder(oClient.Imap4Folders, "Inbox");

                if (folder == null)
                {
                    return lstmail;
                }

                oClient.SelectFolder(folder);
                oClient.GetMailInfosParam.Reset();
                oClient.GetMailInfosParam.SubjectContains = enqno.Trim();
                MailInfo[] infos = oClient.GetMailInfos();

                int i = 1;
                foreach (var email in infos)
                {

                    var ssub = email;
                    Mail oMail = oClient.GetMail(email);
                    EmailDetail ed = new EmailDetail();
                    var ccemail = "";
                    var bccemail = "";
                    ed.From = oMail.From.Address;
                    ed.To = oMail.To[0].Address.ToString();
                    if (oMail.Cc.Length > 0)
                    {
                        foreach (EAGetMail.MailAddress cc in oMail.Cc)
                        {
                            ccemail = ccemail == "" ? cc.Address : ccemail + ";" + cc.Address;
                        }
                    }

                    ed.cc = ccemail;
                    if (oMail.Bcc.Length > 0)
                    {
                        foreach (EAGetMail.MailAddress bcc in oMail.Bcc)
                        {
                            bccemail = ccemail == "" ? bcc.Address : bccemail + ";" + bcc.Address;
                        }
                    }
                    ed.bcc = bccemail;
                    ed.Body = oMail.HtmlBody;
                    ed.Subject = oMail.Subject;
                    ed.Uuid = i.ToString();
                    lstmail.Add(ed);
                    i++;
                }


                oClient.Quit();

            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }


            return lstmail;
        }
        static string _postString(string uri, string requestData)
        {
            HttpWebRequest httpRequest = WebRequest.Create(uri) as HttpWebRequest;
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";

            using (Stream requestStream = httpRequest.GetRequestStream())
            {
                byte[] requestBuffer = Encoding.UTF8.GetBytes(requestData);
                requestStream.Write(requestBuffer, 0, requestBuffer.Length);
                requestStream.Close();
            }

            try
            {
                HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string responseText = reader.ReadToEnd();

                    return responseText;
                }
            }
            catch (WebException ex)
            {
                Logger.WriteError(ex);
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        Console.WriteLine("HTTP: " + response.StatusCode);
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseText = reader.ReadToEnd();

                        }
                    }
                }

                throw;
            }
        }

        private MailServer _buildServer()
        {
            ServerAuthType authType = ServerAuthType.AuthLogin;

            authType = (ServerAuthType)0;
            //MailServer server = new MailServer("outlook.office365.com",
            //    "Noreply@miamigloballines.com",
            //    "Reply123!",
            //    true,
            //    authType,
            //    ServerProtocol.Pop3);
            MailServer server = new MailServer("localhost",
                "vimal_s390@hotmail.com",
                "Vimal1234",
                true,
                authType,
                EAGetMail.ServerProtocol.Imap4);


            // server.AuthType = ServerAuthType.AuthXOAUTH2;
            //MailServer server = new MailServer("mail.miamigloballines.com",
            //    "Noreply@miamigloballines.com",
            //    "Reply123!",
            //    true,
            //    authType,
            //    ServerProtocol.Imap4);

            //MailServer server = new MailServer("mail.sosservices.in",
            //    "solanki.vikas@sosservices.in",
            //    "August@2023",
            //    true,
            //    authType,
            //    ServerProtocol.Imap4);

            server.Port = 993; //imap
                               // server.Port = 995; // pop3
            server.SSLConnection = true;


            return server;
        }
        public void OnAuthorized(object sender, ref bool cancel)
        {

        }
        public void OnConnected(object sender, ref bool cancel)
        {

        }
        public void OnIdle(object sender, ref bool cancel)
        {

        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult GetEnquiryEmailDetail(EnquiryDetailDTO objEnquiry)
        {



            try
            {
                var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
                              new SqlParameter("UserID", objEnquiry.UpdatedBy)).FirstOrDefault();
                if (string.IsNullOrEmpty(userDetail.UsrSmtpPassword))
                {
                    return AppResult(null, 1, "Password does not exixts.", EnumResult.ValidationFailed);
                }




                userDetail.UsrSmtpPassword = SecurityHelper.DecryptString(userDetail.UsrSmtpPassword, "sblw-3hn8-sqoy19");
                // List<EmailDetail> lst = GetEmailS("qbsyncmgl@miamigloballines.com", "Hut96499", 1, objEnquiry.EnquiryNo);

                Logger.WriteWarning("UsrSmtpUsername " + userDetail.UsrSmtpUsername, false);

                Logger.WriteWarning("UsrSmtpPassword " + userDetail.UsrSmtpPassword, false);
                // List<EmailDetail> lst = GetEmailsOutLook(userDetail.UsrSmtpUsername, userDetail.UsrSmtpPassword, 1, objEnquiry.EnquiryNo, objEnquiry.location);

                List<EmailDetail> lst = GetEmailS(userDetail.UsrSmtpUsername, userDetail.UsrSmtpPassword, 1, objEnquiry.EnquiryNo, objEnquiry.location);
                return AppResult(lst, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        //public virtual ActionResult SendReplytoEmail()
        //{

        //    try
        //    {
        //        var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
        //                      new SqlParameter("UserID", objEnquiry.UpdatedBy)).FirstOrDefault();
        //        if (string.IsNullOrEmpty(userDetail.UsrSmtpPassword))
        //        {
        //            return AppResult(null, 1, "Password does not exixts.", EnumResult.ValidationFailed);
        //        }

        //        string emailAddress = "your_email@example.com";
        //        string password = "your_password";

        //        // The ID of the email you want to reply to
        //        string messageIdToReplyTo = "message-id-of-the-email";

        //        // Connect to the IMAP server
        //        using (var client = new MailClient("TryIt"))
        //        {
        //            await client.ConnectAsync("imap.example.com", 993, true); // Use appropriate IMAP server details
        //            await client.AuthenticateAsync(emailAddress, password);

        //            // Access the INBOX folder
        //            var inbox = client.Inbox;
        //            await inbox.OpenAsync(FolderAccess.ReadOnly);

        //            // Fetch the email by message ID
        //            var message = await inbox.GetMessageAsync(messageIdToReplyTo);

        //            // Create a reply message
        //            var replyMessage = new MimeMessage
        //            {
        //                From = { new MailboxAddress("Your Name", emailAddress) },
        //                To = { message.From }, // Reply to the original sender
        //                Subject = "Re: " + message.Subject,
        //                Body = new TextPart("plain")
        //                {
        //                    Text = "This is your reply text."
        //                }
        //            };

        //            // Set the "In-Reply-To" header
        //            replyMessage.InReplyTo = message.MessageId;

        //            // Send the reply via SMTP
        //            using (var smtpClient = new SmtpClient())
        //            {
        //                await smtpClient.ConnectAsync("smtp.example.com", 587, false); // Use appropriate SMTP server details
        //                await smtpClient.AuthenticateAsync(emailAddress, password);
        //                await smtpClient.SendAsync(replyMessage);
        //                await smtpClient.DisconnectAsync(true);
        //            }
        //        }



        //        return AppResult(lst, 1, "", EnumResult.Success);
        //    }
        //    catch (Exception ex)
        //    {
        //        return AppResult(ex);
        //    }

        //}

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveEnquiryAsIncompleteDraft(EnquiryDetailDTO objEnquiryDraft)
        {
            try
            {
                if (!string.IsNullOrEmpty(objEnquiryDraft.location))
                {
                    //if (objEnquiryDraft.location == "crm" && objEnquiryDraft.isapproved_enq == 1)
                    //{
                    //    IEnumerable<int> source1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CHECK_CUSTOMERCONTACT_ISAPPROVED @CompanyName", new object[1]
                    //    {

                    //new SqlParameter("CompanyName", objEnquiryDraft.CompanyName)
                    //    }).ToList();
                    //    List<int> entity = source1.ToList();
                    //    if (entity[0] == 0)
                    //    {
                    //        return AppResult(entity, 1L, "Please Approve this Contact Account as Regular Contact before Approving this Lead.", EnumResult.Failed);
                    //    }

                    //}
                }


                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_INSERT_INCOMPLETE_DRAFT @EnquiryDate,@EnquiryNo,@fkCompanyID,@BillTo,@BillToCompanyId,@ShipmentMoveDate,@PickupType,@PickupRemark,@PortOfOrigin,@PortOfDischarge,@CityOfDischarge,@CityOfOrigin,@CountryOfDischarge,@CountryOfOrigin,@StateOfDischarge,@StateOfOrigin,@NoOfContainer,@TypeOfEnquiry,@ReceivedBy,@Remarks,@Type,@Description,@Class,@UNNo,@PackingType,@HazRemarks,@ChkHAZ,@CreatedBy, @UpdatedBy,@ModifiedOn, @Hazweight,@HazVolume,@DepartmentID,@LastEnquiryNo,@ControlNo,@IsDraft,@GalRepID, @OriginDoor,@DestinationDoor,@CustomerInqNo,@OriginRailRamp,@DestinationTerminal,@LicenseType,@SiteId,@isapproved_enq,@sourceofcontact",
                new SqlParameter("EnquiryDate", objEnquiryDraft.EnquiryDate),
                new SqlParameter("EnquiryNo", objEnquiryDraft.EnquiryNo),
                new SqlParameter("fkCompanyID", objEnquiryDraft.fkCompanyID ?? Convert.DBNull),
                new SqlParameter("BillTo", objEnquiryDraft.BillTo ?? Convert.DBNull),
                new SqlParameter("BillToCompanyId", objEnquiryDraft.BillToCompanyId ?? Convert.DBNull),
                new SqlParameter("ShipmentMoveDate", objEnquiryDraft.ShipmentMoveDate ?? Convert.DBNull),
                new SqlParameter("PickupType", objEnquiryDraft.PickupType ?? Convert.DBNull),
                new SqlParameter("PickupRemark", objEnquiryDraft.PickupRemark ?? Convert.DBNull),
                new SqlParameter("PortOfOrigin", objEnquiryDraft.OriginID),
                new SqlParameter("PortOfDischarge", objEnquiryDraft.DischargeID),
                new SqlParameter("CityOfDischarge", objEnquiryDraft.DestinationCityID ?? Convert.DBNull),
                new SqlParameter("CityOfOrigin", objEnquiryDraft.OriginCityID ?? Convert.DBNull),
                new SqlParameter("CountryOfDischarge", objEnquiryDraft.DestinationCountryID ?? Convert.DBNull),
                new SqlParameter("CountryOfOrigin", objEnquiryDraft.OriginCountryID ?? Convert.DBNull),
                new SqlParameter("StateOfDischarge", objEnquiryDraft.DestinationStateID ?? Convert.DBNull),
                new SqlParameter("StateOfOrigin", objEnquiryDraft.OrignStateID ?? Convert.DBNull),
                new SqlParameter("NoOfContainer", objEnquiryDraft.NoOfContainer ?? Convert.DBNull),
                new SqlParameter("TypeOfEnquiry", Convert.ToInt32(objEnquiryDraft.TypeOfEnquiry)),
                new SqlParameter("ReceivedBy", objEnquiryDraft.ReceivedByID ?? Convert.DBNull),
                new SqlParameter("Remarks", objEnquiryDraft.Remarks ?? Convert.DBNull),
                new SqlParameter("Type", "insert"),
                new SqlParameter("Description", objEnquiryDraft.Description ?? Convert.DBNull),
                new SqlParameter("Class", objEnquiryDraft.Class ?? Convert.DBNull),
                new SqlParameter("UNNo", objEnquiryDraft.UNNo ?? Convert.DBNull),
                new SqlParameter("PackingType", objEnquiryDraft.PackingType ?? Convert.DBNull),
                new SqlParameter("HazRemarks", objEnquiryDraft.HazRemarks ?? Convert.DBNull),
                new SqlParameter("ChkHAZ", objEnquiryDraft.IsHaz ?? Convert.DBNull),
                new SqlParameter("CreatedBy", objEnquiryDraft.UserID ?? Convert.DBNull),
                new SqlParameter("UpdatedBy", objEnquiryDraft.UpdatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedOn", System.DateTime.Now.Date),
                new SqlParameter("Hazweight", objEnquiryDraft.Hazweight ?? Convert.DBNull),
                new SqlParameter("HazVolume", objEnquiryDraft.HazVolume ?? Convert.DBNull),
                new SqlParameter("DepartmentID", objEnquiryDraft.DepartmentID ?? Convert.DBNull),
                new SqlParameter("LastEnquiryNo", objEnquiryDraft.LastEnquiryNo ?? Convert.DBNull),
                new SqlParameter("ControlNo", objEnquiryDraft.EnquiryControlNo ?? Convert.DBNull),
                new SqlParameter("IsDraft", 1),
                new SqlParameter("GalRepID", objEnquiryDraft.GalRepID ?? Convert.DBNull),
                new SqlParameter("OriginDoor", objEnquiryDraft.OriginDoorID ?? Convert.DBNull),
                new SqlParameter("DestinationDoor", objEnquiryDraft.DestinationDoorID ?? Convert.DBNull),
                new SqlParameter("CustomerInqNo", objEnquiryDraft.CustomerInqNo ?? Convert.DBNull),
                new SqlParameter("OriginRailRamp", objEnquiryDraft.OrgnRailRampId ?? Convert.DBNull),
                new SqlParameter("DestinationTerminal", objEnquiryDraft.DestnTerminalId ?? Convert.DBNull),
                new SqlParameter("LicenseType", objEnquiryDraft.LicenseType ?? Convert.DBNull),
                new SqlParameter("SiteId", objEnquiryDraft.SiteId),
                new SqlParameter("isapproved_enq", objEnquiryDraft.isapproved_enq == null ? 0 : objEnquiryDraft.isapproved_enq),
                new SqlParameter("sourceofcontact", objEnquiryDraft.sourceofcontact == null ? "" : objEnquiryDraft.sourceofcontact)
                ).ToList();
                List<int> objList = objResult.ToList();
                int EnquiryId = objList[0];
                if (EnquiryId > 0)
                {
                    IEnumerable<int> objDeleteResult;
                    //delete existing enquiry Commdity details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "EnquiryDraft")).ToList();
                    if (objEnquiryDraft.CommodityDTOList != null)
                    {
                        if (objEnquiryDraft.CommodityDTOList.Count() > 0)
                        {
                            SaveEnquiryCommodityDetails(objEnquiryDraft.CommodityDTOList, EnquiryId, "Enquiry");
                        }
                    }
                    //save next action remarks
                    if (objEnquiryDraft.NextActionRemarks != "" && objEnquiryDraft.NextActionDueDate != null)
                    {
                        IEnumerable<int> objRemarksResult;
                        objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                        new SqlParameter("RefID", EnquiryId),
                        new SqlParameter("NextActionDate", objEnquiryDraft.NextActionDueDate),
                        new SqlParameter("Type", "EnquiryDraft"),
                        new SqlParameter("ActivityID", 4),
                        new SqlParameter("Remarks", objEnquiryDraft.NextActionRemarks)).ToList();
                    }

                    int modeOfService = Convert.ToInt32(objEnquiryDraft.ModeOfService);
                    //delete existing Container Service details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "EnquiryDraft")).ToList();
                    //For containers & FCL = 1
                    if (objEnquiryDraft.EnquiryContainerServiceDTOList != null)
                    {
                        if (objEnquiryDraft.EnquiryContainerServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryContainerService(objEnquiryDraft.EnquiryContainerServiceDTOList, EnquiryId, modeOfService, "EnquiryDraft", objEnquiryDraft.NoOfContainer);

                            if (modeOfService == 7) //AIR
                            {
                                if (objEnquiryDraft.EnquiryAIRServiceDTOList.Count() > 0)
                                {
                                    SaveEnquiryAIRService(objEnquiryDraft.EnquiryAIRServiceDTOList, EnquiryId, "EnquiryDraft", objEnquiryDraft.NoOfContainer);
                                }
                            }
                            if (modeOfService == 5) //Break Bulk
                            {
                                if (objEnquiryDraft.EnquiryBreakBulkServiceDTOList.Count() > 0)
                                {
                                    SaveEnquiryBreakBulkService(objEnquiryDraft.EnquiryBreakBulkServiceDTOList, EnquiryId, "EnquiryDraft", objEnquiryDraft.NoOfContainer);
                                }
                            }
                            if (modeOfService == 2) //LCL
                            {
                                if (objEnquiryDraft.EnquiryLCLServiceDTOList.Count() > 0)
                                {
                                    SaveEnquiryLCLService(objEnquiryDraft.EnquiryLCLServiceDTOList, EnquiryId, "EnquiryDraft", objEnquiryDraft.NoOfContainer);
                                }
                            }

                            if (modeOfService == 3)  //RORO Service details
                            {
                                if (objEnquiryDraft.EnquiryROROServiceDTOList.Count() > 0)
                                {
                                    SaveEnquiryROROService(objEnquiryDraft.EnquiryROROServiceDTOList, EnquiryId, "EnquiryDraft", objEnquiryDraft.NoOfContainer);
                                }
                            }
                        }
                    }
                }
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveEnquiry(EnquiryDetailDTO objEnquiryDraft)
        {
            try
            {
                //if( objEnquiryDraft.is==1 )
                // {
                //     AppResult(null, 0, "You can not edit this.", EnumResult.Failed);
                // }

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_INSERT_UPDATE_DRAFT @Id,@EnquiryDate,@EnquiryNo,@fkCompanyID,@BillTo,@BillToCompanyId,@ShipmentMoveDate,@PickupType,@PickupRemark,@PortOfOrigin,@PortOfDischarge,@CityOfDischarge,@CityOfOrigin,@CountryOfDischarge,@CountryOfOrigin,@StateOfDischarge,@StateOfOrigin,@NoOfContainer,@TypeOfEnquiry,@ReceivedBy,@Remarks,@Type,@Description,@Class,@UNNo,@PackingType,@HazRemarks,@ChkHAZ,@CreatedBy,@UpdatedBy, @ModifiedOn, @Hazweight,@HazVolume,@DepartmentID,@LastEnquiryNo,@ControlNo,@IsDraft,@GalRepID, @OriginDoor,@DestinationDoor,@CustomerInqNo,@OriginRailRamp,@DestinationTerminal,@LicenseType,@SiteId",
                new SqlParameter("Id", objEnquiryDraft.EnquiryID),
                new SqlParameter("EnquiryDate", objEnquiryDraft.EnquiryDate),
                new SqlParameter("EnquiryNo", objEnquiryDraft.EnquiryNo),
                new SqlParameter("fkCompanyID", objEnquiryDraft.fkCompanyID ?? Convert.DBNull),
                new SqlParameter("BillTo", objEnquiryDraft.BillTo ?? Convert.DBNull),
                new SqlParameter("BillToCompanyId", objEnquiryDraft.BillToCompanyId ?? Convert.DBNull),
                new SqlParameter("ShipmentMoveDate", objEnquiryDraft.ShipmentMoveDate ?? Convert.DBNull),
                new SqlParameter("PickupType", objEnquiryDraft.PickupType ?? Convert.DBNull),
                new SqlParameter("PickupRemark", objEnquiryDraft.PickupRemark ?? Convert.DBNull),
                new SqlParameter("PortOfOrigin", objEnquiryDraft.OriginID),
                new SqlParameter("PortOfDischarge", objEnquiryDraft.DischargeID),
                new SqlParameter("CityOfDischarge", objEnquiryDraft.DestinationCityID ?? Convert.DBNull),
                new SqlParameter("CityOfOrigin", objEnquiryDraft.OriginCityID ?? Convert.DBNull),
                new SqlParameter("CountryOfDischarge", objEnquiryDraft.DestinationCountryID ?? Convert.DBNull),
                new SqlParameter("CountryOfOrigin", objEnquiryDraft.OriginCountryID ?? Convert.DBNull),
                new SqlParameter("StateOfDischarge", objEnquiryDraft.DestinationStateID ?? Convert.DBNull),
                new SqlParameter("StateOfOrigin", objEnquiryDraft.OrignStateID ?? Convert.DBNull),
                new SqlParameter("NoOfContainer", objEnquiryDraft.NoOfContainer ?? Convert.DBNull),
                new SqlParameter("TypeOfEnquiry", Convert.ToInt32(objEnquiryDraft.TypeOfEnquiry)),
                new SqlParameter("ReceivedBy", objEnquiryDraft.ReceivedByID ?? Convert.DBNull),
                new SqlParameter("Remarks", objEnquiryDraft.Remarks ?? Convert.DBNull),
                new SqlParameter("Type", "insert"),
                new SqlParameter("Description", objEnquiryDraft.Description ?? Convert.DBNull),
                new SqlParameter("Class", objEnquiryDraft.Class ?? Convert.DBNull),
                new SqlParameter("UNNo", objEnquiryDraft.UNNo ?? Convert.DBNull),
                new SqlParameter("PackingType", objEnquiryDraft.PackingType ?? Convert.DBNull),
                new SqlParameter("HazRemarks", objEnquiryDraft.HazRemarks ?? Convert.DBNull),
                new SqlParameter("ChkHAZ", objEnquiryDraft.IsHaz ?? Convert.DBNull),
                new SqlParameter("CreatedBy", objEnquiryDraft.UserID ?? Convert.DBNull),
                new SqlParameter("UpdatedBy", objEnquiryDraft.UpdatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedOn", System.DateTime.Now.Date),
                new SqlParameter("Hazweight", objEnquiryDraft.Hazweight ?? Convert.DBNull),
                new SqlParameter("HazVolume", objEnquiryDraft.HazVolume ?? Convert.DBNull),
                new SqlParameter("DepartmentID", objEnquiryDraft.DepartmentID ?? Convert.DBNull),
                new SqlParameter("LastEnquiryNo", objEnquiryDraft.LastEnquiryNo ?? Convert.DBNull),
                new SqlParameter("ControlNo", objEnquiryDraft.EnquiryControlNo ?? Convert.DBNull),
                new SqlParameter("IsDraft", objEnquiryDraft.IsDraft),
                new SqlParameter("GalRepID", objEnquiryDraft.GalRepID ?? Convert.DBNull),
                new SqlParameter("OriginDoor", objEnquiryDraft.OriginDoorID ?? Convert.DBNull),
                new SqlParameter("DestinationDoor", objEnquiryDraft.DestinationDoorID ?? Convert.DBNull),
                new SqlParameter("CustomerInqNo", objEnquiryDraft.CustomerInqNo ?? Convert.DBNull),
                new SqlParameter("OriginRailRamp", objEnquiryDraft.OrgnRailRampId ?? Convert.DBNull),
                new SqlParameter("DestinationTerminal", objEnquiryDraft.DestnTerminalId ?? Convert.DBNull),
                new SqlParameter("LicenseType", objEnquiryDraft.LicenseType ?? Convert.DBNull),
                new SqlParameter("SiteId", objEnquiryDraft.SiteId)).ToList();

                List<int> objList = objResult.ToList();
                int EnquiryId = objList[0];
                if (EnquiryId > 0)
                {
                    IEnumerable<int> objDeleteResult;
                    //delete existing enquiry Commdity details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "Enquiry")).ToList();
                    if (objEnquiryDraft.CommodityDTOList.Count() > 0)
                    {
                        SaveEnquiryCommodityDetails(objEnquiryDraft.CommodityDTOList, EnquiryId, "Enquiry");
                    }
                    //save next action remarks
                    if (objEnquiryDraft.NextActionRemarks != "" && objEnquiryDraft.NextActionDueDate != null)
                    {
                        IEnumerable<int> objRemarksResult;
                        objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                        new SqlParameter("RefID", EnquiryId),
                        new SqlParameter("NextActionDate", objEnquiryDraft.NextActionDueDate),
                        new SqlParameter("Type", "Enquiry"),
                        new SqlParameter("ActivityID", 1),
                        new SqlParameter("Remarks", objEnquiryDraft.NextActionRemarks)).ToList();
                    }
                    int modeOfService = Convert.ToInt32(objEnquiryDraft.ModeOfService);
                    //delete existing Container Service details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "Enquiry")).ToList();
                    //For containers & FCL = 1
                    if (objEnquiryDraft.EnquiryContainerServiceDTOList.Count() > 0)
                    {
                        SaveEnquiryContainerService(objEnquiryDraft.EnquiryContainerServiceDTOList, EnquiryId, modeOfService, "Enquiry", objEnquiryDraft.NoOfContainer);

                        if (modeOfService == 7) //AIR
                        {
                            if (objEnquiryDraft.EnquiryAIRServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryAIRService(objEnquiryDraft.EnquiryAIRServiceDTOList, EnquiryId, "Enquiry", objEnquiryDraft.NoOfContainer);
                            }
                        }
                        if (modeOfService == 5) //Break Bulk
                        {
                            if (objEnquiryDraft.EnquiryBreakBulkServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryBreakBulkService(objEnquiryDraft.EnquiryBreakBulkServiceDTOList, EnquiryId, "Enquiry", objEnquiryDraft.NoOfContainer);
                            }
                        }
                        if (modeOfService == 2) //LCL
                        {
                            if (objEnquiryDraft.EnquiryLCLServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryLCLService(objEnquiryDraft.EnquiryLCLServiceDTOList, EnquiryId, "Enquiry", objEnquiryDraft.NoOfContainer);
                            }
                        }

                        if (modeOfService == 3)  //RORO Service details
                        {
                            if (objEnquiryDraft.EnquiryROROServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryROROService(objEnquiryDraft.EnquiryROROServiceDTOList, EnquiryId, "Enquiry", objEnquiryDraft.NoOfContainer);
                            }
                        }
                    }
                }
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteEnquiry(EnquiryListDTO objEnquiry)
        {
            try
            {
                //if (objEnquiry.IsDraft == 0)
                //{
                //    AppResult(null, 0, "You can not delete this.", EnumResult.Failed);
                //}

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_DELETE @ID, @Remarks",
                                    new SqlParameter("ID", objEnquiry.EnquiryID),
                                    new SqlParameter("Remarks", objEnquiry.Remarks)
                                    ).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetEnquiryNo(int id)
        {
            try
            {
                //long depId = 2;
                //Get Enquiry NO based on Department id;
                List<EnquiryDetailDTO> objEnquiryList = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", id)).ToList();
                EnquiryDetailDTO objEnquiryDetail = objEnquiryList[0];
                return AppResult(objEnquiryDetail, 1, "", EnumResult.Success);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetEnquiryDepartments()
        {
            try
            {
                //int TotalCount;                
                var result = _context.ExecuteQuery<LG_DEPARTMENT>("EXEC dbo.USP_LG_ENQUIRY_GET_DEPARTMENTS").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public void SaveEnquiryCommodityDetails(CommodityDTO[] objCommodityDTOList, int EnquiryID, string RefType)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (CommodityDTO objCommodityDTO in objCommodityDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_INSERT @fkCommodityId,@RefId,@RefType",
                    new SqlParameter("fkCommodityId", objCommodityDTO.CommodityId),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType)).ToList();
            }
        }

        public void SaveEnquiryContainerService(EnquiryContainerServiceDTO[] EnquiryContainerServiceDTOList, int EnquiryID, int modeOfService, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryContainerServiceDTO objContainerServiceDTO in EnquiryContainerServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_FCL_INSERT @ServiceRequiredID,@NoofContainer,@ContainerSizeID,@QTY,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", modeOfService),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("ContainerSizeID", objContainerServiceDTO.ContainerTypeID),
                    new SqlParameter("QTY", objContainerServiceDTO.Quantity),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType)).ToList();
            }
        }

        public void SaveEnquiryAIRService(EnquiryAIRServiceDTO[] EnquiryAIRServiceDTOList, int EnquiryID, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryAIRServiceDTO objAIRService in EnquiryAIRServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_AIR_INSERT @ServiceRequiredID,@NoofContainer,@PieceCount,@Weight,@TotalWt,@Length,@Width,@Height,@Volume,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", 7),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("PieceCount", objAIRService.PieceCount),
                    new SqlParameter("Weight", objAIRService.Weight),
                    new SqlParameter("TotalWt", objAIRService.TotalWt),
                    new SqlParameter("Length", objAIRService.Length),
                    new SqlParameter("Width", objAIRService.Width),
                    new SqlParameter("Height", objAIRService.Height),
                    new SqlParameter("Volume", objAIRService.Volume),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType)).ToList();
            }

        }

        public void SaveEnquiryBreakBulkService(EnquiryBreakBulkServiceDTO[] EnquiryBreakBulkServiceDTOList, int EnquiryID, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryBreakBulkServiceDTO objBBulkService in EnquiryBreakBulkServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_BREAK_BULK_INSERT @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@Weight,@Length,@Width,@Height,@Volume,@Description,@Qty,@ChkInland,@InlandRemarks,@MafiCharges",
                    new SqlParameter("ServiceRequiredID", 5),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType),
                    new SqlParameter("Weight", objBBulkService.Weight),
                    new SqlParameter("Length", objBBulkService.Length),
                    new SqlParameter("Width", objBBulkService.Width),
                    new SqlParameter("Height", objBBulkService.Height),
                    new SqlParameter("Volume", objBBulkService.Volume ?? Convert.DBNull),
                    new SqlParameter("Description", objBBulkService.Description ?? Convert.DBNull),
                    new SqlParameter("Qty", objBBulkService.Qty),
                    new SqlParameter("ChkInland", objBBulkService.ChkInland ?? Convert.DBNull),
                    new SqlParameter("InlandRemarks", objBBulkService.InlandRemarks ?? Convert.DBNull),
                    new SqlParameter("MafiCharges", objBBulkService.MafiCharges ?? Convert.DBNull)).ToList();
            }
        }
        public void SaveEnquiryLCLService(EnquiryLCLServiceDTO[] EnquiryLCLServiceDTOList, int EnquiryID, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult;
            foreach (EnquiryLCLServiceDTO objLCLService in EnquiryLCLServiceDTOList)
            {
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_LCL_INSERT @ServiceRequiredID,@NoofContainer,@PieceCount,@Weight,@TotalWt,@Length,@Width,@Height,@Volume,@RefId,@RefType",
                    new SqlParameter("ServiceRequiredID", 2),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("PieceCount", objLCLService.PieceCount),
                    new SqlParameter("Weight", objLCLService.Weight),
                    new SqlParameter("TotalWt", objLCLService.TotalWt),
                    new SqlParameter("Length", objLCLService.Length),
                    new SqlParameter("Width", objLCLService.Width),
                    new SqlParameter("Height", objLCLService.Height),
                    new SqlParameter("Volume", objLCLService.Volume ?? Convert.DBNull),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType)).ToList();
            }

        }
        public void SaveEnquiryROROService(EnquiryROROServiceDTO[] EnquiryROROServiceDTOList, int EnquiryID, string RefType, int? NoOfContainer)
        {
            //insert enquiry Commodity details
            IEnumerable<int> objResult1;
            foreach (EnquiryROROServiceDTO objROROService in EnquiryROROServiceDTOList)
            {
                objResult1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_RORO_INSERT @ServiceRequiredID,@NoofContainer,@RefId,@RefType,@NoofCrain,@Weight,@Length,@Width,@Volume,@Height,@ChkInland,@InlandRemarks,@MafiCharges",
                    new SqlParameter("ServiceRequiredID", 3),
                    new SqlParameter("NoofContainer", NoOfContainer),
                    new SqlParameter("RefId", EnquiryID),
                    new SqlParameter("RefType", RefType),
                    new SqlParameter("NoofCrain", objROROService.NoofCrain ?? Convert.DBNull),
                    new SqlParameter("Weight", objROROService.Weight),
                    new SqlParameter("Length", objROROService.Length),
                    new SqlParameter("Width", objROROService.Width),
                    new SqlParameter("Volume", objROROService.Volume),
                    new SqlParameter("Height", objROROService.Height),
                    new SqlParameter("ChkInland", objROROService.ChkInland ?? Convert.DBNull),
                    new SqlParameter("InlandRemarks", objROROService.InlandRemarks ?? Convert.DBNull),
                    new SqlParameter("MafiCharges", objROROService.MafiCharges ?? Convert.DBNull)).ToList();
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetContainerCategories()
        {
            try
            {
                var result = _context.ExecuteQuery<ContainerCategory>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_CATEGORIES").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetContainerSizes()
        {
            try
            {
                var result = _context.ExecuteQuery<ContainerTypeDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_SIZES").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        #endregion

        [System.Web.Http.HttpPost]
        public virtual ActionResult SendEmail(EmailDetail emailData)
        {
            try
            {

                List<string> EmailList = null;
                EmailList = new List<string>();
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress("Noreply@miamigloballines.com");
                //mailMessage.To.Add(new MailAddress(emailData.EmailTo));
                if (emailData.To.Contains(";"))
                {
                    string[] strUser = emailData.To.Split(';');
                    for (int i = 0; i < strUser.Length; i++)
                    {
                        if (strUser[i] != "")
                        {
                            mailMessage.To.Add(new System.Net.Mail.MailAddress(strUser[i]));
                            EmailList.Add(strUser[i]);
                        }
                    }
                }
                else
                {
                    mailMessage.To.Add(new System.Net.Mail.MailAddress(emailData.To));
                    EmailList.Add(emailData.To);
                }
                if (emailData.cc != null)
                {
                    if (emailData.cc.Contains(";"))
                    {
                        string[] strUser = emailData.cc.Split(';');
                        for (int i = 0; i < strUser.Length; i++)
                        {
                            if (strUser[i] != "")
                            {
                                mailMessage.CC.Add(new System.Net.Mail.MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.CC.Add(new System.Net.Mail.MailAddress(emailData.cc));
                        EmailList.Add(emailData.cc);
                    }
                }
                if (emailData.bcc != null)
                {
                    if (emailData.bcc.Contains(";"))
                    {
                        string[] strUser = emailData.bcc.Split(';');
                        for (int i = 0; i < strUser.Length; i++)
                        {
                            if (strUser[i] != "")
                            {
                                mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(strUser[i]));
                                EmailList.Add(strUser[i]);
                            }
                        }
                    }
                    else
                    {
                        mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(emailData.bcc));
                        EmailList.Add(emailData.bcc);
                    }
                }
                //IEnumerable<int> objEmailResult;
                //foreach (string email in EmailList)
                //{
                //    objEmailResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_EMAIL_INSERT @EMAIL",
                //                    new SqlParameter("EMAIL", email)).ToList();
                //}

                mailMessage.Subject = emailData.Subject;

                mailMessage.Body = emailData.Body.Replace("src='Images/", "src='" + ConfigurationManager.AppSettings["WebPath"] + "Images/"); ;
                mailMessage.IsBodyHtml = true;
                string shtml = "<html><head><body> <table><tr><td>" + mailMessage.Body + "</td></tr></table></body></head></html>";
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;


                // List<QuotationListDTO> result = _context.ExecuteQuery<QuotationListDTO>("EXEC dbo.USP_LG_QUOTATION_EMAIL_Content_UPDATE @QuotationID, @EmailTo,@EmailBcc,@EmailCC,@EmailBody,@EmailSubject",
                //new SqlParameter("QuotationID", emailData.QuotationID),
                //new SqlParameter("EmailTo", emailData.EmailTo),
                //new SqlParameter("EmailBcc", emailData.EmailBcc == null ? "" : emailData.EmailBcc),
                //new SqlParameter("EmailCC", emailData.Emailcc == null ? "" : emailData.Emailcc),
                //new SqlParameter("EmailBody", emailData.EmailBody),
                // new SqlParameter("EmailSubject", emailData.EmailSubject == null ? "" : emailData.EmailSubject)

                //).ToList();

                EmailHelper.Send(principal, mailMessage);

                return AppResult(EmailList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult SendEmail_EA(EmailDetail emailData)
        {
            try {
                List<string> EmailList = null;
                string officeUser = "";
                EmailList = new List<string>();
                string client_id = ConfigurationManager.AppSettings["client_idemail"];
                string client_secret = ConfigurationManager.AppSettings["client_secretemail"];
                string tenant = ConfigurationManager.AppSettings["tenantemail"];
                string crmemail = ConfigurationManager.AppSettings["crmemail"];
                string EASendEmailLicense = ConfigurationManager.AppSettings["EASendEmailLicense"];

                //var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
                //                  new SqlParameter("UserID", emailData.createdby)).FirstOrDefault();
                //if (!string.IsNullOrEmpty(userDetail.UsrSmtpPassword))
                //       {
                //        officeUser = userDetail.UsrSmtpUsername;
                //       }

                officeUser = crmemail;
                string requestData =
                    string.Format("client_id={0}&client_secret={1}&scope=https://graph.microsoft.com/.default&grant_type=client_credentials",
                        client_id, client_secret);

                string tokenUri = string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/token", tenant);
                string responseText = _postString(tokenUri, requestData);

                OAuthResponseParser parser = new OAuthResponseParser();
                parser.Load(responseText);

                var server = new SmtpServer("graph.microsoft.com");
                server.Protocol = EASendMail.ServerProtocol.MsGraphApi;
                server.User = officeUser;
                server.Password = parser.AccessToken;
                server.AuthType = SmtpAuthType.XOAUTH2;
                server.ConnectType = SmtpConnectType.ConnectSSLAuto;
                // server.Alias = crmemail;
                var mail = new SmtpMail(EASendEmailLicense);
                mail.From = officeUser;
                mail.To = emailData.To;
                mail.Cc = emailData.cc;
                mail.Bcc = emailData.bcc;
                mail.Subject = emailData.Subject;
                mail.HtmlBody = emailData.Body.Replace("src='Images/", "src='" + ConfigurationManager.AppSettings["WebPath"] + "Images/");

                var smtp = new EASendMail.SmtpClient();
                smtp.SendMail(server, mail);

                return AppResult(EmailList, 1);

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
            //try
            //{
            //    var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
            //                     new SqlParameter("UserID", emailData.createdby)).FirstOrDefault();
            //    if (string.IsNullOrEmpty(userDetail.UsrSmtpPassword))
            //    {
            //        return AppResult(null, 1, "Password does not exixts.", EnumResult.ValidationFailed);
            //    }

            //    userDetail.UsrSmtpPassword = SecurityHelper.DecryptString(userDetail.UsrSmtpPassword, "sblw-3hn8-sqoy19");

            //    List<string> EmailList = null;
            //    EmailList = new List<string>();
            //    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            //    mailMessage.From = new System.Net.Mail.MailAddress(userDetail.UsrSmtpUsername);

            //    if (emailData.To.Contains(";"))
            //    {
            //        string[] strUser = emailData.To.Split(';');
            //        for (int i = 0; i < strUser.Length; i++)
            //        {
            //            if (strUser[i] != "")
            //            {
            //                mailMessage.To.Add(new System.Net.Mail.MailAddress(strUser[i]));
            //                EmailList.Add(strUser[i]);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        mailMessage.To.Add(new System.Net.Mail.MailAddress(emailData.To));
            //        EmailList.Add(emailData.To);
            //    }
            //    if (emailData.cc != null && emailData.cc != "")
            //    {
            //        if (emailData.cc.Contains(";"))
            //        {
            //            string[] strUser = emailData.cc.Split(';');
            //            for (int i = 0; i < strUser.Length; i++)
            //            {
            //                if (strUser[i] != "")
            //                {
            //                    mailMessage.CC.Add(new System.Net.Mail.MailAddress(strUser[i]));
            //                    EmailList.Add(strUser[i]);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            mailMessage.CC.Add(new System.Net.Mail.MailAddress(emailData.cc));
            //            EmailList.Add(emailData.cc);
            //        }
            //    }
            //    if (emailData.bcc != null && emailData.bcc != "")
            //    {
            //        if (emailData.bcc.Contains(";"))
            //        {
            //            string[] strUser = emailData.bcc.Split(';');
            //            for (int i = 0; i < strUser.Length; i++)
            //            {
            //                if (strUser[i] != "")
            //                {
            //                    mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(strUser[i]));
            //                    EmailList.Add(strUser[i]);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(emailData.bcc));
            //            EmailList.Add(emailData.bcc);
            //        }
            //    }

            //    mailMessage.Subject = emailData.Subject;

            //    mailMessage.Body = emailData.Body.Replace("src='Images/", "src='" + ConfigurationManager.AppSettings["WebPath"] + "Images/"); 
            //    mailMessage.IsBodyHtml = true;
            //    string shtml = "<html><head><body> <table><tr><td>" + mailMessage.Body + "</td></tr></table></body></head></html>";
            //    ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            //    EmailHelper.Send(principal, mailMessage);

            //    return AppResult(EmailList, 1);
            //}
            //catch (Exception ex)
            //{
            //    return AppResult(ex);
            //}
        }

        public ActionResult ChangeSite(EnquiryDetailDTO objEnquiry)
        {
            try
            {
                string oldEnqNo = objEnquiry.EnquiryID.ToString();
                string newEnqNo = "";
                List<string> d = null;
                // objEnquiry.IsComplete = 0;


                List<EnquiryDetailDTO> result = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_DETAIL @ENQUIRYID,@IS_COMPLETE,@SIT_ID",
                                      new SqlParameter("ENQUIRYID", objEnquiry.EnquiryID),
                                      new SqlParameter("IS_COMPLETE", objEnquiry.IsComplete),
                                      new SqlParameter("SIT_ID", objEnquiry.SiteId)).ToList();
                EnquiryDetailDTO objEnquiryDetail = result[0];
                string RefType = "";
                if (objEnquiry.IsComplete == 0)
                {
                    RefType = "EnquiryDraft";
                }
                else
                {
                    RefType = "Enquiry";
                }
                if (result[0].EnquiryID > 0)
                {
                    //get commodity
                    List<CommodityDTO> CommodityDTOList = _context.ExecuteQuery<CommodityDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_COMMODITY @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", "Enquiry")).ToList();
                    if (CommodityDTOList.Count > 0)
                    {
                        objEnquiryDetail.CommodityDTOList = CommodityDTOList.ToArray();
                    }
                    ////get all Next Action remarks
                    int bookingid = 0;
                    int quotationID = 0;
                    IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.USP_LG_QUOTATION_GET_ALL_ACTION_REMARKS @QUOTATIONID,@ENQUIRYID,@BookingID,@RefType",
                         new SqlParameter("QUOTATIONID", quotationID),
                         new SqlParameter("ENQUIRYID", result[0].EnquiryID),
                         new SqlParameter("BookingID", bookingid),
                         //new SqlParameter("RefType", "Enquiry")).ToList();  comment by vikas solanki on 12 Nov 2020
                         new SqlParameter("RefType", RefType)).ToList();
                    List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();

                    if (RemarksResultList.Count > 0)
                    {
                        objEnquiryDetail.NextActionRemarksDTOList = RemarksResult.ToArray();
                        objEnquiryDetail.LastRemarks = RemarksResultList[0].Remarks;

                        objEnquiryDetail.LastRemarkDate = RemarksResultList[0].CurrentDate.Value.Date;

                        if (RemarksResultList.Count > 1)
                        {
                            objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                        }
                        else
                        {
                            objEnquiryDetail.PrevActionDueDate = RemarksResultList[0].NextActionDate;
                        }
                    }
                    //get Container details / FCL                                     
                    List<EnquiryContainerServiceDTO> ContainerServiceDTOList = _context.ExecuteQuery<EnquiryContainerServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_CONTAINER_LIST @RefId,@RefType",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType)).ToList();
                    if (ContainerServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryContainerServiceDTOList = ContainerServiceDTOList.ToArray();
                    }
                    //get Air
                    List<EnquiryAIRServiceDTO> AirServiceDTOList = _context.ExecuteQuery<EnquiryAIRServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_AIR @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 7)).ToList();
                    if (AirServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryAIRServiceDTOList = AirServiceDTOList.ToArray();
                    }
                    //get Break Bulk
                    List<EnquiryBreakBulkServiceDTO> BreakBulkServiceDTOList = _context.ExecuteQuery<EnquiryBreakBulkServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_BREAK_BULK @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 5)).ToList();
                    if (BreakBulkServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryBreakBulkServiceDTOList = BreakBulkServiceDTOList.ToArray();
                    }
                    //get LCL
                    List<EnquiryLCLServiceDTO> LCLServiceDTOList = _context.ExecuteQuery<EnquiryLCLServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_LCL @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 2)).ToList();
                    if (LCLServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryLCLServiceDTOList = LCLServiceDTOList.ToArray();
                    }
                    //get RORO
                    List<EnquiryROROServiceDTO> ROROServiceDTOList = _context.ExecuteQuery<EnquiryROROServiceDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_RORO @RefId,@RefType,@fkServiceRequiredID",
                                    new SqlParameter("RefId", result[0].EnquiryID),
                                    new SqlParameter("RefType", RefType),
                                    new SqlParameter("fkServiceRequiredID", 3)).ToList();
                    if (ROROServiceDTOList.Count > 0)
                    {
                        objEnquiryDetail.EnquiryROROServiceDTOList = ROROServiceDTOList.ToArray();
                    }
                }
                // get new enqueryid 

                EnquiryDetailDTO objEnquiryList = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", objEnquiryDetail.DepartmentID)).FirstOrDefault();
                objEnquiryDetail.EnquiryID = objEnquiryList.EnquiryID;
                objEnquiryDetail.SiteId = Convert.ToDecimal(objEnquiryDetail.NewSiteId);
                newEnqNo = objEnquiryDetail.EnquiryID.ToString();
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_INSERT_UPDATE_DRAFT @Id,@EnquiryDate,@EnquiryNo,@fkCompanyID,@BillTo,@BillToCompanyId,@ShipmentMoveDate,@PickupType,@PickupRemark,@PortOfOrigin,@PortOfDischarge,@CityOfDischarge,@CityOfOrigin,@CountryOfDischarge,@CountryOfOrigin,@StateOfDischarge,@StateOfOrigin,@NoOfContainer,@TypeOfEnquiry,@ReceivedBy,@Remarks,@Type,@Description,@Class,@UNNo,@PackingType,@HazRemarks,@ChkHAZ,@CreatedBy,@UpdatedBy, @ModifiedOn, @Hazweight,@HazVolume,@DepartmentID,@LastEnquiryNo,@ControlNo,@IsDraft,@GalRepID, @OriginDoor,@DestinationDoor,@CustomerInqNo,@OriginRailRamp,@DestinationTerminal,@LicenseType,@SiteId",
               new SqlParameter("Id", objEnquiryDetail.EnquiryID),
               new SqlParameter("EnquiryDate", objEnquiryDetail.EnquiryDate),
               new SqlParameter("EnquiryNo", objEnquiryDetail.EnquiryNo),
               new SqlParameter("fkCompanyID", objEnquiryDetail.fkCompanyID ?? Convert.DBNull),
               new SqlParameter("BillTo", objEnquiryDetail.BillTo ?? Convert.DBNull),
               new SqlParameter("BillToCompanyId", objEnquiryDetail.BillToCompanyId ?? Convert.DBNull),
               new SqlParameter("ShipmentMoveDate", objEnquiryDetail.ShipmentMoveDate ?? Convert.DBNull),
               new SqlParameter("PickupType", objEnquiryDetail.PickupType ?? Convert.DBNull),
               new SqlParameter("PickupRemark", objEnquiryDetail.PickupRemark ?? Convert.DBNull),
               new SqlParameter("PortOfOrigin", objEnquiryDetail.OriginID),
               new SqlParameter("PortOfDischarge", objEnquiryDetail.DischargeID),
               new SqlParameter("CityOfDischarge", objEnquiryDetail.DestinationCityID ?? Convert.DBNull),
               new SqlParameter("CityOfOrigin", objEnquiryDetail.OriginCityID ?? Convert.DBNull),
               new SqlParameter("CountryOfDischarge", objEnquiryDetail.DestinationCountryID ?? Convert.DBNull),
               new SqlParameter("CountryOfOrigin", objEnquiryDetail.OriginCountryID ?? Convert.DBNull),
               new SqlParameter("StateOfDischarge", objEnquiryDetail.DestinationStateID ?? Convert.DBNull),
               new SqlParameter("StateOfOrigin", objEnquiryDetail.OrignStateID ?? Convert.DBNull),
               new SqlParameter("NoOfContainer", objEnquiryDetail.NoOfContainer ?? Convert.DBNull),
               new SqlParameter("TypeOfEnquiry", Convert.ToInt32(objEnquiryDetail.TypeOfEnquiry)),
               new SqlParameter("ReceivedBy", objEnquiryDetail.ReceivedByID ?? Convert.DBNull),
               new SqlParameter("Remarks", objEnquiryDetail.Remarks ?? Convert.DBNull),
               new SqlParameter("Type", "insert"),
               new SqlParameter("Description", objEnquiryDetail.Description ?? Convert.DBNull),
               new SqlParameter("Class", objEnquiryDetail.Class ?? Convert.DBNull),
               new SqlParameter("UNNo", objEnquiryDetail.UNNo ?? Convert.DBNull),
               new SqlParameter("PackingType", objEnquiryDetail.PackingType ?? Convert.DBNull),
               new SqlParameter("HazRemarks", objEnquiryDetail.HazRemarks ?? Convert.DBNull),
               new SqlParameter("ChkHAZ", objEnquiryDetail.IsHaz ?? Convert.DBNull),
               new SqlParameter("CreatedBy", objEnquiryDetail.UserID ?? Convert.DBNull),
               new SqlParameter("UpdatedBy", objEnquiryDetail.UpdatedBy ?? Convert.DBNull),
               new SqlParameter("ModifiedOn", System.DateTime.Now.Date),
               new SqlParameter("Hazweight", objEnquiryDetail.Hazweight ?? Convert.DBNull),
               new SqlParameter("HazVolume", objEnquiryDetail.HazVolume ?? Convert.DBNull),
               new SqlParameter("DepartmentID", objEnquiryDetail.DepartmentID ?? Convert.DBNull),
               new SqlParameter("LastEnquiryNo", objEnquiryDetail.LastEnquiryNo ?? Convert.DBNull),
               new SqlParameter("ControlNo", objEnquiryDetail.EnquiryControlNo ?? Convert.DBNull),
               new SqlParameter("IsDraft", objEnquiryDetail.IsDraft),
               new SqlParameter("GalRepID", objEnquiryDetail.GalRepID ?? Convert.DBNull),
               new SqlParameter("OriginDoor", objEnquiryDetail.OriginDoorID ?? Convert.DBNull),
               new SqlParameter("DestinationDoor", objEnquiryDetail.DestinationDoorID ?? Convert.DBNull),
               new SqlParameter("CustomerInqNo", objEnquiryDetail.CustomerInqNo ?? Convert.DBNull),
               new SqlParameter("OriginRailRamp", objEnquiryDetail.OrgnRailRampId ?? Convert.DBNull),
               new SqlParameter("DestinationTerminal", objEnquiryDetail.DestnTerminalId ?? Convert.DBNull),
               new SqlParameter("LicenseType", objEnquiryDetail.LicenseType ?? Convert.DBNull),
               new SqlParameter("SiteId", objEnquiryDetail.SiteId)).ToList();

                List<int> objList = objResult.ToList();
                int EnquiryId = objList[0];
                if (EnquiryId > 0)
                {
                    IEnumerable<int> objDeleteResult;
                    //delete existing enquiry Commdity details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "Enquiry")).ToList();
                    if (objEnquiryDetail.CommodityDTOList.Count() > 0)
                    {
                        SaveEnquiryCommodityDetails(objEnquiryDetail.CommodityDTOList, EnquiryId, "Enquiry");
                    }
                    //save next action remarks
                    if (objEnquiryDetail.NextActionRemarks != "" && objEnquiryDetail.NextActionDueDate != null)
                    {
                        IEnumerable<int> objRemarksResult;
                        objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                        new SqlParameter("RefID", EnquiryId),
                        new SqlParameter("NextActionDate", objEnquiryDetail.NextActionDueDate),
                        new SqlParameter("Type", "Enquiry"),
                        new SqlParameter("ActivityID", 1),
                        new SqlParameter("Remarks", objEnquiryDetail.NextActionRemarks)).ToList();
                    }
                    int modeOfService = Convert.ToInt32(objEnquiryDetail.ModeOfService);
                    //delete existing Container Service details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "Enquiry")).ToList();
                    //For containers & FCL = 1
                    if (objEnquiryDetail.EnquiryContainerServiceDTOList.Count() > 0)
                    {
                        SaveEnquiryContainerService(objEnquiryDetail.EnquiryContainerServiceDTOList, EnquiryId, modeOfService, "Enquiry", objEnquiryDetail.NoOfContainer);

                        if (modeOfService == 7) //AIR
                        {
                            if (objEnquiryDetail.EnquiryAIRServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryAIRService(objEnquiryDetail.EnquiryAIRServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                        if (modeOfService == 5) //Break Bulk
                        {
                            if (objEnquiryDetail.EnquiryBreakBulkServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryBreakBulkService(objEnquiryDetail.EnquiryBreakBulkServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                        if (modeOfService == 2) //LCL
                        {
                            if (objEnquiryDetail.EnquiryLCLServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryLCLService(objEnquiryDetail.EnquiryLCLServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }

                        if (modeOfService == 3)  //RORO Service details
                        {
                            if (objEnquiryDetail.EnquiryROROServiceDTOList.Count() > 0)
                            {
                                SaveEnquiryROROService(objEnquiryDetail.EnquiryROROServiceDTOList, EnquiryId, "Enquiry", objEnquiryDetail.NoOfContainer);
                            }
                        }
                    }
                }


                return AppResult(d, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReportRemark(ListParams listParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/EnquiryReport_Remark";
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                // var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", searchCriteria["optionValue"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", searchCriteria["seachValue"]));
                reportParams.Add(new KeyValuePair<string, string>("SORTCOLUMN", "EnquiryDate"));
                reportParams.Add(new KeyValuePair<string, string>("SORTORDER", "DESC"));
                reportParams.Add(new KeyValuePair<string, string>("DASHBOARD_FILTER", searchCriteria["dashboardOption"]));
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", listParams.SiteId.ToString()));


                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "EnquiryRemark_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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
                DAL.Helper.Logging.Logger.WriteError(ex);
                output.AddMessage(ex.Message, true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { output });
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportReportRemark_CRM(ListParams listParams)
        {
            var output = new StandardJsonResult { ResultId = Convert.ToInt32(EnumResult.Failed) };

            try
            {
                string reportPath = "/AppMGL.Report/EnquiryReport_Remark_CRM";
                var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                // var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", searchCriteria["optionValue"]));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", searchCriteria["seachValue"]));
                reportParams.Add(new KeyValuePair<string, string>("SORTCOLUMN", "EnquiryDate"));
                reportParams.Add(new KeyValuePair<string, string>("SORTORDER", "DESC"));
                reportParams.Add(new KeyValuePair<string, string>("DASHBOARD_FILTER", searchCriteria["dashboardOption"]));
                reportParams.Add(new KeyValuePair<string, string>("SIT_ID", listParams.SiteId.ToString()));


                ReportServerProxy report = new ReportServerProxy();
                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "EnquiryRemark_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";

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
                DAL.Helper.Logging.Logger.WriteError(ex);
                output.AddMessage(ex.Message, true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { output });
            }
        }


        [System.Web.Http.HttpPost]
        public ActionResult CompanySearch(ListParams listParams)
        {
            try
            {

                // int count;
                var result = _context.ExecuteQuery<Company>("EXEC dbo.SP_Company_Search @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        public string SaveEnquiryAsIncompleteDraftMethod(EnquiryDetailDTO objEnquiryDraft)
        {
            try
            {
                if (!string.IsNullOrEmpty(objEnquiryDraft.location))
                {
                    //if (objEnquiryDraft.location == "crm" && objEnquiryDraft.isapproved_enq == 1)
                    //{
                    //    IEnumerable<int> source1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CHECK_CUSTOMERCONTACT_ISAPPROVED @CompanyName", new object[1]
                    //    {

                    //new SqlParameter("CompanyName", objEnquiryDraft.CompanyName)
                    //    }).ToList();
                    //    List<int> entity = source1.ToList();
                    //    if (entity[0] == 0)
                    //    {
                    //        return AppResult(entity, 1L, "Please Approve this Contact Account as Regular Contact before Approving this Lead.", EnumResult.Failed);
                    //    }

                    //}
                }
                // EnquiryListRepository   _context = new EnquiryListRepository();

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_INSERT_INCOMPLETE_DRAFT @EnquiryDate,@EnquiryNo,@fkCompanyID,@BillTo,@BillToCompanyId,@ShipmentMoveDate,@PickupType,@PickupRemark,@PortOfOrigin,@PortOfDischarge,@CityOfDischarge,@CityOfOrigin,@CountryOfDischarge,@CountryOfOrigin,@StateOfDischarge,@StateOfOrigin,@NoOfContainer,@TypeOfEnquiry,@ReceivedBy,@Remarks,@Type,@Description,@Class,@UNNo,@PackingType,@HazRemarks,@ChkHAZ,@CreatedBy, @UpdatedBy,@ModifiedOn, @Hazweight,@HazVolume,@DepartmentID,@LastEnquiryNo,@ControlNo,@IsDraft,@GalRepID, @OriginDoor,@DestinationDoor,@CustomerInqNo,@OriginRailRamp,@DestinationTerminal,@LicenseType,@SiteId,@isapproved_enq,@sourceofcontact",
                new SqlParameter("EnquiryDate", objEnquiryDraft.EnquiryDate),
                new SqlParameter("EnquiryNo", objEnquiryDraft.EnquiryNo),
                new SqlParameter("fkCompanyID", objEnquiryDraft.fkCompanyID ?? Convert.DBNull),
                new SqlParameter("BillTo", objEnquiryDraft.BillTo ?? Convert.DBNull),
                new SqlParameter("BillToCompanyId", objEnquiryDraft.BillToCompanyId ?? Convert.DBNull),
                new SqlParameter("ShipmentMoveDate", objEnquiryDraft.ShipmentMoveDate ?? Convert.DBNull),
                new SqlParameter("PickupType", objEnquiryDraft.PickupType ?? Convert.DBNull),
                new SqlParameter("PickupRemark", objEnquiryDraft.PickupRemark ?? Convert.DBNull),
                new SqlParameter("PortOfOrigin", objEnquiryDraft.OriginID),
                new SqlParameter("PortOfDischarge", objEnquiryDraft.DischargeID),
                new SqlParameter("CityOfDischarge", objEnquiryDraft.DestinationCityID ?? Convert.DBNull),
                new SqlParameter("CityOfOrigin", objEnquiryDraft.OriginCityID ?? Convert.DBNull),
                new SqlParameter("CountryOfDischarge", objEnquiryDraft.DestinationCountryID ?? Convert.DBNull),
                new SqlParameter("CountryOfOrigin", objEnquiryDraft.OriginCountryID ?? Convert.DBNull),
                new SqlParameter("StateOfDischarge", objEnquiryDraft.DestinationStateID ?? Convert.DBNull),
                new SqlParameter("StateOfOrigin", objEnquiryDraft.OrignStateID ?? Convert.DBNull),
                new SqlParameter("NoOfContainer", objEnquiryDraft.NoOfContainer ?? Convert.DBNull),
                new SqlParameter("TypeOfEnquiry", Convert.ToInt32(objEnquiryDraft.TypeOfEnquiry)),
                new SqlParameter("ReceivedBy", objEnquiryDraft.ReceivedByID ?? Convert.DBNull),
                new SqlParameter("Remarks", objEnquiryDraft.Remarks ?? Convert.DBNull),
                new SqlParameter("Type", "insert"),
                new SqlParameter("Description", objEnquiryDraft.Description ?? Convert.DBNull),
                new SqlParameter("Class", objEnquiryDraft.Class ?? Convert.DBNull),
                new SqlParameter("UNNo", objEnquiryDraft.UNNo ?? Convert.DBNull),
                new SqlParameter("PackingType", objEnquiryDraft.PackingType ?? Convert.DBNull),
                new SqlParameter("HazRemarks", objEnquiryDraft.HazRemarks ?? Convert.DBNull),
                new SqlParameter("ChkHAZ", objEnquiryDraft.IsHaz ?? Convert.DBNull),
                new SqlParameter("CreatedBy", objEnquiryDraft.UserID ?? Convert.DBNull),
                new SqlParameter("UpdatedBy", objEnquiryDraft.UpdatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedOn", System.DateTime.Now.Date),
                new SqlParameter("Hazweight", objEnquiryDraft.Hazweight ?? Convert.DBNull),
                new SqlParameter("HazVolume", objEnquiryDraft.HazVolume ?? Convert.DBNull),
                new SqlParameter("DepartmentID", objEnquiryDraft.DepartmentID ?? Convert.DBNull),
                new SqlParameter("LastEnquiryNo", objEnquiryDraft.LastEnquiryNo ?? Convert.DBNull),
                new SqlParameter("ControlNo", objEnquiryDraft.EnquiryControlNo ?? Convert.DBNull),
                new SqlParameter("IsDraft", 1),
                new SqlParameter("GalRepID", objEnquiryDraft.GalRepID ?? Convert.DBNull),
                new SqlParameter("OriginDoor", objEnquiryDraft.OriginDoorID ?? Convert.DBNull),
                new SqlParameter("DestinationDoor", objEnquiryDraft.DestinationDoorID ?? Convert.DBNull),
                new SqlParameter("CustomerInqNo", objEnquiryDraft.CustomerInqNo ?? Convert.DBNull),
                new SqlParameter("OriginRailRamp", objEnquiryDraft.OrgnRailRampId ?? Convert.DBNull),
                new SqlParameter("DestinationTerminal", objEnquiryDraft.DestnTerminalId ?? Convert.DBNull),
                new SqlParameter("LicenseType", objEnquiryDraft.LicenseType ?? Convert.DBNull),
                new SqlParameter("SiteId", objEnquiryDraft.SiteId),
                new SqlParameter("isapproved_enq", objEnquiryDraft.isapproved_enq == null ? 0 : objEnquiryDraft.isapproved_enq),
                new SqlParameter("sourceofcontact", objEnquiryDraft.sourceofcontact == null ? "" : objEnquiryDraft.sourceofcontact)
                ).ToList();
                List<int> objList = objResult.ToList();
                int EnquiryId = objList[0];
                if (EnquiryId > 0)
                {
                    IEnumerable<int> objDeleteResult;
                    //delete existing enquiry Commdity details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_COMMODITY_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "EnquiryDraft")).ToList();
                    if (objEnquiryDraft.CommodityDTOList != null)
                    {
                        if (objEnquiryDraft.CommodityDTOList.Count() > 0)
                        {
                            SaveEnquiryCommodityDetails(objEnquiryDraft.CommodityDTOList, EnquiryId, "Enquiry");
                        }
                    }
                    //save next action remarks
                    if (objEnquiryDraft.NextActionRemarks != "" && objEnquiryDraft.NextActionDueDate != null)
                    {
                        IEnumerable<int> objRemarksResult;
                        objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                        new SqlParameter("RefID", EnquiryId),
                        new SqlParameter("NextActionDate", objEnquiryDraft.NextActionDueDate),
                        new SqlParameter("Type", "EnquiryDraft"),
                        new SqlParameter("ActivityID", 4),
                        new SqlParameter("Remarks", objEnquiryDraft.NextActionRemarks)).ToList();
                    }

                    int modeOfService = Convert.ToInt32(objEnquiryDraft.ModeOfService);
                    //delete existing Container Service details
                    objDeleteResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ENQUIRY_SERVICE_REF_DELETE @RefId,@RefType",
                        new SqlParameter("RefId", EnquiryId),
                        new SqlParameter("RefType", "EnquiryDraft")).ToList();
                    //For containers & FCL = 1
                    if (objEnquiryDraft.EnquiryContainerServiceDTOList != null)
                    {
                        if (objEnquiryDraft.EnquiryContainerServiceDTOList.Count() > 0)
                        {
                            SaveEnquiryContainerService(objEnquiryDraft.EnquiryContainerServiceDTOList, EnquiryId, modeOfService, "EnquiryDraft", objEnquiryDraft.NoOfContainer);

                            if (modeOfService == 7) //AIR
                            {
                                if (objEnquiryDraft.EnquiryAIRServiceDTOList.Count() > 0)
                                {
                                    SaveEnquiryAIRService(objEnquiryDraft.EnquiryAIRServiceDTOList, EnquiryId, "EnquiryDraft", objEnquiryDraft.NoOfContainer);
                                }
                            }
                            if (modeOfService == 5) //Break Bulk
                            {
                                if (objEnquiryDraft.EnquiryBreakBulkServiceDTOList.Count() > 0)
                                {
                                    SaveEnquiryBreakBulkService(objEnquiryDraft.EnquiryBreakBulkServiceDTOList, EnquiryId, "EnquiryDraft", objEnquiryDraft.NoOfContainer);
                                }
                            }
                            if (modeOfService == 2) //LCL
                            {
                                if (objEnquiryDraft.EnquiryLCLServiceDTOList.Count() > 0)
                                {
                                    SaveEnquiryLCLService(objEnquiryDraft.EnquiryLCLServiceDTOList, EnquiryId, "EnquiryDraft", objEnquiryDraft.NoOfContainer);
                                }
                            }

                            if (modeOfService == 3)  //RORO Service details
                            {
                                if (objEnquiryDraft.EnquiryROROServiceDTOList.Count() > 0)
                                {
                                    SaveEnquiryROROService(objEnquiryDraft.EnquiryROROServiceDTOList, EnquiryId, "EnquiryDraft", objEnquiryDraft.NoOfContainer);
                                }
                            }
                        }
                    }
                }
                return EnquiryId.ToString();
            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult NextActionDateList(Dictionary<string, string> listParams)
        {
            try
            {

                string startdate = listParams["StartNextActionDate"].ToString();
                string enddate = listParams["EndNextActionDate"].ToString();
                string selectOption = listParams["selectOption"].ToString();
                string searchBox = listParams["searchBox"].ToString();
                string SiteId = listParams["SiteId"].ToString();
                IEnumerable<NextActionRemarksDTO> RemarksResult = _context.ExecuteQuery<NextActionRemarksDTO>("EXEC dbo.NextActonRemarksList @StartNextActionDate,@EndNextActionDate,@OPTIONVALUE,@SEARCHVALUE,@SiteId",
                         new SqlParameter("StartNextActionDate", startdate),
                         new SqlParameter("EndNextActionDate", enddate),
                         new SqlParameter("OPTIONVALUE", selectOption),
                         new SqlParameter("SEARCHVALUE", searchBox),
                         new SqlParameter("SiteId", SiteId)
                        ).ToList();
                List<NextActionRemarksDTO> RemarksResultList = RemarksResult.ToList();
                return AppResult(RemarksResultList, 1, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportToExcelRemarks(Dictionary<string, string> listParams)
        {

            ReportServerProxy report = new ReportServerProxy();
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                string startdate = listParams["StartNextActionDate"].ToString();
                string enddate = listParams["EndNextActionDate"].ToString();
                string selectOption = listParams["selectOption"].ToString();
                string searchBox = listParams["searchBox"].ToString();
                string SiteId = listParams["SiteId"].ToString();
                string reportPath = "/AppMGL.Report/EnquiryReport_Remark_CRM_ActionRemark";
                // var searchCriteria = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                // var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);

                List<KeyValuePair<string, string>> reportParams = new List<KeyValuePair<string, string>>();
                reportParams.Add(new KeyValuePair<string, string>("StartNextActionDate", startdate));
                reportParams.Add(new KeyValuePair<string, string>("EndNextActionDate", enddate));
                reportParams.Add(new KeyValuePair<string, string>("OPTIONVALUE", selectOption));
                reportParams.Add(new KeyValuePair<string, string>("SEARCHVALUE", searchBox));
                reportParams.Add(new KeyValuePair<string, string>("SiteId", SiteId));

                byte[] reportBuffer = report.Render(reportPath, reportParams, ReportFormat.XLSX);

                if (reportBuffer.Length > 0)
                {
                    string fileName = "EnquiryRemark_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";


                    result.Content = new StreamContent(new MemoryStream(reportBuffer));
                    result.Content.Headers.Add("X-FileName", fileName);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                    result.StatusCode = HttpStatusCode.OK;


                }

                return result;

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, ex.Message);
            }
            //return Request.CreateResponse(HttpStatusCode.NoContent);

        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult GetBookingHistory(Dictionary<string, string> listParams)
        {
            try
            {

                //string startdate = listParams["StartNextActionDate"].ToString();
                //string enddate = listParams["EndNextActionDate"].ToString();
                //string selectOption = listParams["selectOption"].ToString();
                //string searchBox = listParams["searchBox"].ToString();
                string SiteId = listParams["SiteId"].ToString();

                var parameters = new List<SqlParameter>()
                {
                   // new SqlParameter("PageIndex", listParams["PageIndex"]),
                   // new SqlParameter("PageSize", listParams["PageSize"]),
                   // new SqlParameter("Sort", Utility.GetSort(listParams["Sort"])),
                    new SqlParameter("EnquiryId", listParams["EnquiryId"]),
                    new SqlParameter("CompanyId", listParams["CompanyId"]),
                    new SqlParameter("SiteId", listParams["SiteId"]),
                   // new SqlParameter("Count", SqlDbType.Int) {Direction = ParameterDirection.Output}
                };
                // List<BookingReportHistoryDTO> result = _context.ExecuteQuery<BookingReportHistoryDTO>("EXEC dbo.GET_BOOKING_LIST_BY_Company @PageIndex, @PageSize, @Sort,@EnquiryId, @CompanyId, @SitId, @Count OUT", parameters.ToArray()).ToList();

                List<MCSReportDTOCompany> result = _context.ExecuteQuery<MCSReportDTOCompany>("EXEC dbo.USP_LG_GET_MCS_BY_UNIT_Report_Company @SiteId, @EnquiryId,@CompanyId", parameters.ToArray()).ToList();

                //  List<BookingListDTO> result = _context.ExecuteQuery<BookingListDTO>("EXEC dbo.USP_LG_BOOKING_GET_EXPORT_REGISTER_LIST_Company @PAGENO, @PAGESIZE,@SORTORDER,,@CompanyId", parameters.ToArray()).ToList();
                int count = result.Count;
                return AppResult(result, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult AddEnquiryRemark(Dictionary<string, string> listParams)
        {
            try
            {
                var remark = listParams["remark"];
                var rdate = listParams["remarkdate"];
                var EnquiryId = listParams["EnquiryId"];
                if (remark != "" && rdate != null)
                {
                    IEnumerable<int> objRemarksResult;
                    objRemarksResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_QUOTATION_ACTION_REMARKS_INSERT @RefID,@NextActionDate,@Type,@ActivityID,@Remarks",
                                    new SqlParameter("RefID", EnquiryId),
                                    new SqlParameter("NextActionDate", rdate),
                                    new SqlParameter("Type", "Enquiry"),
                                    new SqlParameter("ActivityID", 1),
                                    new SqlParameter("Remarks", remark)).ToList();

                    
                }
                return AppResult(null, "1");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


}
}
   