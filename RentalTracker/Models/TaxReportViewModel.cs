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

        [Required]
        public int Year { get; set; }
    }
}