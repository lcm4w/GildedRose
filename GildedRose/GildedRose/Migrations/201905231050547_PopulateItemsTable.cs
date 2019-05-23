namespace GildedRose.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateItemsTable : DbMigration
    {
       public override void Up()
        {
			Sql("INSERT INTO Items (Sku, Name, Description, Price, Quantity) VALUES ('GR-0001', 'Copper spoon', 'The finest spoon in town.', 49.97, 100)");
			Sql("INSERT INTO Items (Sku, Name, Description, Price, Quantity) VALUES ('GR-0002', 'Silver Plate', 'The finest plate in town.', 59.97, 200)");
			Sql("INSERT INTO Items (Sku, Name, Description, Price, Quantity) VALUES ('GR-0003', 'Gold fork', 'The finest fork in town.', 299.97, 50)");
			Sql("INSERT INTO Items (Sku, Name, Description, Price, Quantity) VALUES ('GR-0004', 'Platinum glass', 'The finest glass in town.', 1479.97, 20)");
        }
        
        public override void Down()
        {
			Sql("DELETE FROM Items WHERE Sku IN ('GR-0001', 'GR-0002', 'GR-0003', 'GR-0004')");
        }
    }
}
