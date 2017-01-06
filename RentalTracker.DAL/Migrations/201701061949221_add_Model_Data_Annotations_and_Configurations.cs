namespace RentalTracker.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Model_Data_Annotations_and_Configurations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Transactions", "PayeeId", "dbo.Payees");
            DropIndex("dbo.Transactions", new[] { "CategoryId" });
            RenameColumn(table: "dbo.Transactions", name: "CategoryId", newName: "Category_Id");
            RenameColumn(table: "dbo.Transactions", name: "PayeeId", newName: "Payee_Id");
            RenameIndex(table: "dbo.Transactions", name: "IX_PayeeId", newName: "IX_Payee_Id");
            AddColumn("dbo.Payees", "DefaultCategory_Id", c => c.Int());
            AlterColumn("dbo.Accounts", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Accounts", "OpeningBalance", c => c.Decimal(nullable: false, precision: 10, scale: 2));
            AlterColumn("dbo.Transactions", "Category_Id", c => c.Int());
            AlterColumn("dbo.Transactions", "Amount", c => c.Decimal(nullable: false, precision: 10, scale: 2));
            AlterColumn("dbo.Transactions", "Reference", c => c.String(maxLength: 100));
            AlterColumn("dbo.Transactions", "Number", c => c.String(maxLength: 20));
            AlterColumn("dbo.Transactions", "Memo", c => c.String(maxLength: 200));
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Payees", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Payees", "Memo", c => c.String(maxLength: 200));
            CreateIndex("dbo.Transactions", "Category_Id");
            CreateIndex("dbo.Payees", "DefaultCategory_Id");
            AddForeignKey("dbo.Payees", "DefaultCategory_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts", "Id");
            AddForeignKey("dbo.Transactions", "Category_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Transactions", "Payee_Id", "dbo.Payees", "Id");
            DropColumn("dbo.Payees", "DefaultCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payees", "DefaultCategory", c => c.String());
            DropForeignKey("dbo.Transactions", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.Transactions", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Payees", "DefaultCategory_Id", "dbo.Categories");
            DropIndex("dbo.Payees", new[] { "DefaultCategory_Id" });
            DropIndex("dbo.Transactions", new[] { "Category_Id" });
            AlterColumn("dbo.Payees", "Memo", c => c.String());
            AlterColumn("dbo.Payees", "Name", c => c.String());
            AlterColumn("dbo.Categories", "Name", c => c.String());
            AlterColumn("dbo.Transactions", "Memo", c => c.String());
            AlterColumn("dbo.Transactions", "Number", c => c.String());
            AlterColumn("dbo.Transactions", "Reference", c => c.String());
            AlterColumn("dbo.Transactions", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Transactions", "Category_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Accounts", "OpeningBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Accounts", "Name", c => c.String());
            DropColumn("dbo.Payees", "DefaultCategory_Id");
            RenameIndex(table: "dbo.Transactions", name: "IX_Payee_Id", newName: "IX_PayeeId");
            RenameColumn(table: "dbo.Transactions", name: "Payee_Id", newName: "PayeeId");
            RenameColumn(table: "dbo.Transactions", name: "Category_Id", newName: "CategoryId");
            CreateIndex("dbo.Transactions", "CategoryId");
            AddForeignKey("dbo.Transactions", "PayeeId", "dbo.Payees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
    }
}
