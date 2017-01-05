using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Domain;
using System.Data.Entity;
using System.Text;

namespace RentalTracker.DAL.Tests
{
    [TestClass]
    public class RentalTrackerContextIntegrationTests
    {
        private string _log;
        private StringBuilder _logBuilder = new StringBuilder();
        private RentalTrackerContext _context;

        public RentalTrackerContextIntegrationTests()
        {
            //app.config points to a special testing database:RentalTrackerTest
            Database.SetInitializer(new DropCreateDatabaseAlways<RentalTrackerContext>());
            _context = new RentalTrackerContext();
            _context.Database.Initialize(true); //get this out of the way before logging
            SetupLogging();
        }

        private void SetupLogging()
        {
            _context.Database.Log = BuildLogString;
        }

        private void BuildLogString(string message)
        {
            _logBuilder.Append(message);
            _log = _logBuilder.ToString();
        }

        [TestMethod, TestCategory("Integration")]
        public void CanAddNewAccount()
        {
            var accountToAdd = new Account()
            {
                Name = "BankAccount1",
                OpeningBalance = 100.99m
            };

            _context.Accounts.Add(accountToAdd);
            _context.SaveChanges();

            Assert.AreEqual(1, _context.Accounts.Local.Count);
        }
    }
}
