using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using AppMGL.Manager.Infrastructure.Dependency;
using Newtonsoft.Json.Serialization;

namespace AppMGL.Manager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API cors
            var cors = new EnableCorsAttribute("*", "*", "*", "*");
            cors.ExposedHeaders.Add("Content-Disposition");
            cors.ExposedHeaders.Add("X-FileName");
            config.EnableCors(cors);

            // Web API configuration and services
            config.Services.Replace(typeof(IHttpControllerActivator), new StructureMapControllerActivator(config));
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApiWithAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
