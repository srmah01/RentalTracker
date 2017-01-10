using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RentalTracker.Domain;

namespace RentalTracker.DAL.Tests
{
    [TestClass]
    public class RentalTrackerServiceIntegrationTests
    {
        [TestMethod, TestCategory("Integration")]
        public void CanGetAnEmptyListOfAccounts()
        {
            DataHelper.NewDb(false);

            var service = new RentalTrackerService();

            Assert.AreEqual(0, service.GetAllAccounts().Count);
        }

        [TestMethod, TestCategory("Integration")]
        public void CanGetAListOfAccounts()
        {
            DataHelper.NewDb();

            var service = new RentalTrackerService();

            Assert.AreEqual(3, service.GetAllAccounts().Count);
        }

    }
}
