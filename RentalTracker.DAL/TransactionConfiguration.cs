using RentalTracker.Domain;
using System.Data.Entity.ModelConfiguration;

namespace RentalTracker.DAL
{
    public class TransactionConfiguration : EntityTypeConfiguration<Transaction>
    {
        public TransactionConfiguration()
        {
            this.Property(t => t.Amount)
                .HasPrecision(10, 2);
            //this.HasRequired(t => t.Payee)
            //    .WithRequiredDependent()
            //    .WillCascadeOnDelete(false);
            //this.HasOptional(t => t.Category)
            //    .WithOptionalDependent()
            //    .WillCascadeOnDelete(false);
        }
    }
}