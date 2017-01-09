using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Domain;
using System.Data.Entity;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace RentalTracker.DAL.Tests
{
    [TestClass]
    public class RentalTrackerContextIntegrationTests
    {
        private string _log;
        private StringBuilder _logBuilder = new StringBuilder();
        private RentalTrackerContext _context;

        public RentalTrackerContextIntegrationTests()
        {
            //app.config points to a special testing database:RentalTrackerTest
            Database.SetInitializer(new DropCreateDatabaseAlways<RentalTrackerContext>());
            _context = new RentalTrackerContext();
            _context.Database.Initialize(true); //get this out of the way before logging
            SetupLogging();
        }

        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewAccount()
        {
            var accountToAdd = new Account()
            {
                Name = "BankAccount1",
                OpeningBalance = 100.99m
            };

            _context.Accounts.Add(accountToAdd);
            _context.SaveChanges();
            WriteLog();
            Assert.AreEqual(1, _context.Accounts.Local.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanFindAccountByID()
        {
            PrepareData();

            var actual = _context.Accounts.Find(1);
            WriteLog();
            Assert.AreEqual(1, actual.Id);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanUpdateAccount()
        {
            PrepareData();

            var account = _context.Accounts.Find(1);
            string expected = "ChangedBankName";
            account.Name = expected;

            _context.Accounts.Attach(account);
            _context.SaveChanges();
            WriteLog();

            var actual = _context.Accounts.Find(1);
            Assert.AreEqual(expected, actual.Name);
        }

        [TestMethod, TestCategory("Integration"), ExpectedException(typeof(InvalidOperationException))]
        public void CannotDeleteAccountWithAssociatedTransactions()
        {
            PrepareData();

            var account = _context.Accounts.Find(3);
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        [TestMethod, TestCategory("Integration")]
        public void CanDeleteAccountWithNoAssociatedTransactions()
        {
            PrepareData();

            var transaction = _context.Transactions.Find(5);
            _context.Transactions.Remove(transaction);

            var account = _context.Accounts.Find(3);
            _context.Accounts.Remove(account);
            _context.SaveChanges();
            WriteLog();

            Assert.AreEqual(2, _context.Accounts.Local.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewPayeeWithNoDefaultCategory()
        {
            var payeeToAdd = new Payee()
            {
                Name = "Payee Name",
            };

            _context.Payees.Add(payeeToAdd);
            _context.SaveChanges();
            WriteLog();
            Assert.AreEqual(1, _context.Payees.Local.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewPayeeWithDefaultCategory()
        {
            var payeeToAdd = new Payee()
            {
                Name = "Payee Name",
                DefaultCategory = new Category() { Name = "Category Name", Type = CategoryType.Income }
            };

            _context.Payees.Add(payeeToAdd);
            _context.SaveChanges();
            WriteLog();
            Assert.AreEqual(1, _context.Payees.Local.Count);
            Assert.AreEqual(1, _context.Catgories.Local.Count);
        }

        private void SetupLogging()
        {
            _context.Database.Log = BuildLogString;
        }

        private void BuildLogString(string message)
        {
            _logBuilder.Append(message);
            _log = _logBuilder.ToString();
        }

        private void WriteLog()
        {
            Debug.WriteLine(_log);
        }

        private void PrepareData()
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

            _context.Accounts.AddRange(accountsToAdd);
            _context.Catgories.AddRange(categoriesToAdd);
            _context.Payees.AddRange(payeesToAdd);
            _context.Transactions.AddRange(transactionsToAdd);

            // Add transactions to bank accounts
            foreach (var transaction in transactionsToAdd)
            {
                var account = accountsToAdd.ElementAt(transaction.AccountId - 1);

                account.Transactions.Add(transaction);
            }

            _context.SaveChanges();

        }

    }
}
