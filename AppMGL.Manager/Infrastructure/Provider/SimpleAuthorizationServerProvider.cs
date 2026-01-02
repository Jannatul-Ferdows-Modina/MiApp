using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.UDT;
using Newtonsoft.Json;
using AutoMapper;
using AppMGL.DTO.Security;
using AppMGL.DAL.Helper.Logging;

namespace AppMGL.Manager.Infrastructure.Provider
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            AspNetClient client;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (var repo = new AuthRepository())
            {
                client = repo.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == 1002)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                if (client.Secret != Helper.StringHelper.GetHash(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");
                    return Task.FromResult<object>(null);
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString(CultureInfo.InvariantCulture));

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", allowedOrigin.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            LG_USER userInfo;
            List<LG_SITE> userSite;            
            
            using (var repo = new AuthRepository())
            {
                
                IdentityUser user = await repo.FindByName(context.UserName);

                if (user == null)
                {
                    repo.InsertLoginHistory(context.UserName, false);
                    context.SetError("2004", "User is not exists.");
                    Logger.WriteWarning("User is not exists.");
                    return;
                }
                else
                {
                    DateTimeOffset lockedDate = await repo.GetLockoutEnabled(user);
                    if (lockedDate > DateTimeOffset.Now)
                    {
                        repo.InsertLoginHistory(context.UserName, false);
                        context.SetError("2004", "Your account is locked.");
                        Logger.WriteWarning("Your account is locked.");
                        return;
                    }
                    else
                    {
                        bool isValid = await repo.CheckPassword(user, context.Password);
                        if (isValid)
                        {
                            await repo.ResetAccessFailedCount(user);
                            userInfo = repo.FindUser(user.Id);
                            repo.InsertLoginHistory(context.UserName, true);
                            if (userInfo.LG_CONTACT.CntStatus != true)
                            {
                                context.SetError("2004", "User is Inactive, Please contact MGL Admin");
                                Logger.WriteWarning("User is Inactive, Please contact MGL Admin");
                                return;
                            }
                            if(userInfo.UsrIsLocked == 1)
                            {
                                context.SetError("2004", "User is Locked, Please contact MGL Admin");
                                Logger.WriteWarning("User is Inactive, Please contact MGL Admin");
                                return;
                            }

                            
                            if ( userInfo.UsrValidFrom !=null)
                            {
                                repo.InsertLoginHistory(userInfo.UsrValidFrom.ToString(), true);
                                if (userInfo.UsrValidFrom >= DateTimeOffset.Now)
                                {
                                   
                                    context.SetError("2004", "User Account is not Valid, Please contact MGL Admin");
                                    Logger.WriteWarning("User Account is not Valid, Please contact MGL Admin");
                                    return;
                                }
                            }

                            if (userInfo.UsrValidTo != null)
                            {
                                if (userInfo.UsrValidTo > DateTime.Now)
                                {
                                    context.SetError("2004", "User Account is not Valid, Please contact MGL Admin");
                                    Logger.WriteWarning("User Account is not Valid, Please contact MGL Admin");
                                    return;
                                }
                            }
                           
                            userSite = repo.FindSite(userInfo.UsrId.ToString());
                        }
                        else
                        {
                            await repo.AccessFailed(user);
                            repo.InsertLoginHistory(context.UserName, false);
                            context.SetError("2004", "The user name or password is incorrect.");
                            Logger.WriteWarning("The user name or password is incorrect.");
                            return;
                        }
                    }
                }
            }
            
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            identity.AddClaim(new Claim("UserId", userInfo.UsrId.ToString()));
            identity.AddClaim(new Claim("UsrSmtpUsername", (string.IsNullOrEmpty(userInfo.UsrSmtpUsername) ? "" : userInfo.UsrSmtpUsername)));
            identity.AddClaim(new Claim("UsrSmtpPassword", (string.IsNullOrEmpty(userInfo.UsrSmtpPassword) ? "" : userInfo.UsrSmtpPassword)));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                { "as:client_id", context.ClientId ?? string.Empty },
                { "userId", userInfo.UsrId.ToString() },
                { "userType", userInfo.LG_CONTACT.CntId.ToString() },// 1 for external
                { "userName", context.UserName },
                { "aspNetUserId", userInfo.AspNetUserId },
                { "changePassword", (!string.IsNullOrEmpty(userInfo.UsrTempPwd)).ToString() },
                { "userSite", JsonConvert.SerializeObject(Mapper.Map<List<SiteDTO>>(userSite)) }
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}