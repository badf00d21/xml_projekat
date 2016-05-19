namespace Parliament.DataAccess.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Utils;
    internal sealed class Configuration : DbMigrationsConfiguration<Parliament.DataAccess.Database.ParliamentDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Parliament.DataAccess.Database.ParliamentDbContext context)
        {
            if (!context.Roles.Any())
            {
                using (var roleManager = new ParliamentRoleManager(context))
                {
                    roleManager.Create(new IdentityRole(ParliamentRole.Citizen.ToString()));
                    roleManager.Create(new IdentityRole(ParliamentRole.Alderman.ToString()));
                    roleManager.Create(new IdentityRole(ParliamentRole.Citizen.ToString()));
                }
            }
        }
    }
}
