using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Parliament.AuthServer.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(Parliament.AuthServer.ServerConfig.Startup))]
namespace Parliament.AuthServer.ServerConfig
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            ConfigureOAuth(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/auth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new ParliamentOAuthProvider(),
                AccessTokenFormat = new ParliamentJWTFormat("http://localhost:8904")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }
    }
}