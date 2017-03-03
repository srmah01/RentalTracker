using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using RentalTracker.DAL;
using RentalTracker.Controllers;
using Moq;
using System.Linq;
using RentalTracker.Service;
using RentalTracker.Models;

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
            var mockService = new Mock<ITaxReportService>();
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();
            var mockedAccounts = mockedData.Accounts.ToList();
            mockService.Setup(s => s.RentalTrackerService).Returns(mockRentalTrackerService.Object);
            mockRentalTrackerService.Setup(s => s.GetAllAccounts()).Returns(mockedAccounts);

            TaxReportController controller = new TaxReportController(mockService.Object);

            // Act
            ViewResult result = controller.Generate() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Mock.VerifyAll(mockService);
            Mock.VerifyAll(mockRentalTrackerService);
        }

        [TestMethod]
        public void GeneratingAReportCanReturnARedirectToReportView()
        {
            // Arrange
            var mockService = new Mock<ITaxReportService>();
            var accountId = 1;
            var year = DateTime.Now.Year;

            TaxReportController controller = new TaxReportController(mockService.Object);

            // Act
            RedirectToRouteResult result = controller.Generate(accountId, year) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Report", result.RouteValues["action"]);
            Assert.AreEqual(accountId, result.RouteValues["accountId"]);
            Assert.AreEqual(year, result.RouteValues["year"]);
        }

        [TestMethod]
        public void CanGetAReportView()
        {
            // Arrange
            var mockService = new Mock<ITaxReportService>();
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();
            var mockedAccounts = mockedData.Accounts.ToList();
            var accountId = 1;
            var year = DateTime.Now.Year;
            mockService.Setup(s => s.GenerateReport(accountId, year)).Returns(new TaxReportViewModel());
            mockService.Setup(s => s.RentalTrackerService).Returns(mockRentalTrackerService.Object);
            mockRentalTrackerService.Setup(s => s.GetAllAccounts()).Returns(mockedAccounts);

            TaxReportController controller = new TaxReportController(mockService.Object);

            // Act
            ViewResult result = controller.Report(accountId, year) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Mock.VerifyAll(mockService);
            Mock.VerifyAll(mockRentalTrackerService);
        }

        [TestMethod]
        public void GetAReportWithNoAccountIdReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<ITaxReportService>();
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();
            int? accountId = null;
            var year = DateTime.Now.Year;
            
            TaxReportController controller = new TaxReportController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Report(accountId, year) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void GetAReportWithNoYearReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<ITaxReportService>();
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();
            var accountId = 1;
            int? year = DateTime.Now.Year;

            TaxReportController controller = new TaxReportController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Report(accountId, year) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void GetAReportForAnNonExistenrtAccountIdReturnsHttpNotFound()
        {
            // Arrange
            var mockService = new Mock<ITaxReportService>();
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();
            var accountId = 999;
            var year = DateTime.Now.Year;
            mockService.Setup(s => s.GenerateReport(accountId, year)).Returns((TaxReportViewModel) null);

            TaxReportController controller = new TaxReportController(mockService.Object);

            // Act
            HttpNotFoundResult result = controller.Report(accountId, year) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Mock.VerifyAll(mockService);
        }
    }
}
