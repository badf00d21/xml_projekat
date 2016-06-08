using Microsoft.AspNet.Identity.EntityFramework;
using Parliament.DataAccess.Entities;

namespace Parliament.DataAccess.Database
{
    public class ParliamentDbContext : IdentityDbContext<ParliamentUser>
    {
		public ParliamentDbContext() : base("ParliamentDb") { }
    }
}
