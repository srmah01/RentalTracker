using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Controllers;
using RentalTracker.DAL;
using Moq;

namespace RentalTracker.Tests.Controllers
{
    /// <summary>
    /// A class representing the tests of the Home Controller.
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            mockService.Setup(s => s.GetTotalOfAccountBalances()).Returns(0m);
            mockService.Setup(s => s.GetNumberOfAccounts()).Returns(0);
            mockService.Setup(s => s.GetNumberOfCategories()).Returns(0);
            mockService.Setup(s => s.GetNumberOfPayees()).Returns(0);
            mockService.Setup(s => s.GetNumberOfTransactions()).Returns(0);

            HomeController controller = new HomeController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
