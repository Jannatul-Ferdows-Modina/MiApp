using System;
using System.Configuration;
using System.Web.Http;
using AppMGL.Manager;
using AppMGL.Manager.Infrastructure.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace AppMGL.Manager
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpire"])),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}