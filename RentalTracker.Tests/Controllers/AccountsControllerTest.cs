using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker;
using RentalTracker.Controllers;
using RentalTracker.DAL;
using Moq;
using RentalTracker.Domain;
using RentalTracker.Models;

namespace RentalTracker.Tests.Controllers
{
    [TestClass]
    public class AccountsControllerTest
    {
        [TestMethod]
        public void CanGetIndexView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllAccounts()).Returns(new List<Account>());

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ICollection<Account>;
            Assert.AreEqual(0, model.Count);
        }

        [TestMethod]
        public void CanGetIndexViewWithListOfAccounts()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllAccounts()).Returns(new List<Account>()
                {
                    new Account() { Id = 1, Name = "BankAccount1", OpeningBalance = 0.00m },
                    new Account() { Id = 2, Name = "BankAccount2", OpeningBalance = 1.99m },
                    new Account() { Id = 3, Name = "BankAccount3", OpeningBalance = 1000.00m },
                }
            );

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ICollection<Account>;
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public void CanGetDetailsOfAccountWithNoTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var name = "BankAccount1";
            var openingBalance = 20.00m;
            var balance = 20.00m;
            mockService.Setup(s => s.FindAccountWithTransactions(It.IsAny<int>())).Returns(
                new Account() { Id = 1, Name = name, OpeningBalance = openingBalance,Balance = balance }
            );
            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Account>;
            Assert.AreEqual(name, model.Entity.Name);
            Assert.AreEqual(openingBalance, model.Entity.OpeningBalance);
            Assert.AreEqual(balance, model.Entity.Balance);
            Assert.AreEqual(0, model.Transactions.Count);
        }

        [TestMethod]
        public void CanGetDetailsOfAccountWithTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var name = "BankAccount1";
            var openingBalance = 0.00m;
            var balance = 0.00m;
            var account = new Account() { Id = 1, Name = name, OpeningBalance = openingBalance, Balance = balance };
            var amount = 10.00m;
            var today = DateTime.Today;
            mockService.Setup(s => s.FindAccountWithTransactions(It.IsAny<int>())).Returns(
                new Account() { Id = 1, Name = account.Name, OpeningBalance = account.OpeningBalance,
                                Transactions = new List<Transaction>()
                                {
                                   new Transaction { Account = account,
                                       Payee = new Payee() { Name = "Payee1" },
                                       Category = new Category { Name = "Category1", Type = CategoryType.Income },
                                       Amount = amount, Date = today },
                                   new Transaction { Account = account,
                                       Payee = new Payee() { Name = "Payee2" },
                                       Category = new Category { Name = "Category2", Type = CategoryType.Expense },
                                       Amount = -amount, Date = today }
                                }
                }
            );
            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Account>;
            Assert.AreEqual(name, model.Entity.Name);
            Assert.AreEqual(openingBalance, model.Entity.OpeningBalance);
            Assert.AreEqual(balance, model.Entity.Balance);
            Assert.AreEqual(2, model.Transactions.Count);
            Assert.AreEqual(amount, model.Transactions.ElementAt(0).Income);
            Assert.AreEqual(today, model.Transactions.ElementAt(0).Date);
            Assert.AreEqual(amount, model.Transactions.ElementAt(1).Expense);
            Assert.AreEqual(today, model.Transactions.ElementAt(0).Date);
        }


        [TestMethod]
        public void CanGetCreateNewAccountView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
