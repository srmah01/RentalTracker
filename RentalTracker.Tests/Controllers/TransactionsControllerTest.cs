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
            mockService.Setup(s => s.GetAllAccounts()).Returns(new List<Account> {
                new Account() { Id = 1, Name = "Account1" },
                new Account() { Id = 2, Name = "Account2" }
            });
            mockService.Setup(s => s.GetAllCategories()).Returns(new List<Category> {
                new Category() { Id = 1, Name = "Category1" },
                new Category() { Id = 2, Name = "Category2" }
            });
            mockService.Setup(s => s.GetAllPayees()).Returns(new List<Payee> {
                new Payee() { Id = 1, Name = "Payee1" },
                new Payee() { Id = 2, Name = "Payee2" }
            });

            TransactionsController controller = new TransactionsController(mockService.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
