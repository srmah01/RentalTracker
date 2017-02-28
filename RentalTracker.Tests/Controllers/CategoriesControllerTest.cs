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
    /// A class representing the tests of the Categories Controller.
    /// </summary>
    [TestClass]
    public class CategoriesControllerTest
    {
        private MockedData mockedData;

        [TestInitialize]
        public void Setup()
        {
            mockedData = new MockedData();
        }

        [TestMethod]
        public void CanReturnACategoryIndexViewWithAnEmptyListOfCategories()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllCategories()).Returns(new List<Category>());

           CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ICollection<Category>;
            Assert.AreEqual(0, model.Count);
        }

        [TestMethod]
        public void CanReturnACategoryIndexViewWithAListOfCategories()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllCategories()).Returns(mockedData.Categories.Take(3).ToList());

            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ICollection<Category>;
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public void CanReturnACategoryDetailsViewWithAnEmptyListOfTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedCategory = mockedData.Categories.Where(c => c.Name == "CategoryWithNoTransactions").Single();
            var id = 1;
            mockService.Setup(s => s.FindCategoryWithTransactions(id, null, null, true)).Returns(
                mockedCategory
            );
            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Category>;
            Assert.AreEqual(mockedCategory.Name, model.Entity.Name);
            Assert.AreEqual(mockedCategory.Type, model.Entity.Type);
            Assert.IsNotNull(model.Transactions);
            Assert.AreEqual(0, model.Transactions.Count);
        }

        [TestMethod]
        public void CanReturnACategoryDetailsViewWithAListOfTransactions()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var id = 1;
            mockService.Setup(s => s.FindCategoryWithTransactions(id, null, null, true)).Returns(
                mockedData.Categories.First());
            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Category>;
            Assert.AreEqual(mockedData.Categories.First().Name, model.Entity.Name);
            Assert.AreEqual(mockedData.Categories.First().Type, model.Entity.Type);
            Assert.AreEqual(mockedData.Categories.First().Transactions.Count(), model.Transactions.Count);
            for (int i = 0; i < model.Transactions.Count; i++)
            {
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Date, model.Transactions.ElementAt(i).Date);
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Account.Name, model.Transactions.ElementAt(i).Account);
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Payee.Name, model.Transactions.ElementAt(i).Payee);
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Amount, model.Transactions.ElementAt(i).Income);
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Taxable, model.Transactions.ElementAt(i).Taxable);
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Reference, model.Transactions.ElementAt(i).Reference);
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Memo, model.Transactions.ElementAt(i).Memo);
            }
        }

        [TestMethod]
        public void CanReturnACategoryDetailsViewWithDateFilterModel()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedCategory = mockedData.Categories.First();
            var id = 1;
            var filter = DateFilterSelector.CustomDate;
            var date = DateTime.Today;
            var sortOrder = SortDirection.Descending;
            mockService.Setup(s => s.FindCategoryWithTransactions(
                id, date, date, false))
                .Returns(
                    mockedCategory
                );
            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(id,
                DateFilterSelector.CustomDate, date.ToShortDateString(),
                date.ToShortDateString(), sortOrder) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Category>;
            Assert.IsNotNull(model.Filter);
            var filterVM = model.Filter as DateFilterViewModel;
            Assert.AreEqual(filter, filterVM.DateFilter);
            Assert.AreEqual(date, filterVM.FromDate);
            Assert.AreEqual(date, filterVM.ToDate);
            Assert.AreEqual(sortOrder, filterVM.SortOrder);
        }

        [TestMethod]
        public void DisplayingDetailsOfANonExistentCategoryReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var id = 1;
            mockService.Setup(s => s.FindCategoryWithTransactions(
                id, null, null, true)).Returns((Category)null);

            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Details(id) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void CanReturnACategoryCreateView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();

            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InsertingAnInvalidNewCategoryDisplaysSameView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveNewCategory(It.IsAny<Category>())).Throws(new RentalTrackerServiceValidationException("Error",
                     new List<ValidationResult>()
                     {
                        new ValidationResult("Some error.", new [] {  "Name" } )
                     }));

            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Create(new Category()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }


        [TestMethod]
        public void CanReturnACategoryEditView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedCategory = mockedData.Categories.First();
            mockService.Setup(s => s.FindCategory(It.IsAny<int>())).Returns(mockedCategory);

            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Edit(1) as ViewResult;
            var model = result.Model as Category;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockedCategory.Name, model.Name);
            Assert.AreEqual(mockedCategory.Type, model.Type);
        }

        [TestMethod]
        public void EditingANonExistentCategoryReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.FindCategory(It.IsAny<int>())).Returns((Category)null);

            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Edit(1) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdatingAValidCategoryRedirectsToIndexView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveUpdatedAccount(It.IsAny<Account>()));

            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            RedirectToRouteResult result = controller.Edit(new Category()) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void UpdatingAnInvalidCategoryDisplaysSameView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.SaveUpdatedCategory(It.IsAny<Category>())).Throws(new RentalTrackerServiceValidationException("Error",
                    new List<ValidationResult>()
                    {
                        new ValidationResult("Some error.", new [] {  "Name" } )
                    }));

            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Edit(new Category()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

    }
}
