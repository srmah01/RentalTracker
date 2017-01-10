using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Domain;
using System.Data.Entity;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace RentalTracker.DAL.Tests
{
    [TestClass]
    public class RentalTrackerRepositoryIntegrationTests
    {
        private RentalTrackerRepository _repo = new RentalTrackerRepository();

        public RentalTrackerRepositoryIntegrationTests()
        {
            //app.config points to a special testing database:RentalTrackerTest
            DataHelper.NewDb();
            _repo = new RentalTrackerRepository();
         }

        [TestMethod, TestCategory("Integration")]
        public void CanGetAllAccounts()
        {
            var accounts = _repo.GetAllAccounts();

            Assert.AreEqual(3, accounts.Count);
        }
    }
}
