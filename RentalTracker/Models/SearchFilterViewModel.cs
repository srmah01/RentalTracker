using RentalTracker.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace RentalTracker.Models
{
    public class SearchFilterViewModel : DateFilterViewModel, IFilterViewModel
    {
        public String Account { get; private set; }
        public String Payee { get; private set; }
        public String Category { get; private set; }

        public SearchFilterViewModel() : base()
        {
        }

        public void SetSearchFilter(String account, String payee, String category,
            DateFilterSelector selector, string from = null, string to = null,
            SortDirection sortOrder = SortDirection.Ascending)
        {
            Account = account;
            Payee = payee;
            Category = category;

            base.SetDateFilter(selector, from, to, sortOrder);
        }
    }
}