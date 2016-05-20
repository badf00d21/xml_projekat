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
                //context.AuthClients.Add(new AuthClient("SGNS-Api"));
                //context.AuthClients.Add(new AuthClient("IAGNS-Api"));

                context.AuthClients.Add(new AuthClient
                {
                   ClientName = "SGNS-Api",
                   AuthClientId = "7fb613284f504776ad94ddadb65036bd",
                   Base64Secret = "f_3VrtVYdrgg2W0ORBBfjBaPbiqOAbirKMRpPBDOZcM"
                });
                context.AuthClients.Add(new AuthClient
                {
                    ClientName = "IAGNS-Api",
                    AuthClientId = "f360404661eb4520b96f8a226792caf7",
                    Base64Secret = "Um2L8E9Z2hi99RYg5RgyG8tyExRA1OLUq5RJIor5PTc"
                });
            }
        }
    }
}
