using RentalTracker.Domain;
using System.Data.Entity.ModelConfiguration;

namespace RentalTracker.DAL
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            //this.HasMany(p => p.Transactions)
            //    .WithOptional();
        }
    }
}