using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalTracker.Models.Accounts
{
    public class AccountDetailsViewModel
    {
        public AccountDetailsViewModel()
        {
            Transactions = new List<TransactionsListViewModel>();
        }

        public Account Account { get; set; }

        public ICollection<TransactionsListViewModel> Transactions { get; set; }
    }
}