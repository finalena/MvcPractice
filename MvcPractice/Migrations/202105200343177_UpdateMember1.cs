namespace MvcPractice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMember1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Gender", c => c.String(nullable: false, maxLength: 5));
            AddColumn("dbo.Members", "AuthCode", c => c.String(maxLength: 36));
            AlterColumn("dbo.Members", "Password", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Members", "Sex");
            DropColumn("dbo.Members", "AutoCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "AutoCode", c => c.String(maxLength: 36));
            AddColumn("dbo.Members", "Sex", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "Password", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.Members", "AuthCode");
            DropColumn("dbo.Members", "Gender");
        }
    }
}
