using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentalTracker.Models
{
    public class TaxReportViewModel
    {
        [Required]
        [DisplayName("Account")]
        public int AccountId { get; set; }

        public String AccountName { get; set; }

        [Required]
        public int Year { get; set; }

        public ICollection<Transaction> Income { get; set; }

        public ICollection<Transaction> Expenses{ get; set; }

        [DisplayName("Total Income")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public Decimal TotalIncome { get; set; }

        [DisplayName("Total Expense")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public Decimal TotalExpense { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public Decimal Profit { get; set; }

        [DisplayName("Tax Liability")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public Decimal TaxLiability { get; set; }

        public TaxReportViewModel()
        {
            Income = new List<Transaction>();
            Expenses = new List<Transaction>();
        }
    }
}