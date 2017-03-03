using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Service;
using RentalTracker.DAL;
using Moq;
using System.Linq;

namespace RentalTracker.Tests.Service
{
    /// <summary>
    ///  A class representing the tests of the Tax Report Service.
    /// </summary>
    [TestClass]
    public class TaxReportServiceTests
    {
        private MockedData mockedData;

        [TestInitialize]
        public void Setup()
        {
            mockedData = new MockedData();
        }

        [TestMethod]
        public void CanCreateService()
        {
            // Arrange
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();

            // Act
            var service = new TaxReportService(mockRentalTrackerService.Object);

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void CanGetRentalTrackerService()
        {
            // Arrange
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();
            var service = new TaxReportService(mockRentalTrackerService.Object);

            // Act
            var result = service.RentalTrackerService;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GenerateReportReturnsAViewModel()
        {
            // Arrange
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();
            var accountId = 1;
            var year = 2017;
            var mockedAccount = mockedData.Accounts.Where(a => a.Id == accountId).FirstOrDefault();
            mockRentalTrackerService.Setup(s => s.FindAccountWithTransactions(accountId, null, null, true)).Returns(mockedAccount);
            var service = new TaxReportService(mockRentalTrackerService.Object);

            // Act
            var result = service.GenerateReport(accountId, year);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(accountId, result.AccountId);
            Assert.AreEqual(mockedAccount.Name, result.AccountName);
            Assert.AreEqual(year, result.Year);
            Mock.VerifyAll(mockRentalTrackerService);
        }

        [TestMethod]
        public void GenerateReportReturnsNullWhenAccountCannotBeFound()
        {
            // Arrange
            var mockRentalTrackerService = new Mock<IRentalTrackerService>();
            var accountId = 999;
            var year = 2017;
            var mockedAccount = mockedData.Accounts.Where(a => a.Id == accountId).FirstOrDefault();
            mockRentalTrackerService.Setup(s => s.FindAccountWithTransactions(accountId, null, null, true)).Returns(mockedAccount);
            var service = new TaxReportService(mockRentalTrackerService.Object);

            // Act
            var result = service.GenerateReport(accountId, year);

            // Assert
            Assert.IsNull(result);
            Mock.VerifyAll(mockRentalTrackerService);
        }

    }
}
