using RentalTracker.Domain;
using System.Data.Entity.ModelConfiguration;

namespace RentalTracker.DAL
{
    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            this.Property(a => a.OpeningBalance).HasPrecision(10, 2);
        }
    }
}