using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppMGL.DAL;
using AppMGL.DAL.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using AutoMapper;



using System.Web.Http;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Helper.Logging;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;
using AppMGL.Manager.Infrastructure.Helper;
using AppMGL.Manager.Infrastructure.Results;
using Newtonsoft.Json;
using System.Security.Claims;
using Intuit.Ipp.OAuth2PlatformClient;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AppMGL.Manager
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        //private  async void getaccess()
        //{
        //    string url = "https://api-stage.maersk.com/customer-identity/oauth/v2/access_token";
        //    string consumerKey = "EXUdkTC2WBbys4seKPQNXQ7mWOr75f6N";
        //    string clientSecret = "qJOiN7B0gKSJmZ8b";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        // Set headers
        //        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
        //        client.DefaultRequestHeaders.Add("Consumer-Key", consumerKey);

        //        // Prepare the form data
        //        var requestData = new FormUrlEncodedContent(new[]
        //        {
        //        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        //        new KeyValuePair<string, string>("client_id", consumerKey),
        //        new KeyValuePair<string, string>("client_secret", clientSecret),
        //    });

        //        // Make the POST request
        //        HttpResponseMessage response = await client.PostAsync(url, requestData);

        //        // Read the response
        //        string responseContent = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine(responseContent);
        //    }
        //}
        protected  void Page_Load(object sender, EventArgs e)
        {

           // getaccess();

            //string scope = "com.intuit.quickbooks.accounting com.intuit.quickbooks.payment openid address email phone profile";
            //var state = Guid.NewGuid().ToString("N");

            //var tempId = new ClaimsIdentity("TempState");
            //tempId.AddClaim(new Claim("state", state));

            //Request.GetOwinContext().Authentication.SignIn(tempId);

            ////Make Authorization request
            //var request = new AuthorizeRequest("https://appcenter.intuit.com/connect/oauth2");

            //string url = request.CreateAuthorizeUrl(
            //   clientId: ConfigurationManager.AppSettings["clientId"],
            //   responseType: OidcConstants.AuthorizeResponse.Code,
            //   scope: scope,
            //   redirectUri: "http://localhost:52965/WebForm1.aspx",
            //   state: state);

            //Response.Redirect( url);




            //var s = Request.RawUrl;

            //Logger.WriteInfo(Request.QueryString.Count.ToString());

            //try
            //{
            //        UserDTO dto = new UserDTO();
            //        dto.DptName = "dept";
            //        dto.FullName = "vikas";
            //        dto.UserName = "solankivikas";
            //        dto.UsrCreatedBy = 116;
            //        dto.UsrPwd = "vikas";
            //        dto.Contact.CntEmail = "solankivikas@gmail.com";
            //        var entity = Mapper.Map<LG_USER>(dto);

            //        var user = _context.RegisterUser(entity);
            //        var token = _context.GenerateEmailConfirmationToken(user);
            //        //var url = Url.Link("ConfirmEmailRoute", new { UserId = user.Id, Token = token });
            //        //_context.SendEmailConfirmation(entity, user, url);

            //        entity.UsrPwd = null;
            //        entity.AspNetUserId = user.Id;

            //        if (!string.IsNullOrEmpty(entity.UsrSmtpPassword))
            //        {
            //            entity.UsrSmtpPassword = SecurityHelper.EncryptString(entity.UsrSmtpPassword, "sblw-3hn8-sqoy19");
            //        }

            //        _context.Insert(entity);
            //        _context.UnitOfWork.Commit();

            //        var result = _context.Detail(GetPrimaryKeyValue(entity));
            //        var dtoResult = Mapper.Map<UserDTO>(result);

            //        dtoResult.Contact = Mapper.Map<ContactDTO>(result.LG_CONTACT);
            //        if (dto.UserUnitRoleList.Length > 0)
            //        {
            //            InsertUserUnitRoles(dto.UserUnitRoleList, dtoResult.Contact.CntId);
            //        }
            //        //get User's Unit & Role details
            //        List<SiteRoleDTO> SiteRoleDTOList = _context.ExecuteQuery<SiteRoleDTO>("EXEC dbo.USP_LG_USER_UNIT_ROLE_GET @CONTACT_ID",
            //                            new SqlParameter("CONTACT_ID", dtoResult.Contact.CntId)).ToList();
            //        if (SiteRoleDTOList.Count > 0)
            //        {
            //            dtoResult.UserUnitRoleList = SiteRoleDTOList.ToArray();
            //        }
            //        return AppResult(dtoResult, PrepareMessage(EnumAction.Insert));
            //    }
            //    catch (Exception ex)
            //    {
            //        return AppResult(ex);
            //    }
        }

       
    }
}