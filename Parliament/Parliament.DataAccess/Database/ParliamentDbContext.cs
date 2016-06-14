using Microsoft.AspNet.Identity.EntityFramework;
using Parliament.DataAccess.Entities;

namespace Parliament.DataAccess.Database
{
    public class ParliamentDbContext : IdentityDbContext<ParliamentUser>
    {
		//Server=MARKO-PC\\IME_MOJE;Database=ParliamentDb;Integrated Security = True;
		//"Server=W7ENT\\OASYSHDB;Database=ParliamentDb;Integrated Security=True;"

		public ParliamentDbContext() : base("Server=W7ENT\\OASYSHDB;Database=ParliamentDb;Integrated Security=True;") { }
    }
}
