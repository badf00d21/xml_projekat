using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Parliament.DataAccess.Database;

namespace Parliament.DataAccess.Utils
{
    public class ParliamentRoleManager : RoleManager<IdentityRole>
    {
        public ParliamentRoleManager(ParliamentDbContext context) : base(new RoleStore<IdentityRole>(context)) { }      
    }
}
