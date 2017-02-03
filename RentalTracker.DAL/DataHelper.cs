using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.DAL
{
    /// <summary>
    /// Class to help create and seed the database for testing.
    /// </summary>
    public class DataHelper
    {
        // Getters / Setters of Seed data to help with testing
        public static IList<Account> Accounts { get; set; }
        public static IList<Category> Categories { get; set; }
        public static IList<Payee> Payees { get; set; }
        public static IList<Transaction> Transactions { get; set; }

        /// <summary>
        /// Drop and re-add the database instance with or without seeded data
        /// </summary>
        /// <param name="withSeed"></param>
        public static void NewDb(bool withSeed = true)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<RentalTrackerContext>());
            using (var context = new RentalTrackerContext())
            {
                context.Database.Initialize(true);
                if (withSeed)
                {
                    PrepareData(context);
                }
            }
        }

        /// <summary>
        /// Seed the context with some representative dummy data
        /// </summary>
        /// <param name="context"></param>
        public static void PrepareData(RentalTrackerContext context)
        {
            var accountsToAdd = new List<Account>()
            {
                new Account() { Name = "BankAccount1", OpeningBalance = 100.99m },
                new Account() { Name = "BankAccount2" },
                new Account() { Name = "BankAccount3", OpeningBalance = 1000.00m }
            };
            context.Accounts.AddRange(accountsToAdd);
            context.SaveChanges();
            var accountsAdded = context.Accounts.ToList();

            var categoriesToAdd = new List<Category>()
            {
                new Category() { Name = "Rental Income", Type = CategoryType.Income },
                new Category() { Name = "Bank Interest", Type = CategoryType.Income },
                new Category() { Name = "Utilities", Type = CategoryType.Expense },
                new Category() { Name = "Bank Charges", Type = CategoryType.Expense }
            };
            context.Categories.AddRange(categoriesToAdd);
            context.SaveChanges();
            var categoriesAdded = context.Categories.ToList();

            var payeesToAdd = new List<Payee>()
            {
                new Payee() { Name = "Renter A", DefaultCategoryId = categoriesAdded.Where(c => c.Name == "Rental Income").Single().Id },
                new Payee() { Name = "Renter B", DefaultCategoryId = categoriesAdded.Where(c => c.Name == "Rental Income").Single().Id },
                new Payee() { Name = "MyBank Interest", DefaultCategoryId = categoriesAdded.Where(c => c.Name == "Bank Interest").Single().Id, Memo = "Paid Monthly" },
                new Payee() { Name = "MyBank Charges", DefaultCategoryId = categoriesAdded.Where(c => c.Name == "Utilities").Single().Id },
                new Payee() { Name = "Gas Supplier", DefaultCategoryId = categoriesAdded.Where(c => c.Name == "Utilities").Single().Id },
                new Payee() { Name = "Electricity Supplier", DefaultCategoryId = categoriesAdded.Where(c => c.Name == "Bank Charges").Single().Id, Memo = "For Quarter Feb - May" }
            };
            context.Payees.AddRange(payeesToAdd);
            context.SaveChanges();
            var payeesAdded = context.Payees.Include(p => p.DefaultCategory).ToList();

            var defaultTransactionDate = new DateTime(2016, 1, 1);
            var transactionsToAdd = new List<Transaction>()
            {
                new Transaction() { AccountId = accountsAdded.Where(a => a.Name == "BankAccount1").Single().Id,
                    PayeeId = payeesAdded.Where(p => p.Name == "Renter A").Single().Id,
                    CategoryId = payeesAdded.Where(p => p.Name == "Renter A").Single().DefaultCategoryId,
                    Amount = 10.00m, Date = defaultTransactionDate},
                new Transaction() { AccountId = accountsAdded.Where(a => a.Name == "BankAccount1").Single().Id,
                    PayeeId = payeesAdded.Where(p => p.Name == "Renter B").Single().Id,
                    CategoryId = categoriesAdded.Where(p => p.Name == "Rental Income").Single().Id,
                    Amount = 100.00m, Date = defaultTransactionDate},
                new Transaction() { AccountId = accountsAdded.Where(a => a.Name == "BankAccount2").Single().Id,
                    PayeeId = payeesAdded.Where(p => p.Name == "Renter A").Single().Id,
                    CategoryId = payeesAdded.Where(p => p.Name == "Renter A").Single().DefaultCategoryId,
                    Amount = 200.00m, Date = defaultTransactionDate.AddDays(1)},
                new Transaction() { AccountId = accountsAdded.Where(a => a.Name == "BankAccount2").Single().Id,
                    PayeeId = payeesAdded.Where(p => p.Name == "Gas Supplier").Single().Id,
                    CategoryId = categoriesAdded.Where(p => p.Name == "Utilities").Single().Id,
                    Amount = 200.00m, Date = defaultTransactionDate.AddDays(2),  Memo = "For Quarter May - Aug"},
                new Transaction() { AccountId = accountsAdded.Where(a => a.Name == "BankAccount3").Single().Id,
                    PayeeId = payeesAdded.Where(p => p.Name == "MyBank Charges").Single().Id,
                    CategoryId = categoriesAdded.Where(p => p.Name == "Bank Charges").Single().Id,
                    Amount = 30.00m, Date = defaultTransactionDate.AddDays(3)},
            };
            context.Transactions.AddRange(transactionsToAdd);
            context.SaveChanges();

            Accounts = accountsAdded;
            Categories = categoriesAdded;
            Payees = payeesAdded;
            Transactions = context.Transactions.ToList();
        }

        /// <summary>
        /// Get the Account balance of an account using seed transactions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public Decimal GetAccountBalance(int id)
        {
            var openingBalance = Accounts.FirstOrDefault(a => a.Id == id).OpeningBalance;

            var amounts = Transactions.Where(t => t.AccountId == id)
                                       .Select(t => new { Amount = t.Amount, CategoryType = t.Category.Type })
                                       .ToList();

            var income = amounts.Where(a => a.CategoryType == CategoryType.Income).ToList().Sum(a => a.Amount);

            var expense = amounts.Where(a => a.CategoryType == CategoryType.Expense).ToList().Sum(a => a.Amount);

            return (openingBalance + income - expense);
        }
    }
}
