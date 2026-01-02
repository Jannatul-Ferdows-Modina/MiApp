using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AppMGL.DAL.UDT;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AppMGL.Manager.Infrastructure.Results
{
    public class StandardJsonResult : JsonResult
    {
        public int ResultId { get; set; }
        public IList<string> Messages { get; private set; }
        public long Count { get; set; }

        public StandardJsonResult()
        {
            ResultId = (int)EnumResult.Success;
            Messages = new List<string>();
        }

        public void AddMessage(string message, bool clearLastMessages)
        {
            if (clearLastMessages)
            {
                Messages.Clear();
            }
            Messages.Add(message);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("GET access is not allowed.  Change the JsonRequestBehavior if you need GET access.");
            }

            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            SerializeData(response);
        }

        protected virtual void SerializeData(HttpResponseBase response)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[]
                {
                    new StringEnumConverter()
                }
            };

            response.Write(JsonConvert.SerializeObject(Data, settings));
        }
    }
}