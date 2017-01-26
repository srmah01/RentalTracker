using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using RentalTracker.DAL.Exceptions;

namespace RentalTracker.DAL
{
    public class RentalTrackerContext : DbContext
    {
        public RentalTrackerContext() : base("RentalTracker")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Payee> Payees { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new AccountConfiguration());
            //modelBuilder.Configurations.Add(new CategoryConfiguration());
            //modelBuilder.Configurations.Add(new PayeeConfiguration());
            modelBuilder.Configurations.Add(new TransactionConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new RentalTrackerServiceValidationException(exceptionMessage, GetErrors(ex.EntityValidationErrors));
            }
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
            if (entityEntry.Entity is Account &&
                (entityEntry.State == EntityState.Added))
            {
                Account post = entityEntry.Entity as Account;
                //check for uniqueness of Account Name
                if (Accounts.Where(p => p.Name == post.Name).Count() > 0)
                {
                    result.ValidationErrors.Add(
                            new DbValidationError("Name", "Account name must be unique."));
                }
            }

            if (result.ValidationErrors.Count > 0)
            {
                return result;
            }
            else
            {
                return base.ValidateEntity(entityEntry, items);
            }
        }

        private IEnumerable<ValidationResult> GetErrors(IEnumerable<DbEntityValidationResult> errors)
        {
            return errors.SelectMany(
                        x => x.ValidationErrors.Select(y =>
                              new ValidationResult(y.ErrorMessage, new[] { y.PropertyName })))
                        .ToList();
        }

    }
}
