namespace RentalTracker.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class make_category_required_in_Payee_and_Transaction : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Transactions", new[] { "CategoryId" });
            DropIndex("dbo.Payees", new[] { "DefaultCategoryId" });
            AlterColumn("dbo.Transactions", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Payees", "DefaultCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Transactions", "CategoryId");
            CreateIndex("dbo.Payees", "DefaultCategoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Payees", new[] { "DefaultCategoryId" });
            DropIndex("dbo.Transactions", new[] { "CategoryId" });
            AlterColumn("dbo.Payees", "DefaultCategoryId", c => c.Int());
            AlterColumn("dbo.Transactions", "CategoryId", c => c.Int());
            CreateIndex("dbo.Payees", "DefaultCategoryId");
            CreateIndex("dbo.Transactions", "CategoryId");
        }
    }
}
