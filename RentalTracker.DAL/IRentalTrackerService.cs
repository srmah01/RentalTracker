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
        ICollection<Account> GetAllAccounts();

        Account FindAccount(int? id);

        Account FindAccountWithTransactions(int? id);

        void SaveNewAccount(Account account);

        void SaveUpdatedAccount(Account account);
    }
}
