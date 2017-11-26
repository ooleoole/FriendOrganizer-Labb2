namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendPhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        FriendId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Friend", t => t.FriendId, cascadeDelete: true)
                .Index(t => t.FriendId);
            
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        FavoriteLanguageId = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProgrammingLanguage", t => t.FavoriteLanguageId)
                .Index(t => t.FavoriteLanguageId);
            
            CreateTable(
                "dbo.ProgrammingLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        LocationName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Location", t => t.LocationName, cascadeDelete: false)
                .Index(t => t.LocationName);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.MeetingFriend",
                c => new
                    {
                        Meeting_Id = c.Int(nullable: false),
                        Friend_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Meeting_Id, t.Friend_Id })
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id, cascadeDelete: true)
                .ForeignKey("dbo.Friend", t => t.Friend_Id, cascadeDelete: true)
                .Index(t => t.Meeting_Id)
                .Index(t => t.Friend_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendPhoneNumber", "FriendId", "dbo.Friend");
            DropForeignKey("dbo.Meeting", "LocationName", "dbo.Location");
            DropForeignKey("dbo.MeetingFriend", "Friend_Id", "dbo.Friend");
            DropForeignKey("dbo.MeetingFriend", "Meeting_Id", "dbo.Meeting");
            DropForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage");
            DropIndex("dbo.MeetingFriend", new[] { "Friend_Id" });
            DropIndex("dbo.MeetingFriend", new[] { "Meeting_Id" });
            DropIndex("dbo.Meeting", new[] { "LocationName" });
            DropIndex("dbo.Friend", new[] { "FavoriteLanguageId" });
            DropIndex("dbo.FriendPhoneNumber", new[] { "FriendId" });
            DropTable("dbo.MeetingFriend");
            DropTable("dbo.Location");
            DropTable("dbo.Meeting");
            DropTable("dbo.ProgrammingLanguage");
            DropTable("dbo.Friend");
            DropTable("dbo.FriendPhoneNumber");
        }
    }
}
