namespace SecureChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Sender_Id = c.String(maxLength: 128),
                        Chat_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .ForeignKey("dbo.Chats", t => t.Chat_Id)
                .Index(t => t.Sender_Id)
                .Index(t => t.Chat_Id);
            
            CreateTable(
                "dbo.EncryptedMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Content = c.String(),
                        DateRead = c.DateTime(),
                        PublicKey = c.String(),
                        Recipient_Id = c.String(maxLength: 128),
                        Message_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Recipient_Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Message_Id);
            
            CreateTable(
                "dbo.UserChats",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Chat_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Chat_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Chats", t => t.Chat_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Chat_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Chat_Id", "dbo.Chats");
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EncryptedMessages", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.EncryptedMessages", "Recipient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserChats", "Chat_Id", "dbo.Chats");
            DropForeignKey("dbo.UserChats", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserChats", new[] { "Chat_Id" });
            DropIndex("dbo.UserChats", new[] { "User_Id" });
            DropIndex("dbo.EncryptedMessages", new[] { "Message_Id" });
            DropIndex("dbo.EncryptedMessages", new[] { "Recipient_Id" });
            DropIndex("dbo.Messages", new[] { "Chat_Id" });
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropTable("dbo.UserChats");
            DropTable("dbo.EncryptedMessages");
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
        }
    }
}
