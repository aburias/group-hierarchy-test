namespace BH.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGroupForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "ParentGroup_Id", newName: "ParentGroupId");
            RenameIndex(table: "dbo.Groups", name: "IX_ParentGroup_Id", newName: "IX_ParentGroupId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Groups", name: "IX_ParentGroupId", newName: "IX_ParentGroup_Id");
            RenameColumn(table: "dbo.Groups", name: "ParentGroupId", newName: "ParentGroup_Id");
        }
    }
}
