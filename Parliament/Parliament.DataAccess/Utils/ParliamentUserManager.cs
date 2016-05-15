using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Parliament.DataAccess.Databases;
using Parliament.DataAccess.Entities;

namespace Parliament.DataAccess.Utils
{
    public class ParliamentUserManager : UserManager<ParliamentUser>
    {
        public ParliamentUserManager(ParliamentDbContext context)
            : base(new UserStore<ParliamentUser>(context))
        {
            // Configure validation logic for usernames
            UserValidator = new UserValidator<ParliamentUser>(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
        }
    }
}
