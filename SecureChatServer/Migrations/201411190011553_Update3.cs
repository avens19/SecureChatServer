namespace SecureChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Messages", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Type");
            DropColumn("dbo.Chats", "DateCreated");
        }
    }
}
