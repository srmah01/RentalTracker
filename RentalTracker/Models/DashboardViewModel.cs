using System.ComponentModel.DataAnnotations;

namespace RentalTracker.Models
{
    public class DashboardViewModel
    {
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal NetTotal { get; set; }

        public int NumberOfAccounts { get; set; }

        public int NumberOfCategories { get; set; }

        public int NumberOfPayess { get; set; }

        public int NumberOfTransactions { get; set; }
    }
}