namespace RentalTracker.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_balance_to_Account : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Balance");
        }
    }
}
