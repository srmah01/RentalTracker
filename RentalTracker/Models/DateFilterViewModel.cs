using RentalTracker.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace RentalTracker.Models
{
    /// <summary>
    /// Class representing the View Model for the Date Filter form.
    /// </summary>
    public class DateFilterViewModel : IFilterViewModel
    {
        /// <summary>
        /// Gets or sets the Date Filter selection
        /// </summary>
        [Display(Name = "Date")]
        public DateFilterSelector DateFilter { get; protected set; }

        /// <summary>
        /// Gets or sets the Date to match for the earliest transaction. 
        /// </summary>
        [Required]
        [Display(Name = "From")]
        public DateTime? FromDate { get; protected set; }

        /// <summary>
        /// Gets or sets the Date to match for the latest transaction. 
        /// </summary>
        [Required]
        [Display(Name = "To")]
        public DateTime? ToDate { get; protected set; }

        /// <summary>
        /// Gets or sets the date order to display the list of transactions. 
        /// </summary>
        [Required]
        public SortDirection SortOrder { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DateFilterViewModel()
        {
            DateFilter = DateFilterSelector.AllDates;
            SortOrder = SortDirection.Ascending;
            FromDate = DateTime.MinValue;
            ToDate = DateTime.MaxValue;
        }

        /// <summary>
        /// Sets the Date Filter with the given parameters.
        /// </summary>
        /// <param name="selector">The value of the Date Filter selection.</param>
        /// <param name="from">The value of the From date.</param>
        /// <param name="to">The value of the To date.</param>
        /// <param name="sortOrder">The order to sort the list.</param>
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