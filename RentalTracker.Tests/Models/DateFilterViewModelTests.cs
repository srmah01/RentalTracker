using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Models;
using RentalTracker.Enums;
using System.Web.Helpers;

namespace RentalTracker.Tests.Models
{
    /// <summary>
    /// A class representing the tests of the Date Filter View Model.
    /// </summary>
    [TestClass]
    public class DateFilterViewModelTests
    {
        [TestMethod]
        public void CanCreateModel()
        {
            var actual = new DateFilterViewModel();

            Assert.IsNotNull(actual);
            Assert.AreEqual(DateFilterSelector.AllDates, actual.DateFilter);
            Assert.AreEqual(SortDirection.Ascending, actual.SortOrder);
        }

        [TestMethod]
        public void CanSetDateFilterWithLastMonthFilter()
        {
            var actual = new DateFilterViewModel();

            actual.SetDateFilter(DateFilterSelector.LastMonth);

            Assert.AreEqual(DateFilterSelector.LastMonth, actual.DateFilter);
            Assert.AreEqual(DateTime.Today, actual.ToDate);
            Assert.AreEqual(DateTime.Today.AddMonths(-1), actual.FromDate);
            Assert.AreEqual(SortDirection.Ascending, actual.SortOrder);
        }

        [TestMethod]
        public void CanSetDateFilterWithLast3MonthsFilter()
        {
            var actual = new DateFilterViewModel();

            actual.SetDateFilter(DateFilterSelector.Last3Months);

            Assert.AreEqual(DateFilterSelector.Last3Months, actual.DateFilter);
            Assert.AreEqual(DateTime.Today, actual.ToDate);
            Assert.AreEqual(DateTime.Today.AddMonths(-3), actual.FromDate);
            Assert.AreEqual(SortDirection.Ascending, actual.SortOrder);
        }

        [TestMethod]
        public void CanSetDateFilterWithLast6MonthsFilter()
        {
            var actual = new DateFilterViewModel();

            actual.SetDateFilter(DateFilterSelector.Last6Months);

            Assert.AreEqual(DateFilterSelector.Last6Months, actual.DateFilter);
            Assert.AreEqual(DateTime.Today, actual.ToDate);
            Assert.AreEqual(DateTime.Today.AddMonths(-6), actual.FromDate);
            Assert.AreEqual(SortDirection.Ascending, actual.SortOrder);
        }

        [TestMethod]
        public void CanSetDateFilterWithLast12MonthsFilter()
        {
            var actual = new DateFilterViewModel();

            actual.SetDateFilter(DateFilterSelector.Last12Months);

            Assert.AreEqual(DateFilterSelector.Last12Months, actual.DateFilter);
            Assert.AreEqual(DateTime.Today, actual.ToDate);
            Assert.AreEqual(DateTime.Today.AddMonths(-12), actual.FromDate);
            Assert.AreEqual(SortDirection.Ascending, actual.SortOrder);
        }

        [TestMethod]
        public void CanSetDateFilterWithCustomDateFilter()
        {
            var actual = new DateFilterViewModel();

            var dateStr = DateTime.Today.ToShortDateString();
            actual.SetDateFilter(DateFilterSelector.CustomDate, dateStr, dateStr);

            Assert.AreEqual(DateFilterSelector.CustomDate, actual.DateFilter);
            Assert.AreEqual(DateTime.Today, actual.ToDate);
            Assert.AreEqual(DateTime.Today, actual.FromDate);
            Assert.AreEqual(SortDirection.Ascending, actual.SortOrder);
        }

        [TestMethod]
        public void SetDateFilterWithNoDatesUsesDefaults()
        {
            var actual = new DateFilterViewModel();

            actual.SetDateFilter(DateFilterSelector.CustomDate, null, null);

            Assert.AreEqual(DateTime.MaxValue, actual.ToDate);
            Assert.AreEqual(DateTime.MinValue, actual.FromDate);
        }

        [TestMethod]
        public void SetDateFilterWithOnlyFromDateUsesDefaultToDate()
        {
            var actual = new DateFilterViewModel();

            var dateStr = DateTime.Today.ToShortDateString();
            actual.SetDateFilter(DateFilterSelector.CustomDate, dateStr, null);

            Assert.AreEqual(DateTime.MaxValue, actual.ToDate);
            Assert.AreEqual(DateTime.Today, actual.FromDate);
        }

        [TestMethod]
        public void SetDateFilterWithOnlyToDateUsesDefaultFromDate()
        {
            var actual = new DateFilterViewModel();

            var dateStr = DateTime.Today.ToShortDateString();
            actual.SetDateFilter(DateFilterSelector.CustomDate, null, dateStr);

            Assert.AreEqual(DateTime.Today, actual.ToDate);
            Assert.AreEqual(DateTime.MinValue, actual.FromDate);
        }

        [TestMethod]
        public void CanSetDateFilterWithDescendingSortFilter()
        {
            var actual = new DateFilterViewModel();

            actual.SetDateFilter(DateFilterSelector.AllDates, null, null, SortDirection.Descending);

            Assert.AreEqual(SortDirection.Descending, actual.SortOrder);
        }
    }
}
