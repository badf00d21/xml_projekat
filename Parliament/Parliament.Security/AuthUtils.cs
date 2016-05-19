using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parliament.Security
{
    public static class AuthUtils
    {
        public static string GenerateBase64Secret()
        {
            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            return TextEncodings.Base64Url.Encode(key);
        }
    }
}
