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
        public void DisplayingDetailsOfANonExistentPayeeReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.FindPayeeWithTransactions(It.IsAny<int>())).Returns((Payee)null);

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Details(1) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
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

        [TestMethod]
        public void InsertingAnInvalidNewPayeeDisplaysSameView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveNewPayee(It.IsAny<Payee>())).Throws(new RentalTrackerServiceValidationException("Error",
                     new List<ValidationResult>()
                     {
                        new ValidationResult("Some error.", new [] {  "Name" } )
                     }));

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Create(new Payee()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }


        [TestMethod]
        public void CanReturnAnPayeeEditView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedPayee = mockedData.Payees.First();
            mockService.Setup(s => s.FindPayee(It.IsAny<int>())).Returns(mockedPayee);

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var model = result.Model as Payee;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockedPayee.Name, model.Name);
            Assert.AreEqual(mockedPayee.DefaultCategory.Name, model.DefaultCategory.Name);
            Assert.AreEqual(mockedPayee.Memo, model.Memo);
        }

        [TestMethod]
        public void EditingANonExistentPayeeReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.FindPayee(It.IsAny<int>())).Returns((Payee)null);

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Edit(1) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdatingAValidPayeeRedirectsToIndexView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveUpdatedPayee(It.IsAny<Payee>()));

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            RedirectToRouteResult result = controller.Edit(new Payee()) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void UpdatingAnInvalidPayeeDisplaysSameView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveUpdatedPayee(It.IsAny<Payee>())).Throws(new RentalTrackerServiceValidationException("Error",
                    new List<ValidationResult>()
                    {
                        new ValidationResult("Some error.", new [] {  "Name" } )
                    }));

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Edit(new Payee()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }
    }
}
