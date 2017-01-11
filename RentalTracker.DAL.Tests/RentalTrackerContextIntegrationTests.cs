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

            DataHelper.PrepareData(_context);

            SetupLogging();
        }

        //[TestMethod, TestCategory("Integration")]
        //public void CanInsertNewAccount()
        //{
        //    var accountToAdd = new Account()
        //    {
        //        Name = "BankAccount4",
        //        OpeningBalance = 100.99m
        //    };

        //    _context.Accounts.Add(accountToAdd);
        //    _context.SaveChanges();
        //    WriteLog();
        //    Assert.AreEqual(4, _context.Accounts.Local.Count);
        //}

        //[TestMethod, TestCategory("Integration")]
        //public void CanFindAccountByID()
        //{
        //    var actual = _context.Accounts.Find(1);
        //    WriteLog();
        //    Assert.AreEqual(1, actual.Id);
        //}

        //[TestMethod, TestCategory("Integration")]
        //public void CanUpdateAccount()
        //{
        //    var account = _context.Accounts.Find(1);
        //    string expected = "ChangedBankName";
        //    account.Name = expected;

        //    _context.Accounts.Attach(account);
        //    _context.SaveChanges();
        //    WriteLog();

        //    var actual = _context.Accounts.Find(1);
        //    Assert.AreEqual(expected, actual.Name);
        //}

        //[TestMethod, TestCategory("Integration"), ExpectedException(typeof(InvalidOperationException))]
        //public void CannotDeleteAccountWithAssociatedTransactions()
        //{
        //    var account = _context.Accounts.Find(3);
        //    _context.Accounts.Remove(account);
        //    _context.SaveChanges();
        //}

        //[TestMethod, TestCategory("Integration")]
        //public void CanDeleteAccountWithNoAssociatedTransactions()
        //{
        //    var transaction = _context.Transactions.Find(5);
        //    _context.Transactions.Remove(transaction);

        //    var account = _context.Accounts.Find(3);
        //    _context.Accounts.Remove(account);
        //    _context.SaveChanges();
        //    WriteLog();

        //    Assert.AreEqual(2, _context.Accounts.Local.Count);
        //}

        //[TestMethod, TestCategory("Integration")]
        //public void CanInsertNewPayeeWithNoDefaultCategory()
        //{
        //    var payeeToAdd = new Payee()
        //    {
        //        Name = "Payee Name",
        //    };

        //    _context.Payees.Add(payeeToAdd);
        //    _context.SaveChanges();
        //    WriteLog();
        //    Assert.AreEqual(7, _context.Payees.Local.Count);
        //}

        //[TestMethod, TestCategory("Integration")]
        //public void CanInsertNewPayeeWithDefaultCategory()
        //{
        //    var payeeToAdd = new Payee()
        //    {
        //        Name = "Payee Name",
        //        DefaultCategory = new Category() { Name = "Category Name", Type = CategoryType.Income }
        //    };

        //    _context.Payees.Add(payeeToAdd);
        //    _context.SaveChanges();
        //    WriteLog();
        //    Assert.AreEqual(7, _context.Payees.Local.Count);
        //    Assert.AreEqual(5, _context.Catgories.Local.Count);
        //}

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

    }
}
