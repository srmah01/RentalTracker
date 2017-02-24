using RentalTracker.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace RentalTracker.Models
{
    public class DateFilterViewModel : IFilterViewModel
    {
        [Display(Name = "Date")]
        public DateFilterSelector DateFilter { get; protected set; }

        [Required]
        [Display(Name = "From")]
        public DateTime? FromDate { get; protected set; }

        [Required]
        [Display(Name = "To")]
        public DateTime? ToDate { get; protected set; }

        [Required]
        public SortDirection SortOrder { get; protected set; }

        public DateFilterViewModel()
        {
            DateFilter = DateFilterSelector.AllDates;
            SortOrder = SortDirection.Ascending;
            FromDate = DateTime.MinValue;
            ToDate = DateTime.MaxValue;
        }

        public void SetDateFilter(DateFilterSelector selector, string from = null, string to = null,
            SortDirection sortOrder = SortDirection.Ascending)
        {
            DateFilter = selector;
            SortOrder = sortOrder;

            switch (DateFilter)
            {
                case DateFilterSelector.AllDates:
                    ToDate = null;
                    FromDate = null;
                    break;
                case DateFilterSelector.LastMonth:
                    ToDate = DateTime.Today;
                    FromDate = ToDate.Value.AddMonths(-1);
                    break;
                case DateFilterSelector.Last3Months:
                    ToDate = DateTime.Today;
                    FromDate = ToDate.Value.AddMonths(-3);
                    break;
                case DateFilterSelector.Last6Months:
                    ToDate = DateTime.Today;
                    FromDate = ToDate.Value.AddMonths(-6);
                    break;
                case DateFilterSelector.Last12Months:
                    ToDate = DateTime.Today;
                    FromDate = ToDate.Value.AddMonths(-12);
                    break;
                default:
                    // If either date string is null or empty then default Min or Max value is used
                    // Shouldn't happen because client validation ensures the fields are completed.
                    if (!String.IsNullOrEmpty(from))
                    {
                        FromDate = DateTime.Parse(from);
                    }

                    if (!String.IsNullOrEmpty(to))
                    {
                        ToDate = DateTime.Parse(to);
                    }
                    break;
            }
        }
    }
}