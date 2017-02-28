using System.ComponentModel.DataAnnotations;

namespace RentalTracker.Models
{
    /// <summary>
    /// Class representing the view model of the Dasboard. 
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// Gets or sets the Net Total of all Account balances.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal NetTotal { get; set; }

        /// <summary>
        /// Gets or sets the number of Account entities.
        /// </summary>
        public int NumberOfAccounts { get; set; }

        /// <summary>
        /// Gets or sets the number of Category entities.
        /// </summary>
        public int NumberOfCategories { get; set; }

        /// <summary>
        /// Gets or sets the number of Payee entities.
        /// </summary>
        public int NumberOfPayess { get; set; }

        /// <summary>
        /// Gets or sets the number of Transaction entities.
        /// </summary>
        public int NumberOfTransactions { get; set; }
    }
}