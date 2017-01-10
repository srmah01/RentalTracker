using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.DAL
{
    public interface IRentalTrackerService
    {
        ICollection<Account> getAllAccounts();

        Account FindAccount(int? id);

        void AddAccount(Account account);
    }
}
