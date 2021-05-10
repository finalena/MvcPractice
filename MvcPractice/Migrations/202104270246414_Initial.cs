namespace MvcPractice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 250),
                        Password = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 10),
                        NickName = c.String(maxLength: 20),
                        Sex = c.Int(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        Phone = c.String(maxLength: 25),
                        Location = c.String(maxLength: 5),
                        Profile = c.String(maxLength: 200),
                        RegisterOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Members");
        }
    }
}
