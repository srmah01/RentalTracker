using RentalTracker.Domain;
using System.Data.Entity.ModelConfiguration;

namespace RentalTracker.DAL
{
    public class PayeeConfiguration : EntityTypeConfiguration<Payee>
    {
        public PayeeConfiguration()
        {
            //this.HasOptional(p => p.DefaultCategory)
            //    .WithOptionalDependent()
            //    .WillCascadeOnDelete(false);

            //this.HasMany(p => p.Transactions)
            //    .WithOptional();
         }
    }
}