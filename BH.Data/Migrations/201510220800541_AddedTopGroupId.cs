namespace BH.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTopGroupId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "TopGroupId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "TopGroupId");
        }
    }
}
