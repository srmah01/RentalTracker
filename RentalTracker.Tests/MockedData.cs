using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentalTracker.Tests
{
    /// <summary>
    /// A class representing mocked data to be used by the controller tests.
    /// </summary>
    public class MockedData
    {
        /// <summary>
        /// A collection of Account entities.
        /// </summary>
        private ICollection<Account> accounts;

        /// <summary>
        /// A collection of Category entities.
        /// </summary>
        private ICollection<Category> categories;

        /// <summary>
        /// A collection of Payee entities.
        /// </summary>
        private ICollection<Payee> payees;

        /// <summary>
        /// A collection of Transaction entities.
        /// </summary>
        private ICollection<Transaction> transactions;

        /// <summary>
        /// Constructor.
        /// Creates collections of entities and links between them.
        /// </summary>
        public MockedData()
        {
            var defaultTransactionDate = new DateTime(2017, 1, 1);
            accounts = new List<Account>() {
                new Account() { Id = 1, Name = "BankAccount1", OpeningBalance = 100.99m },
                new Account() { Id = 2, Name = "BankAccount2" },
                new Account() { Id = 3, Name = "BankAccount3", OpeningBalance = 1000.00m },
                new Account() { Id = 4, Name = "AccountWithNoTransactions", OpeningBalance = 0.0m }
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
                    Amount = 10.00m, Balance = 110.99m,
                    Date = defaultTransactionDate
                },
                new Transaction()
                {
                    Id = 2,
                    AccountId = accounts.Where(a => a.Name == "BankAccount1").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "Renter B").Single().Id,
                    CategoryId = categories.Where(p => p.Name == "Rental Income").Single().Id,
                    Amount = 100.00m, Balance = 210.99m,
                    Date = defaultTransactionDate
                },
                new Transaction()
                {
                    Id = 3,
                    AccountId = accounts.Where(a => a.Name == "BankAccount2").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "Renter A").Single().Id,
                    CategoryId = payees.Where(p => p.Name == "Renter A").Single().DefaultCategoryId,
                    Amount = 200.00m, Balance = 200.00m,
                    Date = defaultTransactionDate.AddDays(1)
                },
                new Transaction()
                {
                    Id = 4,
                    AccountId = accounts.Where(a => a.Name == "BankAccount2").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "Gas Supplier").Single().Id,
                    CategoryId = categories.Where(p => p.Name == "Utilities").Single().Id,
                    Amount = 200.00m, Balance = 0.00m,
                    Date = defaultTransactionDate.AddDays(2),
                    Memo = "For Quarter May - Aug"
                },
                new Transaction()
                {
                    Id = 5,
                    AccountId = accounts.Where(a => a.Name == "BankAccount3").Single().Id,
                    PayeeId = payees.Where(p => p.Name == "MyBank Charges").Single().Id,
                    CategoryId = categories.Where(p => p.Name == "Bank Charges").Single().Id,
                    Amount = 30.00m, Balance = 970.00m,
                    Date = defaultTransactionDate.AddDays(3)
                }
            };

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

        /// <summary>
        /// Gets or Sets the collection of Accounts.
        /// </summary>
        public ICollection<Account> Accounts
        {
            get { return accounts; }
            private set { accounts = value; }
        }


        /// <summary>
        /// Gets or Sets the collection of Categories.
        /// </summary>
        public ICollection<Category> Categories
        {
            get { return categories; }
            private set { categories = value; }
        }

        /// <summary>
        /// Gets or Sets the collection of Payees.
        /// </summary>
        public ICollection<Payee> Payees
        {
            get { return payees; }
            private set { payees = value; }
        }

        /// <summary>
        /// Gets or Sets the collection of Transactions.
        /// </summary>
        public ICollection<Transaction> Transactions
        {
            get { return transactions; }
            private set { transactions = value; }
        }

        /// <summary>
        /// Gets the collection of Transactions associated with a specified account.
        /// </summary>
        /// <param name="accountId">The ID of the specified account.</param>
        /// <returns></returns>
        public ICollection<Transaction> GetAccountTransactions(int accountId)
        {
            return transactions.Where(t => t.AccountId == accountId).ToList();
        }

        /// <summary>
        /// Gets the collection of Transactions associated with a specified Category.
        /// </summary>
        /// <param name="categoryId">The ID of the specified category.</param>
        /// <returns></returns>
        public ICollection<Transaction> GetCategoryTransactions(int categoryId)
        {
            return transactions.Where(t => t.CategoryId == categoryId).ToList();
        }

        /// <summary>
        /// Gets the collection of Transactions associated with a specified payee.
        /// </summary>
        /// <param name="payeeId">The ID of the specified payee.</param>
        /// <returns></returns>
        public ICollection<Transaction> GetPayeeTransactions(int payeeId)
        {
            return transactions.Where(t => t.PayeeId == payeeId).ToList();
        }
    }
}
