using RentalTracker.Enums;
using System;
using System.Web.Helpers;

namespace RentalTracker.Models
{
    /// <summary>
    /// Class representing the View Model of the Search Filter form.
    /// The Search Filter form also includes the parameters of a Date Filter.
    /// </summary>
    public class SearchFilterViewModel : DateFilterViewModel, IFilterViewModel
    {
        /// <summary>
        /// Gets or sets the name or partial name of the account to match.
        /// </summary>
        public String Account { get; private set; }

        /// <summary>
        /// Gets or sets the name or partial name of the payee to match.
        /// </summary>
        public String Payee { get; private set; }

        /// <summary>
        /// Gets or sets the name or partial name of the category to match.
        /// </summary>
        public String Category { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchFilterViewModel() : base()
        {
        }

        /// <summary>
        /// Sets the Search Filter with the given parameters.
        /// </summary>
        /// <param name="account">The name or partial name of the account to match.</param>
        /// <param name="payee">The name or partial name of the payee to match.</param>
        /// <param name="category">The name or partial name of the category to match.</param>
        /// <param name="selector">The value of the Date Filter selection.</param>
        /// <param name="from">The value of the From date.</param>
        /// <param name="to">The value of the To date.</param>
        /// <param name="sortOrder">The order to sort the list.</param>
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