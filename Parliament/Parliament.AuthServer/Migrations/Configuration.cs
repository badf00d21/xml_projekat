namespace Parliament.AuthServer.Migrations
{
    using Database;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Parliament.AuthServer.Database.AuthDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Parliament.AuthServer.Database.AuthDbContext context)
        {
            if (!context.AuthClients.Any())
            {
                context.AuthClients.Add(new AuthClient("SGNS-Api"));
                context.AuthClients.Add(new AuthClient("IAGNS-Api"));
            }
        }
    }
}
