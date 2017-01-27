using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalTracker.Models
{
    public class TransactionViewModel
    {
        public Transaction Entity { get; set; }

        public IEnumerable<SelectListItem> Accounts { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Payees { get; set; }
    }
}