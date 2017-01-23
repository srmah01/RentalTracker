using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentalTracker.Models
{
    public class TransactionsListViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime Date { get; set; }

        public string Payee { get; set; }

        public string Category { get; set; }

        public string Account { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText = "")]
        public decimal? Income { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText ="")]
        public decimal? Expense { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Balance { get; set; }

        [Display(Name = "")]
        public bool? Reconciled { get; set; }

        public string Reference { get; set; }

        public string Memo { get; set; }
    }
}