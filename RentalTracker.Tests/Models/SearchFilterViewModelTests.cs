using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Models;
using RentalTracker.Enums;
using System.Web.Helpers;

namespace RentalTracker.Tests.Models
{
    [TestClass]
    public class SearchFilterViewModelTests
    {
        [TestMethod]
        public void CanCreateModel()
        {
            var actual = new SearchFilterViewModel();

            Assert.IsNotNull(actual);
            Assert.IsNull(actual.Account);
            Assert.IsNull(actual.Payee);
            Assert.IsNull(actual.Category);
            Assert.AreEqual(DateFilterSelector.AllDates, actual.DateFilter);
            Assert.AreEqual(SortDirection.Ascending, actual.SortOrder);
        }

        [TestMethod]
        public void CanSetSearchFilterWithAccount()
        {
            var actual = new SearchFilterViewModel();

            var expected = "account";
            actual.SetSearchFilter(expected, null, null, DateFilterSelector.AllDates);

            Assert.AreEqual(expected, actual.Account);
            Assert.AreEqual(DateFilterSelector.AllDates, actual.DateFilter);
        }

        [TestMethod]
        public void CanSetSearchFilterWithPayee()
        {
            var actual = new SearchFilterViewModel();

            var expected = "payee";
            actual.SetSearchFilter(null, expected, null, DateFilterSelector.AllDates);

            Assert.AreEqual(expected, actual.Payee);
            Assert.AreEqual(DateFilterSelector.AllDates, actual.DateFilter);
        }

        [TestMethod]
        public void CanSetSearchFilterWithCategory()
        {
            var actual = new SearchFilterViewModel();

            var expected = "category";
            actual.SetSearchFilter(null, null, expected, DateFilterSelector.AllDates);

            Assert.AreEqual(expected, actual.Category);
            Assert.AreEqual(DateFilterSelector.AllDates, actual.DateFilter);
        }
    }
}
