using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Parliament.DataAccess.Entities
{
    public class ParliamentUser : IdentityUser
    {
        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        public ParliamentRole Role { get; set; }

        public ParliamentUser()
        {
            EmailConfirmed = true;
            LockoutEnabled = true;

            Role = ParliamentRole.Citizen;
        }

        public ParliamentUser(string firstName, string lastName, string email, ParliamentRole role)
            : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            Role = role;
        }
    }
}
