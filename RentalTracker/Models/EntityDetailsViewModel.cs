using System.Collections.Generic;

namespace RentalTracker.Models
{
    public class EntityDetailsViewModel<T>
    {
        public EntityDetailsViewModel()
        {
            Transactions = new List<TransactionsListViewModel>();
        }

        public T Entity { get; set; }

        public IFilterViewModel Filter { get; set; }

        public ICollection<TransactionsListViewModel> Transactions { get; set; }
    }
}