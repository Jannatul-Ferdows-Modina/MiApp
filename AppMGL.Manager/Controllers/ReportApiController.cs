using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Syncfusion.EJ.RDL.ServerProcessor;
using Syncfusion.EJ.ReportViewer;

namespace AppMGL.Manager.Controllers
{
    public class ReportApiController : ApiController, IReportController
    {
        //Post action for processing the rdl/rdlc report 
        public object PostReportAction(Dictionary<string, object> jsonResult)
        {
            return ReportHelper.ProcessReport(jsonResult, this);
        }

        //Get action for getting resources from the report
        [System.Web.Http.ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(string key, string resourcetype, bool isPrint)
        {
            return ReportHelper.GetResource(key, resourcetype, isPrint);
        }

        //Method will be called when initialize the report options before start processing the report        
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            // This is the place to configure the External server and other credential information.
            //reportOption.ReportModel.ReportingServer = new ReportingServerExt();
            //Add SSRS Server and database credentials here
            reportOption.ReportModel.ReportServerCredential = new NetworkCredential("admin", "Logic123!");
            reportOption.ReportModel.DataSourceCredentials.Add(new Syncfusion.Reports.EJ.DataSourceCredentials("MIAMI", "sa", "Logic123!"));
        }

        //Method will be called when reported is loaded
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            //You can update report options here
        }
    }

    public class ReportingServerExt : ReportingServer
    {
        private T Deserialize<T>(string json)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        T DeseralizeObj<T>(Stream str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader reader = XmlReader.Create(str);
            return (T)serializer.Deserialize(reader);
        }

        Syncfusion.EJ.RDL.ServerProcessor.DataSourceDefinition GetDataSourceDefinition(ItemResponse response)
        {
            var _rptDefinition = new Syncfusion.EJ.RDL.ServerProcessor.DataSourceDefinition();
            var _datasourceStream = this.GetFileToStream(response.FileContent);
            var _umpDefinition = this.DeseralizeObj<DataSourceDefinition>(_datasourceStream);
            _rptDefinition.ConnectString = _umpDefinition.ConnectString;
            _rptDefinition.CredentialRetrieval = (CredentialRetrievalEnum)Enum.Parse(typeof(CredentialRetrievalEnum), _umpDefinition.CredentialRetrieval.ToString(), true);
            _rptDefinition.Enabled = _umpDefinition.Enabled;
            _rptDefinition.EnabledSpecified = _umpDefinition.EnabledSpecified;
            _rptDefinition.Extension = _umpDefinition.Extension;
            _rptDefinition.ImpersonateUser = _umpDefinition.ImpersonateUser;
            _rptDefinition.ImpersonateUserSpecified = _umpDefinition.ImpersonateUserSpecified;
            _rptDefinition.OriginalConnectStringExpressionBased = _umpDefinition.OriginalConnectStringExpressionBased;
            _rptDefinition.Password = _umpDefinition.Password;
            _rptDefinition.Prompt = _umpDefinition.Prompt;
            _rptDefinition.UseOriginalConnectString = _umpDefinition.UseOriginalConnectString;
            _rptDefinition.UserName = _umpDefinition.UserName;
            _rptDefinition.WindowsCredentials = _umpDefinition.WindowsCredentials;
            return _rptDefinition;
        }

        public override Syncfusion.EJ.RDL.ServerProcessor.DataSourceDefinition GetDataSourceDefinition(string dataSource)
        {
            var _credential = this.ReportServerCredential as NetworkCredential;
            try
            {
                Guid itemId;
                ItemRequest itemRequest = new ItemRequest();
                itemRequest.UserName = _credential.UserName;
                itemRequest.Password = _credential.Password;
                itemRequest.ItemType = ItemType.Datasource;

                if (Guid.TryParse(dataSource, out itemId))
                {
                    itemRequest.ItemId = itemId;
                }
                else
                {
                    var dataSourceDetails = new DataSourceMappingInfo { Name = dataSource };
                    itemRequest.DatasourceDetails = dataSourceDetails;
                }

                using (var proxy = new CustomWebClient())
                {
                    var ser = new DataContractJsonSerializer(typeof(ItemRequest));
                    var mem = new MemoryStream();
                    ser.WriteObject(mem, itemRequest);
                    proxy.Headers["Content-type"] = "application/json";
                    proxy.Encoding = Encoding.UTF8;
                    var data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

                    var rdata = proxy.UploadString(new Uri(this.ReportServerUrl + "/api/reportserverapi/download-data-source"),
                        "POST", data);

                    var result = JsonConvert.DeserializeObject<ItemResponse>(rdata);
                    return this.GetDataSourceDefinition(result);
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public override Stream GetReport()
        {
            var _credential = this.ReportServerCredential as NetworkCredential;
            try
            {
                var itemId = Guid.Parse(this.ReportPath);
                var itemRequest = new ItemRequest
                {
                    ItemId = itemId,
                    UserName = _credential.UserName,
                    Password = _credential.Password,
                    ItemType = ItemType.Report
                };

                using (var proxy = new CustomWebClient())
                {
                    var ser = new DataContractJsonSerializer(typeof(ItemRequest));
                    var mem = new MemoryStream();
                    ser.WriteObject(mem, itemRequest);
                    proxy.Headers["Content-type"] = "application/json";
                    proxy.Encoding = Encoding.UTF8;
                    var data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                    var rdata = proxy.UploadString(new Uri(this.ReportServerUrl + "/api/reportserverapi/download-report"), "POST", data);
                    var result = JsonConvert.DeserializeObject<ItemResponse>(rdata);
                    if (result.Status && result.ItemType == ItemType.Report)
                    {
                        throw new Exception("Report is incorrect Format");
                    }
                    return this.GetFileToStream(result.FileContent);
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private Stream GetFileToStream(byte[] _fileContent)
        {
            MemoryStream memStream = new MemoryStream();
            memStream.Write(_fileContent, 0, _fileContent.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }
    }

    class CustomWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            var request = base.GetWebRequest(uri);
            request.Timeout = 4 * 60 * 1000; //Increase time out
            return request;
        }
    }

    [Serializable]
    public class DataSourceDefinition
    {
        private string extensionField;
        private string connectStringField;
        private bool useOriginalConnectStringField;
        private bool originalConnectStringExpressionBasedField;
        private CredentialRetrievalEnum credentialRetrievalField;
        private bool windowsCredentialsFieldSpecified;
        private bool windowsCredentialsField;
        private bool impersonateUserField;
        private bool impersonateUserFieldSpecified;
        private string promptField;
        private string userNameField;
        private string passwordField;
        private bool enabledField;
        private bool enabledFieldSpecified;
        public string Extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }
        public string ConnectString
        {
            get
            {
                return connectStringField;
            }
            set
            {
                connectStringField = value;
            }
        }
        public bool UseOriginalConnectString
        {
            get
            {
                return useOriginalConnectStringField;
            }
            set
            {
                useOriginalConnectStringField = value;
            }
        }
        public bool OriginalConnectStringExpressionBased
        {
            get
            {
                return originalConnectStringExpressionBasedField;
            }
            set
            {
                originalConnectStringExpressionBasedField = value;
            }
        }
        public CredentialRetrievalEnum CredentialRetrieval
        {
            get
            {
                return credentialRetrievalField;
            }
            set
            {
                credentialRetrievalField = value;
            }
        }
        public bool WindowsCredentials
        {
            get
            {
                return windowsCredentialsField;
            }
            set
            {
                windowsCredentialsField = value;
            }
        }
        [XmlIgnore()]
        public bool WindowsCredentialsSpecified
        {
            get
            {
                return windowsCredentialsFieldSpecified;
            }
            set
            {
                windowsCredentialsFieldSpecified = value;
            }
        }
        public bool ImpersonateUser
        {
            get
            {
                return impersonateUserField;
            }
            set
            {
                impersonateUserField = value;
            }
        }
        [XmlIgnore()]
        public bool ImpersonateUserSpecified
        {
            get
            {
                return impersonateUserFieldSpecified;
            }
            set
            {
                impersonateUserFieldSpecified = value;
            }
        }
        public string Prompt
        {
            get
            {
                return promptField;
            }
            set
            {
                promptField = value;
            }
        }
        public string UserName
        {
            get
            {
                return userNameField;
            }
            set
            {
                userNameField = value;
            }
        }
        public string Password
        {
            get
            {
                return passwordField;
            }
            set
            {
                passwordField = value;
            }
        }
        public bool Enabled
        {
            get
            {
                return enabledField;
            }
            set
            {
                enabledField = value;
            }
        }
        [XmlIgnore()]
        public bool EnabledSpecified
        {
            get
            {
                return enabledFieldSpecified;
            }
            set
            {
                enabledFieldSpecified = value;
            }
        }
    }

    public class ItemRequest
    {
        public Guid ItemId { get; set; }

        public DataSourceMappingInfo DatasourceDetails { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? CategoryId { get; set; }

        public byte[] ItemContent { get; set; }

        public DataSourceDefinition DataSourceDefinition { get; set; }

        public string UploadedReportName { get; set; }

        public List<DataSourceMappingInfo> DataSourceMappingInfo { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VersionComment { get; set; }

        public ItemType ItemType { get; set; }
    }

    public class ItemResponse
    {
        public bool Status { get; set; }

        public byte[] FileContent { get; set; }

        public string StatusMessage { get; set; }

        public string ItemName { get; set; }

        public ItemType ItemType { get; set; }

        public Guid PublishedItemId { get; set; }
    }

    [DataContract(Name = "Data5", Namespace = "http://schemas.datacontract.org/2004/07/Syncfusion.Server.Base.DataClasses")]
    public enum ItemType
    {
        [EnumMember]
        [Description("Category")]
        Category = 1,
        [EnumMember]
        [Description("Dashboard")]
        Dashboard,
        [EnumMember]
        [Description("Report")]
        Report,
        [EnumMember]
        [Description("Datasource")]
        Datasource,
        [EnumMember]
        [Description("Dataset")]
        Dataset,
        [EnumMember]
        [Description("File")]
        File,
        [EnumMember]
        [Description("Schedule")]
        Schedule
    }

    public class DataSourceMappingInfo
    {
        public string Name { get; set; }

        public string DataSourceId { get; set; }
    }


}