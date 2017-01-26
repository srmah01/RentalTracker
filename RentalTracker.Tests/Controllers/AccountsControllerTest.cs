using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Controllers;
using RentalTracker.DAL;
using Moq;
using RentalTracker.Domain;
using RentalTracker.Models;
using RentalTracker.DAL.Exceptions;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace RentalTracker.Tests.Controllers
{
    [TestClass]
    public class AccountsControllerTest
    {
        private MockedData mockedData;

        [TestInitialize]
        public void Setup()
        {
            mockedData = new MockedData();
        }

        [TestMethod]
        public void CanReturnAnAccountIndexViewWithAnEmptyList()
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
        public void CanReturnAnAccountIndexViewWithAListOfAccounts()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllAccounts()).Returns(
                mockedData.Accounts.Take(3).ToList()
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
        public void CanReturnAnAccountDetailsViewWithAnEmptyListOfTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedAccount = mockedData.Accounts.Where(c => c.Name == "AccountWithNoTransactions").Single(); ;
            mockService.Setup(s => s.FindAccountWithTransactions(It.IsAny<int>())).Returns(
                mockedAccount
            );
            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Account>;
            Assert.AreEqual(mockedAccount.Name, model.Entity.Name);
            Assert.AreEqual(mockedAccount.OpeningBalance, model.Entity.OpeningBalance);
            Assert.AreEqual(mockedAccount.Balance, model.Entity.Balance);
            Assert.AreEqual(0, model.Transactions.Count);
        }

        [TestMethod]
        public void CanReturnAnAccountDetailsViewWithAListOfTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.FindAccountWithTransactions(It.IsAny<int>())).Returns(
                mockedData.Accounts.First()
            );
            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Account>;
            Assert.AreEqual(mockedData.Accounts.First().Name, model.Entity.Name);
            Assert.AreEqual(mockedData.Accounts.First().OpeningBalance, model.Entity.OpeningBalance);
            Assert.AreEqual(mockedData.Accounts.First().Balance, model.Entity.Balance);
            Assert.AreEqual(mockedData.Accounts.First().Transactions.Count(), model.Transactions.Count);
            for (int i = 0; i < model.Transactions.Count; i++)
            {
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Amount, model.Transactions.ElementAt(i).Income);
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Date, model.Transactions.ElementAt(i).Date);
            }
        }


        [TestMethod]
        public void CanReturnAnAccountCreateView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //TODO: Need to re-write this test to use an Exception that can be mocked
        [TestMethod]
        public void CreatingAnAccountWithADuplicateNameReturnsModelErrors()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            Account account = new Account() { Name = "BankAccount1", OpeningBalance = 0.00m };
            mockService.Setup(s => s.SaveNewAccount(It.IsAny<Account>())).Throws(
                new RentalTrackerServiceValidationException("Error", 
                    new List<ValidationResult>()
                    {
                        new ValidationResult("Duplicate Name.", new [] {  "Name" } )
                    })
            );

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Create(account) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

    }
}
