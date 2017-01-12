namespace RentalTracker.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Payee_and_Category_keys : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Transactions", name: "Category_Id", newName: "CategoryId");
            RenameColumn(table: "dbo.Transactions", name: "Payee_Id", newName: "PayeeId");
            RenameColumn(table: "dbo.Payees", name: "DefaultCategory_Id", newName: "DefaultCategoryId");
            RenameIndex(table: "dbo.Transactions", name: "IX_Payee_Id", newName: "IX_PayeeId");
            RenameIndex(table: "dbo.Transactions", name: "IX_Category_Id", newName: "IX_CategoryId");
            RenameIndex(table: "dbo.Payees", name: "IX_DefaultCategory_Id", newName: "IX_DefaultCategoryId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Payees", name: "IX_DefaultCategoryId", newName: "IX_DefaultCategory_Id");
            RenameIndex(table: "dbo.Transactions", name: "IX_CategoryId", newName: "IX_Category_Id");
            RenameIndex(table: "dbo.Transactions", name: "IX_PayeeId", newName: "IX_Payee_Id");
            RenameColumn(table: "dbo.Payees", name: "DefaultCategoryId", newName: "DefaultCategory_Id");
            RenameColumn(table: "dbo.Transactions", name: "PayeeId", newName: "Payee_Id");
            RenameColumn(table: "dbo.Transactions", name: "CategoryId", newName: "Category_Id");
        }
    }
}
