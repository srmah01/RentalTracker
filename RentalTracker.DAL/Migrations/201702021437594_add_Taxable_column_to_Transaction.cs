namespace RentalTracker.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Taxable_column_to_Transaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Taxable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Taxable");
        }
    }
}
