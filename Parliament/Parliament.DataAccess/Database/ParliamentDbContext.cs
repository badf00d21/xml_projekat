﻿using Microsoft.AspNet.Identity.EntityFramework;
using Parliament.DataAccess.Entities;

namespace Parliament.DataAccess.Database
{
    public class ParliamentDbContext : IdentityDbContext<ParliamentUser>
    {
        //Server=MARKO-PC\\IME_MOJE;Database=ParliamentDb;Integrated Security = True;

        public ParliamentDbContext() : base("Server=MARKO-PC\\IME_MOJE; Database=ParliamentDb;Integrated Security = True;") { }
    }
}
