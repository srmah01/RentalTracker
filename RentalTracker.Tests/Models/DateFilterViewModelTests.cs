using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalTracker.Models;
using RentalTracker.Enums;
using System.Web.Helpers;

namespace RentalTracker.Tests.Models
{
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

        [TestMethod, 
         ExpectedException(typeof(ArgumentException))]
        public void SetDateFilterWithNoDatesThrows()
        {
            var actual = new DateFilterViewModel();

            actual.SetDateFilter(DateFilterSelector.CustomDate, null, null);

            Assert.Fail("Set a custom date with no dates");
        }

        [TestMethod,
         ExpectedException(typeof(ArgumentException))]
        public void SetDateFilterWithOnlyFromDateThrows()
        {
            var actual = new DateFilterViewModel();

            var dateStr = DateTime.Today.ToShortDateString();
            actual.SetDateFilter(DateFilterSelector.CustomDate, dateStr, null);

            Assert.Fail("Set a custom date with only From date");
        }

        [TestMethod,
         ExpectedException(typeof(ArgumentException))]
        public void SetDateFilterWithOnlyToDateThrows()
        {
            var actual = new DateFilterViewModel();

            var dateStr = DateTime.Today.ToShortDateString();
            actual.SetDateFilter(DateFilterSelector.CustomDate, null, dateStr);

            Assert.Fail("Set a custom date with only To date");
        }

        [TestMethod,
         ExpectedException(typeof(ArgumentException))]
        public void SetDateFilterWithToDateEarlierThanFromDateThrows()
        {
            var actual = new DateFilterViewModel();

            var todayStr = DateTime.Today.ToShortDateString();
            var tomorrowStr = DateTime.Today.AddDays(1).ToShortDateString();
            actual.SetDateFilter(DateFilterSelector.CustomDate, tomorrowStr, todayStr);

            Assert.Fail("Set a custom date with an earlier To date");
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
