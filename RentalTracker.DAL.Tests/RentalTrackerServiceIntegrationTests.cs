﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RentalTracker.Domain;

namespace RentalTracker.DAL.Tests
{
    [TestClass]
    public class RentalTrackerServiceIntegrationTests
    {
        [TestMethod, TestCategory("Integration")]
        public void CanGetAnEmptyListOfAccounts()
        {
            DataHelper.NewDb(false);

            var service = new RentalTrackerService();

            Assert.AreEqual(0, service.GetAllAccounts().Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanGetAListOfAccounts()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(3, service.GetAllAccounts().Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewAccount()
        {
            DataHelper.NewDb();

            var accountToAdd = new Account()
            {
                Name = "BankAccount4",
                OpeningBalance = 100.99m
            };

            var service = new RentalTrackerService();
            service.SaveNewAccount(accountToAdd);
            Assert.AreEqual(4, service.GetAllAccounts().Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanUpdateAccount()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var account = service.FindAccount(1);
            string expected = "ChangedBankName";
            account.Name = expected;

            service.SaveUpdatedAccount(account);

            Assert.AreEqual(expected, service.FindAccount(1).Name);
        }

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


    }
}