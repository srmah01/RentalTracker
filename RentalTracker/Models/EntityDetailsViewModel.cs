using System.Collections.Generic;

namespace RentalTracker.Models
{
    /// <summary>
    /// A generic class to represent the Details View Model of an entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity this View Model represents.</typeparam>
    public class EntityDetailsViewModel<T>
    {
        /// <summary>
        /// Gets or sets a handle to Entity being viewed on this Details page.
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// Gets or sets a handle to Filter View being used to select Transactions viewed on this Details page.
        /// </summary>
        public IFilterViewModel Filter { get; set; }

        /// <summary>
        /// Gets or sets the list of Transactions returned by the Filter View.
        /// </summary>
        public ICollection<TransactionsListViewModel> Transactions { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityDetailsViewModel()
        {
            Transactions = new List<TransactionsListViewModel>();
        }
    }
}