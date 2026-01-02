using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Operation;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using AppMGL.Manager.Infrastructure.Results;
using System.Web;
using System.Net;
using System.Configuration;
using System.IO;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DTO.Report;

using System.Net.Http.Headers;

using AppMGL.DAL.Repository.Report;


using AppMGL.Manager.Infrastructure.Report;
using System.Reflection;
using System.Data;
using System.Net.Mail;
using System.Security.Claims;
using AppMGL.Manager.Infrastructure.Helper;
using Intuit.Ipp.Data;
using Intuit.Ipp.Core;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Security;
using Intuit.Ipp.QueryFilter;
using OfficeOpenXml;
using ClosedXML.Excel;
using AppMGL.DTO.Security;
using AppMGL.Manager.Areas.Security.Controllers;

namespace AppMGL.Manager.Areas.Operation.Controllers
{

    public class CustomerContactController : BaseController<CustomerContactDTO, CustomerContactRepository, USP_GET_CUSTOMERCONTACT_LIST_Result>
    {
        public CustomerContactController(CustomerContactRepository context)
        {
            _context = context;
            base.BaseModule = EnumModule.CustomerContact;
            base.KeyField = "ContactID";
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult ContactList()
        {
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                List<USP_GET_CUSTOMERCONTACT_LIST_Result> entity = _context.ExecuteQuery<USP_GET_CUSTOMERCONTACT_LIST_Result>("EXEC dbo.USP_GET_CUSTOMERCONTACT_DATA @SEARCH, @COLNAME", new object[1]
                {
                    list
                }).ToList();
                return AppResult(entity, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage downloadAttachment()
        {
            try
            {
                string text = "";
                text = base.Request.Headers.GetValues("fileName").ToList()[0];
                int num = int.Parse(base.Request.Headers.GetValues("contactID").ToList()[0]);
                text = num + "_" + text;
                HttpResponseMessage httpResponseMessage = null;
                if (!File.Exists(text))
                {
                    httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.Gone);
                }
                if (!string.IsNullOrEmpty(text))
                {
                    string str = ConfigurationManager.AppSettings["CustomerPath"];
                    string path = HttpContext.Current.Server.MapPath(str + "/") + text;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            byte[] array = new byte[fileStream.Length];
                            fileStream.Read(array, 0, (int)fileStream.Length);
                            memoryStream.Write(array, 0, (int)fileStream.Length);
                            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();
                            httpResponseMessage2.Content = new ByteArrayContent(array.ToArray());
                            httpResponseMessage2.Content.Headers.Add("x-filename", text);
                            httpResponseMessage2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            httpResponseMessage2.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage2;
                        }
                    }
                }
                return base.Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return base.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int count = 0;
                 List<USP_GET_CUSTOMERCONTACT_LIST_Result> list = _context.ExecuteQuery<USP_GET_CUSTOMERCONTACT_LIST_Result>("EXEC dbo.USP_GET_CUSTOMERCONTACT_LIST @PAGENO, @PAGESIZE,@CompanyName,@CustomerCode,@MiamiRep,@Category,@ForwarderNetwork,@AttachedSite,@CompanyGradation, @OriginCountries, @Commodity, @Region, @Country, @State, @City, @IsVendor,@SIT_ID,@SORTCOLUMN,@SORTORDER,@IsSepatraCustomer,@IsSepatraPartner", new object[21]
               // List<USP_GET_CUSTOMERCONTACT_LIST_Result> list = _context.ExecuteQuery<USP_GET_CUSTOMERCONTACT_LIST_Result>("EXEC dbo.USP_GET_CUSTOMERCONTACT_LIST @PAGENO, @PAGESIZE,@CompanyName,@CustomerCode,@MiamiRep,@Category,@ForwarderNetwork,@AttachedSite,@CompanyGradation, @OriginCountries, @Commodity, @Region, @Country, @State, @City, @IsVendor,@SIT_ID,@SORTCOLUMN,@SORTORDER", new object[19]
                 {
                    new SqlParameter("PAGENO", listParams.PageIndex),
                    new SqlParameter("PAGESIZE", listParams.PageSize),
                    new SqlParameter("CompanyName", dictionary.ContainsKey("companyName") ? dictionary["companyName"] : string.Empty),
                    new SqlParameter("CustomerCode", dictionary.ContainsKey("customerCode") ? dictionary["customerCode"] : string.Empty),
                    new SqlParameter("MiamiRep", dictionary.ContainsKey("galRepresentative") ? dictionary["galRepresentative"] : string.Empty),
                    new SqlParameter("Category", dictionary.ContainsKey("contactCategoryID") ? dictionary["contactCategoryID"] : string.Empty),
                    new SqlParameter("ForwarderNetwork", dictionary.ContainsKey("forwarderNetworkId") ? dictionary["forwarderNetworkId"] : string.Empty),
                    new SqlParameter("AttachedSite", dictionary.ContainsKey("attachedsiteId") ? dictionary["attachedsiteId"] : string.Empty),
                    new SqlParameter("CompanyGradation", dictionary.ContainsKey("companyGradation") ? dictionary["companyGradation"] : string.Empty),
                    new SqlParameter("OriginCountries", dictionary.ContainsKey("OriginCountry") ? dictionary["OriginCountry"] : string.Empty),
                    new SqlParameter("Commodity", dictionary.ContainsKey("commodity") ? dictionary["commodity"] : string.Empty),
                    new SqlParameter("Region", dictionary.ContainsKey("continent") ? dictionary["continent"] : string.Empty),
                    new SqlParameter("Country", dictionary.ContainsKey("cryName") ? dictionary["cryName"] : string.Empty),
                    new SqlParameter("State", dictionary.ContainsKey("state") ? dictionary["state"] : string.Empty),
                    new SqlParameter("City", dictionary.ContainsKey("city") ? dictionary["city"] : string.Empty),
                    new SqlParameter("IsVendor", dictionary["isVendor"]),
                    new SqlParameter("SIT_ID", dictionary["siteId"]),
                    new SqlParameter("SORTCOLUMN", source.First().Key),
                    new SqlParameter("SORTORDER", source.First().Value),
                     new SqlParameter("IsSepatraCustomer", dictionary["IsSepatraCustomer"]),
                     new SqlParameter("IsSepatraPartner", dictionary["IsSepatraPartner"])
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount.Value;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult listcrm(ListParams listParams)
        {
            try
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int count = 0;
                 List<USP_GET_CUSTOMERCONTACT_LIST_Result> list = _context.ExecuteQuery<USP_GET_CUSTOMERCONTACT_LIST_Result>("EXEC dbo.USP_GET_CUSTOMERCONTACT_LIST_CRM @PAGENO, @PAGESIZE,@CompanyName,@CustomerCode,@MiamiRep,@Category,@ForwarderNetwork,@AttachedSite,@CompanyGradation, @OriginCountries, @Commodity, @Region, @Country, @State, @City, @IsVendor,@SIT_ID,@SORTCOLUMN,@SORTORDER,@IsSepatraCustomer,@IsSepatraPartner", new object[21]
                //List<USP_GET_CUSTOMERCONTACT_LIST_Result> list = _context.ExecuteQuery<USP_GET_CUSTOMERCONTACT_LIST_Result>("EXEC dbo.USP_GET_CUSTOMERCONTACT_LIST_CRM @PAGENO, @PAGESIZE,@CompanyName,@CustomerCode,@MiamiRep,@Category,@ForwarderNetwork,@AttachedSite,@CompanyGradation, @OriginCountries, @Commodity, @Region, @Country, @State, @City, @IsVendor,@SIT_ID,@SORTCOLUMN,@SORTORDER", new object[19]
                 {

                    new SqlParameter("PAGENO", listParams.PageIndex),
                    new SqlParameter("PAGESIZE", listParams.PageSize),
                    new SqlParameter("CompanyName", dictionary.ContainsKey("companyName") ? dictionary["companyName"] : string.Empty),
                    new SqlParameter("CustomerCode", dictionary.ContainsKey("customerCode") ? dictionary["customerCode"] : string.Empty),
                    new SqlParameter("MiamiRep", dictionary.ContainsKey("galRepresentative") ? dictionary["galRepresentative"] : string.Empty),
                    new SqlParameter("Category", dictionary.ContainsKey("contactCategoryID") ? dictionary["contactCategoryID"] : string.Empty),
                    new SqlParameter("ForwarderNetwork", dictionary.ContainsKey("forwarderNetworkId") ? dictionary["forwarderNetworkId"] : string.Empty),
                    new SqlParameter("AttachedSite", dictionary.ContainsKey("attachedsiteId") ? dictionary["attachedsiteId"] : string.Empty),
                    new SqlParameter("CompanyGradation", dictionary.ContainsKey("companyGradation") ? dictionary["companyGradation"] : string.Empty),
                    new SqlParameter("OriginCountries", dictionary.ContainsKey("OriginCountry") ? dictionary["OriginCountry"] : string.Empty),
                    new SqlParameter("Commodity", dictionary.ContainsKey("commodity") ? dictionary["commodity"] : string.Empty),
                    new SqlParameter("Region", dictionary.ContainsKey("continent") ? dictionary["continent"] : string.Empty),
                    new SqlParameter("Country", dictionary.ContainsKey("cryName") ? dictionary["cryName"] : string.Empty),
                    new SqlParameter("State", dictionary.ContainsKey("state") ? dictionary["state"] : string.Empty),
                    new SqlParameter("City", dictionary.ContainsKey("city") ? dictionary["city"] : string.Empty),
                    new SqlParameter("IsVendor", dictionary["isVendor"]),
                    new SqlParameter("SIT_ID", dictionary["siteId"]),
                    new SqlParameter("SORTCOLUMN", source.First().Key),
                    new SqlParameter("SORTORDER", source.First().Value),
                    new SqlParameter("IsSepatraCustomer", dictionary["IsSepatraCustomer"]),
                      new SqlParameter("IsSepatraPartner", dictionary["IsSepatraPartner"])
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount.Value;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpGet]
        public virtual ActionResult getContactDetail(int id)
        {
            try
            {
                List<CustomerContactDTO> list = _context.ExecuteQuery<CustomerContactDTO>("EXEC dbo.USP_LG_CONTACT_GET_DETAIL @ContactID", new object[1]
                {
                    new SqlParameter("ContactID", id)
                }).ToList();
                CustomerContactDTO customerContactDTO = list[0];
                customerContactDTO.isconsolidatedreport = (customerContactDTO.isconsolidatedreport == null ? 0 : customerContactDTO.isconsolidatedreport);
                if (list[0].ContactID > 0)
                {
                    customerContactDTO.IDNumberTypeValue = customerContactDTO.IDNumberType;
                    List<AdditionalContactDTO> list2 = _context.ExecuteQuery<AdditionalContactDTO>("EXEC dbo.USP_LG_CONTACT_GET_CONTACT_DETAILS @ContactID", new object[1]
                    {
                        new SqlParameter("ContactID", id)
                    }).ToList();
                    if (list2.Count > 0)
                    {
                        customerContactDTO.AdditionalContactDTOList = list2.ToArray();
                    }
                    List<ContactBranchDetailDTO> list3 = _context.ExecuteQuery<ContactBranchDetailDTO>("EXEC dbo.USP_LG_CONTACT_GET_BRANCH_DETAIL @ContactID", new object[1]
                    {
                        new SqlParameter("ContactID", id)
                    }).ToList();
                    if (list3.Count > 0)
                    {
                        customerContactDTO.ContactBranchDetailDTOList = list3.ToArray();
                    }
                    List<ContactCommodityDTO> list4 = _context.ExecuteQuery<ContactCommodityDTO>("EXEC dbo.USP_LG_CONTACT_GET_SUB_CATEGORY_TRANSACTION_DETAIL @ContactID,@TableName", new object[2]
                    {
                        new SqlParameter("ContactID", id),
                        new SqlParameter("TableName", "Commodity")
                    }).ToList();
                    if (list4.Count > 0)
                    {
                        customerContactDTO.ContactCommodityDTOList = list4.ToArray();
                    }
                    List<ContactOrigionDTO> list5 = _context.ExecuteQuery<ContactOrigionDTO>("EXEC dbo.USP_LG_CONTACT_GET_SUB_CATEGORY_TRANSACTION_DETAIL @ContactID,@TableName", new object[2]
                    {
                        new SqlParameter("ContactID", id),
                        new SqlParameter("TableName", "Origin")
                    }).ToList();
                    if (list5.Count > 0)
                    {
                        customerContactDTO.ContactOrigionDTOList = list5.ToArray();
                    }
                }
                return AppResult(customerContactDTO, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult saveCustomerContact(CustomerContactDTO objContactDTO)
        {
            try
            {

                IEnumerable<int> source1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CUSTOMERCONTACT_DUPLICATE @ContactID,@CompanyName", new object[2]
                {
                    new SqlParameter("ContactID", objContactDTO.ContactID),
                    new SqlParameter("CompanyName", objContactDTO.CompanyName)
                }).ToList();
                List<int> entity = source1.ToList();
                if (entity.Count > 0)
                {
                    return AppResult(entity, 1L, "Duplicate Contact! Contact Already Exist, To Add New Contact Category, Please Edit existing Contact.", EnumResult.Failed);
                }


                string text = "";
                CustomerContactDTO data = null;
                text = ((objContactDTO.ContactID != 0) ? "update" : "insert");
                CustomerContactRepository context = _context;
               //  object[] obj = new object[46]
                object[] obj = new object[48]
                 {
                    new SqlParameter("ContactID", objContactDTO.ContactID),
                    new SqlParameter("Address", objContactDTO.Address ?? Convert.DBNull),
                    new SqlParameter("CellNo", objContactDTO.CellNo ?? Convert.DBNull),
                    new SqlParameter("CompanyName", objContactDTO.CompanyName),
                    new SqlParameter("ContactCategoryID", objContactDTO.ContactCategoryID ?? Convert.DBNull),
                    new SqlParameter("ForwarderNetworkId", objContactDTO.ForwarderNetworkId ?? Convert.DBNull),
                    new SqlParameter("ContactPerson", objContactDTO.ContactPerson ?? Convert.DBNull),
                    new SqlParameter("Email", objContactDTO.Email ?? Convert.DBNull),
                    new SqlParameter("Fax", objContactDTO.Fax ?? Convert.DBNull),
                    new SqlParameter("Remarks", objContactDTO.Remarks ?? Convert.DBNull),
                    new SqlParameter("TelNo", objContactDTO.TelNo ?? Convert.DBNull),
                    new SqlParameter("WebSite", objContactDTO.WebSite ?? Convert.DBNull),
                    new SqlParameter("ZipCode", objContactDTO.ZipCode ?? Convert.DBNull),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null
                };
                int? createdBy = objContactDTO.CreatedBy;
                obj[13] = new SqlParameter("CreatedBy", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.ModifiedBy;
                obj[14] = new SqlParameter("ModifiedBy", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                DateTime? modifiedOn = objContactDTO.ModifiedOn;
                obj[15] = new SqlParameter("ModifiedOn", modifiedOn.HasValue ? ((object)modifiedOn.GetValueOrDefault()) : Convert.DBNull);
                obj[16] = new SqlParameter("Type", text);
                obj[17] = new SqlParameter("AccountDetail", objContactDTO.AccountDetail ?? Convert.DBNull);
                obj[18] = new SqlParameter("Attachment", objContactDTO.Attachment ?? Convert.DBNull);
                obj[19] = new SqlParameter("CompanyGradation", objContactDTO.companyGradation ?? Convert.DBNull);
                obj[20] = new SqlParameter("CustomerCode", objContactDTO.CustomerCode ?? Convert.DBNull);
                //createdBy = Convert.ToInt32( objContactDTO.RepresentativeID);
                obj[21] = new SqlParameter("RepID", objContactDTO.RepresentativeID ?? Convert.DBNull);
                createdBy = objContactDTO.ContinentId;
                obj[22] = new SqlParameter("ContinentId", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.CountryId;
                obj[23] = new SqlParameter("CountryId", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.StateId;
                obj[24] = new SqlParameter("StateID", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.CityId;
                obj[25] = new SqlParameter("CityID", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.AcyID;
                obj[26] = new SqlParameter("AcyID", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                obj[27] = new SqlParameter("SIT_ID", objContactDTO.SiteId);
                obj[28] = new SqlParameter("IDNumberType", objContactDTO.IDNumberTypeValue ?? Convert.DBNull);
                obj[29] = new SqlParameter("IDNumber", objContactDTO.IDNumber ?? Convert.DBNull);
                obj[30] = new SqlParameter("IRSNumber", objContactDTO.IRSNumber ?? Convert.DBNull);
                obj[31] = new SqlParameter("isconsolidatedreport", objContactDTO.isconsolidatedreport == null ? 0 : objContactDTO.isconsolidatedreport);
                obj[32] = new SqlParameter("scac", objContactDTO.SCAC == null ? "" : objContactDTO.SCAC);
                obj[33] = new SqlParameter("iata", objContactDTO.IATA == null ? "" : objContactDTO.IATA);
                obj[34] = new SqlParameter("ContactPersonFirstName", objContactDTO.ContactPersonFirstName == null ? "" : objContactDTO.ContactPersonFirstName);
                obj[35] = new SqlParameter("ContactPersonLastName", objContactDTO.ContactPersonLastName == null ? "" : objContactDTO.ContactPersonLastName);
                obj[36] = new SqlParameter("EnquiryUrl", objContactDTO.EnquiryUrl == null ? "" : objContactDTO.EnquiryUrl);
                obj[37] = new SqlParameter("siteids", objContactDTO.siteids == null ? "" : objContactDTO.siteids);
                obj[38] = new SqlParameter("isapproved", objContactDTO.isapproved == null ? 0 : objContactDTO.isapproved);
                obj[39] = new SqlParameter("sourceofcontact", objContactDTO.sourceofcontact == null ? "" : objContactDTO.sourceofcontact);
                obj[40] = new SqlParameter("createdfrom", objContactDTO.createdfrom == null ? "" : objContactDTO.createdfrom);
                obj[41] = new SqlParameter("Designation", objContactDTO.designation == null ? "" : objContactDTO.designation);
                obj[42] = new SqlParameter("IsSepatraCustomer", objContactDTO.IsSepatraCustomer == null ? "No" : objContactDTO.IsSepatraCustomer);
               
                obj[43] = new SqlParameter("SepatraCustomer", objContactDTO.SepatraCustomer == null ? "" : objContactDTO.SepatraCustomer);
                obj[44] = new SqlParameter("SepatraCustomerId", objContactDTO.SepatraCustomerId == null ? "0" : objContactDTO.SepatraCustomerId);
                 obj[45] = new SqlParameter("IsSepatraPartner", objContactDTO.IsSepatraPartner == null ? "No" : objContactDTO.IsSepatraPartner);
                obj[46] = new SqlParameter("contactpersonemail", objContactDTO.contactpersonemail == null ? "" : objContactDTO.contactpersonemail);
                obj[47] = new SqlParameter("contactpersonphone", objContactDTO.contactpersonphone == null ? "" : objContactDTO.contactpersonphone);
                IEnumerable<int> source = context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_INSERT_UPDATE @ContactID ,@Address,@CellNo,@CompanyName,@ContactCategoryID,@ForwarderNetworkId, @ContactPerson, @Email, @Fax, @Remarks, @TelNo, @WebSite, @ZipCode,@CreatedBy, @ModifiedBy, @ModifiedOn, @Type, @AccountDetail, @Attachment, @CompanyGradation, @CustomerCode,@RepID,@ContinentId,@CountryId,@StateID,@CityID,@AcyID,@SIT_ID,@IDNumberType,@IDNumber,@IRSNumber,@isconsolidatedreport,@scac,@iata,@ContactPersonFirstName,@ContactPersonLastName,@EnquiryUrl,@siteids,@isapproved,@sourceofcontact,@createdfrom,@Designation,@IsSepatraCustomer,@SepatraCustomer,@SepatraCustomerId,@IsSepatraPartner,@contactpersonemail,@contactpersonphone", obj).ToList();
                //IEnumerable<int> source = context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_INSERT_UPDATE @ContactID ,@Address,@CellNo,@CompanyName,@ContactCategoryID,@ForwarderNetworkId, @ContactPerson, @Email, @Fax, @Remarks, @TelNo, @WebSite, @ZipCode,@CreatedBy, @ModifiedBy, @ModifiedOn, @Type, @AccountDetail, @Attachment, @CompanyGradation, @CustomerCode,@RepID,@ContinentId,@CountryId,@StateID,@CityID,@AcyID,@SIT_ID,@IDNumberType,@IDNumber,@IRSNumber,@isconsolidatedreport,@scac,@iata,@ContactPersonFirstName,@ContactPersonLastName,@EnquiryUrl,@siteids,@isapproved,@sourceofcontact,@createdfrom,@Designation,@IsSepatraCustomer,@SepatraCustomer,@SepatraCustomerId", obj).ToList();
                List<int> list = source.ToList();
                int num = list[0];
                if (num > 0)
                {
                    IEnumerable<int> enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_ADDITIONAL_DETAIL_DELETE @ContactID", new object[1]
                    {
                        new SqlParameter("@ContactID", num)
                    }).ToList();
                    if (objContactDTO.AdditionalContactDTOList.Count() > 0)
                    {
                        SaveAdditionalContactDetail(objContactDTO.AdditionalContactDTOList, num);
                    }
                    enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_BRANCH_DELETE @ContactID", new object[1]
                    {
                        new SqlParameter("@ContactID", num)
                    }).ToList();
                    if (objContactDTO.ContactBranchDetailDTOList.Count() > 0)
                    {
                        SaveBranchDetail(objContactDTO.ContactBranchDetailDTOList, num);
                    }
                    enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_SUB_CATEGORY_TRANSACTION_DELETE @ContactID,@TableName", new object[2]
                    {
                        new SqlParameter("@ContactID", num),
                        new SqlParameter("TableName", "Commodity")
                    }).ToList();
                    if (objContactDTO.ContactCommodityDTOList.Count() > 0)
                    {
                        SaveCommodityDetail(objContactDTO.ContactCommodityDTOList, num);
                    }
                    enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_SUB_CATEGORY_TRANSACTION_DELETE @ContactID,@TableName", new object[2]
                    {
                        new SqlParameter("@ContactID", num),
                        new SqlParameter("TableName", "Origin")
                    }).ToList();
                    if (objContactDTO.ContactOrigionDTOList.Count() > 0)
                    {
                        SaveContactOrigion(objContactDTO.ContactOrigionDTOList, num);
                    }
                    List<CustomerContactDTO> list2 = _context.ExecuteQuery<CustomerContactDTO>("EXEC dbo.USP_LG_CONTACT_GET_DETAIL @ContactID", new object[1]
                    {
                        new SqlParameter("ContactID", num)
                    }).ToList();
                    data = list2[0];
                }
                if (!string.IsNullOrEmpty(objContactDTO.createdfrom))
                {
                    if (objContactDTO.createdfrom == "crm" && text == "insert") // create enqery
                    {
                        int id = 2;

                        List<EnquiryDetailDTO> objEnquiryList = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", id)).ToList();
                        EnquiryDetailDTO objEnquiryDetail = objEnquiryList[0];
                        EnquiryDetailDTO objEnquiryDraft = new EnquiryDetailDTO();
                        objEnquiryDraft.EnquiryDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                        objEnquiryDraft.EnquiryNo = objEnquiryDetail.EnquiryNo;
                        objEnquiryDraft.fkCompanyID = num;
                        objEnquiryDraft.BillToCompanyId = null;
                        objEnquiryDraft.PickupType = "0";
                        objEnquiryDraft.OriginID = 0;
                        objEnquiryDraft.DischargeID = 0;
                        objEnquiryDraft.TypeOfEnquiry = "1";
                        objEnquiryDraft.IsHaz = false;
                        objEnquiryDraft.Remarks = "Auto Generated";
                        objEnquiryDraft.DepartmentID = 2;

                        objEnquiryDraft.isapproved_enq = 0;
                        objEnquiryDraft.location = "crm";
                        objEnquiryDraft.IsDraft = 1;
                        objEnquiryDraft.UserID = objContactDTO.CreatedBy;
                        objEnquiryDraft.UpdatedBy = objContactDTO.CreatedBy;



                        objEnquiryDraft.LastRemarkDate = DateTime.Now;
                        objEnquiryDraft.SiteId = objContactDTO.SiteId;
                        objEnquiryDraft.CreatedBy = objContactDTO.CreatedBy;
                        AppMGL.DAL.Models.AppMGL a = new DAL.Models.AppMGL();
                        EnquiryListRepository l = new EnquiryListRepository(a);
                        EnquiryController enqc = new EnquiryController(l);
                        objEnquiryDraft.CustomerInqNo = "";
                        objEnquiryDraft.LicenseType = 1;
                        objEnquiryDraft.LastEnquiryNo = objEnquiryDetail.LastEnquiryNo;
                        objEnquiryDraft.EnquiryControlNo = objEnquiryDetail.EnquiryControlNo;
                        enqc.SaveEnquiryAsIncompleteDraftMethod(objEnquiryDraft);
                        return AppResult(data, 1L, "Contact successfully created and Enquiry No. is  " + objEnquiryDetail.EnquiryNo, EnumResult.Success);
                    }
                }
                return AppResult(data, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(CustomerContactDTO dto)
        {
            try
            {
                IEnumerable<int> source = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CUSTOMERCONTACT_DELETE @ContactID", new object[1]
                {
                    new SqlParameter("ContactID", dto.ContactID)
                }).ToList();
                List<int> entity = source.ToList();
                return AppResult(entity, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public void SaveAdditionalContactDetail(AdditionalContactDTO[] objAdditionalContactDTOList, int ContactID)
        {
            foreach (AdditionalContactDTO additionalContactDTO in objAdditionalContactDTOList)
            {
                IEnumerable<int> enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_ADDITIONAL_DETAIL_INSERT @Name,@Designation,@Email,@ContactNo,@CellNo,@ContactID", new object[6]
                {
                    new SqlParameter("Name", additionalContactDTO.Name ?? Convert.DBNull),
                    new SqlParameter("Designation", additionalContactDTO.Designation ?? Convert.DBNull),
                    new SqlParameter("Email", additionalContactDTO.contactEmail ?? Convert.DBNull),
                    new SqlParameter("ContactNo", additionalContactDTO.ContactNo ?? Convert.DBNull),
                    new SqlParameter("CellNo", additionalContactDTO.ContactCellNo ?? Convert.DBNull),
                    new SqlParameter("ContactID", ContactID)
                }).ToList();
            }
        }

        public void SaveBranchDetail(ContactBranchDetailDTO[] objContactBranchDetailDTOList, int ContactID)
        {
            foreach (ContactBranchDetailDTO contactBranchDetailDTO in objContactBranchDetailDTOList)
            {
                CustomerContactRepository context = _context;
                object[] obj = new object[12]
                {
                    new SqlParameter("BranchName", contactBranchDetailDTO.BranchName ?? Convert.DBNull),
                    new SqlParameter("Address", contactBranchDetailDTO.BranchAddress ?? Convert.DBNull),
                    new SqlParameter("CellNo", contactBranchDetailDTO.BranchCellNo ?? Convert.DBNull),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                int? branchCityID = contactBranchDetailDTO.BranchCityID;
                obj[3] = new SqlParameter("City", branchCityID.HasValue ? ((object)branchCityID.GetValueOrDefault()) : Convert.DBNull);
                obj[4] = new SqlParameter("ContactPerson", contactBranchDetailDTO.BranchContactPerson ?? Convert.DBNull);
                obj[5] = new SqlParameter("Email", contactBranchDetailDTO.BranchEmail ?? Convert.DBNull);
                obj[6] = new SqlParameter("Fax", contactBranchDetailDTO.BranchFax ?? Convert.DBNull);
                branchCityID = contactBranchDetailDTO.BranchStateID;
                obj[7] = new SqlParameter("StateID", branchCityID.HasValue ? ((object)branchCityID.GetValueOrDefault()) : Convert.DBNull);
                obj[8] = new SqlParameter("TaxID", contactBranchDetailDTO.BranchTaxID ?? Convert.DBNull);
                obj[9] = new SqlParameter("TelNo", contactBranchDetailDTO.BranchTelNo ?? Convert.DBNull);
                obj[10] = new SqlParameter("ZipCode", contactBranchDetailDTO.BranchZipCode ?? Convert.DBNull);
                obj[11] = new SqlParameter("ContactID", ContactID);
                IEnumerable<int> enumerable = context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_BRANCH_INSERT @BranchName,@Address,@CellNo,@City,@ContactPerson,@Email,Fax,@StateID,@TaxID,@TelNo,@ZipCode,@ContactID", obj).ToList();
            }
        }

        public void SaveCommodityDetail(ContactCommodityDTO[] objContactCommodityDTOList, int ContactID)
        {
            foreach (ContactCommodityDTO contactCommodityDTO in objContactCommodityDTOList)
            {
                IEnumerable<int> enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_SUB_CATEGORY_TRANSACTION_INSERT @ContactID,@ID,@TableName", new object[3]
                {
                    new SqlParameter("ContactID", ContactID),
                    new SqlParameter("ID", contactCommodityDTO.CommodityID),
                    new SqlParameter("TableName", "Commodity")
                }).ToList();
            }
        }

        public void SaveContactOrigion(ContactOrigionDTO[] objContactOrigionDTOList, int ContactID)
        {
            foreach (ContactOrigionDTO contactOrigionDTO in objContactOrigionDTOList)
            {
                IEnumerable<int> enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_SUB_CATEGORY_TRANSACTION_INSERT @ContactID,@ID,@TableName", new object[3]
                {
                    new SqlParameter("ContactID", ContactID),
                    new SqlParameter("ID", contactOrigionDTO.OrigionID),
                    new SqlParameter("TableName", "Origin")
                }).ToList();
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetContactsUnitList(ListParams listParams)
        {
            try
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int count = 0;
                List<CustomerUnitDTO> list = _context.ExecuteQuery<CustomerUnitDTO>("EXEC dbo.USP_LG_CONTACT_GET_UNIT_LIST @PAGENO, @PAGESIZE,@COMPANYNAME,@CUSTOMERCODE,@Category,@SITE_ID,@REF_SITE_ID,@SFC_ID,@SORTCOLUMN,@SORTORDER", new object[10]
                {
                    new SqlParameter("PAGENO", listParams.PageIndex),
                    new SqlParameter("PAGESIZE", listParams.PageSize),
                    new SqlParameter("COMPANYNAME", dictionary.ContainsKey("companyName") ? dictionary["companyName"] : Convert.DBNull),
                    new SqlParameter("CUSTOMERCODE", dictionary.ContainsKey("customerCode") ? dictionary["customerCode"] : Convert.DBNull),
                    new SqlParameter("Category", dictionary.ContainsKey("contactCategoryID") ? dictionary["contactCategoryID"] : Convert.DBNull),
                    new SqlParameter("SITE_ID", dictionary.ContainsKey("unitId") ? dictionary["unitId"] : Convert.DBNull),
                    new SqlParameter("REF_SITE_ID", dictionary.ContainsKey("refUnitId") ? dictionary["refUnitId"] : Convert.DBNull),
                    new SqlParameter("SFC_ID", dictionary.ContainsKey("sfcID") ? dictionary["sfcID"] : Convert.DBNull),
                    new SqlParameter("SORTCOLUMN", source.First().Key),
                    new SqlParameter("SORTORDER", source.First().Value)
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount.Value;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult UpdateCustomerUnit(CustomerUnitDTO[] objCustomerUnitDTOList)
        {
            try
            {
                List<USP_GET_CUSTOMERCONTACT_DETAIL_Result> entity = null;
                foreach (CustomerUnitDTO customerUnitDTO in objCustomerUnitDTOList)
                {
                    CustomerContactRepository context = _context;
                    object[] obj = new object[6]
                    {
                        new SqlParameter("ContactID", customerUnitDTO.ContactID),
                        new SqlParameter("SITE_ID", customerUnitDTO.SitId),
                        new SqlParameter("REF_SITE_ID", customerUnitDTO.ReferredBySitId),
                        null,
                        null,
                        null
                    };
                    int? sfcID = customerUnitDTO.SfcID;
                    obj[3] = new SqlParameter("SFC_ID", sfcID.HasValue ? ((object)sfcID.GetValueOrDefault()) : Convert.DBNull);
                    sfcID = customerUnitDTO.CreatedBy;
                    obj[4] = new SqlParameter("CreatedBy", sfcID.HasValue ? ((object)sfcID.GetValueOrDefault()) : Convert.DBNull);
                    sfcID = customerUnitDTO.ModifiedBy;
                    obj[5] = new SqlParameter("ModifiedBy", sfcID.HasValue ? ((object)sfcID.GetValueOrDefault()) : Convert.DBNull);
                    IEnumerable<int> enumerable = context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_UNIT_INSERT_UPDATE @ContactID,@SITE_ID,@REF_SITE_ID, @SFC_ID,@CreatedBy, @ModifiedBy", obj).ToList();
                }
                return AppResult(entity, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetFeeCategories()
        {
            try
            {
                List<SPFeeCatagoryDTO> list = _context.ExecuteQuery<SPFeeCatagoryDTO>("EXEC dbo.USP_LG_QUOTATION_GET_SP_FEE_CATEGORY", Array.Empty<object>()).ToList();
                return AppResult(list, list.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetLatestCustomerCode()
        {
            try
            {
                ContactOrigionDTO contactOrigionDTO = new ContactOrigionDTO();
                IEnumerable<string> source = _context.ExecuteQuery<string>("EXEC dbo.USP_LG_CONTACT_GET_CUSTOMER_CODE", Array.Empty<object>()).ToList();
                List<string> list = source.ToList();
                if (list.Count > 0)
                {
                    contactOrigionDTO.CustomerCode = list[0].ToString();
                }
                return AppResult(contactOrigionDTO, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            StandardJsonResult output = new StandardJsonResult
            {
                ResultId = Convert.ToInt32(EnumResult.Failed)
            };
            if (!Request.Content.IsMimeMultipartContent())
            {
                output.AddMessage(HttpContext.GetGlobalResourceObject("CommonTerms", "ErrFileTypeNotSupported").ToString(), clearLastMessages: true);
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, new
                {
                    output
                });
            }
            try
            {
                string path = ConfigurationManager.AppSettings["CustomerPath"];
                string root = HttpContext.Current.Server.MapPath(path);
                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(root);
                MultipartFormDataStreamProvider multipartFormDataStreamProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);
                if (multipartFormDataStreamProvider.FormData.HasKeys())
                {
                    string str = GetUnescapeData(multipartFormDataStreamProvider, "DisplayName").ToString();
                    string str2 = GetUnescapeData(multipartFormDataStreamProvider, "ContactId").ToString();
                    FileInfo[] files = new DirectoryInfo(root).GetFiles(str2 + "_*");
                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i].Delete();
                    }
                    DateTime now = DateTime.Now;
                    new FileInfo(multipartFormDataStreamProvider.FileData.First().LocalFileName).MoveTo(root + "\\" + str);
                    output.AddMessage("File is uploaded successfully.", clearLastMessages: true);
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        output
                    });
                }
                output.AddMessage("Some error found, please contact administrator.", clearLastMessages: true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    output
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                output.AddMessage(ex.Message, clearLastMessages: true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    output
                });
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetAccountCategories()
        {
            try
            {
                List<AccountCatagoriesDTO> list = _context.ExecuteQuery<AccountCatagoriesDTO>("EXEC dbo.USP_LG_CONTACT_GET_ACCOUNT_CATEGORIES", Array.Empty<object>()).ToList();
                return AppResult(list, list.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult GetCities(int id)
        {
            try
            {
                List<SIPLCityDTO> list = _context.ExecuteQuery<SIPLCityDTO>("EXEC dbo.USP_LG_CONTACT_GET_CITIES @StateId", new object[1]
                {
                    new SqlParameter("StateId", id)
                }).ToList();
                return AppResult(list, list.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExportContactReport(Dictionary<string, string> exportParams)
        {
            StandardJsonResult standardJsonResult = new StandardJsonResult
            {
                ResultId = Convert.ToInt32(EnumResult.Failed)
            };
            try
            {
                //string reportPath = "/AppMGL.Report/CustomerContactReport";
                //List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                //list.Add(new KeyValuePair<string, string>("SiteId", exportParams["SiteId"]));
                //ReportServerProxy reportServerProxy = new ReportServerProxy();
                //byte[] array = reportServerProxy.Render(reportPath, list, ReportFormat.XLSX);
                //if (array.Length != 0)
                //{
                //    string text = "CustomerContacts_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";
                //    HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.OK);
                //    httpResponseMessage.Content = new StreamContent(new MemoryStream(array));
                //    httpResponseMessage.Content.Headers.Add("X-FileName", text);
                //    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = text;
                //    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                //    return httpResponseMessage;
                //}
                //standardJsonResult.AddMessage("System cannot generated report, please contact administrator.", clearLastMessages: true);
                //return base.Request.CreateResponse(HttpStatusCode.BadRequest, new
                //{
                //    output = standardJsonResult
                //});
                // end old code

                // Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(exportParams["Filter"]);
                string reportPath = "/AppMGL.Report/CustomerContactReport_New";
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("CompanyName", string.IsNullOrEmpty(exportParams["companyName"]) == false ? exportParams["companyName"] : null));
                list.Add(new KeyValuePair<string, string>("CustomerCode", string.IsNullOrEmpty(exportParams["customerCode"]) == false ? exportParams["customerCode"] : null));
                list.Add(new KeyValuePair<string, string>("MiamiRep", string.IsNullOrEmpty(exportParams["galRepresentative"]) == false ? exportParams["galRepresentative"] : null));
                list.Add(new KeyValuePair<string, string>("Category", string.IsNullOrEmpty(exportParams["contactCategoryID"]) == false ? exportParams["contactCategoryID"] : null));
                list.Add(new KeyValuePair<string, string>("ForwarderNetwork", string.IsNullOrEmpty(exportParams["forwarderNetworkId"]) == false ? exportParams["forwarderNetworkId"] : null));

                list.Add(new KeyValuePair<string, string>("AttachedSite", string.IsNullOrEmpty(exportParams["attachedsiteId"]) == false ? exportParams["attachedsiteId"] : null));

                list.Add(new KeyValuePair<string, string>("CompanyGradation", string.IsNullOrEmpty(exportParams["companyGradation"]) == false ? exportParams["companyGradation"] : null));
                list.Add(new KeyValuePair<string, string>("OriginCountries", string.IsNullOrEmpty(exportParams["OriginCountry"]) == false ? exportParams["OriginCountry"] : null));
                list.Add(new KeyValuePair<string, string>("Commodity", string.IsNullOrEmpty(exportParams["commodity"]) == false ? exportParams["commodity"] : null));
                // list.Add(new KeyValuePair<string, string>("Region", exportParams["Region"] != null ? exportParams["Region"] : string.Empty));
                // list.Add(new KeyValuePair<string, string>("Country", exportParams["Country"] != null ? exportParams["Country"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("Region", string.IsNullOrEmpty(exportParams["continent"]) == false ? exportParams["continent"] : null));
                list.Add(new KeyValuePair<string, string>("Country", string.IsNullOrEmpty(exportParams["cryName"]) == false ? exportParams["cryName"] : null));
                list.Add(new KeyValuePair<string, string>("State", string.IsNullOrEmpty(exportParams["state"]) == false ? exportParams["state"] : null));
                list.Add(new KeyValuePair<string, string>("City", string.IsNullOrEmpty(exportParams["city"]) == false ? exportParams["city"] : null));

                // list.Add(new KeyValuePair<string, string>("IsVendor", "0"));
                list.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SiteId"]));
                list.Add(new KeyValuePair<string, string>("SORTCOLUMN", "CompanyName"));
                list.Add(new KeyValuePair<string, string>("SORTORDER", "desc"));
                list.Add(new KeyValuePair<string, string>("IsSepatraCustomer", string.IsNullOrEmpty(exportParams["IsSepatraCustomer"]) == false ? exportParams["IsSepatraCustomer"] : null));
                list.Add(new KeyValuePair<string, string>("IsSepatraPartner", string.IsNullOrEmpty(exportParams["IsSepatraPartner"]) == false ? exportParams["IsSepatraPartner"] : null));

                ReportServerProxy reportServerProxy = new ReportServerProxy();
                byte[] array = reportServerProxy.Render(reportPath, list, ReportFormat.XLSX);
                if (array.Length != 0)
                {
                    string text = "CustomerContacts_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";
                    HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.OK);
                    httpResponseMessage.Content = new StreamContent(new MemoryStream(array));
                    httpResponseMessage.Content.Headers.Add("X-FileName", text);
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = text;
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
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

        [System.Web.Http.HttpGet]
        public virtual ActionResult getQuickBookContactDetail(int id, int userid)
        {
            try
            {
                List<CustomerQuickbookCreate> list = _context.ExecuteQuery<CustomerQuickbookCreate>("EXEC dbo.LG_QUICKBOOK_CUSTOMER_CREATE @CustID", new object[1]
                {
                    new SqlParameter("CustID", id)
                }).ToList();
                CustomerQuickbookCreate data = list[0];
                return AppResult(data, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult Quckbookcustomercreates(int id)
        {
            try
            {
                int num = Convert.ToInt32(base.Request.Headers.GetValues("Userid").ToList()[0]);
                List<CustomerQuickbookCreate> list = _context.ExecuteQuery<CustomerQuickbookCreate>("EXEC dbo.LG_QUICKBOOK_CUSTOMERAPI_ADDUPD @CustID ,@userid", new object[2]
                {
                    new SqlParameter("CustID", id),
                    new SqlParameter("userid", num)
                }).ToList();
                CustomerQuickbookCreate customerQuickbookCreate = new CustomerQuickbookCreate();
                customerQuickbookCreate.QBStatus = list[0].QBStatus;
                customerQuickbookCreate.QBResult = list[0].QBResult;
                if (customerQuickbookCreate.QBStatus == 200)
                {
                    return AppResult(list[0], 1L, customerQuickbookCreate.QBResult, EnumResult.Success);
                }
                return AppResult(list[0], 1L, customerQuickbookCreate.QBResult, EnumResult.Failed);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult QuckbookVendorcreates(int id)
        {
            try
            {
                int num = Convert.ToInt32(base.Request.Headers.GetValues("Userid").ToList()[0]);
                List<CustomerQuickbookCreate> list = _context.ExecuteQuery<CustomerQuickbookCreate>("EXEC dbo.LG_QUICKBOOK_VENDORRAPI_ADDUPD @CustID ,@userid", new object[2]
                {
                    new SqlParameter("CustID", id),
                    new SqlParameter("userid", num)
                }).ToList();
                CustomerQuickbookCreate customerQuickbookCreate = new CustomerQuickbookCreate();
                customerQuickbookCreate.QBStatus = list[0].QBStatus;
                customerQuickbookCreate.QBResult = list[0].QBResult;
                if (customerQuickbookCreate.QBStatus == 200)
                {
                    return AppResult(list[0], 1L, customerQuickbookCreate.QBResult, EnumResult.Success);
                }
                return AppResult(list[0], 1L, customerQuickbookCreate.QBResult, EnumResult.Failed);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult CheckDuplicate(CustomerContactDTO dto)
        {
            try
            {
                IEnumerable<int> source = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CUSTOMERCONTACT_DUPLICATE @ContactID,@CompanyName", new object[2]
                {
                    new SqlParameter("ContactID", dto.ContactID),
                    new SqlParameter("CompanyName", dto.CompanyName)
                }).ToList();
                List<int> entity = source.ToList();
                return AppResult(entity, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult CompanySearch(ListParams listParams)
        {
            try
            {

                // int count;
                var result = _context.ExecuteQuery<Company>("EXEC dbo.SP_Contact_Search @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult CompanySearchCRM(ListParams listParams)
        {
            try
            {

                // int count;
                var result = _context.ExecuteQuery<Company>("EXEC dbo.SP_Contact_Search_CRM @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult CompanySearchSeptara(ListParams listParams)
        {
            try
            {

                // int count;
                var result = _context.ExecuteQuery<CompanySepatra>("EXEC dbo.SP_Contact_Search_CRM_Septara @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult SiteList(ListParams listParams)
        {
            try
            {
                var result = _context.ExecuteQuery<SiteList>("EXEC dbo.USP_LG_GETAll_SITES").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult SendEmail(FranchiseEmailData emailData)
        {
            try
            {

                List<string> EmailList = null;
                EmailList = new List<string>();
                var userDetail = _context.ExecuteQuery<LG_USER>("EXEC dbo.GetUserDetailBYID @UserID",
                                  new SqlParameter("UserID", emailData.CreatedBy)).FirstOrDefault();
                userDetail.UsrSmtpPassword = SecurityHelper.DecryptString(userDetail.UsrSmtpPassword, "sblw-3hn8-sqoy19");
                List<int> objList = new List<int>(); objList.Add(1);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(userDetail.UsrSmtpUsername);
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

                mailMessage.Subject = emailData.EmailSubject;
                mailMessage.Body = emailData.EmailBody.Replace("src='Images/", "src='" + ConfigurationManager.AppSettings["WebPath"] + "Images/"); ;
                mailMessage.IsBodyHtml = true;
                string shtml = "<html><head><body> <table><tr><td>" + mailMessage.Body + "</td></tr></table></body></head></html>";
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                EmailHelper.Send(principal, mailMessage);

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult getEmailDetail(int id)
        {
            try
            {
                List<FranchiseEmailData> list1 = _context.ExecuteQuery<FranchiseEmailData>("EXEC dbo.LG_GetEmailDetail_SP @ContactId", new object[1]
                {
                    new SqlParameter("ContactId", id)
                }).ToList();

                string shtml = "<p><span style='font - size:11.0pt'><span style='font - family:&quot; Calibri & quot;,sans - serif'>Dear Sir ,</span></span></p>";
                shtml += "<p style ='margin-left:0cm; margin-right:0cm'><span style ='font-size:11pt'><span style='font-family:Calibri,sans-serif'> We thank you very much for contacting us for handling your above - mentioned shipment.</span></span></p>";
                shtml += "<p><span style ='font-size:11.0pt'><span> We have assigned the following team to handle your shipment </span></span></p>";
                shtml += "<p>&nbsp;</p>";
                shtml += "<table border ='1' cellspacing ='0' class='Table' style='border-collapse:collapse; border:solid black 1.0pt; width:515.75pt'>";
                shtml += "<tbody>";
                shtml += "<tr>";
                shtml += "<td style ='height:16.65pt; width:116.45pt''>";

                shtml += "<p style='margin-left:0cm; margin-right:0cm'><span style ='font-size:11pt;><span style='font-family:Calibri,sans-serif'><span style ='font-size:12.0pt'><span style='color:#1f487c'>EXT NBR</span></span></span></span></p>";
                shtml += "</td>";
                shtml += "<td style ='height:16.65pt; width:91.4pt'>";

                shtml += "<p style='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'><span style='font-size:12.0pt'><span style='color:#1f487c'> NAME </span></span></span></span></p>";

                shtml += "</td>";

                shtml += "<td style='height:16.65pt; width:100.05pt'>";

                shtml += "<p style='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'><span style='font-size:12.0pt'><span style='color:#1f487c'> JOB DESCRIPTION</span></span></span></span></p>";
                shtml += "</td>";
                shtml += "<td style ='height:16.65pt; width:207.85pt'>";

                shtml += "<p style='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'><span style='font-size:12.0pt'><span style='color:#1f487c'> Email </span></span></span></span></p>";

                shtml += "</td>";

                shtml += "</tr>";
                string tblhtml = "";
                string ccemail = "";
                if (list1.Count > 0)
                {
                    foreach (var li in list1)
                    {
                        if (!string.IsNullOrEmpty(li.CntEmail))
                        {
                            ccemail = ccemail == "" ? li.CntEmail : ccemail + ';' + li.CntEmail;
                        }
                        tblhtml += "<tr>";

                        tblhtml += "<td style='height:32.6pt; width:116.45pt'>";

                        tblhtml += "<p style='margin-left:0cm; margin-right:0cm; text-align:right'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'><span style='font-size:12.0pt'><span style='color:#1f487c'> " + li.TelNo + " </span></span></span></span></p>";

                        tblhtml += "</td>";

                        tblhtml += "<td style='height:32.6pt; width:91.4pt'>";

                        tblhtml += "<p style='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'><span style='font-size:12.0pt'><span style='color:#1f487c'> " + li.CntName + " </span></span></span></span></p>";

                        tblhtml += "</td>";

                        tblhtml += "<td style='height:32.6pt; width:100.05pt'>";

                        tblhtml += "<p style='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'><span style='font-size:12.0pt'><span style='color:#1f487c'> " + li.JobRole + "</span></span></span></span></p>";
                        tblhtml += "</td>";
                        tblhtml += "<td style ='height:32.6pt; width:207.85pt'>";

                        tblhtml += "<p style='margin-left:0cm; margin-right:0cm'><span style ='font-size:11pt'><span style='font-family:Calibri,sans-serif'><a href ='" + li.CntEmail + "'><span style='color:#0563c1'>" + li.CntEmail + "</span></a></span></span></p>";
                        tblhtml += "</td>";
                        tblhtml += "</tr>";
                    }
                }
                tblhtml += "</tbody>";
                tblhtml += "</table>";
                shtml += tblhtml;
                shtml += "<p>&nbsp;</p>";

                shtml += "<p style ='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style ='font-family:Calibri,sans-serif'> If for any reason you are not satisfied with the service provided by the team or should you have any reason to escalate the matter, please feel free to send mail to the following addresses</span></span></p>";

                shtml += "<p style ='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'> CEO & nbsp;&nbsp;&nbsp;&nbsp; <a href ='mailto:hamidh@miamigloballines.com' style='color:#0563c1; text-decoration:underline'>hamidh @miamigloballines.com</a></span></span></p>";

                shtml += "<p style ='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'> CRO & nbsp;&nbsp;&nbsp;&nbsp; <a href ='mailto:allanw@miamigloballines.com' style='color:#0563c1; text-decoration:underline'>allanw @miamigloballines.com</a></span></span></p>";

                shtml += "<p style ='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style='font-family:Calibri,sans-serif'> COO & nbsp;&nbsp;&nbsp;&nbsp; <a href ='mailto:kirtic@miamigloballines.com' style='color:#0563c1; text-decoration:underline'>kirtic @miamigloballines.com</a></span></span></p>";

                shtml += "<p style ='margin-left:0cm; margin-right:0cm'> &nbsp;</p>";

                shtml += "<p style ='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style ='font-family:Calibri,sans-serif'> Thanks & regards</span></span></p>";

                shtml += "<p style ='margin-left:0cm; margin-right:0cm'><span style='font-size:11pt'><span style ='font-family:Calibri,sans-serif'> Inside Sales</span></span></p>";

                string shtml1 = "<html><head><body> <div>" + shtml + "</div></body></head></html>";

                list1[0].EmailBody = shtml1;
                list1[0].Emailcc = ccemail;
                list1[0].EmailSubject = "Assignment of dedicated team for handling your enquiry";
                return AppResult(list1, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public ActionResult gridConfig(string processName, string gridGuid, string viewName = "")
        {
            string vname = "";
            string accessToken = "";
            if (string.IsNullOrEmpty(viewName))
            {
                vname = "View 1";
            }
            else
            {
                viewName = viewName.TrimStart().TrimEnd();
                vname = viewName;
            }
            try
            {
                // var objAppBL = new CommonUtility.BL.CommonBL();
                gridGuid = string.IsNullOrEmpty(gridGuid) ? "MCompContainer" : gridGuid;


                List<C2M_API_McompGetGridViews> list = _context.ExecuteQuery<C2M_API_McompGetGridViews>("EXEC dbo.C2M_API_McompGetGridViews @V_UserName, @V_PROCESSNM,@var_containerID,@var_configtype,@var_viewName", new object[5]
                {
                     new SqlParameter("V_UserName", accessToken),
                     new SqlParameter("V_PROCESSNM", processName),
                     new SqlParameter("var_containerID", gridGuid),
                     new SqlParameter("var_configtype", 1),
                     new SqlParameter("var_viewName", vname)
                }).ToList();

                List<C2M_API_McompGetGridViews_New> listnew = new List<C2M_API_McompGetGridViews_New>();
                if (string.IsNullOrEmpty(viewName))
                {
                    listnew = (from t in list
                               select new C2M_API_McompGetGridViews_New
                               {
                                   Config = (t.defaultview) == "1" ? (t.CONFIGJSON.ToString()) : string.Empty,
                                   Viewname = t.viewname.ToString(),
                                   IsDefaultview = (t.defaultview) == "1" ? true : false,
                                   MasterConfig = (t.defaultview) == "1" ? (t.MasterConfig.ToString()) : string.Empty,
                                   Viewnames = t.Views.ToString()
                               }).ToList();

                }
                else
                {
                    listnew = (from t in list
                               where t.viewname.ToString().ToLower() == viewName.ToLower()
                               select new C2M_API_McompGetGridViews_New
                               {
                                   Config = (t.CONFIGJSON.ToString()),
                                   Viewname = t.viewname.ToString(),
                                   IsDefaultview = (t.defaultview) == "1" ? true : false,
                                   MasterConfig = (t.MasterConfig.ToString()),
                                   Viewnames = t.Views.ToString()
                               }).ToList();

                }




                int count = 0;



                if (listnew.Count > 0)
                {
                    count = listnew.Count;
                }
                return AppResult(listnew, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult PostGridConfig(string processName, MGridConfig mGridConfig)
        {


            string accessToken = "";
            try
            {
                List<C2M_API_McompGetGridViews> list = _context.ExecuteQuery<C2M_API_McompGetGridViews>("EXEC dbo.C2M_API_McompConfigInsUpd @V_ACCESSTOKEN, @V_PROCESSNM,@var_containerID,@var_configtype,@var_finalXml,@var_ViewName,@var_IsDefault,@var_OldViewName", new object[8]
                 {

               new SqlParameter("V_ACCESSTOKEN", accessToken),
                new SqlParameter("V_PROCESSNM", processName),
                new SqlParameter("var_containerID", mGridConfig.ContainerID),
                new SqlParameter("var_configtype", 1),
                new SqlParameter("var_finalXml", mGridConfig.FinalJson),
                new SqlParameter("var_ViewName", mGridConfig.ViewName),
                new SqlParameter("var_IsDefault", Convert.ToInt32( mGridConfig.IsDefaultView)),
                new SqlParameter("var_OldViewName", string.IsNullOrEmpty(mGridConfig.OldViewName) ? "New" : mGridConfig.OldViewName)


              }).ToList();



                int count = 0;
                if (list.Count > 0)
                {
                    count = list.Count;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpPost]

        public ActionResult GetAllCompanyList(GridConfiguration model)
        {
            try
            {
                // var columnlist = Request.Headers.GetValues("colums").ToList()[0];

                int groupId = 0;
                string UserId = "222";
                if (!string.IsNullOrWhiteSpace(model.EncryptedGroupId))
                {

                    // int.TryParse(EncryptDecrypt.Decrypt(model.EncryptedGroupId), out groupId);
                }
                else
                {
                    //  int.TryParse(Request.Headers["groupId"].ToString(), out groupId);
                }
                //  object dsUser;

                var dsUser = GetAllManageUserList(model, UserId, groupId);
                return AppResult(dsUser, "10000");

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        public gridResultDdata GetAllManageUserList(GridConfiguration gridConfiguration, string userId, int groupID)
        {
            try
            {
                var columnList = new string[] { "ContactID", "CompanyName", "CreatedOn", "TelNo", "Address", "MobNo", "Fax" };
                var defaultSortCol = "CreatedOn";
                string conditions = "";
                var rData = new GridManagerData
                {
                    PageNumber = gridConfiguration.PageNumber,
                    PageSize = gridConfiguration.PageSize,
                    SortColumn = string.IsNullOrEmpty(gridConfiguration.SortColumn) ? defaultSortCol : gridConfiguration.SortColumn,
                    SortDirection = gridConfiguration.SortOrder.ToString(),
                    TimeZone = gridConfiguration.TimeZone,
                };

                if (gridConfiguration.GridFilters != null && gridConfiguration.GridFilters.Count > 0)
                {
                    foreach (var item in gridConfiguration.GridFilters)
                    {
                        var rConditions = "";
                        if (item.FilterType != FilterType.Global_Search)
                        {
                            foreach (var condition in item.GridConditions)
                            {
                                rConditions += $"{item.DataField} {GridHelper.MapOperator(item.DataField, condition.Condition, condition.ConditionValue)} {((item.GridConditions.IndexOf(condition) + 1 < item.GridConditions.Count) ? (!string.IsNullOrEmpty(item.LogicalOperator.ToString()) ? item.LogicalOperator.ToString() : "Or") : string.Empty)} ";
                            }
                        }
                        else
                        {
                            foreach (var condition in item.GridConditions)
                            {
                                if (!string.IsNullOrEmpty(condition.ConditionValue))
                                {
                                    columnList = columnList.Select(p => { p = p + " Like('%" + condition.ConditionValue + "%')"; return p; }).ToArray();
                                    rConditions = string.Join(" or ", columnList);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(rConditions))
                        {
                            conditions += $"({rConditions});";
                        }
                    }
                }
                rData.Condition = string.IsNullOrEmpty(conditions) ? " " : string.Join(" AND ", conditions.Trim().Trim(';').Replace("''", "'").Split(';'));
                rData.PageNumber = rData.PageNumber == 0 ? 0 : (rData.PageNumber) - 1;
                if (!string.IsNullOrWhiteSpace(rData.Condition))
                {
                    rData.Condition = " And " + rData.Condition;
                }
                List<ContactListAngular> objlist = GetAllManageUserListDL(rData, userId, groupID);
                if (objlist != null && objlist.Count > 0)
                {
                    // vikas

                    List<Dictionary<string, string>> lstApp = new List<Dictionary<string, string>>();
                    // DataTable dtProcess = ds.Tables[0];
                    PropertyInfo[] propertyInfos;
                    propertyInfos = typeof(ContactListAngular).GetProperties(BindingFlags.Public |
                                                                  BindingFlags.Static);
                    // sort properties by name
                    Array.Sort(propertyInfos,
                            delegate (PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
                            { return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });

                    // write property names
                    foreach (var item in objlist)
                    {

                        Type type = item.GetType();
                        PropertyInfo[] props = type.GetProperties();
                        Dictionary<string, string> dictApp = new Dictionary<string, string>();
                        foreach (var prop in props)
                        {
                            string p = prop.Name.ToString();
                            string v = Convert.ToString(prop.GetValue(item));
                            dictApp.Add(p, v);

                        }
                        lstApp.Add(dictApp);
                    }
                    gridResultDdata obgriddata = new gridResultDdata();
                    obgriddata.Data = lstApp;
                    obgriddata.RecordsCount = "1000";
                    obgriddata.Begin = "1";
                    obgriddata.End = "50";
                    obgriddata.PageNumber = "1";
                    return obgriddata;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        private static string GetPropertyValues(ContactListAngular company)
        {
            Type type = company.GetType();
            PropertyInfo[] props = type.GetProperties();
            string str = "{";
            foreach (var prop in props)
            {
                str += (prop.Name + ":" + prop.GetValue(company)) + ",";
            }
            return str.Remove(str.Length - 1) + "}";
        }

        private List<ContactListAngular> GetAllManageUserListDL(GridManagerData gridConfiguration, string userId, int groupId)
        {

            // string GetDocumentList { get { return "EXEC dbo.LG_GET_DOCUMENT_LIST_New @PageIndex, @PageSize, @Sort, @SitId, @DocumentStatus,@Count OUT,@FileNo"; } }
            int count = 0;

            List<SqlParameter> list = new List<SqlParameter>
            {
                   new SqlParameter("p_pageNumber", gridConfiguration.PageNumber+1),
                new SqlParameter("p_sortcolumn", gridConfiguration.SortColumn),
                new SqlParameter("p_sortorder", gridConfiguration.SortDirection),
               //  new SqlParameter("p_Count", count),

                  new SqlParameter("p_Count", SqlDbType.Int)
                  {
                      Direction = ParameterDirection.Output
                  },
                  new SqlParameter("v_condition", gridConfiguration.Condition),
                    new SqlParameter("p_pagesize", gridConfiguration.PageSize)
            };


            var result = _context.ExecuteQuery<ContactListAngular>("EXEC dbo.GetAllCompanyList @p_pagesize,@p_pageNumber,@p_sortcolumn,@p_sortorder,@p_Count OUT,@v_condition", list.ToArray()).ToList();
            count = Utility.GetParamValue(list, "p_Count", typeof(int));

            return result;

        }
        //[HttpDelete]
        //[Route("deleteGridConfig")]
        //public ActionResult DeleteGridConfig([FromHeader] string accessToken, string processName, string gridGuid, string viewName = "")
        //{
        //    try
        //    {
        //        var objAppBL = new CommonUtility.BL.CommonBL();
        //        var result = objAppBL.DeleteGridConfig(accessToken, processName, gridGuid, viewName);
        //        if (result > 0)
        //        {
        //            return Ok(true);
        //        }
        //        else
        //        {
        //            return NoContent();
        //        }
        //    }
        //    catch (Exception subEx)
        //    {
        //        Models.ApiException objApiError = new Models.ApiException("DeleteGridConfig");
        //        objApiError.APiLog(accessToken, _appSettings.DomainUrl, "Exception", "DeleteGridConfig", "", subEx.Message);

        //        return BadRequest("Invalid configuration, please try again.");
        //    }
        //}
        [System.Web.Http.HttpPost]
        public virtual ActionResult saveCustomerContactAddress(CustomerContactDTO objContactDTO)
        {
            try
            {

                //IEnumerable<int> source1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CUSTOMERCONTACT_DUPLICATE @ContactID,@CompanyName", new object[2]
                //{
                //    new SqlParameter("ContactID", objContactDTO.ContactID),
                //    new SqlParameter("CompanyName", objContactDTO.CompanyName)
                //}).ToList();
                //List<int> entity = source1.ToList();
                //if (entity.Count > 0)
                //{
                //    return AppResult(entity, 1L, "Duplicate Contact! Contact Already Exist, To Add New Contact Category, Please Edit existing Contact.", EnumResult.Failed);
                //}


                string text = "";
                CustomerContactDTO data = null;
                text = ((objContactDTO.ID != 0) ? "update" : "insert");
                CustomerContactRepository context = _context;
                object[] obj = new object[39]
                {

                    new SqlParameter("ContactID", objContactDTO.ContactID),
                    new SqlParameter("Address", objContactDTO.Address ?? Convert.DBNull),
                    new SqlParameter("CellNo", objContactDTO.CellNo ?? Convert.DBNull),
                    new SqlParameter("CompanyName", objContactDTO.CompanyName?? Convert.DBNull),
                    new SqlParameter("ContactCategoryID", objContactDTO.ContactCategoryID ?? Convert.DBNull),
                    new SqlParameter("ForwarderNetworkId", objContactDTO.ForwarderNetworkId ?? Convert.DBNull),
                    new SqlParameter("ContactPerson", objContactDTO.ContactPerson ?? Convert.DBNull),
                    new SqlParameter("Email", objContactDTO.Email ?? Convert.DBNull),
                    new SqlParameter("Fax", objContactDTO.Fax ?? Convert.DBNull),
                    new SqlParameter("Remarks", objContactDTO.Remarks ?? Convert.DBNull),
                    new SqlParameter("TelNo", objContactDTO.TelNo ?? Convert.DBNull),
                    new SqlParameter("WebSite", objContactDTO.WebSite ?? Convert.DBNull),
                    new SqlParameter("ZipCode", objContactDTO.ZipCode ?? Convert.DBNull),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,null,null,null,null,null,null,null
                };
                int? createdBy = objContactDTO.CreatedBy;
                obj[13] = new SqlParameter("CreatedBy", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.ModifiedBy;
                obj[14] = new SqlParameter("ModifiedBy", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                DateTime? modifiedOn = objContactDTO.ModifiedOn;
                obj[15] = new SqlParameter("ModifiedOn", modifiedOn.HasValue ? ((object)modifiedOn.GetValueOrDefault()) : Convert.DBNull);
                obj[16] = new SqlParameter("Type", text);
                obj[17] = new SqlParameter("AccountDetail", objContactDTO.AccountDetail ?? Convert.DBNull);
                obj[18] = new SqlParameter("Attachment", objContactDTO.Attachment ?? Convert.DBNull);
                obj[19] = new SqlParameter("CompanyGradation", objContactDTO.companyGradation ?? Convert.DBNull);
                obj[20] = new SqlParameter("CustomerCode", objContactDTO.CustomerCode ?? Convert.DBNull);
                //createdBy = Convert.ToInt32( objContactDTO.RepresentativeID);
                obj[21] = new SqlParameter("RepID", objContactDTO.RepresentativeID ?? Convert.DBNull);
                createdBy = objContactDTO.ContinentId;
                obj[22] = new SqlParameter("ContinentId", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.CountryId;
                obj[23] = new SqlParameter("CountryId", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.StateId;
                obj[24] = new SqlParameter("StateID", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.CityId;
                obj[25] = new SqlParameter("CityID", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                createdBy = objContactDTO.AcyID;
                obj[26] = new SqlParameter("AcyID", createdBy.HasValue ? ((object)createdBy.GetValueOrDefault()) : Convert.DBNull);
                obj[27] = new SqlParameter("SIT_ID", objContactDTO.SiteId);
                obj[28] = new SqlParameter("IDNumberType", objContactDTO.IDNumberTypeValue ?? Convert.DBNull);
                obj[29] = new SqlParameter("IDNumber", objContactDTO.IDNumber ?? Convert.DBNull);
                obj[30] = new SqlParameter("IRSNumber", objContactDTO.IRSNumber ?? Convert.DBNull);
                obj[31] = new SqlParameter("isconsolidatedreport", objContactDTO.isconsolidatedreport == null ? 0 : objContactDTO.isconsolidatedreport);
                obj[32] = new SqlParameter("scac", objContactDTO.SCAC == null ? "" : objContactDTO.SCAC);
                obj[33] = new SqlParameter("iata", objContactDTO.IATA == null ? "" : objContactDTO.IATA);
                obj[34] = new SqlParameter("ContactPersonFirstName", objContactDTO.ContactPersonFirstName == null ? "" : objContactDTO.ContactPersonFirstName);
                obj[35] = new SqlParameter("ContactPersonLastName", objContactDTO.ContactPersonLastName == null ? "" : objContactDTO.ContactPersonLastName);
                obj[36] = new SqlParameter("EnquiryUrl", objContactDTO.EnquiryUrl == null ? "" : objContactDTO.EnquiryUrl);
                obj[37] = new SqlParameter("siteids", objContactDTO.siteids == null ? "" : objContactDTO.siteids);
                obj[38] = new SqlParameter("ID", objContactDTO.ID == null ? 0 : objContactDTO.ID);
                IEnumerable<int> source = context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_ADDRESS_INSERT_UPDATE  @ContactID ,@Address,@CellNo,@CompanyName,@ContactCategoryID,@ForwarderNetworkId, @ContactPerson, @Email, @Fax, @Remarks, @TelNo, @WebSite, @ZipCode,@CreatedBy, @ModifiedBy, @ModifiedOn, @Type, @AccountDetail, @Attachment, @CompanyGradation, @CustomerCode,@RepID,@ContinentId,@CountryId,@StateID,@CityID,@AcyID,@SIT_ID,@IDNumberType,@IDNumber,@IRSNumber,@isconsolidatedreport,@scac,@iata,@ContactPersonFirstName,@ContactPersonLastName,@EnquiryUrl,@siteids,@ID", obj).ToList();
                List<int> list = source.ToList();
                int num = list[0];
                if (num > 0)
                {
                    objContactDTO.ID = num;

                }
                return AppResult(objContactDTO, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult ContactListAddress(ListParams listParams)
        {
            try
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Filter);
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int count = 0;
                List<USP_GET_CUSTOMERCONTACT_LIST_Result> list = _context.ExecuteQuery<USP_GET_CUSTOMERCONTACT_LIST_Result>("EXEC dbo.USP_GET_CUSTOMERCONTACT_LIST_Address @PAGENO, @PAGESIZE,@Address,@SORTCOLUMN,@SORTORDER,@contactid", new object[6]
                {
                    new SqlParameter("PAGENO", listParams.PageIndex),
                    new SqlParameter("PAGESIZE", listParams.PageSize),
                    new SqlParameter("Address", dictionary.ContainsKey("address") ? dictionary["address"] : string.Empty),
                    new SqlParameter("SORTCOLUMN", source.First().Key),
                    new SqlParameter("SORTORDER", source.First().Value),
                    new SqlParameter("contactid", dictionary.ContainsKey("contactid") ? dictionary["contactid"] : "0")
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount.Value;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult getContactDetailAddress(int id)
        {
            try
            {
                List<CustomerContactDTO> list = _context.ExecuteQuery<CustomerContactDTO>("EXEC dbo.USP_LG_CONTACT_GET_DETAIL_Address @ID", new object[1]
                {
                    new SqlParameter("ID", id)
                }).ToList();
                CustomerContactDTO customerContactDTO = list[0];
                customerContactDTO.isconsolidatedreport = (customerContactDTO.isconsolidatedreport == null ? 0 : customerContactDTO.isconsolidatedreport);
                if (list[0].ContactID > 0)
                {

                }
                return AppResult(customerContactDTO, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult deleteContactDetailAddress(int id)
        {
            try
            {
                List<CustomerContactDTO> list = _context.ExecuteQuery<CustomerContactDTO>("EXEC dbo.USP_LG_CONTACT_Address_Delete @ID", new object[1]
                {
                    new SqlParameter("ID", id)
                }).ToList();

                return AppResult(id, "Successfully deleted");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }



        [System.Web.Http.HttpPost]
        public ActionResult checkisapprovedauthorization(ListParams listParams)
        {
            string isapproved = "0";
            try
            {

                IEnumerable<int> source1 = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CHECK_ISApproved_Authorization @userid", new object[1]
                {

                             new SqlParameter("userid", listParams.UserId)
                }).ToList();
                List<int> entity = source1.ToList();
                if (entity[0] >= 2)
                {
                    isapproved = "1";
                    return AppResult(isapproved, isapproved);
                }


            }
            catch (Exception ex)
            {
                return AppResult(isapproved, isapproved);
            }

            return AppResult(isapproved, isapproved);
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage exportContactReport_CRM(Dictionary<string, string> exportParams)
        {
            StandardJsonResult standardJsonResult = new StandardJsonResult
            {
                ResultId = Convert.ToInt32(EnumResult.Failed)
            };
            try
            {
                //string reportPath = "/AppMGL.Report/CustomerContactReport";
                //List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                //list.Add(new KeyValuePair<string, string>("SiteId", exportParams["SiteId"]));
                //ReportServerProxy reportServerProxy = new ReportServerProxy();
                //byte[] array = reportServerProxy.Render(reportPath, list, ReportFormat.XLSX);
                //if (array.Length != 0)
                //{
                //    string text = "CustomerContacts_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";
                //    HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.OK);
                //    httpResponseMessage.Content = new StreamContent(new MemoryStream(array));
                //    httpResponseMessage.Content.Headers.Add("X-FileName", text);
                //    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = text;
                //    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                //    return httpResponseMessage;
                //}
                //standardJsonResult.AddMessage("System cannot generated report, please contact administrator.", clearLastMessages: true);
                //return base.Request.CreateResponse(HttpStatusCode.BadRequest, new
                //{
                //    output = standardJsonResult
                //});
                // end old code

                // Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(exportParams["Filter"]);
                string reportPath = "/AppMGL.Report/CustomerContactReport_New_CRM";
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("CompanyName", string.IsNullOrEmpty(exportParams["companyName"]) == false ? exportParams["companyName"] : null));
                list.Add(new KeyValuePair<string, string>("CustomerCode", string.IsNullOrEmpty(exportParams["customerCode"]) == false ? exportParams["customerCode"] : null));
                list.Add(new KeyValuePair<string, string>("MiamiRep", string.IsNullOrEmpty(exportParams["galRepresentative"]) == false ? exportParams["galRepresentative"] : null));
                list.Add(new KeyValuePair<string, string>("Category", string.IsNullOrEmpty(exportParams["contactCategoryID"]) == false ? exportParams["contactCategoryID"] : null));
                list.Add(new KeyValuePair<string, string>("ForwarderNetwork", string.IsNullOrEmpty(exportParams["forwarderNetworkId"]) == false ? exportParams["forwarderNetworkId"] : null));

                list.Add(new KeyValuePair<string, string>("AttachedSite", string.IsNullOrEmpty(exportParams["attachedsiteId"]) == false ? exportParams["attachedsiteId"] : null));

                list.Add(new KeyValuePair<string, string>("CompanyGradation", string.IsNullOrEmpty(exportParams["companyGradation"]) == false ? exportParams["companyGradation"] : null));
                list.Add(new KeyValuePair<string, string>("OriginCountries", string.IsNullOrEmpty(exportParams["OriginCountry"]) == false ? exportParams["OriginCountry"] : null));
                list.Add(new KeyValuePair<string, string>("Commodity", string.IsNullOrEmpty(exportParams["commodity"]) == false ? exportParams["commodity"] : null));
                // list.Add(new KeyValuePair<string, string>("Region", exportParams["Region"] != null ? exportParams["Region"] : string.Empty));
                // list.Add(new KeyValuePair<string, string>("Country", exportParams["Country"] != null ? exportParams["Country"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("Region", string.IsNullOrEmpty(exportParams["continent"]) == false ? exportParams["continent"] : null));
                list.Add(new KeyValuePair<string, string>("Country", string.IsNullOrEmpty(exportParams["cryName"]) == false ? exportParams["cryName"] : null));
                list.Add(new KeyValuePair<string, string>("State", string.IsNullOrEmpty(exportParams["state"]) == false ? exportParams["state"] : null));
                list.Add(new KeyValuePair<string, string>("City", string.IsNullOrEmpty(exportParams["city"]) == false ? exportParams["city"] : null));
                // list.Add(new KeyValuePair<string, string>("IsVendor", "0"));
                list.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SiteId"]));
                list.Add(new KeyValuePair<string, string>("SORTCOLUMN", "CompanyName"));
                list.Add(new KeyValuePair<string, string>("SORTORDER", "desc"));

                list.Add(new KeyValuePair<string, string>("ForwarderNetwork", string.IsNullOrEmpty(exportParams["forwarderNetworkId"]) == false ? exportParams["forwarderNetworkId"] : null));
                list.Add(new KeyValuePair<string, string>("AttachedSite", string.IsNullOrEmpty(exportParams["attachedsiteId"]) == false ? exportParams["attachedsiteId"] : null));
                list.Add(new KeyValuePair<string, string>("IsSepatraCustomer", string.IsNullOrEmpty(exportParams["IsSepatraCustomer"]) == false ? exportParams["IsSepatraCustomer"] : null));
                list.Add(new KeyValuePair<string, string>("IsSepatraPartner", string.IsNullOrEmpty(exportParams["IsSepatraPartner"]) == false ? exportParams["IsSepatraPartner"] : null));

                ReportServerProxy reportServerProxy = new ReportServerProxy();
                byte[] array = reportServerProxy.Render(reportPath, list, ReportFormat.XLSX);
                if (array.Length != 0)
                {
                    string text = "CustomerContacts_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xlsx";
                    HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.OK);
                    httpResponseMessage.Content = new StreamContent(new MemoryStream(array));
                    httpResponseMessage.Content.Headers.Add("X-FileName", text);
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = text;
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
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
        [System.Web.Http.HttpGet]
        public virtual ActionResult getContactById(int id)
        {
            try
            {
                List<ContactSycDTO> list = _context.ExecuteQuery<ContactSycDTO>("EXEC dbo.USP_LG_CONTACT_GET_DETAIL @ContactID", new object[1]
                {
                    new SqlParameter("ContactID", id)
                }).ToList();
                ContactSycDTO customerContactDTO = list[0];
                try
                {
                    string realmId = ConfigurationManager.AppSettings["realmid"];
                    string accessToken = "";
                    var obj = _context.ExecuteQuery<QBToken>("EXEC dbo.LG_QB_Access_TOKEN_GET").ToList();
                    if (obj.Count > 0)
                    {
                        accessToken = obj[0].AccessToken;
                        Logger.WriteWarning("accessToken" + accessToken, false);
                    }

                    ServiceContext serviceContext = IntializeContext(realmId, accessToken);
                    DataService dataService = new DataService(serviceContext);
                    QueryService<Customer> querySvc1 = new QueryService<Customer>(serviceContext);
                    string query = string.Empty;
                    int byid = 0;
                    if (customerContactDTO.qbid != "0" && customerContactDTO.qbid != null)
                    {
                        byid = 1;
                        query = "SELECT * FROM Customer WHERE Id='" + customerContactDTO.qbid + "'";
                    }
                    else
                    {
                         query = "SELECT * FROM Customer WHERE CompanyName='" + customerContactDTO.CompanyName + "'";
                    }
                    var q = querySvc1.ExecuteIdsQuery(query).ToList();
                    if (q.Count == 0 && byid==1)
                    {
                        query = "SELECT * FROM Customer WHERE CompanyName='" + customerContactDTO.CompanyName + "'";
                        q = querySvc1.ExecuteIdsQuery(query).ToList();
                    }
                    if (q.Count > 0)
                    {
                        customerContactDTO.qbid = q[0].Id;
                        customerContactDTO.quickCompanyName = q[0].CompanyName;
                        customerContactDTO.quickAddress = q[0].BillAddr.Line1;
                        customerContactDTO.quickContactPerson = q[0].ContactName;
                        customerContactDTO.quickCountry = q[0].BillAddr.Country;
                        customerContactDTO.quickCity = q[0].BillAddr.City;
                        customerContactDTO.quickZipCode = q[0].BillAddr.PostalCode;
                        customerContactDTO.quickEmail = q[0].PrimaryEmailAddr.Address;
                    }
                    return AppResult(customerContactDTO, 1L, "Success", EnumResult.Success);
                }
                catch (Exception ex)
                {
                    return AppResult(customerContactDTO, 1L, ex.Message, EnumResult.UnauthorizedAccess);

                }
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
            serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
            return serviceContext;
        }
        [System.Web.Http.HttpPost]
        public virtual ActionResult saveCompairData(ContactSycDTO objSycDTO)
        {
            try
            {
                if (objSycDTO.isquickbook == "1")
                {
                    string realmId = ConfigurationManager.AppSettings["realmid"];
                    string accessToken = "";
                    var obj1 = _context.ExecuteQuery<QBToken>("EXEC dbo.LG_QB_Access_TOKEN_GET").ToList();
                    if (obj1.Count > 0)
                    {
                        accessToken = obj1[0].AccessToken;

                    }
                    ServiceContext serviceContext = IntializeContext(realmId, accessToken);
                    QueryService<Customer> querySvc1 = new QueryService<Customer>(serviceContext);
                    string query = string.Empty;
                    int byid = 0;
                    if (objSycDTO.qbid != "" && objSycDTO.qbid != null && objSycDTO.qbid != "0")
                    {
                        byid = 1;
                        query = "SELECT * FROM Customer WHERE Id='" + objSycDTO.qbid + "'";
                    }
                    else
                    {
                        query = "SELECT * FROM Customer WHERE CompanyName='" + objSycDTO.CompanyName + "'";
                    }
                    var q = querySvc1.ExecuteIdsQuery(query).ToList();
                    if (q.Count == 0 && byid == 1)
                    {
                        query = "SELECT * FROM Customer WHERE CompanyName='" + objSycDTO.CompanyName + "'";
                        q = querySvc1.ExecuteIdsQuery(query).ToList();
                    }

                    DataService dataService = new DataService(serviceContext);
                    Customer customer = new Customer();
                    customer.GivenName = objSycDTO.CompanyName;
                    customer.FamilyName = objSycDTO.CompanyName;
                    customer.DisplayName = objSycDTO.CompanyName;
                    customer.CompanyName = objSycDTO.CompanyName;
                    customer.ContactName = objSycDTO.ContactPerson;
                    customer.BillAddr = new PhysicalAddress()
                    {
                        Line1 = objSycDTO.Address,
                        Country= objSycDTO.CountryName,
                        City= objSycDTO.City,
                        PostalCode= objSycDTO.ZipCode
                    };
                    customer.PrimaryEmailAddr = new EmailAddress()
                    {
                        Address = objSycDTO.Email
                    };
                    customer.Mobile = new TelephoneNumber()
                    {
                        FreeFormNumber = objSycDTO.TelNo.ToString()
                    };

                    if (objSycDTO.qbid != "" && objSycDTO.qbid != null && objSycDTO.qbid != "0")
                    {
                        customer.Id = objSycDTO.qbid;
                        customer.SyncToken = q[0].SyncToken;
                        customer = dataService.Update<Customer>(customer);

                    }
                    else
                    {
                        
                        customer = dataService.Add<Customer>(customer);

                    }
                    IEnumerable<int> objResult;
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_UPDATEQBID @QBID,@CustomerID",
                        new SqlParameter("QBID", customer.Id),
                        new SqlParameter("CustomerID", objSycDTO.ContactID)
                       ).ToList();
                    return AppResult(objResult, 1L, "", EnumResult.Success);
                }
                else
                {
                    IEnumerable<ContactSycDTO> objResult = _context.ExecuteQuery<ContactSycDTO>("EXEC dbo.LG_UpdateContactFromQuickBook @ContactID,@CompanyName,@CountryName,@CityName,@ContactNumber,@ContactPerson,@Email,@ZipCode,@ContactAddress,@qbid",
                    new SqlParameter("ContactID", objSycDTO.ContactID),
                    new SqlParameter("CompanyName", objSycDTO.quickCompanyName ?? Convert.DBNull),
                    new SqlParameter("CountryName", objSycDTO.quickCountry ?? Convert.DBNull),
                    new SqlParameter("CityName", objSycDTO.quickCity ?? Convert.DBNull),
                    new SqlParameter("ContactNumber", objSycDTO.quickContactNumber ?? Convert.DBNull),
                    new SqlParameter("ContactPerson", objSycDTO.quickContactPerson ?? Convert.DBNull),
                    new SqlParameter("Email", objSycDTO.quickEmail ?? Convert.DBNull),
                    new SqlParameter("ZipCode", objSycDTO.quickZipCode ?? Convert.DBNull),
                    new SqlParameter("ContactAddress", objSycDTO.quickAddress ?? Convert.DBNull),
                    new SqlParameter("qbid", objSycDTO.qbid ?? Convert.DBNull)).ToList();
                    List<ContactSycDTO> objList = objResult.ToList();
                    return AppResult(objList, 1L, "", EnumResult.Success);

                }

            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        //public async Task<HttpResponseMessage> ImportCustomer(int id)
        //{

        //    StandardJsonResult output = new StandardJsonResult
        //    {
        //        ResultId = Convert.ToInt32(EnumResult.Failed)
        //    };

        //    HttpResponseMessage response = new HttpResponseMessage();
        //    var httpRequest = HttpContext.Current.Request;
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        var postedFile = httpRequest.Files[0];
        //        if (postedFile != null && postedFile.ContentLength > 0)
        //        {
        //            try
        //            {
        //                var filePath = HttpContext.Current.Server.MapPath("~/Documents/ImportContact/" + postedFile.FileName);
        //                postedFile.SaveAs(filePath);
        //                string status = "";
        //                string duplicatecompany = ReadExcelFile(filePath, id, "contact", out status);

        //                if (status.Contains("Success"))
        //                {
        //                    string msg = "";
        //                    if(duplicatecompany.Length>0)
        //                    {
        //                        msg = "Successfully saved with duplicate companies " + duplicatecompany;

        //                        output.AddMessage(msg, clearLastMessages: true);
        //                    }
        //                    else
        //                    {
        //                        msg = "Successfully saved";
        //                        output.AddMessage(msg, clearLastMessages: true);
        //                    }
        //                }


        //                return Request.CreateResponse(HttpStatusCode.OK, new
        //                {
        //                    output
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                output.AddMessage(ex.Message, clearLastMessages: true);
        //                return Request.CreateResponse(HttpStatusCode.BadRequest, new
        //                {
        //                    output
        //                });
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, new
        //            {
        //                output
        //            });
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new
        //        {
        //            output
        //        });
        //    }
        //}
        [System.Web.Http.HttpPost]
        public HttpResponseMessage ImportCustomer(int id)
        {
            var httpRequest = HttpContext.Current.Request;
            var status = string.Empty;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    try
                    {
                        var filePath = HttpContext.Current.Server.MapPath("~/Documents/ImportContact/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        DataTable listdata = ReadExcelFile(filePath, id, "contact", out status);
                        if (listdata.Rows.Count > 0)
                        {
                            XLWorkbook wb = new XLWorkbook();
                            {
                                wb.Worksheets.Add(listdata);
                                MemoryStream ms = new MemoryStream();
                                {
                                    wb.SaveAs(ms);
                                    ms.Position = 0;
                                    HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
                                    result.Content = new StreamContent(ms);
                                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                                    result.Content.Headers.ContentDisposition.FileName = "Invoices.xlsx";
                                    return result;
                                }
                            }
                        }
                        else
                        {
                            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.NoContent);
                            result.Headers.Add("Meaage", new string[] { status });
                            return result;
                        }

                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, ex.Message);
                    }

                }

            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage ImportCustomer_CRM(int id)
        {
            var httpRequest = HttpContext.Current.Request;
            var status = string.Empty;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    try
                    {
                        var filePath = HttpContext.Current.Server.MapPath("~/Documents/ImportContact/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        DataTable listdata = ReadExcelFile(filePath, id, "crm", out status);
                        if (listdata.Rows.Count > 0)
                        {
                            XLWorkbook wb = new XLWorkbook();
                            {
                                wb.Worksheets.Add(listdata);
                                MemoryStream ms = new MemoryStream();
                                {
                                    wb.SaveAs(ms);
                                    ms.Position = 0;
                                    HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
                                    result.Content = new StreamContent(ms);
                                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                                    result.Content.Headers.ContentDisposition.FileName = "Invoices.xlsx";
                                    return result;
                                }
                            }
                        }
                        else
                        {
                            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.NoContent);
                            result.Headers.Add("Meaage", new string[] { status });
                            return result;
                        }

                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, ex.Message);
                    }

                }

            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }



        //public async Task<HttpResponseMessage> ImportCustomer_CRM(int id)
        //{

        //    StandardJsonResult output = new StandardJsonResult
        //    {
        //        ResultId = Convert.ToInt32(EnumResult.Failed)
        //    };

        //    HttpResponseMessage response = new HttpResponseMessage();
        //    var httpRequest = HttpContext.Current.Request;
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        var postedFile = httpRequest.Files[0];
        //        if (postedFile != null && postedFile.ContentLength > 0)
        //        {
        //            try
        //            {
        //                var filePath = HttpContext.Current.Server.MapPath("~/Documents/ImportContact/" + postedFile.FileName);
        //                postedFile.SaveAs(filePath);
        //                string status = "";
        //                string duplicatecompany = ReadExcelFile(filePath, id, "crm", out status);
        //                if (status.Contains("Success"))
        //                {
        //                    string msg = "";
        //                    if (duplicatecompany.Length > 0)
        //                    {
        //                        msg = "Successfully saved with duplicate companies " + duplicatecompany;

        //                        output.AddMessage(msg, clearLastMessages: true);
        //                    }
        //                    else
        //                    {
        //                        msg = "Successfully saved";
        //                        output.AddMessage(msg, clearLastMessages: true);
        //                    }
        //                }


        //                return Request.CreateResponse(HttpStatusCode.OK, new
        //                {
        //                    output
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                output.AddMessage(ex.Message, clearLastMessages: true);
        //                return Request.CreateResponse(HttpStatusCode.BadRequest, new
        //                {
        //                    output
        //                });
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, new
        //            {
        //                output
        //            });
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new
        //        {
        //            output
        //        });
        //    }
        //}

        //private string ReadExcelFile(string filePath, int id, string createdfrom,out string status)
        //{
        //    List<string> excelData = new List<string>();
        //    string duplicatecompany = "";
        //    try
        //    {


        //        FileInfo fileInfo = new FileInfo(filePath);
        //        var dt = new DataTable();
        //        using (ExcelPackage package = new ExcelPackage(fileInfo))
        //        {
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            int rowCount = worksheet.Dimension.Rows;
        //            int colCount = worksheet.Dimension.Columns;
        //            dt = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].ToDataTable(c =>
        //            {
        //                c.FirstRowIsColumnNames = true;
        //            });
        //            if (dt.Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    if (dr["Company Name"] != null && dr["Company Name"].ToString() != "")
        //                    {
        //                   List<int> enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_ImportContact @CompanyName,@Address," +
        //                            "@TelNo,@MobileNumber,@ContactPerson,@ZipCode,@Email," +
        //                            "@WebSite,@Remarks,@AcountDetails,@City,@StateName,@CountryName," +
        //                            "@Continent,@Franchise,@SourceofContact,@ContactCategory,@CreatedBy,@isapproved,@createdfrom,@Designation", new object[21]
        //                          {
        //              new SqlParameter("CompanyName", dr["Company Name"]),
        //              new SqlParameter("Address", dr["Address"]),
        //              new SqlParameter("TelNo", dr["Tel No"]),
        //              new SqlParameter("MobileNumber", dr["Mobile Number"]),
        //              new SqlParameter("ContactPerson", dr["Contact Person"]),
        //              new SqlParameter("ZipCode", dr["Zip Code"]),
        //              new SqlParameter("Email", dr["Email"]),
        //              new SqlParameter("WebSite", dr["WebSite"]),
        //              new SqlParameter("Remarks", dr["Remarks"]),
        //              new SqlParameter("AcountDetails", dr["Acount Details"]),
        //              new SqlParameter("City", dr["City"]),
        //              new SqlParameter("StateName", dr["State Name"]),
        //              new SqlParameter("CountryName", dr["Country Name"]),
        //              new SqlParameter("Continent", dr["Continent"]),
        //              new SqlParameter("Franchise", dr["Franchise"]),
        //              new SqlParameter("SourceofContact", dr["Source of Contact"]),
        //              new SqlParameter("ContactCategory", dr["Contact Category"]),
        //              new SqlParameter("CreatedBy", id),
        //              new SqlParameter("isapproved", createdfrom=="crm"?0:1),
        //              new SqlParameter("createdfrom", createdfrom),
        //              new SqlParameter("Designation", dr["Designation"])}).ToList();
        //                if (enumerable[0] == -1)
        //               {
        //                excelData.Add(dr["Company Name"].ToString());
        //                duplicatecompany = duplicatecompany + dr["Company Name"].ToString() + "; ";
        //              }
        //                    }
        //                }

        //            }

        //        }
        //        status = "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        status = "Error " + ex.Message.ToString();
        //    }
        //    return duplicatecompany;
        //}
        private DataTable ReadExcelFile(string filePath, int id, string createdfrom, out string status)
        {
            
            var dt = new DataTable();
            var dt1 = new DataTable();
            try
            {
                string error = string.Empty;
                FileInfo fileInfo = new FileInfo(filePath);
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;
                    dt = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].ToDataTable(c =>
                    {
                        c.FirstRowIsColumnNames = true;
                        c.AlwaysAllowNull = true;
                    });
                    if (dt.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(error))
                        {
                            if (dt.Columns.Contains("Status") == false)
                                dt.Columns.Add("Status", typeof(string));

                            dt1 = dt.Clone();
                            foreach (DataRow dr in dt.Rows)
                            {
                                DataRow newdr = dt1.NewRow();
                                if (dr["Company Name"] != null && dr["Company Name"].ToString() != "")
                                {
                                    List<string> enumerable = _context.ExecuteQuery<string>("EXEC dbo.USP_ImportContact @CompanyName,@Address," +
                                             "@TelNo,@MobileNumber,@ContactPerson,@ZipCode,@Email," +
                                             "@WebSite,@Remarks,@AcountDetails,@City,@StateName,@CountryName," +
                                             "@Continent,@Franchise,@SourceofContact,@ContactCategory,@CreatedBy,@isapproved,@createdfrom,@Designation", new object[21]
                                           {
                      new SqlParameter("CompanyName", dr["Company Name"]),
                      new SqlParameter("Address", dr["Address"]),
                      new SqlParameter("TelNo", dr["Tel No"]),
                      new SqlParameter("MobileNumber", dr["Mobile Number"]),
                      new SqlParameter("ContactPerson", dr["Contact Person"]),
                      new SqlParameter("ZipCode", dr["Zip Code"]),
                      new SqlParameter("Email", dr["Email"]),
                      new SqlParameter("WebSite", dr["WebSite"]),
                      new SqlParameter("Remarks", dr["Remarks"]),
                      new SqlParameter("AcountDetails", dr["Acount Details"]),
                      new SqlParameter("City", dr["City"]),
                      new SqlParameter("StateName", dr["State Name"]),
                      new SqlParameter("CountryName", dr["Country Name"]),
                      new SqlParameter("Continent", dr["Continent"]),
                      new SqlParameter("Franchise", dr["Franchise"]),
                      new SqlParameter("SourceofContact", dr["Source of Contact"]),
                      new SqlParameter("ContactCategory", dr["Contact Category"]),
                      new SqlParameter("CreatedBy", id),
                      new SqlParameter("isapproved", createdfrom=="crm"?0:1),
                      new SqlParameter("createdfrom", createdfrom),
                      new SqlParameter("Designation", dr["Designation"])}).ToList();
                                    if (enumerable[0] == "Error")
                                    {
                                        dr["Status"] = "Duplicat";
                                        newdr.ItemArray = dr.ItemArray;
                                        dt1.Rows.Add(newdr);
                                    }
                                    else
                                    {
                                        if(string.IsNullOrEmpty(dr["Contact Category"].ToString()))
                                        {
                                            dr["Status"] = "Contact Category Blank (Success)";
                                            newdr.ItemArray = dr.ItemArray;
                                            dt1.Rows.Add(newdr);
                                        }
                                        else if (enumerable[0] == "Category")
                                        {
                                            dr["Status"] = "Contact Category InValid(Not Success)";
                                            newdr.ItemArray = dr.ItemArray;
                                            dt1.Rows.Add(newdr);
                                        }
                                        else
                                        {
                                            dr["Status"] = "Success";
                                            newdr.ItemArray = dr.ItemArray;
                                            dt1.Rows.Add(newdr);
                                        }
                                       
                                    }
                                }
                                else
                                {
                                    dr["Status"] = "Company Name Blank(Not Success)";
                                    newdr.ItemArray = dr.ItemArray;
                                    dt1.Rows.Add(newdr);
                                }
                            }
                            error = "Success";
                        }
                        else
                        {
                            status = error;
                        }

                    }

                }
                status = error;
            }
            catch (Exception ex)
            {
                status = "Error " + ex.Message.ToString();
            }
            return dt1;
        }
    


    [System.Web.Http.HttpPost]
        public  ActionResult CreateEnquieryCRM(CustomerContactDTO objContactDTO)
        {
            try
            {

                //List<CustomerContactDTO> list = _context.ExecuteQuery<CustomerContactDTO>("EXEC dbo.USP_LG_CONTACT_GET_DETAIL @ContactID", new object[1]
                //{
                //    new SqlParameter("ContactID", objContactDTO.ContactID)
                //}).ToList();
                //CustomerContactDTO customerContactDTO_New = list[0];
                        int id = 2;

                        List<EnquiryDetailDTO> objEnquiryList = _context.ExecuteQuery<EnquiryDetailDTO>("EXEC dbo.USP_LG_ENQUIRY_GET_ENQUIRY_NO  @DEPTID", new SqlParameter("DEPTID", id)).ToList();
                        EnquiryDetailDTO objEnquiryDetail = objEnquiryList[0];
                        EnquiryDetailDTO objEnquiryDraft = new EnquiryDetailDTO();
                        objEnquiryDraft.EnquiryDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                        objEnquiryDraft.EnquiryNo = objEnquiryDetail.EnquiryNo;
                        objEnquiryDraft.fkCompanyID = objContactDTO.ContactID;
                        objEnquiryDraft.BillToCompanyId = null;
                        objEnquiryDraft.PickupType = "0";
                        objEnquiryDraft.OriginID = 0;
                        objEnquiryDraft.DischargeID = 0;
                        objEnquiryDraft.TypeOfEnquiry = "1";
                        objEnquiryDraft.IsHaz = false;
                        objEnquiryDraft.Remarks = "Auto Generated";
                        objEnquiryDraft.DepartmentID = 2;

                        objEnquiryDraft.isapproved_enq = 0;
                        objEnquiryDraft.location = "crm";
                        objEnquiryDraft.IsDraft = 1;
                        objEnquiryDraft.UserID = objContactDTO.CreatedBy;
                        objEnquiryDraft.UpdatedBy = objContactDTO.CreatedBy;



                        objEnquiryDraft.LastRemarkDate = DateTime.Now;
                        objEnquiryDraft.SiteId = objContactDTO.SiteId;
                        objEnquiryDraft.CreatedBy = objContactDTO.CreatedBy;
                        AppMGL.DAL.Models.AppMGL a = new DAL.Models.AppMGL();
                        EnquiryListRepository l = new EnquiryListRepository(a);
                        EnquiryController enqc = new EnquiryController(l);
                        objEnquiryDraft.CustomerInqNo = "";
                        objEnquiryDraft.LicenseType = 1;
                        objEnquiryDraft.LastEnquiryNo = objEnquiryDetail.LastEnquiryNo;
                        objEnquiryDraft.EnquiryControlNo = objEnquiryDetail.EnquiryControlNo;
                        enqc.SaveEnquiryAsIncompleteDraftMethod(objEnquiryDraft);
                        return AppResult(null, 1L, "Enquiry created successfully and Enquiry No. is  " + objEnquiryDetail.EnquiryNo, EnumResult.Success);
                    
               
                
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }        

    }    
}
