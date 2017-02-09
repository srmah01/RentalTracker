using RentalTracker.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace RentalTracker.Models
{
    public class DateFilterViewModel
    {
        [Display(Name = "Date")]
        public DateFilterSelector DateFilter { get; set; }

        [Required]
        [Display(Name = "From")]
        public DateTime? FromDate { get; private set; }

        [Required]
        [Display(Name = "To")]
        public DateTime? ToDate { get; private set; }

        [Required]
        public SortDirection SortOrder { get; private set; }

        public DateFilterViewModel()
        {
            DateFilter = DateFilterSelector.AllDates;
            SortOrder = SortDirection.Ascending;
        }

        public void SetDateFilter(DateFilterSelector selector, string from = null, string to = null, 
            SortDirection sortOder = SortDirection.Ascending)
        {
            DateFilter = selector;
            SortOrder = sortOder;

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
                    if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
                    {
                        throw (new ArgumentException("Custom date must specify a from date and a to date."));
                    }
                    FromDate = DateTime.Parse(from);
                    ToDate = DateTime.Parse(to);

                    if (FromDate > ToDate)
                    {
                        throw (new ArgumentException("Custom date must specify a to date that is equal to or later than a from to date."));
                    }
                    break;
            }
        }
    }
}