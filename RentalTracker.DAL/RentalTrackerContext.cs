using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.DAL
{
    public class RentalTrackerContext : DbContext
    {
        public RentalTrackerContext() : base("RentalTracker")
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Payee> Payees { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new AccountConfiguration());
            //modelBuilder.Configurations.Add(new CategoryConfiguration());
            //modelBuilder.Configurations.Add(new PayeeConfiguration());
            modelBuilder.Configurations.Add(new TransactionConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
