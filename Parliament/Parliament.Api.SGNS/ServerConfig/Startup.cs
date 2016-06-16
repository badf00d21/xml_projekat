using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Parliament.DataAccess.Database;
using Parliament.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

[assembly: OwinStartup(typeof(Parliament.Api.SGNS.ServerConfig.Startup))]
namespace Parliament.Api.SGNS.ServerConfig
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            ConfigureOAuth(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseNinjectMiddleware(CreateKernel);
            app.UseNinjectWebApi(config);

            ConfigureJSONFormatter(config);
            config.Formatters.Remove(config.Formatters.JsonFormatter);
        }

        public void ConfigureJSONFormatter(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var issuer = WebConfigurationManager.AppSettings["TokenIssuer"];
            var audience = WebConfigurationManager.AppSettings["ClientID"];
            var secret = TextEncodings.Base64Url.Decode(WebConfigurationManager.AppSettings["ClientSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    }
                });
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<ParliamentDbContext>().To<ParliamentDbContext>().InThreadScope();
            kernel.Bind(typeof(IRepository<,>)).To(typeof(Repository<,>)).InThreadScope();

            return kernel;
        }
    }
}