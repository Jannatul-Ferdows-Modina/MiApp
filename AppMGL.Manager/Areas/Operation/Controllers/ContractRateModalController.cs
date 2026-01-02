using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Document;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Document;
using AppMGL.DTO.Operation;
using AppMGL.Manager.Infrastructure;
using Syncfusion.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using AppMGL.Manager.Infrastructure.Results;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DAL.Helper;
using System.Linq;
using System.Data;

namespace AppMGL.Manager.Areas.Operation.Controllers
{
    public class ContractRateModalController : BaseController<LGShipmentDocsDTO, LGShipMentDocRepository, LG_SHIPMENT_DOCS>
    {
        #region Constructor

        public ContractRateModalController(LGShipMentDocRepository context)
        {
            _context = context;
            BaseModule = EnumModule.ContractRateModal;
            KeyField = "ContractID";
        }

        #endregion

        [System.Web.Http.HttpPost]
        public virtual ActionResult SaveDocumentAttachement(LGShipmentDocsDTO objDocCommonDTO)
        {
            try
            {
                var shipmentDoc = new LGShipmentDocsDTO();
                shipmentDoc.DocumentCommonID = Convert.ToInt32(objDocCommonDTO.DocumentCommonID);
                shipmentDoc.sdDocType = "Contract";
                shipmentDoc.SdDocName = objDocCommonDTO.SdDocName;
                shipmentDoc.SdCreatedTs = DateTime.Now;
                shipmentDoc.SdUpdatedTs = DateTime.Now;
                var parameters = PrepareParameters<LGShipmentDocsDTO>(shipmentDoc);


                if (shipmentDoc.DocumentCommonID != 0)
                {
                    var detailParameters = PrepareParameters<LGShipmentDocsDTO>(shipmentDoc);
                    Utility.SetParamValue(detailParameters, "DocumentCommonID", shipmentDoc.DocumentCommonID);
                    Utility.SetParamValue(detailParameters, "sdDocType", shipmentDoc.sdDocType);
                    _context.ExecuteCommand(DocumentCommonQuery.SaveContratDocuments, detailParameters.ToArray());
                }

                return AppResult(null, "Contract Rate Document saved successfully.");
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
                var uploadFolder = ConfigurationManager.AppSettings["DocumentsPath"];
                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                string filePath = root + "\\" + "Contract";
                var provider = new MultipartFormDataStreamProvider(root);

                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData.HasKeys())
                {
                    var fileName = GetUnescapeData(result, "DisplayName").ToString();
                    var DocumentId = GetUnescapeData(result, "DocumentCommonID").ToString();
                    DirectoryInfo dir = new DirectoryInfo(root);

                    var attachedDate = DateTime.Now;
                    var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

                    if (System.IO.Directory.Exists(filePath))
                    {
                        if (File.Exists(filePath + "\\" + fileName))
                        {
                            File.Delete(filePath + "\\" + fileName);
                            uploadedFileInfo.MoveTo(filePath + "\\" + fileName);
                        }
                        else
                        {
                            uploadedFileInfo.MoveTo(filePath + "\\" + fileName);
                        }                        
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                        uploadedFileInfo.MoveTo(filePath + "\\" + fileName);
                    }

                    //uploadedFileInfo.MoveTo(dir.FullName + "\\" + DocumentId.ToString() + "_" + fileName);

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

        [System.Web.Http.HttpPost]
        public HttpResponseMessage downloadAttachment()
        {
            try
            {
                string fileName = "";
                int DocumentCommonID;
                fileName = Request.Headers.GetValues("fileName").ToList()[0];
                DocumentCommonID = Int32.Parse(Request.Headers.GetValues("documentCommonID").ToList()[0]);
                fileName = DocumentCommonID + "_" + fileName;
                HttpResponseMessage result = null;
                if (!System.IO.File.Exists(fileName))
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    var ShipmentFolder = ConfigurationManager.AppSettings["DocumentsPath"];
                    //string filePath = root + "\\" + "Contract";
                    string filePath = HttpContext.Current.Server.MapPath(ShipmentFolder + "/" + "Contract" + "/") + fileName;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] bytes = new byte[file.Length];
                            file.Read(bytes, 0, (int)file.Length);
                            ms.Write(bytes, 0, (int)file.Length);

                            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                            httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                            httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                            //httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            //httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                            httpResponseMessage.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage;
                        }
                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [System.Web.Http.HttpPost]
        public virtual ActionResult DeleteShipmentDoc(ShipmentDocsDTO objShipmentDocsDTO)
        {
            try
            {
                IEnumerable<int> objResult = null;
                if (objShipmentDocsDTO.DocumentCommonID > 0)
                {
                    objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_BOOKING_SHIPMENT_DOC_DELETE @DocumentCommonID,@DocName,@DocType",
                                        new SqlParameter("DocumentCommonID", objShipmentDocsDTO.DocumentCommonID),
                                        new SqlParameter("DocName", objShipmentDocsDTO.DocName),
                                        new SqlParameter("DocType", "Contract")).ToList();
                }
                var uploadFolder = ConfigurationManager.AppSettings["DocumentsPath"];
                var root = HttpContext.Current.Server.MapPath(uploadFolder);
                DirectoryInfo dir = new DirectoryInfo(root + "\\" + "Contract");
                if (System.IO.File.Exists(dir.FullName + "\\" + objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName))
                {
                    System.IO.File.Delete(dir.FullName + "\\" + objShipmentDocsDTO.DocumentCommonID.ToString() + "_" + objShipmentDocsDTO.DocName);
                }

                List<int> objList = objResult.ToList();
                return AppResult(null, "Contract Rate Document deleted successfully.");
                // return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetDocumentAttachmentDetail(Dictionary<string, string> listParams)
        {
            try
            {
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("PageIndex", listParams["PageIndex"]),
                    new SqlParameter("PageSize", listParams["PageSize"]),
                    new SqlParameter("Sort", Utility.GetSort(listParams["Sort"])),
                    new SqlParameter("SitId", listParams["SitId"]),
                    new SqlParameter("contractID", listParams["contractID"]),
                    new SqlParameter("Count", SqlDbType.Int) {Direction = ParameterDirection.Output}
                };

                List<ShipmentDocsDTO> result = _context.ExecuteQuery<ShipmentDocsDTO>(DocumentCommonQuery.GetDocumentAttachmentDetail, parameters.ToArray()).ToList();
                int count = Utility.GetParamValue(parameters, "Count", typeof(int));
                return AppResult(result, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

    }
}