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

namespace RentalTracker.Tests.Controllers
{
    [TestClass]
    public class AccountsControllerTest
    {
        [TestMethod]
        public void CanGetIndexView()
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
        public void CanGetIndexViewWithListOfAccounts()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetAllAccounts()).Returns(new List<Account>()
                {
                    new Account() { Id = 1, Name = "BankAccout1", OpeningBalance = 0.00m },
                    new Account() { Id = 2, Name = "BankAccout2", OpeningBalance = 1.99m },
                    new Account() { Id = 3, Name = "BankAccout3", OpeningBalance = 1000.00m },
                }
            );

            AccountsController controller = new AccountsController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ICollection<Account>;
            Assert.AreEqual(3, model.Count);
        }

        //    [TestMethod]
        //    public void About()
        //    {
        //        // Arrange
        //        HomeController controller = new HomeController();

        //        // Act
        //        ViewResult result = controller.About() as ViewResult;

        //        // Assert
        //        Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        //    }

        //    [TestMethod]
        //    public void Contact()
        //    {
        //        // Arrange
        //        HomeController controller = new HomeController();

        //        // Act
        //        ViewResult result = controller.Contact() as ViewResult;

        //        // Assert
        //        Assert.IsNotNull(result);
        //    }
    }
}
