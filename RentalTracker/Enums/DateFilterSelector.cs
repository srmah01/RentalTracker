using System.ComponentModel.DataAnnotations;

namespace RentalTracker.Enums
{
    public enum DateFilterSelector
    {
        [Display(Name="All Dates")]
        AllDates = 1,

        [Display(Name = "Last Month")]
        LastMonth = 2,

        [Display(Name = "Last 3 Months")]
        Last3Months = 3,

        [Display(Name = "Last 6 Months")]
        Last6Months = 4,

        [Display(Name = "Last 12 Months")]
        Last12Months = 5,

        [Display(Name = "Custom Date")]
        CustomDate = 6
    }
}