using RentalTracker.Domain;
using System.Collections.Generic;

namespace RentalTracker.DAL
{
    public interface IRepository
    {
        ICollection<Account> GetAllAccounts();
    }
}