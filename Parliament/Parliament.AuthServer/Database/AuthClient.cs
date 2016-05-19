using Parliament.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Parliament.AuthServer.Database
{
    public class AuthClient
    {
        [Key]
        [MaxLength(32)]
        public string AuthClientId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Base64Secret { get; set; }

        [Required]
        [MaxLength(100)]
        public string ClientName { get; set; }

        public AuthClient()
        {
            ClientName = string.Empty;
            Base64Secret = AuthUtils.GenerateBase64Secret();
        }

        public AuthClient(string clientName)
        {
            ClientName = clientName;
            AuthClientId = Guid.NewGuid().ToString("N");
            Base64Secret = AuthUtils.GenerateBase64Secret();
        }
    }
}