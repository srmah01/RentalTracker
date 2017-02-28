using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Controllers;
using RentalTracker.DAL;
using Moq;
using RentalTracker.Domain;
using RentalTracker.Models;
using RentalTracker.DAL.Exceptions;
using System.ComponentModel.DataAnnotations;
using RentalTracker.Enums;
using System.Web.Helpers;

namespace RentalTracker.Tests.Controllers
{
    /// <summary>
    /// A class representing the tests of the Payees Controller.
    /// </summary>
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
            var id = 1;
            mockService.Setup(s => s.FindPayeeWithTransactions(id, null, null, true)).Returns(
                mockedPayee
            );
            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

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
            var id = 1;
            mockService.Setup(s => s.FindPayeeWithTransactions(id, null, null, true)).Returns(
                mockedData.Payees.First());
            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Payee>;
            Assert.AreEqual(mockedData.Payees.First().Name, model.Entity.Name);
            Assert.AreEqual(mockedData.Payees.First().DefaultCategory.Name, model.Entity.DefaultCategory.Name);
            Assert.AreEqual(mockedData.Payees.First().Transactions.Count(), model.Transactions.Count);
            for (int i = 0; i < model.Transactions.Count; i++)
            {
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Date, model.Transactions.ElementAt(i).Date);
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Account.Name, model.Transactions.ElementAt(i).Account);
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Category.Name, model.Transactions.ElementAt(i).Category);
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Amount, model.Transactions.ElementAt(i).Income);
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Taxable, model.Transactions.ElementAt(i).Taxable);
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Reference, model.Transactions.ElementAt(i).Reference);
                Assert.AreEqual(mockedData.Payees.First().Transactions.ElementAt(i).Memo, model.Transactions.ElementAt(i).Memo);
            }
        }

        [TestMethod]
        public void CanReturnAPayeeDetailsViewWithDateFilterModel()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedPayee = mockedData.Payees.First();
            var id = 1;
            var filter = DateFilterSelector.CustomDate;
            var date = DateTime.Today;
            var sortOrder = SortDirection.Descending;
            mockService.Setup(s => s.FindPayeeWithTransactions(
                id, date, date, false))
                .Returns(
                    mockedPayee
                );
            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(id,
                DateFilterSelector.CustomDate, date.ToShortDateString(),
                date.ToShortDateString(), sortOrder) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Payee>;
            Assert.IsNotNull(model.Filter);
            var filterVM = model.Filter as DateFilterViewModel;
            Assert.AreEqual(filter, filterVM.DateFilter);
            Assert.AreEqual(date, filterVM.FromDate);
            Assert.AreEqual(date, filterVM.ToDate);
            Assert.AreEqual(sortOrder, filterVM.SortOrder);
        }

        [TestMethod]
        public void DisplayingDetailsOfANonExistentPayeeReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var id = 1;
            mockService.Setup(s => s.FindPayeeWithTransactions(id, null, null, true)).Returns((Payee)null);

            PayeesController controller = new PayeesController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Details(id) as HttpNotFoundResult;

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
