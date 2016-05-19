using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Parliament.AuthServer.Database;
using Parliament.DataAccess.Database;
using Parliament.DataAccess.Entities;
using Parliament.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace Parliament.AuthServer.JWT
{
    public class ParliamentJWTFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "authClientId";
        private readonly string _issuer = string.Empty;

        public ParliamentJWTFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string authClientId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;

            if (string.IsNullOrWhiteSpace(authClientId))
                throw new InvalidOperationException("AuthenticationTicket.Properties does not include audience");

            using (var authDbContext = new AuthDbContext())
            {
                using (var authRepo = new Repository<AuthDbContext, AuthClient>(authDbContext))
                {
                    var client = authRepo.Get(c => c.AuthClientId == authClientId).Single();

                    if (client == null)
                        throw new InvalidOperationException("Invalid client ID");

                    string symmetricKeyAsBase64 = client.Base64Secret;

                    var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
                    var signingKey = new HmacSigningCredentials(keyByteArray);

                    var issued = data.Properties.IssuedUtc;
                    var expires = data.Properties.ExpiresUtc;

                    var token = new JwtSecurityToken(_issuer, authClientId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

                    var handler = new JwtSecurityTokenHandler();

                    var jwt = handler.WriteToken(token);

                    return jwt;
                }
            }
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}