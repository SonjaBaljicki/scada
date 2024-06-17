namespace ScadaCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlarmEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        TagName = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 100),
                        EncryptedPassword = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Username" });
            DropTable("dbo.Users");
            DropTable("dbo.TagEntities");
            DropTable("dbo.AlarmEntities");
        }
    }
}
