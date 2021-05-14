namespace MvcPractice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "AutoCode", c => c.String(maxLength: 36));
            AlterColumn("dbo.Members", "Sex", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Sex", c => c.Int(nullable: false));
            DropColumn("dbo.Members", "AutoCode");
        }
    }
}
