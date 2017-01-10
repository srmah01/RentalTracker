using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.DAL
{
    public class DataHelper
    {
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

        public static void PrepareData(RentalTrackerContext context)
        {
            var accountsToAdd = new List<Account>()
            {
                new Account() { Name = "BankAccount1", OpeningBalance = 100.99m },
                new Account() { Name = "BankAccount2" },
                new Account() { Name = "BankAccount3", OpeningBalance = 1000.00m }
            };

            var rentalIncome = new Category() { Name = "Rental Income", Type = CategoryType.Income };
            var bankInterest = new Category() { Name = "Bank Interest", Type = CategoryType.Income };
            var utilities = new Category() { Name = "Utilities", Type = CategoryType.Expenditure };
            var bankCharges = new Category() { Name = "Bank Charges", Type = CategoryType.Expenditure };
            var categoriesToAdd = new List<Category>()
            {
                rentalIncome,
                bankInterest,
                utilities,
                bankCharges
            };

            var renterA = new Payee() { Name = "Renter A" };
            var renterB = new Payee() { Name = "Renter B", DefaultCategory = rentalIncome };
            var myBankInterest = new Payee() { Name = "MyBank Interest" };
            var myBankCharges = new Payee() { Name = "MyBank Charges", DefaultCategory = bankCharges };
            var gasSupplier = new Payee() { Name = "Gas Supplier" };
            var electricitySupplier = new Payee() { Name = "Electricity Supplier", DefaultCategory = utilities };
            var payeesToAdd = new List<Payee>()
            {
                renterA,
                renterB,
                myBankInterest,
                myBankCharges,
                gasSupplier,
                electricitySupplier
            };

            var transactionsToAdd = new List<Transaction>()
            {
                new Transaction() { AccountId = 1, Payee = renterA, Amount = 10.00m, Date = DateTime.Today},
                new Transaction() { AccountId = 1, Payee = renterB, Amount = 100.00m, Date = DateTime.Today},
                new Transaction() { AccountId = 2, Payee = renterA, Category = rentalIncome, Amount = 200.00m, Date = DateTime.Today},
                new Transaction() { AccountId = 2, Payee = renterB, Category = bankInterest, Amount = 20.00m, Date = DateTime.Today},
                new Transaction() { AccountId = 3, Payee = myBankCharges, Category = bankCharges, Amount = 30.00m, Date = DateTime.Today},
            };

            context.Accounts.AddRange(accountsToAdd);
            context.Catgories.AddRange(categoriesToAdd);
            context.Payees.AddRange(payeesToAdd);
            context.Transactions.AddRange(transactionsToAdd);

            // Add transactions to bank accounts
            foreach (var transaction in transactionsToAdd)
            {
                var account = accountsToAdd.ElementAt(transaction.AccountId - 1);

                account.Transactions.Add(transaction);
            }

            context.SaveChanges();

        }

    }
}
