using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.Tests
{
    public class MockedData
    {
        private ICollection<Account> accounts;
        private ICollection<Category> categories;
        private ICollection<Payee> payees;
        private ICollection<Transaction> transactions;
        private DateTime defaultTransactionDate;

        public MockedData()
        {
            defaultTransactionDate = new DateTime(2017, 1, 1);
            accounts = new List<Account>() {
                new Account() { Id = 1, Name = "BankAccount1", OpeningBalance = 100.99m, Balance = 0m },
                new Account() { Id = 2, Name = "BankAccount2", Balance = 0m },
                new Account() { Id = 3, Name = "BankAccount3", OpeningBalance = 1000.00m, Balance = 0m },
                new Account() { Id = 4, Name = "AccountWithNoTransactions", OpeningBalance = 0.0m, Balance = 0.0m }
            };

            categories = new List<Category>() {
                new Category() { Id = 1, Name = "Rental Income", Type = CategoryType.Income },
                new Category() { Id = 2, Name = "Bank Interest", Type = CategoryType.Income },
                new Category() { Id = 3, Name = "Utilities", Type = CategoryType.Expense },
                new Category() { Id = 4, Name = "Bank Charges", Type = CategoryType.Expense },
                new Category() { Id = 5, Name = "CategoryWithNoTransactions", Type = CategoryType.Expense },
            };

            payees = new List<Payee>() {
                new Payee() { Id = 1, Name = "Renter A", DefaultCategoryId = categories.Where(c => c.Name == "Rental Income").Single().Id },
                new Payee() { Id = 2, Name = "Renter B", DefaultCategoryId = categories.Where(c => c.Name == "Rental Income").Single().Id },
                new Payee() { Id = 3, Name = "MyBank Interest", DefaultCategoryId = categories.Where(c => c.Name == "Bank Interest").Single().Id, Memo = "Paid Monthly" },
                new Payee() { Id = 4, Name = "MyBank Charges", DefaultCategoryId = categories.Where(c => c.Name == "Utilities").Single().Id },
                new Payee() { Id = 5, Name = "Gas Supplier", DefaultCategoryId = categories.Where(c => c.Name == "Utilities").Single().Id },
                new Payee() { Id = 6, Name = "Electricity Supplier", DefaultCategoryId = categories.Where(c => c.Name == "Bank Charges").Single().Id, Memo = "For Quarter Feb - May" },
                new Payee() { Id = 7, Name = "PayeeWithNoTransactions", DefaultCategoryId = categories.Where(c => c.Name == "CategoryWithNoTransactions").Single().Id, Memo = "Empty Transactions" }
            };

            transactions = new List<Transaction>() {
                new Transaction()
                {
                    Id = 1,
                    AccountId = accounts.Where(a => a.Name == "BankAccount1").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "Renter A").Single().Id,
                    CategoryId = payees.Where(p => p.Name == "Renter A").Single().DefaultCategoryId,
                    Amount = 10.00m,
                    Date = defaultTransactionDate
                },
                new Transaction()
                {
                    Id = 2,
                    AccountId = accounts.Where(a => a.Name == "BankAccount1").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "Renter B").Single().Id,
                    CategoryId = categories.Where(p => p.Name == "Rental Income").Single().Id,
                    Amount = 100.00m,
                    Date = defaultTransactionDate
                },
                new Transaction()
                {
                    Id = 3,
                    AccountId = accounts.Where(a => a.Name == "BankAccount2").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "Renter A").Single().Id,
                    CategoryId = payees.Where(p => p.Name == "Renter A").Single().DefaultCategoryId,
                    Amount = 200.00m,
                    Date = defaultTransactionDate.AddDays(1)
                },
                new Transaction()
                {
                    Id = 4,
                    AccountId = accounts.Where(a => a.Name == "BankAccount2").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "Gas Supplier").Single().Id,
                    CategoryId = categories.Where(p => p.Name == "Utilities").Single().Id,
                    Amount = -200.00m,
                    Date = defaultTransactionDate.AddDays(2),
                    Memo = "For Quarter May - Aug"
                },
                new Transaction()
                {
                    Id = 5,
                    AccountId = accounts.Where(a => a.Name == "BankAccount3").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "MyBank Charges").Single().Id,
                    CategoryId = categories.Where(p => p.Name == "Bank Charges").Single().Id,
                    Amount = -30.00m,
                    Date = defaultTransactionDate.AddDays(3)
                }
            };


            // Seed Balance with value including amounts from transactions
            foreach (var account in accounts)
            {
                var amounts = transactions.Where(t => t.AccountId == account.Id).Sum(t => t.Amount);
                account.Balance = account.OpeningBalance + amounts;
            }

            // Wire up references
            foreach(var payee in payees)
            {
                payee.DefaultCategory = categories.Where(c => c.Id == payee.DefaultCategoryId).Single();
            }

            foreach(var transaction in transactions)
            {
                transaction.Account = accounts.Where(a => a.Id == transaction.AccountId).Single();
                accounts.Where(a => a.Id == transaction.AccountId).Single().Transactions.Add(transaction);

                transaction.Category = categories.Where(c => c.Id == transaction.CategoryId).Single();
                categories.Where(c => c.Id == transaction.CategoryId).Single().Transactions.Add(transaction);

                transaction.Payee = payees.Where(p => p.Id == transaction.PayeeId).Single();
                payees.Where(c => c.Id == transaction.PayeeId).Single().Transactions.Add(transaction);
            }
        }

        public ICollection<Account> Accounts
        {
            get { return accounts; }
            set { accounts = value; }
        }


        public ICollection<Category> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        public ICollection<Payee> Payees
        {
            get { return payees; }
            set { payees = value; }
        }

        public ICollection<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions = value; }
        }

        public ICollection<Transaction> GetAccountTransactions(int accountId)
        {
            return transactions.Where(t => t.AccountId == accountId).ToList();
        }

        public ICollection<Transaction> GetCategoryTransactions(int categoryId)
        {
            return transactions.Where(t => t.AccountId == categoryId).ToList();
        }

        public ICollection<Transaction> GetPayeeTransactions(int payeeId)
        {
            return transactions.Where(t => t.AccountId == payeeId).ToList();
        }

    }
}
