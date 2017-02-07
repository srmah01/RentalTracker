using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalTracker.Utilities
{
    public enum DateFilterSelector
    {
        AllDates = 1,
        Last3Months = 2,
        Last6Months = 3,
        Last12Months = 4,
        CustomDate = 5
    }
}