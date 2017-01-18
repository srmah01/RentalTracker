namespace RentalTracker.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_Number_column_and_reduce_size_of_Reference_column : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "Reference", c => c.String(maxLength: 30));
            DropColumn("dbo.Transactions", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "Number", c => c.String(maxLength: 20));
            AlterColumn("dbo.Transactions", "Reference", c => c.String(maxLength: 100));
        }
    }
}
