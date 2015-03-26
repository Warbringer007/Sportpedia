namespace EFDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Teams", "Comment");
        }
    }
}
