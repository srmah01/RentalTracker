using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using RentalTracker.DAL;
using RentalTracker.Controllers;
using Moq;
using System.Linq;

namespace RentalTracker.Tests.Controllers
{
    /// <summary>
    /// A class representing the tests of the Tax Controller Controller.
    /// </summary>
    [TestClass]
    public class TaxControllerTest
    {
        private MockedData mockedData;

        [TestInitialize]
        public void Setup()
        {
            mockedData = new MockedData();
        }

        [TestMethod]
        public void CanReturnATaxReportGenerateView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();
            var mockedAccounts = mockedData.Accounts.ToList();
            mockService.Setup(s => s.GetAllAccounts()).Returns(mockedAccounts);

            TaxReportController controller = new TaxReportController(mockService.Object);

            // Act
            ViewResult result = controller.Generate() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
