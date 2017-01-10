using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.DAL
{
    public class RentalTrackerRepository : IRentalTrackerRepository
    {
        public ICollection<Account> GetAllAccounts()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Accounts.ToList();
            }
        }
    }
}
