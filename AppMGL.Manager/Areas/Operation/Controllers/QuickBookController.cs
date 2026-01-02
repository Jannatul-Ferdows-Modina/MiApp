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

using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Intuit.Ipp.Exception;
using Intuit.Ipp.ReportService;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace AppMGL.Manager.Areas.Operation.Controllers
{
                                       
    public class QuickBookController : BaseController<BookingListDTO, BookingListRepository, USP_GET_ENQUIRY_LIST_Result>
    {
       

        public static string clientid = ConfigurationManager.AppSettings["clientId"];
        public static string clientsecret = ConfigurationManager.AppSettings["clientSecret"];
        public static string redirectUrl = ConfigurationManager.AppSettings["redirectURI"];
        public static string environment = ConfigurationManager.AppSettings["appEnvironment"];
        public static OAuth2Client auth2Client = new OAuth2Client(clientid, clientsecret, redirectUrl, environment);
        public static string realmId = ConfigurationManager.AppSettings["realmid"].ToString();// "4620816365236649600";
        public static IList<JsonWebKey> keys; 
        public static string issuerEndpoint = "";
        public static string mod;
        public static string expo;
        public static string userinfoEndpoint = "";
        
        public QuickBookController(BookingListRepository context, EnquiryListRepository enquiryListRepository)
        {
            _context = context;
            BaseModule = EnumModule.QuickBook;
            KeyField = "EnquiryNo";
        }

        
        [System.Web.Http.HttpPost]
        public virtual ActionResult SendContactDataToQuickBook(CustomerContactDTO objContactDTO)
        {
            try
            {

                List<OidcScopes> scopes = new List<OidcScopes>();
                scopes.Add(OidcScopes.Accounting);
                //string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);
                try
                {
                    var principal = User as ClaimsPrincipal;
                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                    string realmId = "";//Session["realmId"].ToString();
                                        // Create a ServiceContext with Auth tokens and realmId
                    ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                    serviceContext.IppConfiguration.MinorVersion.Qbo = "23";

                    // Create a QuickBooks QueryService using ServiceContext
                    //QueryService<CompanyInfo> querySvc = new QueryService<CompanyInfo>(serviceContext);
                    //CompanyInfo companyInfo = querySvc.ExecuteIdsQuery("SELECT * FROM CompanyInfo").FirstOrDefault();

                    //string output = "Company Name: " + companyInfo.CompanyName + " Company Address: " + companyInfo.CompanyAddr.Line1 + ", " + companyInfo.CompanyAddr.City + ", " + companyInfo.CompanyAddr.Country + " " + companyInfo.CompanyAddr.PostalCode;
                    //return View("ApiCallService", (object)("QBO API call Successful!! Response: " + output));
                }
                catch (Exception ex)
                {
                    return AppResult(ex);
                    //return View("ApiCallService", (object)("QBO API call Failed!" + " Error message: " + ex.Message));
                }

                IEnumerable<int> objResult;
                objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_CAPTURE_ROUTE_INSERT_UPDATE  @RouteId,@RouteName,@fkOriginID,@fkDestinationID,@OriginType,@DestinationType,@Via1,@Via2,@ViaType1,@ViaType2",
                        new SqlParameter("ContactID", objContactDTO.ContactID),
                        new SqlParameter("Address", objContactDTO.Address ?? Convert.DBNull),
                        new SqlParameter("CellNo", objContactDTO.CellNo ?? Convert.DBNull),
                        new SqlParameter("CompanyName", objContactDTO.CompanyName)
                        ).ToList();
                List<int> objList = objResult.ToList();

                return AppResult(objList, 1);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }

        }


       
        private string GetAuthorizeUrl()
        {
           string scope = "com.intuit.quickbooks.accounting com.intuit.quickbooks.payment openid address email phone profile";
            var state = Guid.NewGuid().ToString("N");

            var tempId = new ClaimsIdentity("TempState");
            tempId.AddClaim(new Claim("state", state));

            Request.GetOwinContext().Authentication.SignIn(tempId);

            //Make Authorization request
            var request = new AuthorizeRequest("https://appcenter.intuit.com/connect/oauth2");

            string url = request.CreateAuthorizeUrl(
               clientId: clientid,
               responseType: OidcConstants.AuthorizeResponse.Code,
               scope: scope,
               redirectUri: redirectUrl,
               state: state);

            return url;
        }

        [System.Web.Http.HttpPost]
        public async Task<ActionResult> RefreshToken()
        {
            try
            {
                var obj = _context.ExecuteQuery<QBToken>("EXEC dbo.LG_QB_Access_TOKEN_GET").ToList();
                var refreshTokenCurrent = "AB116710093344x4pNMRFoYr8GQu1Cbg8Bkro8EQDqmgTLGzV0";
                if (obj.Count > 0)
                {
                    refreshTokenCurrent = obj[0].RefreshToken;
                }
                var tokenClient = new TokenClient("https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer", clientid, clientsecret);

                TokenResponse response = await tokenClient.RequestRefreshTokenAsync(refreshTokenCurrent);
                UpdateCookie(response);
                return AppResult(response, "1");
            }catch(Exception ex)
            {
                return AppResult(ex);
            }
        }
        private async Task<Tuple<string>> GetTempStateAsync()
        {
            var data = await Request.GetOwinContext().Authentication.AuthenticateAsync("TempState");

            var state = data.Identity.FindFirst("state").Value;


            return Tuple.Create(state);
        }       
        
        private bool ValidateToken(string identityToken)
        {
            if (keys != null)
            {

                //IdentityToken
                if (identityToken != null)
                {
                    //Split the identityToken to get Header and Payload
                    string[] splitValues = identityToken.Split('.');
                    if (splitValues[0] != null)
                    {

                        //Decode header 
                        var headerJson = Encoding.UTF8.GetString(Base64Url.Decode(splitValues[0].ToString()));

                        //Deserilaize headerData
                        IdTokenHeader headerData = JsonConvert.DeserializeObject<IdTokenHeader>(headerJson);

                        //Verify if the key id of the key used to sign the payload is not null
                        if (headerData.Kid == null)
                        {
                            return false;
                        }

                        //Verify if the hashing alg used to sign the payload is not null
                        if (headerData.Alg == null)
                        {
                            return false;
                        }

                    }
                    if (splitValues[1] != null)
                    {
                        //Decode payload
                        var payloadJson = Encoding.UTF8.GetString(Base64Url.Decode(splitValues[1].ToString()));


                       var payloadData = JsonConvert.DeserializeObject<IdTokenJWTClaimTypes>(payloadJson);



                        //Verify Aud matches ClientId
                        if (payloadData.Aud != null)
                        {
                            if (payloadData.Aud[0].ToString() != clientid)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }


                        //Verify Authtime matches the time the ID token was authorized.                
                        if (payloadData.Auth_time == null)
                        {
                            return false;
                        }



                        //Verify exp matches the time the ID token expires, represented in Unix time (integer seconds).                
                        if (payloadData.Exp != null)
                        {
                            long expiration = Convert.ToInt64(payloadData.Exp);
                            long currentEpochTime = EpochTimeExtensions.ToEpochTime(DateTime.UtcNow);
                            //Verify the ID expiration time with what expiry time you have calculated and saved in your application
                            //If they are equal then it means IdToken has expired 

                            if ((expiration - currentEpochTime) <= 0)
                            {
                                return false;

                            }

                        }

                        //Verify Iat matches the time the ID token was issued, represented in Unix time (integer seconds).            
                        if (payloadData.Iat == null)
                        {
                            return false;
                        }


                        //verify Iss matches the  issuer identifier for the issuer of the response.     
                        if (payloadData.Iss != null)
                        {
                            if (payloadData.Iss.ToString() != issuerEndpoint)
                            {

                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }



                        //Verify sub. Sub is an identifier for the user, unique among all Intuit accounts and never reused. 
                        //An Intuit account can have multiple emails at different points in time, but the sub value is never changed.
                        //Use sub within your application as the unique-identifier key for the user.
                        if (payloadData.Sub == null)
                        {

                            return false;
                        }



                    }

                    //Use external lib to decode mod and expo value and generte hashes
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                    //Read values of n and e from discovery document.
                    rsa.ImportParameters(
                      new RSAParameters()
                      {
                          //Read values from discovery document
                          Modulus = Base64Url.Decode(mod),
                          Exponent = Base64Url.Decode(expo)
                      });

                    //Verify Siganture hash matches the signed concatenation of the encoded header and the encoded payload with the specified algorithm
                    SHA256 sha256 = SHA256.Create();

                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(splitValues[0] + '.' + splitValues[1]));

                    RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                    rsaDeformatter.SetHashAlgorithm("SHA256");
                    if (rsaDeformatter.VerifySignature(hash, Base64Url.Decode(splitValues[2])))
                    {
                        //identityToken is valid
                        return true;
                    }
                    else
                    {
                        //identityToken is not valid
                        return false;

                    }
                }
                else
                {
                    //identityToken is not valid
                    return false;
                }
            }
            else
            {
                //Missing mod and expo values
                return false;
            }
        }
      
      
        private void UpdateCookie(TokenResponse response)
        {
            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            var identity = (User as ClaimsPrincipal).Identities.First();
            var result = from c in identity.Claims
                         where c.Type != "access_token" &&
                               c.Type != "refresh_token" &&
                               c.Type != "access_token_expires_at" &&
                               c.Type != "access_token_expires_at"
                         select c;

            var claims = result.ToList();

            claims.Add(new Claim("access_token", response.AccessToken));

            claims.Add(new Claim("access_token_expires_at", (DateTime.Now.AddSeconds(response.AccessTokenExpiresIn)).ToString()));
            claims.Add(new Claim("refresh_token", response.RefreshToken));

            claims.Add(new Claim("refresh_token_expires_at", (DateTime.UtcNow.ToEpochTime() + response.RefreshTokenExpiresIn).ToDateTimeFromEpoch().ToString()));

            var newId = new ClaimsIdentity(claims, "Cookies");
            Request.GetOwinContext().Authentication.SignIn(newId);
            var  obj = _context.ExecuteQuery<int>("EXEC dbo.LG_QB_Access_TOKEN_Insert @accessToken ,@refreshToken", new object[2]
                {
                    new SqlParameter("accessToken", response.AccessToken),
                    new SqlParameter("refreshToken", response.RefreshToken)
                }).ToList();

        }



    }
    public class QBToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class BillAddr
    {
        public string Line1 { get; set; }
        public string Line3 { get; set; }
    }
    public class Mobile
    {
        public string freeformnumber { get; set; }
    }
     
  
   
}