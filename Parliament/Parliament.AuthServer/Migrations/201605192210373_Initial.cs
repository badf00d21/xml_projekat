namespace Parliament.AuthServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthClients",
                c => new
                    {
                        AuthClientId = c.String(nullable: false, maxLength: 32),
                        Base64Secret = c.String(nullable: false, maxLength: 80),
                        ClientName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.AuthClientId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuthClients");
        }
    }
}
