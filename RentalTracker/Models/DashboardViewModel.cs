using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalTracker.Models
{
    public class DashboardViewModel
    {
        public decimal NetTotal { get; set; }
        public int NumberOfAccounts { get; set; }
        public int NumberOfCategories { get; set; }
        public int NumberOfPayess { get; set; }
        public int NumberOfTransactions { get; set; }
    }
}