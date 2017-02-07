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
            var mockedAccount = mockedData.Accounts.Where(c => c.Name == "AccountWithNoTransactions").Single();
            var id = 1;
            mockService.Setup(s => s.FindAccountWithTransactions(
                id, null, null, true))
                .Returns(
                    mockedAccount
                );
            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

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
            var mockedAccount = mockedData.Accounts.First();
            var id = 1;
            mockService.Setup(s => s.FindAccountWithTransactions(
                id, null, null, true))
                .Returns(
                    mockedAccount
                );
            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Account>;
            Assert.AreEqual(mockedAccount.Name, model.Entity.Name);
            Assert.AreEqual(mockedAccount.OpeningBalance, model.Entity.OpeningBalance);
            Assert.AreEqual(mockedAccount.Balance, model.Entity.Balance);
            Assert.AreEqual(mockedAccount.Transactions.Count(), model.Transactions.Count);
            for (int i = 0; i < model.Transactions.Count; i++)
            {
                var expected = mockedData.Accounts.First().Transactions.ElementAt(i);
                var actual = model.Transactions.ElementAt(i);
                bool isIncome = expected.Category.Type == CategoryType.Income;
                Assert.AreEqual(expected.Date, actual.Date);
                Assert.AreEqual(expected.Category.Name, actual.Category);
                Assert.AreEqual(expected.Payee.Name, actual.Payee);
                if (isIncome)
                {
                    Assert.AreEqual(expected.Amount, actual.Income);
                }
                else
                {
                    Assert.AreEqual(expected.Amount, actual.Expense);
                }
                Assert.AreEqual(expected.Taxable, actual.Taxable);
                Assert.AreEqual(expected.Balance, actual.Balance);
                Assert.AreEqual(expected.Reference, actual.Reference);
                Assert.AreEqual(expected.Memo, actual.Memo);
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
