using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parliament.Api.SGNS.ViewModels
{
    public class UserInfoViewModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}