using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using RentalTracker.Domain;

namespace RentalTracker.DAL.Tests
{
    [TestClass]
    public class RentalTrackerServiceTests
    {
        [TestMethod]
        public void CanGetAnEmptyListOfAccounts()
        {
            var mockedRepo = new Mock<IRentalTrackerRepository>();
            mockedRepo.Setup(r => r.GetAllAccounts()).Returns(new List<Account>());

            var service = new RentalTrackerService(mockedRepo.Object);

            Assert.AreEqual(0, service.GetAllAccounts().Count);
        }

        [TestMethod]
        public void CanGetAListOfAccounts()
        {
            var mockedRepo = new Mock<IRentalTrackerRepository>();
            mockedRepo.Setup(r => r.GetAllAccounts()).Returns(new List<Account>()
                {
                    new Account() { Id = 1, Name = "BankAccout1", OpeningBalance = 0.00m },
                    new Account() { Id = 2, Name = "BankAccout2", OpeningBalance = 1.99m },
                    new Account() { Id = 3, Name = "BankAccout3", OpeningBalance = 1000.00m },
                }
            );

            var service = new RentalTrackerService(mockedRepo.Object);

            Assert.AreEqual(3, (service.GetAllAccounts()).Count);
        }

    }
}
