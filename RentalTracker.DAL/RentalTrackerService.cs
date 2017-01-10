using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalTracker.Domain;
using System.Data.Entity;

namespace RentalTracker.DAL
{
    public class RentalTrackerService : IRentalTrackerService
    {
        public ICollection<Account> getAllAccounts()
        {
            return new List<Account>();
        }

        public void AddAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public Account FindAccount(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
