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
        public ICollection<Account> GetAllAccounts()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Accounts.AsNoTracking().ToList();
            }
        }

        public void SaveNewAccount(Account account)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Accounts.Add(account);
                context.SaveChanges();
            }
        }

        public Account FindAccountWithTransactions(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Accounts.AsNoTracking()
                                       .Include(a => a.Transactions)
                                       .FirstOrDefault(a => a.Id == id);
            }
        }

        public Account FindAccount(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                 return context.Accounts.AsNoTracking()
                                        .SingleOrDefault(a => a.Id == id);
            }
        }

        public void SaveUpdatedAccount(Account account)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
