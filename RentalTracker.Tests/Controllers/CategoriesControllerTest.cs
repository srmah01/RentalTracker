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
            var mockedCategory = mockedData.Categories.Where(c => c.Name == "CategoryWithNoTransactions").Single(); ;
            mockService.Setup(s => s.FindCategoryWithTransactions(It.IsAny<int>())).Returns(
                mockedCategory
            );
            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

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
            mockService.Setup(s => s.FindCategoryWithTransactions(It.IsAny<int>())).Returns(mockedData.Categories.First());
            CategoriesController controller = new CategoriesController(mockService.Object);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as EntityDetailsViewModel<Category>;
            Assert.AreEqual(mockedData.Categories.First().Name, model.Entity.Name);
            Assert.AreEqual(mockedData.Categories.First().Type, model.Entity.Type);
            Assert.AreEqual(mockedData.Categories.First().Transactions.Count(), model.Transactions.Count);
            for (int i = 0; i < model.Transactions.Count; i++)
            {
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Amount, model.Transactions.ElementAt(i).Income);
                Assert.AreEqual(mockedData.Categories.First().Transactions.ElementAt(i).Date, model.Transactions.ElementAt(i).Date);
            }
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
    }
}
