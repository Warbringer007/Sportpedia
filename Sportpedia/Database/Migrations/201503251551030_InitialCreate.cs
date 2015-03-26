namespace EFDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.League_contestants",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        League_ID = c.Int(nullable: false),
                        Team_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Leagues", t => t.League_ID)
                .ForeignKey("dbo.Teams", t => t.Team_ID)
                .Index(t => t.League_ID)
                .Index(t => t.Team_ID);
            
            CreateTable(
                "dbo.Leagues",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Season = c.Int(nullable: false),
                        Country = c.String(nullable: false),
                        Type_of_league = c.Int(nullable: false),
                        Sport = c.Int(nullable: false),
                        Emblem = c.Binary(),
                        Comment = c.String(),
                        Creator_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.Creator_ID)
                .Index(t => t.Creator_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(),
                        Permissions = c.Int(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        Date_of_birth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Match_Information",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Home_first_quarter_goals = c.Int(nullable: false),
                        Home_first_half_goals = c.Int(nullable: false),
                        Home_third_quarter_goals = c.Int(nullable: false),
                        Home_end_goals = c.Int(nullable: false),
                        Away_first_quarter_goals = c.Int(nullable: false),
                        Away_first_half_goals = c.Int(nullable: false),
                        Away_third_quarter_goals = c.Int(nullable: false),
                        Away_end_goals = c.Int(nullable: false),
                        Viewers = c.Int(nullable: false),
                        Contributor_ID = c.Int(nullable: false),
                        Match_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.Contributor_ID)
                .ForeignKey("dbo.Matches", t => t.Match_ID)
                .Index(t => t.Contributor_ID)
                .Index(t => t.Match_ID);
            
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Locked = c.Boolean(nullable: false),
                        Away_Team_ID = c.Int(nullable: false),
                        Current_Match_Information_ID = c.Int(),
                        Home_Team_ID = c.Int(nullable: false),
                        League_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Teams", t => t.Away_Team_ID)
                .ForeignKey("dbo.Match_Information", t => t.Current_Match_Information_ID)
                .ForeignKey("dbo.Teams", t => t.Home_Team_ID)
                .ForeignKey("dbo.Leagues", t => t.League_ID)
                .Index(t => t.Away_Team_ID)
                .Index(t => t.Current_Match_Information_ID)
                .Index(t => t.Home_Team_ID)
                .Index(t => t.League_ID);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Founded = c.DateTime(nullable: false),
                        Country = c.String(nullable: false),
                        Fans_Name = c.String(),
                        Stadium = c.String(),
                        Emblem = c.Binary(),
                        Last_contributor_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.Last_contributor_ID)
                .Index(t => t.Last_contributor_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.League_contestants", "Team_ID", "dbo.Teams");
            DropForeignKey("dbo.League_contestants", "League_ID", "dbo.Leagues");
            DropForeignKey("dbo.Leagues", "Creator_ID", "dbo.Users");
            DropForeignKey("dbo.Match_Information", "Match_ID", "dbo.Matches");
            DropForeignKey("dbo.Matches", "League_ID", "dbo.Leagues");
            DropForeignKey("dbo.Matches", "Home_Team_ID", "dbo.Teams");
            DropForeignKey("dbo.Matches", "Current_Match_Information_ID", "dbo.Match_Information");
            DropForeignKey("dbo.Matches", "Away_Team_ID", "dbo.Teams");
            DropForeignKey("dbo.Teams", "Last_contributor_ID", "dbo.Users");
            DropForeignKey("dbo.Match_Information", "Contributor_ID", "dbo.Users");
            DropIndex("dbo.Teams", new[] { "Last_contributor_ID" });
            DropIndex("dbo.Matches", new[] { "League_ID" });
            DropIndex("dbo.Matches", new[] { "Home_Team_ID" });
            DropIndex("dbo.Matches", new[] { "Current_Match_Information_ID" });
            DropIndex("dbo.Matches", new[] { "Away_Team_ID" });
            DropIndex("dbo.Match_Information", new[] { "Match_ID" });
            DropIndex("dbo.Match_Information", new[] { "Contributor_ID" });
            DropIndex("dbo.Leagues", new[] { "Creator_ID" });
            DropIndex("dbo.League_contestants", new[] { "Team_ID" });
            DropIndex("dbo.League_contestants", new[] { "League_ID" });
            DropTable("dbo.Teams");
            DropTable("dbo.Matches");
            DropTable("dbo.Match_Information");
            DropTable("dbo.Users");
            DropTable("dbo.Leagues");
            DropTable("dbo.League_contestants");
        }
    }
}
