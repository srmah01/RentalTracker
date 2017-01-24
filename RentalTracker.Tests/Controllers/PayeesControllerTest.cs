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
    public class PayeesControllerTest
    {
        private MockedData mockedData;

        [TestInitialize]
        public void Setup()
        {
            mockedData = new MockedData();
        }

        [TestMethod]
        public void CanReturnAPayeeIndexViewWithAnEmptyList()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllPayees()).Returns(new List<Payee>());

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ICollection<Payee>;
            Assert.AreEqual(0, model.Count);
        }

        [TestMethod]
        public void CanReturnAPayeeIndexViewWithListOfPayees()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllPayees()).Returns(
                mockedData.Payees.Take(3).ToList()
            );
            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ICollection<Payee>;
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public void CanReturnAPayeeDetailsViewWithAnEmptyListOfTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedPayee = mockedData.Payees.Where(c => c.Name == "PayeeWithNoTransactions").Single();
            mockService.Setup(s => s.FindPayeeWithTransactions(It.IsAny<int>())).Returns(
                mockedPayee
            );
            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Payee>;
            Assert.AreEqual(mockedPayee.Name, model.Entity.Name);
            Assert.AreEqual(mockedPayee.DefaultCategory.Name, model.Entity.DefaultCategory.Name);
            Assert.AreEqual(0, model.Transactions.Count);
        }

        [TestMethod]
        public void CanReturnAPayeeDetailsViewWithAListOfTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.FindPayeeWithTransactions(It.IsAny<int>())).Returns(
                mockedData.Payees.First());
            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Payee>;
            Assert.AreEqual(mockedData.Payees.First().Name, model.Entity.Name);
            Assert.AreEqual(mockedData.Payees.First().DefaultCategory.Name, model.Entity.DefaultCategory.Name);
            Assert.AreEqual(mockedData.Payees.First().Transactions.Count(), model.Transactions.Count);
            for (int i = 0; i < model.Transactions.Count; i++)
            {
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Amount, model.Transactions.ElementAt(i).Income);
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Date, model.Transactions.ElementAt(i).Date);
            }
        }


        [TestMethod]
        public void CanReturnAPayeeCreateView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
