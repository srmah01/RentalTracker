using RentalTracker.Domain;
using System.Collections.Generic;

namespace RentalTracker.DAL
{
    public interface IRentalTrackerRepository
    {
        ICollection<Account> GetAllAccounts();
    }
}