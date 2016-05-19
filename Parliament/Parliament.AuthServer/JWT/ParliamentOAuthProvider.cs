using Microsoft.Owin.Security.OAuth;
using Parliament.DataAccess.Database;
using Parliament.DataAccess.Entities;
using Parliament.DataAccess.Repository;
using Parliament.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Parliament.AuthServer.Database;

namespace Parliament.AuthServer.JWT
{
    public class ParliamentOAuthProvider : OAuthAuthorizationServerProvider
    {
        public async override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
                context.TryGetFormCredentials(out clientId, out clientSecret);

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return;
            }

            using (var authDbContext = new AuthDbContext())
            {
                using (var authRepo = new Repository<AuthDbContext, AuthClient>(authDbContext))
                {
                    var authClient = await authRepo.GetByIDAsync(context.ClientId);

                    if (authClient == null)
                    {
                        context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                        return;
                    }
                }
            }

            context.Validated();
        }

        public async override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //Dummy check here, you need to do your DB checks against memebrship system http://bit.ly/SPAAuthCode
            using (var parliamentDbContext = new ParliamentDbContext())
            {
                using (var userManager = new ParliamentUserManager(parliamentDbContext))
                {
                    var user = await userManager.FindByNameAsync(context.UserName);

                    if (user == null || !(await userManager.CheckPasswordAsync(user, context.Password)))
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect");
                        return;
                    }

                    var identity = new ClaimsIdentity("JWT");

                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    identity.AddClaim(new Claim("sub", context.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

                    var props = new AuthenticationProperties(new Dictionary<string, string>
                     {
                         {
                             "authClientId", (context.ClientId == null) ? string.Empty : context.ClientId
                         }
                     });

                    var ticket = new AuthenticationTicket(identity, props);
                    context.Validated(ticket);
                }
            }
        }
    }
}