using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using RentalTracker.DAL;
using RentalTracker.Controllers;
using Moq;

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
        public void CanReturnATaxReportIndexView()
        {
            // Arrange
            var mockService = new Mock<IRentalTrackerService>();

            TaxReportController controller = new TaxReportController(mockService.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
