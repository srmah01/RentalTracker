using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RentalTracker.Domain;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using RentalTracker.DAL.Exceptions;

namespace RentalTracker.DAL.Tests
{
    [TestClass]
    public class RentalTrackerServiceIntegrationTests
    {
        #region Dashboard
        [TestMethod, TestCategory("Integration")]
        public void CanGetNumberOfAccounts()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(3, service.GetNumberOfAccounts());
        }

        [TestMethod, TestCategory("Integration")]
        public void CanGetNumberOfCategories()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(4, service.GetNumberOfCategories());
        }

        [TestMethod, TestCategory("Integration")]
        public void CanGetNumberOfPayees()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(6, service.GetNumberOfPayees());
        }

        [TestMethod, TestCategory("Integration")]
        public void CanGetNumberOfTransactions()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(5, service.GetNumberOfTransactions());
        }

        [TestMethod, TestCategory("Integration")]
        public void CanGetTotalOfAccountBalances()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(1180.99m, service.GetTotalOfAccountBalances());
        }
        #endregion

        #region Accounts
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
        public void CanGetAccountBalanceByAccountId()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(210.99m, service.GetAccountBalance(1));
            Assert.AreEqual(0.00m, service.GetAccountBalance(2));
            Assert.AreEqual(970.00m, service.GetAccountBalance(3));
        }

        [TestMethod, TestCategory("Integration")]
        public void GettingBalanceOfNonExistentAccountReturnsZero()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(0.00m, service.GetAccountBalance(4));
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

        [TestMethod, TestCategory("Integration"),
            ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotInsertNewAccountWithSameNameAsAnExistingAccount()
        {
            DataHelper.NewDb();

            var accountToAdd = new Account()
            {
                Name = "BankAccount3",
                OpeningBalance = 100.99m
            };

            var service = new RentalTrackerService();
            service.SaveNewAccount(accountToAdd);
            Assert.Fail("Added an account with same name as an exiting account");
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

        [TestMethod, TestCategory("Integration"),
            ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotUpdateAnAccountNameToTheSameNameAsAnExistingAccount()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();
            var account = service.FindAccount(1);
            account.Name = "BankAccount3";

            service.SaveUpdatedAccount(account);

            Assert.Fail("Added an account with same name as an exiting account");
        }

        #endregion

        #region Categories
        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewCategory()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var categoryToAdd = new Category()
            {
                Name = "Category Name",
                Type = CategoryType.Expense
            };

            service.SaveNewCategory(categoryToAdd);

            Assert.AreEqual(5, service.GetNumberOfCategories());
        }

        [TestMethod, TestCategory("Integration"),
         ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotInsertANewCategoryWithoutACategoryType()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var categoryToAdd = new Category()
            {
                Name = "Category Name"
            };

            service.SaveNewCategory(categoryToAdd);

            Assert.Fail("Category was added without a CatgeoryType");
        }

        [TestMethod, TestCategory("Integration"),
            ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotInsertNewCategorytWithSameNameAsAnExistingCategory()
        {
            DataHelper.NewDb();

            var categoryToAdd = new Category()
            {
                Name = "Rental Income",
                Type = CategoryType.Expense
            };

            var service = new RentalTrackerService();
            service.SaveNewCategory(categoryToAdd);
            Assert.Fail("Added an category with same name as an exiting category");
        }

        [TestMethod, TestCategory("Integration")]
        public void CanFindCategoryById()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var actual = service.FindCategory(1);

            Assert.AreEqual("Rental Income", actual.Name);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanFindCategoryByIdWithAllItsTransactions()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var actual = service.FindCategoryWithTransactions(1);

            Assert.AreEqual("Rental Income", actual.Name);
            Assert.AreNotEqual(0, actual.Transactions.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanUpdateCategory()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var category = service.FindCategory(1);
            string expected = "Changed Name";
            category.Name = expected;

            service.SaveUpdatedCategory(category);

            Assert.AreEqual(expected, service.FindCategory(1).Name);
        }

        [TestMethod, TestCategory("Integration"),
            ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotUpdateACategoryNameToTheSameNameAsAnExistingCategory()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();
            var category = service.FindCategory(1);
            category.Name = "Utilities";

            service.SaveUpdatedCategory(category);

            Assert.Fail("Added an category with same name as an exiting category");
        }

        #endregion

        #region Payees
        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewPayeeWithExistingDefaultCategory()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var payeeToAdd = new Payee()
            {
                Name = "Payee Name",
                DefaultCategoryId = 1
            };

            service.SaveNewPayee(payeeToAdd);

            Assert.AreEqual(7, service.GetNumberOfPayees());
        }

        [TestMethod, TestCategory("Integration"),
         ExpectedException(typeof(DbUpdateException))]
        public void CannotInsertANewPayeeWithoutADefaultCategory()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var payeeToAdd = new Payee()
            {
                Name = "Payee Name",
            };

            service.SaveNewPayee(payeeToAdd);

            Assert.Fail("Payee was added without a DefaultCategory");
        }

        [TestMethod, TestCategory("Integration"),
            ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotInsertNewPayeetWithSameNameAsAnExistingPayee()
        {
            DataHelper.NewDb();

            var payeeToAdd = new Payee()
            {
                Name = "Renter A",
                DefaultCategoryId = 1
            };

            var service = new RentalTrackerService();
            service.SaveNewPayee(payeeToAdd);
            Assert.Fail("Added an payee with same name as an exiting payee");
        }

        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewPayeeWithANewDefaultCategoryAndBothWillBeInserted()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var payeeToAdd = new Payee()
            {
                Name = "Payee Name",
                DefaultCategory = new Category() { Name = "NewCategory", Type = CategoryType.Income }
            };

            service.SaveNewPayee(payeeToAdd);

            Assert.AreEqual(7, service.GetNumberOfPayees());
            Assert.AreEqual(5, service.GetNumberOfCategories());
        }

        [TestMethod, TestCategory("Integration")]
        public void CanFindPayeeById()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var actual = service.FindPayee(1);

            Assert.AreEqual("Renter A", actual.Name);
            Assert.IsNotNull(actual.DefaultCategory);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanFindPayeeByIdWithAllItsTransactions()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var actual = service.FindPayeeWithTransactions(1);

            Assert.AreEqual("Renter A", actual.Name);
            Assert.AreNotEqual(0, actual.Transactions.Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanUpdatePayee()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var payee = service.FindPayee(1);
            string expected = "Changed Name";
            payee.Name = expected;

            service.SaveUpdatedPayee(payee);

            Assert.AreEqual(expected, service.FindPayee(1).Name);
        }

        [TestMethod, TestCategory("Integration"),
            ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotUpdateAPayeeNameToTheSameNameAsAnExistingPayee()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();
            var payee = service.FindPayee(1);
            payee.Name = "Renter B";

            service.SaveUpdatedPayee(payee);

            Assert.Fail("Added an payee with same name as an exiting payee");
        }
        #endregion

        #region Transactions
        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewTransactionWithAllMandatoryDataPresent()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var transactionToAdd = new Transaction()
            {
                Date = DateTime.Today,
                Amount = 10.00m,
                AccountId = 1,
                PayeeId = 1,
                CategoryId = 1
            };

            service.SaveNewTransaction(transactionToAdd);

            Assert.AreEqual(6, service.GetNumberOfTransactions());
        }

        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewTransactionWithAllFieldsPresent()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var transactionToAdd = new Transaction()
            {
                Date = DateTime.Today,
                Amount = 10.00m,
                AccountId = 1,
                PayeeId = 1,
                CategoryId = 1,
                Reference = "Reference",
                Memo = "Memo"
            };

            service.SaveNewTransaction(transactionToAdd);

            Assert.AreEqual(6, service.GetNumberOfTransactions());
        }


        [TestMethod, TestCategory("Integration"),
         ExpectedException(typeof(DbUpdateException))]
        public void CannotInsertANewTransactionWithoutAnAccount()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var transactionToAdd = new Transaction()
            {
                Date = DateTime.Today,
                Amount = 10.00m,
                PayeeId = 1,
                CategoryId = 1,
                Reference = "Reference",
                Memo = "Memo"
            };

            service.SaveNewTransaction(transactionToAdd);

            Assert.Fail("Transaction was added without an Account");
        }

        [TestMethod, TestCategory("Integration"),
         ExpectedException(typeof(DbUpdateException))]
        public void CannotInsertANewTransactionWithoutAPayee()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var transactionToAdd = new Transaction()
            {
                Date = DateTime.Today,
                Amount = 10.00m,
                AccountId = 1,
                CategoryId = 1,
                Reference = "Reference",
                Memo = "Memo"
            };

            service.SaveNewTransaction(transactionToAdd);

            Assert.Fail("Transaction was added without a Payee");
        }

        [TestMethod, TestCategory("Integration"),
         ExpectedException(typeof(DbUpdateException))]
        public void CannotInsertANewTransactionWithoutACategory()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var transactionToAdd = new Transaction()
            {
                Date = DateTime.Today,
                Amount = 10.00m,
                AccountId = 1,
                PayeeId = 1,
                Reference = "Reference",
                Memo = "Memo"
            };

            service.SaveNewTransaction(transactionToAdd);

            Assert.Fail("Transaction was added without a Category");
        }

        [TestMethod, TestCategory("Integration"),
         ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotInsertANewTransactionWithoutADate()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var transactionToAdd = new Transaction()
            {
                Amount = 10.00m,
                AccountId = 1,
                PayeeId = 1,
                CategoryId = 1,
                Reference = "Reference",
                Memo = "Memo"
            };

            service.SaveNewTransaction(transactionToAdd);

            Assert.Fail("Transaction was added without a Date");
        }

        [TestMethod, TestCategory("Integration"),
         ExpectedException(typeof(RentalTrackerServiceValidationException))]
        public void CannotInsertANewTransactionWithoutAnAmount()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var transactionToAdd = new Transaction()
            {
                Date = DateTime.Today,
                AccountId = 1,
                PayeeId = 1,
                CategoryId = 1,
                Reference = "Reference",
                Memo = "Memo"
            };

            service.SaveNewTransaction(transactionToAdd);

            Assert.Fail("Transaction was added without an Amount");
        }


        [TestMethod, TestCategory("Integration")]
        public void CanInsertNewTransactionWithANewAccountCategoryAndPayeeAndAllWillBeInserted()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            var transactionToAdd = new Transaction()
            {
                Date = DateTime.Today,
                Amount = 10.00m,
                Account = new Account() { Name = "New Accoount", OpeningBalance = 0.00m },
                Payee = new Payee() {  Name = "New Payee", DefaultCategoryId = 1},
                Category = new Category() { Name = "New Category", Type = CategoryType.Income },
                Reference = "Reference",
                Memo = "Memo"
            };

            service.SaveNewTransaction(transactionToAdd);

            Assert.AreEqual(6, service.GetNumberOfTransactions());
            Assert.AreEqual(4, service.GetNumberOfAccounts());
            Assert.AreEqual(7, service.GetNumberOfPayees());
            Assert.AreEqual(5, service.GetNumberOfCategories());
        }
        #endregion
    }
}
