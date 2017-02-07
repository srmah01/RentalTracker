using RentalTracker.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace RentalTracker.Models
{
    public class DateFilterViewModel
    {
        [Required]
        [Display(Name = "Date")]
        public DateFilterSelector DateFilter { get; private set; }

        [Required]
        [Display(Name = "From")]
        public DateTime? FromDate { get; private set; }

        [Required]
        [Display(Name = "To")]
        public DateTime? ToDate { get; private set; }

        public DateFilterViewModel()
        {
            DateFilter = DateFilterSelector.AllDates;
        }

        public void SetDateFilter(DateFilterSelector selector, string from = null, string to = null)
        {
            DateFilter = selector;

            switch (DateFilter)
            {
                case DateFilterSelector.AllDates:
                    FromDate = null;
                    ToDate = null;
                    break;
                case DateFilterSelector.LastMonth:
                    FromDate = DateTime.Today;
                    ToDate = FromDate.Value.AddMonths(-1);
                    break;
                case DateFilterSelector.Last3Months:
                    FromDate = DateTime.Today;
                    ToDate = FromDate.Value.AddMonths(-3);
                    break;
                case DateFilterSelector.Last6Months:
                    FromDate = DateTime.Today;
                    ToDate = FromDate.Value.AddMonths(-6);
                    break;
                case DateFilterSelector.Last12Months:
                    FromDate = DateTime.Today;
                    ToDate = FromDate.Value.AddMonths(-12);
                    break;
                default:
                    if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
                    {
                        throw (new ArgumentException("Custom date must specify a from date and a to date."));
                    }
                    FromDate = DateTime.Parse(from);
                    ToDate = DateTime.Parse(to);
                    break;
            }
        }
    }
}