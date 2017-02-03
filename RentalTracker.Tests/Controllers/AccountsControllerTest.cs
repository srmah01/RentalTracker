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
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Date, model.Transactions.ElementAt(i).Date);
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Category.Name, model.Transactions.ElementAt(i).Category);
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Payee.Name, model.Transactions.ElementAt(i).Payee);
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Amount, model.Transactions.ElementAt(i).Income);
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Taxable, model.Transactions.ElementAt(i).Taxable);
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Reference, model.Transactions.ElementAt(i).Reference);
                Assert.AreEqual(mockedData.Accounts.First().Transactions.ElementAt(i).Memo, model.Transactions.ElementAt(i).Memo);
            }
        }

        [TestMethod]
        public void DisplayingDetailsOfANonExistentAccountReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.FindAccount(It.IsAny<int>())).Returns((Account)null);

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Details(1) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
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

        [TestMethod]
        public void InsertingAnInvalidNewAccountDisplaysSameView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
           mockService.Setup(s => s.SaveNewAccount(It.IsAny<Account>())).Throws(new RentalTrackerServiceValidationException("Error",
                    new List<ValidationResult>()
                    {
                        new ValidationResult("Some error.", new [] {  "Name" } )
                    }));

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Create(new Account()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }


        [TestMethod]
        public void CanReturnAnAccountEditView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedAccount = mockedData.Accounts.First();
            mockService.Setup(s => s.FindAccount(It.IsAny<int>())).Returns(mockedAccount);

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var model = result.Model as Account;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockedAccount.Name, model.Name);
            Assert.AreEqual(mockedAccount.OpeningBalance, model.OpeningBalance);
        }

        [TestMethod]
        public void EditingANonExistentAccountReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.FindAccount(It.IsAny<int>())).Returns((Account) null);

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Edit(1) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdatingAValidAccountRedirectsToIndexView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveUpdatedAccount(It.IsAny<Account>()));

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            RedirectToRouteResult result = controller.Edit(new Account()) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void UpdatingAnInvalidAccountDisplaysSameView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveUpdatedAccount(It.IsAny<Account>())).Throws(new RentalTrackerServiceValidationException("Error",
                    new List<ValidationResult>()
                    {
                        new ValidationResult("Some error.", new [] {  "Name" } )
                    }));

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Edit(new Account()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

    }
}
