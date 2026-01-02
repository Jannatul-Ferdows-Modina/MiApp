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

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class FreightForwarderNetworkController : BaseController<FreightForwarderNetworkDTO, CustomerContactRepository, USP_GET_CUSTOMERCONTACT_LIST_Result>
    {
        public FreightForwarderNetworkController(CustomerContactRepository context)
        {
            _context = context;
            base.BaseModule = EnumModule.FreightForwarderNetwork;
            base.KeyField = "FFNetworkID";
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
                    string str = ConfigurationManager.AppSettings["FFNetworkFilePath"];
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
                List<USP_GET_FFCONTACT_LIST_Result> list = _context.ExecuteQuery<USP_GET_FFCONTACT_LIST_Result>("EXEC dbo.USP_GET_FFCONTACT_LIST @PAGENO, @PAGESIZE,@FFNetworkName,@FFNetworkCode,@MiamiRep, @Region, @Country, @State, @City,@SIT_ID,@SORTCOLUMN,@SORTORDER", new object[12]
                {
                    new SqlParameter("PAGENO", listParams.PageIndex),
                    new SqlParameter("PAGESIZE", listParams.PageSize),
                    new SqlParameter("FFNetworkName", dictionary.ContainsKey("companyName") ? dictionary["companyName"] : string.Empty),
                    new SqlParameter("FFNetworkCode", dictionary.ContainsKey("customerCode") ? dictionary["customerCode"] : string.Empty),
                    new SqlParameter("MiamiRep", dictionary.ContainsKey("galRepresentative") ? dictionary["galRepresentative"] : string.Empty),
                    new SqlParameter("Region", dictionary.ContainsKey("continent") ? dictionary["continent"] : string.Empty),
                    new SqlParameter("Country", dictionary.ContainsKey("cryName") ? dictionary["cryName"] : string.Empty),
                    new SqlParameter("State", dictionary.ContainsKey("state") ? dictionary["state"] : string.Empty),
                    new SqlParameter("City", dictionary.ContainsKey("city") ? dictionary["city"] : string.Empty),
                    new SqlParameter("SIT_ID", dictionary["siteId"]),
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
        [System.Web.Http.HttpGet]
        public virtual ActionResult getContactDetail(int id)
        {
            try
            {
                List<FreightForwarderNetworkDTO> list = _context.ExecuteQuery<FreightForwarderNetworkDTO>("EXEC dbo.USP_LG_FFNETWORK_GET_DETAILBYID @FFNetworkID", new object[1]
                {
                    new SqlParameter("FFNetworkID", id)
                }).ToList();
                FreightForwarderNetworkDTO customerContactDTO = list[0];
                if (list[0].FFNetworkID > 0)
                {
                    List<AdditionalFFContactDTO> list2 = _context.ExecuteQuery<AdditionalFFContactDTO>("EXEC dbo.USP_LG_FFNETWORK_GET_DETAILSBYID @FFNetworkID", new object[1]
                    {
                        new SqlParameter("FFNetworkID", id)
                    }).ToList();
                    if (list2.Count > 0)
                    {
                        customerContactDTO.AdditionalFFContactDTOList = list2.ToArray();
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
        public virtual ActionResult saveCustomerContact(FreightForwarderNetworkDTO objContactDTO)
        {
            try
            {
                string text = "";
                FreightForwarderNetworkDTO data = null;
                text = ((objContactDTO.FFNetworkID != 0) ? "update" : "insert");
                CustomerContactRepository context = _context;
                object[] obj = new object[24]
                {
                    new SqlParameter("FFNetworkID", objContactDTO.FFNetworkID),
                    new SqlParameter("FFNetworkName", objContactDTO.FFNetworkName),
                    new SqlParameter("FFNetworkCode", objContactDTO.FFNetworkCode ?? Convert.DBNull),
                    new SqlParameter("Address", objContactDTO.Address ?? Convert.DBNull),
                    new SqlParameter("CellNo", objContactDTO.CellNo ?? Convert.DBNull),
                    new SqlParameter("TelNo", objContactDTO.TelNo ?? Convert.DBNull),
                    new SqlParameter("ContinentId", objContactDTO.ContinentId ?? Convert.DBNull),
                    new SqlParameter("Email", objContactDTO.Email ?? Convert.DBNull),
                    new SqlParameter("CountryId", objContactDTO.CountryId ?? Convert.DBNull),
                    new SqlParameter("Fax", objContactDTO.Fax ?? Convert.DBNull),
                    new SqlParameter("StateID", objContactDTO.StateId ?? Convert.DBNull),
                    new SqlParameter("WebSite", objContactDTO.WebSite ?? Convert.DBNull),
                    new SqlParameter("CityID", objContactDTO.CityId ?? Convert.DBNull),
                    new SqlParameter("ContactPerson", objContactDTO.ContactPerson ?? Convert.DBNull),
                    new SqlParameter("ZipCode", objContactDTO.ZipCode ?? Convert.DBNull),
                    new SqlParameter("RepID", objContactDTO.RepresentativeID ?? Convert.DBNull),
                    new SqlParameter("Remarks", objContactDTO.Remarks ?? Convert.DBNull),
                    new SqlParameter("CreatedBy", objContactDTO.CreatedBy ?? Convert.DBNull),
                    new SqlParameter("ModifiedBy", objContactDTO.ModifiedBy ?? Convert.DBNull),
                    new SqlParameter("ModifiedOn", objContactDTO.ModifiedOn ?? Convert.DBNull),
                    new SqlParameter("Attachment", objContactDTO.Attachment ?? Convert.DBNull),
                    new SqlParameter("SIT_ID",  objContactDTO.SiteId),
                    new SqlParameter("Type", text),
                    new SqlParameter("siteids", objContactDTO.siteids?? Convert.DBNull)

                };
                IEnumerable<int> source = context.ExecuteQuery<int>("EXEC dbo.USP_LG_FFCONTACT_INSERT_UPDATE @FFNetworkID,@FFNetworkName,@FFNetworkCode,@Address,@CellNo,@TelNo,@ContinentId,@Email,@CountryId,@Fax,@StateID,@WebSite,@CityID,@ContactPerson,@ZipCode,@RepID,@Remarks,@CreatedBy, @ModifiedBy, @ModifiedOn, @Attachment,@SIT_ID,@Type,@siteids", obj).ToList();
                List<int> list = source.ToList();
                int num = list[0];
                if (num > 0)
                {
                    IEnumerable<int> enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_FFNETWORK_DETAIL_DELETE @FFNetworkID", new object[1]
                    {
                        new SqlParameter("@FFNetworkID", num)
                    }).ToList();
                    if (objContactDTO.AdditionalFFContactDTOList.Count() > 0)
                    {
                        SaveAdditionalContactDetail(objContactDTO.AdditionalFFContactDTOList, num);
                    }
                   
                    List<FreightForwarderNetworkDTO> list2 = _context.ExecuteQuery<FreightForwarderNetworkDTO>("EXEC dbo.USP_LG_FFCONTACT_GET_DETAIL @FFNetworkID", new object[1]
                    {
                        new SqlParameter("FFNetworkID", num)
                    }).ToList();
                    data = list2[0];
                }
                return AppResult(data, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Delete(FreightForwarderNetworkDTO dto)
        {
            try
            {
                IEnumerable<int> source = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_FFNETWORKCONTACT_DELETE @FFNetworkID", new object[1]
                {
                    new SqlParameter("FFNetworkID", dto.FFNetworkID)
                }).ToList();
                List<int> entity = source.ToList();
                return AppResult(entity, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public void SaveAdditionalContactDetail(AdditionalFFContactDTO[] objAdditionalContactDTOList, int ContactID)
        {
            foreach (AdditionalFFContactDTO additionalContactDTO in objAdditionalContactDTOList)
            {
                IEnumerable<int> enumerable = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CONTACT_FF_DETAIL_INSERT @ContactName,@Designation,@Email,@ContactNo,@CellNo,@FFNetworkID", new object[6]
                {
                    new SqlParameter("ContactName", additionalContactDTO.ContactName ?? Convert.DBNull),
                    new SqlParameter("Designation", additionalContactDTO.Designation ?? Convert.DBNull),
                    new SqlParameter("Email", additionalContactDTO.contactEmail ?? Convert.DBNull),
                    new SqlParameter("ContactNo", additionalContactDTO.ContactNo ?? Convert.DBNull),
                    new SqlParameter("CellNo", additionalContactDTO.ContactCellNo ?? Convert.DBNull),
                    new SqlParameter("FFNetworkID", ContactID)
                }).ToList();
            }
        }
        [System.Web.Http.HttpGet]
        public virtual ActionResult GetLatestCustomerCode()
        {
            try
            {
                FFNetworkCodeDTO fFNetworkCodeDTO = new FFNetworkCodeDTO();
                IEnumerable<string> source = _context.ExecuteQuery<string>("EXEC dbo.USP_LG_FREIGHTFORWARDER_GET_CODE", Array.Empty<object>()).ToList();
                List<string> list = source.ToList();
                if (list.Count > 0)
                {
                    fFNetworkCodeDTO.FFNetworkCode = list[0].ToString();
                }
                return AppResult(fFNetworkCodeDTO, 1L, "", EnumResult.Success);
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
                string path = ConfigurationManager.AppSettings["FFNetworkFilePath"];
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
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(exportParams["Filter"]);
                string reportPath = "/AppMGL.Report/CustomerContactReport_New";
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("CompanyName", dictionary.ContainsKey("companyName") ? dictionary["companyName"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("CustomerCode", dictionary.ContainsKey("CustomerCode") ? dictionary["CustomerCode"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("MiamiRep", dictionary.ContainsKey("MiamiRep") ? dictionary["MiamiRep"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("Category", dictionary.ContainsKey("Category") ? dictionary["Category"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("CompanyGradation", dictionary.ContainsKey("CompanyGradation") ? dictionary["CompanyGradation"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("OriginCountries", dictionary.ContainsKey("OriginCountries") ? dictionary["OriginCountries"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("Commodity", dictionary.ContainsKey("Commodity") ? dictionary["Commodity"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("Region", dictionary.ContainsKey("Region") ? dictionary["Region"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("Country", dictionary.ContainsKey("Country") ? dictionary["Country"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("Region", dictionary.ContainsKey("Region") ? dictionary["Region"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("Country", dictionary.ContainsKey("Country") ? dictionary["Country"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("State", dictionary.ContainsKey("State") ? dictionary["State"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("IsVendor", dictionary.ContainsKey("IsVendor") ? dictionary["IsVendor"] : string.Empty));
                list.Add(new KeyValuePair<string, string>("SIT_ID", exportParams["SiteId"]));
                list.Add(new KeyValuePair<string, string>("SORTCOLUMN", "CompanyName"));
                list.Add(new KeyValuePair<string, string>("SORTORDER", exportParams["DESC"]));

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
        public override ActionResult Lookup(ListParams listParams)
        {
            try
            {
                var result = _context.ExecuteQuery<FreightForwarderNetworkDTO>("EXEC dbo.SP_ForwarderNetwork_Search @SEARCHVALUE",
                   new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
}