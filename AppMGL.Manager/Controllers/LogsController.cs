using System;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Helper.Logging;

namespace AppMGL.Manager.Controllers
{
    public class LogsController : ApiController
    {
        [System.Web.Http.HttpPost]
        public IHttpActionResult Error(Log log)
        {
            try
            {
                log.Url = "Username => " + log.UserName 
                    + Environment.NewLine + "              " + "IP Address => " + GetClientIp() 
                    + Environment.NewLine + "              " + "Url => " + log.Url
                    + Environment.NewLine + "              " + "Browser => " + log.Browser
                    + Environment.NewLine + "              " + "UserAgent => " + log.UserAgent
                    + Environment.NewLine + "              " + "Cause => " + log.Cause;
                Logger.WriteError(log);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult Warn(Log log)
        {
            try
            {
                string message = "Username => " + log.UserName
                    + Environment.NewLine + "              " + "IP Address => " + GetClientIp()
                    + Environment.NewLine + "              " + "Url => " + log.Url
                    + Environment.NewLine + "              " + "Browser => " + log.Browser
                    + Environment.NewLine + "              " + "UserAgent => " + log.UserAgent
                    + Environment.NewLine + "              " + "Message => " + log.Message;
                Logger.WriteWarning(message, true);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult Info(Log log)
        {
            try
            {
                string message = "Username => " + log.UserName
                    + Environment.NewLine + "              " + "IP Address => " + GetClientIp()
                    + Environment.NewLine + "              " + "Url => " + log.Url
                    + Environment.NewLine + "              " + "Browser => " + log.Browser
                    + Environment.NewLine + "              " + "UserAgent => " + log.UserAgent
                    + Environment.NewLine + "              " + "Message => " + log.Message;
                Logger.WriteInfo(message, true);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return BadRequest(ex.Message);
            }
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
    }
}
