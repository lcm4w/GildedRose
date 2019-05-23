namespace GildedRose.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemsSku : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Sku", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Sku");
        }
    }
}
