using System;
using System.ComponentModel.DataAnnotations;

namespace RentalTracker.Models
{
    /// <summary>
    /// Class representing the View Model of a list of Transactions
    /// </summary>
    public class TransactionsListViewModel
    {
        /// <summary>
        /// Gets or Sets the ID of the Transaction entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets the Date of the Transaction.
        /// </summary>
        [DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or Sets the Payee of the Transaction.
        /// </summary>
        public string Payee { get; set; }

        /// <summary>
        /// Gets or Sets the Category of the Transaction.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or Sets the Account of the Transaction.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Gets or Sets the Amount of the Transaction, if the associated Category is an Income type.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText = "")]
        public decimal? Income { get; set; }

        /// <summary>
        /// Gets or Sets the Amount of the Transaction, if the associated Category is an Expense type.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText ="")]
        public decimal? Expense { get; set; }

        /// <summary>
        /// Gets or Sets the Balance of the Account resulting from this the Transaction.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or Sets the value of the Taxable flag of the Transaction.
        /// </summary>
        [Display(Name = "")]
        public bool? Taxable { get; set; }

        /// <summary>
        /// Gets or Sets the Reference of the Transaction.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or Sets the Memo of the Transaction.
        /// </summary>
        public string Memo { get; set; }
    }
}