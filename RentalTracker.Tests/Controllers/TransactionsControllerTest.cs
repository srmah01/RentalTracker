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
using RentalTracker.DAL.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace RentalTracker.Tests.Controllers
{
    [TestClass]
    public class TransactionsControllerTest
    {
        private MockedData mockedData;

        [TestInitialize]
        public void Setup()
        {
            mockedData = new MockedData();
        }

        [TestMethod]
        public void CanReturnATransactionIndexViewWithAnEmptyList()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllTransactionsWithAccountAndPayeeAndCategory()).Returns(new List<Transaction>());

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Transaction>;
            Assert.AreEqual(0, model.Transactions.Count);
        }

        [TestMethod]
        public void CanReturnATransactionIndexViewWithListOfTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllTransactionsWithAccountAndPayeeAndCategory()).Returns(
                mockedData.Transactions
            );

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Transaction>;
            Assert.AreEqual(mockedData.Transactions.Count(), model.Transactions.Count);
            for (int i = 0; i < model.Transactions.Count; i++)
            {
                Assert.AreEqual(mockedData.Transactions.ElementAt(i).Date, model.Transactions.ElementAt(i).Date);
                Assert.IsTrue(
                    mockedData.Transactions.ElementAt(i).Amount == model.Transactions.ElementAt(i).Income ||
                    (mockedData.Transactions.ElementAt(i).Amount *-1) == model.Transactions.ElementAt(i).Expense);
                Assert.AreEqual(mockedData.Transactions.ElementAt(i).Account.Name, model.Transactions.ElementAt(i).Account);
                Assert.AreEqual(mockedData.Transactions.ElementAt(i).Category.Name, model.Transactions.ElementAt(i).Category);
                Assert.AreEqual(mockedData.Transactions.ElementAt(i).Payee.Name, model.Transactions.ElementAt(i).Payee);
                Assert.AreEqual(mockedData.Transactions.ElementAt(i).Reference, model.Transactions.ElementAt(i).Reference);
                Assert.AreEqual(mockedData.Transactions.ElementAt(i).Memo, model.Transactions.ElementAt(i).Memo);
            }
        }

        [TestMethod]
        public void RequestingATransactionDetailsViewReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            HttpStatusCodeResult result = controller.Details(1) as HttpStatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void CanReturnATransactionCreateView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllAccounts()).Returns(mockedData.Accounts.ToList());
            mockService.Setup(s => s.GetAllCategories()).Returns(mockedData.Categories.ToList());
            mockService.Setup(s => s.GetAllPayees()).Returns(mockedData.Payees.ToList());

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockedData.Accounts.Count(), controller.ViewBag.AccountId.Items.Count);
            Assert.AreEqual(mockedData.Categories.Count(), controller.ViewBag.CategoryId.Items.Count);
            Assert.AreEqual(mockedData.Payees.Count(), controller.ViewBag.PayeeId.Items.Count);
        }

        [TestMethod]
        public void InsertingAValidNewTransactionRedirectsToIndexView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveNewTransaction(It.IsAny<Transaction>()));

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            RedirectToRouteResult result = controller.Create(new Transaction()) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void InsertingAnInvalidNewTransactionDisplaysSameView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllAccounts()).Returns(mockedData.Accounts.ToList());
            mockService.Setup(s => s.GetAllCategories()).Returns(mockedData.Categories.ToList());
            mockService.Setup(s => s.GetAllPayees()).Returns(mockedData.Payees.ToList());
            mockService.Setup(s => s.SaveNewTransaction(It.IsAny<Transaction>())).Throws(new RentalTrackerServiceValidationException("Error",
                    new List<ValidationResult>()
                    {
                        new ValidationResult("Amount must be non-zero.", new [] {  "Amount" } )
                    }));

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            ViewResult result = controller.Create(new Transaction()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.AreEqual(mockedData.Accounts.Count(), controller.ViewBag.AccountId.Items.Count);
            Assert.AreEqual(mockedData.Categories.Count(), controller.ViewBag.CategoryId.Items.Count);
            Assert.AreEqual(mockedData.Payees.Count(), controller.ViewBag.PayeeId.Items.Count);
        }


        [TestMethod]
        public void CanReturnATransactionEditView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedTransaction = mockedData.Transactions.First();
            mockService.Setup(s => s.FindTransaction(It.IsAny<int>())).Returns(mockedTransaction);
            mockService.Setup(s => s.GetAllAccounts()).Returns(mockedData.Accounts.ToList());
            mockService.Setup(s => s.GetAllCategories()).Returns(mockedData.Categories.ToList());
            mockService.Setup(s => s.GetAllPayees()).Returns(mockedData.Payees.ToList());

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var model = result.Model as Transaction;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockedData.Accounts.Count(), controller.ViewBag.AccountId.Items.Count);
            Assert.AreEqual(mockedData.Categories.Count(), controller.ViewBag.CategoryId.Items.Count);
            Assert.AreEqual(mockedData.Payees.Count(), controller.ViewBag.PayeeId.Items.Count);
            Assert.AreEqual(mockedTransaction.Date, model.Date);
            Assert.AreEqual(mockedTransaction.Amount, model.Amount);
            Assert.AreEqual(mockedTransaction.Reference, model.Reference);
            Assert.AreEqual(mockedTransaction.Memo, model.Memo);
        }

        [TestMethod]
        public void UpdatingAValidTransactionRedirectsToIndexView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveUpdatedTransaction(It.IsAny<Transaction>()));

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            RedirectToRouteResult result = controller.Edit(new Transaction()) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void UpdatingAnInvalidTransactionDisplaysSameView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllAccounts()).Returns(mockedData.Accounts.ToList());
            mockService.Setup(s => s.GetAllCategories()).Returns(mockedData.Categories.ToList());
            mockService.Setup(s => s.GetAllPayees()).Returns(mockedData.Payees.ToList());
            mockService.Setup(s => s.SaveUpdatedTransaction(It.IsAny<Transaction>())).Throws(new RentalTrackerServiceValidationException("Error",
                    new List<ValidationResult>()
                    {
                        new ValidationResult("Amount must be non-zero.", new [] {  "Amount" } )
                    }));

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            ViewResult result = controller.Edit(new Transaction()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.AreEqual(mockedData.Accounts.Count(), controller.ViewBag.AccountId.Items.Count);
            Assert.AreEqual(mockedData.Categories.Count(), controller.ViewBag.CategoryId.Items.Count);
            Assert.AreEqual(mockedData.Payees.Count(), controller.ViewBag.PayeeId.Items.Count);
        }


    }
}
