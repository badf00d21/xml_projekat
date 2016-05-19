using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Parliament.AuthServer.Database
{
    public class AuthDbContext : DbContext
    {
        public DbSet<AuthClient> AuthClients { get; set; }

        public AuthDbContext() : base("AuthDb") { }
    }
}