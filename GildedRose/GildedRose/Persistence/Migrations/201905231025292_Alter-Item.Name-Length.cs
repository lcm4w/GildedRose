namespace GildedRose.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterItemNameLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Items", "Name", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "Name", c => c.String(nullable: false));
        }
    }
}
